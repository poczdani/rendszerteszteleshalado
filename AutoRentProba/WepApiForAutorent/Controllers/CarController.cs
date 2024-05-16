using Autorent.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutorentApi.Models;
using AutoRent.API.Services;
using WepApiForAutorent.Models;

namespace AutoRent.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private static List<Car> _cars = new List<Car>
        {
            new Car { CarID = 1, CategoryID = 1, Brand = "Toyota", Model = "Corolla", DailyPrice = 80 },
            new Car { CarID = 2, CategoryID = 2, Brand = "Honda", Model = "Civic", DailyPrice = 60 },
            new Car { CarID = 3, CategoryID = 3, Brand = "Audi", Model = "A7", DailyPrice = 120 },
            new Car { CarID = 4, CategoryID = 4, Brand = "Skoda", Model = "Superb", DailyPrice = 65 },
            new Car { CarID = 5, CategoryID = 5, Brand = "Mercedes", Model = "AMG-GT ", DailyPrice = 280 },
            // ... 
        };

        private readonly AuthService _authService;
        private readonly RentalService _rentalService;

        public CarController(AuthService authService, RentalService rentalService)
        {
            _authService = authService;
            _rentalService = rentalService;
        }

        public class RentalAvailability
        {
            public int CarID { get; set; }
            public List<DateTime> AvailableDates { get; set; }
        }


        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] UserLoginRequest request)
        {
            if (_authService.Authenticate(request.Username, request.Password))
            {
                return Ok("Sikeres belépés!");
            }
            else
            {
                return Unauthorized("Hibás felhasználónév vagy jelszó.");
            }
        }

        [HttpGet("{carId}/availability")]
        public ActionResult<RentalAvailability> GetRentalAvailability(int carId)
        {
            var car = _cars.FirstOrDefault(c => c.CarID == carId);
            if (car == null)
            {
                return NotFound($"Nincs ilyen autó azonosítóval: {carId}");
            }

            var availableDates = new List<DateTime>();
            var currentDate = DateTime.Today;
            var maxDate = DateTime.Today.AddDays(30);

            while (currentDate <= maxDate)
            {
                if (_rentalService.IsCarAvailableForDate(carId, currentDate))
                {
                    availableDates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(1);
            }

            var rentalAvailability = new RentalAvailability
            {
                CarID = carId,
                AvailableDates = availableDates
            };

            return Ok(rentalAvailability);
        }

        [HttpPost("rent")]
        public ActionResult RentCar([FromBody] Rentals rental)
        {
            if (!int.TryParse(rental.CarID, out int carId) ||
                !_rentalService.IsCarAvailableForDate(carId, rental.StartDate) ||
                !_rentalService.IsCarAvailableForDate(carId, rental.EndDate))
            {
                return BadRequest("Az autó ebben az időszakban nem elérhető vagy az autó azonosítója nem megfelelő.");
            }

            _rentalService.AddRental(rental);
            return Ok("Autó sikeresen bérelve!");
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<Car>> ListCars()
        {
            return Ok(_cars);
        }

        [HttpGet]
        public ActionResult<PagedResult<Car>> GetCars(int pageNumber = 1, int pageSize = 10)
        {
            PagedResult.CheckParameters(ref pageNumber, ref pageSize);
            var totalItems = _cars.Count();
            var cars = _cars.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var pagedCars = PagedResult.Create(cars, totalItems, pageNumber, pageSize);
            return Ok(pagedCars);
        }

        
    }



    public class UserLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RentalAvailability
    {
        public int CarID { get; set; }
        public List<DateTime> AvailableDates { get; set; }
    }


}
