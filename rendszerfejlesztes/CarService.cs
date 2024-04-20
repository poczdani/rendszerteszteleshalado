using AutorentAPI.Controllers;
using System.Collections.Generic;

namespace AutorentAPI.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public IEnumerable<Car> GetAvailableCars()
        {
            // Logika az autók lekérdezésére
            return _carRepository.GetAvailableCars();
        }
    }

    public interface ICarRepository
    {
        IEnumerable<Car> GetAvailableCars();
    }
}