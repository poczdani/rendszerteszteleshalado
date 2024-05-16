using AutoMapper;
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
    public interface ICarService
    {
        Task<Car> GetCar(int id);
        Task<PagedResult<Car>> GetCars(int pageNumber, int pageSize);
        Task<PagedResult<CarCategory>> GetCategories(int pageNumber, int pageSize);
        Task<PagedResult<Car>> CreateCar(CreateCarDto createCarDto);
        Task<PagedResult<Car>> UpdateCar(int id, CreateCarDto updateCarDto);
        Task<PagedResult<Car>> DeleteCar(int id);
    }

    public class CarService : ICarService
    {
        private readonly CarRentalDbContext _context;
        private readonly IMapper _mapper;

        public CarService(CarRentalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<Car>> CreateCar(CreateCarDto createCarDto)
        {
            if (await _context.CarCategories.AnyAsync(c => c.Id == createCarDto.CategoryId))
            {
                var car = _mapper.Map<Car>(createCarDto);
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return await GetCars(1, 10);
            }
            else
            {
                throw new Exception("Kategória nem található");
            }

        }

        public async Task<PagedResult<Car>> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id) ?? throw new Exception("Autó nem található");
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return await GetCars(1, 10);
        }

        public async Task<PagedResult<CarCategory>> GetCategories(int pageNumber, int pageSize)
        {
            var categories = await _context.CarCategories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var result = categories.Create(await _context.CarCategories.CountAsync(), pageNumber, pageSize);
            return result;
        }

        public async Task<Car> GetCar(int id)
        {
            return await _context.Cars.Include(c => c.Category).FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("Autó nem található");
        }

        public async Task<PagedResult<Car>> GetCars(int pageNumber, int pageSize)
        {
            var cars = await _context.Cars.Include(c => c.Category).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var result = cars.Create(await _context.Cars.CountAsync(), pageNumber, pageSize);
            return result;
        }

        public async Task<PagedResult<Car>> UpdateCar(int id, CreateCarDto updateCarDto)
        {
            if (await _context.CarCategories.AnyAsync(c => c.Id == updateCarDto.CategoryId))
            {
                var car = await _context.Cars.FindAsync(id) ?? throw new Exception("Autó nem található");
                _mapper.Map(updateCarDto, car);
                _context.Cars.Update(car);
                await _context.SaveChangesAsync();
                return await GetCars(1, 10);
            }
            else
            {
                throw new Exception("Kategória nem található");
            }
        }
    }
}
