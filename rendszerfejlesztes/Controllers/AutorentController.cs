using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AutorentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorentController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IRentalService _rentalService;

        public AutorentController(ICarService carService, IRentalService rentalService)
        {
            _carService = carService;
            _rentalService = rentalService;
        }

        [HttpGet("cars")]
        public ActionResult<IEnumerable<Car>> GetCars()
        {
            var cars = _carService.GetAvailableCars();
            return Ok(cars);
        }

        [HttpPost("rentals")]
        public ActionResult<string> RentCar(RentalInfo rentalInfo)
        {
            var result = _rentalService.RentCar(rentalInfo);
            if (result.Success)
                return Ok(result.Message);
            else
                return BadRequest(result.Message);
        }

        // Egyéb végpontok implementálása hasonló módon
    }

    public interface ICarService
    {
        IEnumerable<Car> GetAvailableCars();
    }

    public interface IRentalService
    {
        RentalResult RentCar(RentalInfo rentalInfo);
    }

    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int DailyPrice { get; set; }
    }

    public class RentalInfo
    {
        public int CarId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class RentalResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

