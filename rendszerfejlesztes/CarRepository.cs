using AutorentAPI.Controllers;
using AutorentAPI.Services;
using System;
using System.Collections.Generic;

namespace AutorentAPI.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbContext _context;

        public CarRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Car> GetAvailableCars()
        {
            // Logika az autók lekérdezésére
            throw new NotImplementedException();
        }
    }
}