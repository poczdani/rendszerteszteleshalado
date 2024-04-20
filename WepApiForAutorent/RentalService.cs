using System;
using System.Collections.Generic;
using System.Linq;
using Autorent.Core.Models;
using WepApiForAutorent.Models;

namespace AutoRent.API.Services
{
    public class RentalService
    {
        private readonly List<Rentals> _rentals = new List<Rentals>();
        private int _nextRentalId = 1;

        public bool IsCarAvailableForDate(string carId, DateTime startDate, DateTime endDate)
        {
            // Ellenőrizzük, hogy van-e már foglalás az adott időintervallumban
            return !_rentals.Any(r => r.CarID == carId && (
                (startDate >= r.StartDate && startDate <= r.EndDate) ||
                (endDate >= r.StartDate && endDate <= r.EndDate) ||
                (startDate <= r.StartDate && endDate >= r.EndDate)
            ));
        }

        public bool IsCarAvailableForDate(int carId, DateTime date)
        {
            return !_rentals.Any(r => r.CarID == carId.ToString() && date >= r.StartDate && date <= r.EndDate);
        }


        public void AddRental(Rentals rental)
        {
            rental.RentalID = _nextRentalId++;
            _rentals.Add(rental);
        }
        public int GetNextRentalId()
        {
            return _nextRentalId;
        }

        public List<Rentals> GetUserRentals(string userId)
        {
            return _rentals.Where(r => r.UserID == userId).ToList();
        }



    }
}
