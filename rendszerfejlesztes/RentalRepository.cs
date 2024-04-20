using AutorentAPI.Controllers;
using AutorentAPI.Services;
using System;

namespace AutorentAPI.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }

        public RentalResult RentCar(RentalInfo rentalInfo)
        {
            // Logika az autó foglalására
            throw new NotImplementedException();
        }
    }
}