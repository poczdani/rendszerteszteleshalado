using AutorentAPI.Controllers;
using System;

namespace AutorentAPI.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public RentalResult RentCar(RentalInfo rentalInfo)
        {
            // Logika az autó foglalására
            return _rentalRepository.RentCar(rentalInfo);
        }
    }

    public interface IRentalRepository
    {
        RentalResult RentCar(RentalInfo rentalInfo);
    }
}