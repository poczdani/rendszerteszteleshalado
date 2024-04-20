using CarRent.Core.Models;
using CarRent.Core.Models.Car;
using CarRent.Data;
using CarRent.Data.Entities;
using CarRental.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Core.Services
{
    public interface IRentService
    {
        Task AddToRent(int carId, int quantity, int userId);
        Task RemoveFromRent(int carId, int userId);
        Task ClearRent(int userId);
        Task<PagedResult<CarOrderDto>> GetRentedCars(int userId, int pageNumber, int pageSize);
        Task FinishRent(int userId);
    }

    public class RentService : IRentService
    {
        private readonly CarRentalDbContext _context;

        public RentService(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task AddToRent(int carId, int quantity, int userId)
        {
            if (!await _context.Cars.AnyAsync(c => c.Id == carId))
            {
                throw new Exception("Autó nem található");
            }

            var rental = await GetActiveRent(userId);

            if (rental == null)
            {
                rental = new Rental
                {
                    UserId = userId,
                    Status = RentalStatus.Open
                };

                await _context.Rentals.AddAsync(rental);
                await _context.SaveChangesAsync();
            }

            var rentCar = await _context.RentCars
                .FirstOrDefaultAsync(rc => rc.RentalId == rental.Id && rc.CarId == carId);

            if (rentCar == null)
            {
                rentCar = new RentCar
                {
                    RentalId = rental.Id,
                    CarId = carId,
                    Quantity = quantity
                };

                await _context.RentCars.AddAsync(rentCar);
                await _context.SaveChangesAsync();
            }
            else
            {
                rentCar.Quantity += quantity;
                _context.RentCars.Update(rentCar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearRent(int userId)
        {
            var rental = await GetActiveRent(userId);

            if (rental != null)
            {
                var rentCars = await _context.RentCars
                    .Where(rc => rc.RentalId == rental.Id)
                    .ToListAsync();

                _context.RentCars.RemoveRange(rentCars);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("A kölcsönzés nem található");
            }
        }

        public async Task FinishRent(int userId)
        {
            var rental = await GetActiveRent(userId);

            if (rental != null)
            {
                rental.Status = RentalStatus.Finished;
                _context.Rentals.Update(rental);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("A kölcsönzés nem található");
            }
        }

        public async Task<PagedResult<CarOrderDto>> GetRentedCars(int userId, int pageNumber, int pageSize)
        {
            var rental = await GetActiveRent(userId);

            if (rental == null)
            {
                return new PagedResult<CarOrderDto>();
            }

            var rentedCars = await _context.RentCars
                .Include(rc => rc.Car)
                .Where(rc => rc.RentalId == rental.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(rc => new CarOrderDto
                {
                    CarId = rc.Car.Id,
                    CarName = $"{rc.Car.Brand} {rc.Car.Model}",
                    DailyPrice = rc.Car.DailyPrice,
                    Quantity = rc.Quantity
                })
                .ToListAsync();

            return rentedCars.Create(await _context.RentCars.CountAsync(), pageNumber, pageSize);
        }

        public async Task RemoveFromRent(int carId, int userId)
        {
            if (!await _context.Cars.AnyAsync(c => c.Id == carId))
            {
                throw new Exception("Autó nem található");
            }

            var rental = await GetActiveRent(userId);

            if (rental != null)
            {
                var rentCar = await _context.RentCars
                    .FirstOrDefaultAsync(rc => rc.RentalId == rental.Id && rc.CarId == carId);

                if (rentCar != null)
                {
                    _context.RentCars.Remove(rentCar);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new Exception("A kölcsönzés nem található");
            }
        }

        private async Task<Rental?> GetActiveRent(int userId)
        {
            return await _context.Rentals
                .FirstOrDefaultAsync(r => r.UserId == userId && r.Status == RentalStatus.Open);
        }
    }
}
