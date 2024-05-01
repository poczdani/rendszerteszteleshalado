using Autorent.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutorentApi.Models;
using AutoRent.API.Services;
using WepApiForAutorent.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;


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
        private readonly AutoRentDbContext _dbContext;


        public CarController(AutoRentDbContext dbContext, AuthService authService, RentalService rentalService)
        {
            _dbContext = dbContext;
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
                return Ok(new { Message = "Sikeres belépés!" });
            }
            else
            {
                return Unauthorized(new { Error = "Hibás felhasználónév vagy jelszó." });
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




        [HttpGet("list")]
        public IActionResult ListCars()
        {
            var cars = _dbContext.Cars?.ToList();
            var jsonResult = JsonConvert.SerializeObject(cars);

            return Content(jsonResult, "application/json");
        }


        [HttpPost("reserve")]
        public ActionResult ReserveCar([FromBody] RentalRequest rentalRequest)
        {
            var car = _cars.FirstOrDefault(c => c.CarID.ToString() == rentalRequest.CarID);
            if (car == null)
            {
                return NotFound($"Nincs ilyen autó azonosítóval: {rentalRequest.CarID}");
            }

            if (!_rentalService.IsCarAvailableForDate(rentalRequest.CarID.ToString(), rentalRequest.StartDate, rentalRequest.EndDate))
            {
                return BadRequest("Az autó ebben az időszakban nem elérhető.");
            }

            var rentalDays = (rentalRequest.EndDate - rentalRequest.StartDate).Days;
            var totalCost = rentalDays * car.DailyPrice;

            var rental = new Rentals
            {
                RentalID = rentalRequest.RentalID,
                UserID = rentalRequest.UserID,
                CarID = rentalRequest.CarID.ToString(),
                StartDate = rentalRequest.StartDate,
                EndDate = rentalRequest.EndDate,
                CreatedAt = rentalRequest.CreatedAt
            };

            // Foglalás hozzáadása az adatbázishoz
            _dbContext.Rentals.Add(rental);
            _dbContext.SaveChanges(); // Véglegesítés az adatbázisban

            // Válasz JSON formátumban
            var responseJson = new
            {
                Message = "Sikeres foglalás történt",
                RentalID = rentalRequest.RentalID,
                UserID = rentalRequest.UserID,
                CarID = rentalRequest.CarID.ToString(),
                StartDate = rentalRequest.StartDate,
                EndDate = rentalRequest.EndDate,
                CreatedAt = rentalRequest.CreatedAt
            };

            return Ok(responseJson);
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




        [HttpGet("rentals")]
        public ActionResult<IEnumerable<Rentals>> GetRentalsByUsername(string username)
        {
            var users = _dbContext.Users.ToList();
            var rentals = _dbContext.Rentals.ToList();
            List<Rentals> result = null;
            foreach (var rent in rentals)
            {
                foreach (var user in users)
                {
                    if (user.UserID.ToString() == rent.UserID)
                    {
                        result = new List<Rentals>();
                        result.Add(rent);
                    }
                }
            }

            if (result == null)
            {
                return NotFound($"Nincsenek foglalások a következő felhasználóhoz: {username}");
            }

            return Ok(result);
        }




        public class UserLoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class RentalRequest
        {
            public int RentalID { get; set; }
            public string UserID { get; set; }
            public string CarID { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
