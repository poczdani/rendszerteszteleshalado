using CarRent.Data;
using CarRent.Data.Entities;
using CarRental.Data;

namespace CarRent.Core.Services.Init
{
    public class CarInit
    {
        private readonly CarRentalDbContext _context;

        public CarInit(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task Init()
        {
            if (!_context.CarCategories.Any())
            {
                await _context.CarCategories.AddAsync(new CarCategory { Name = "Economy" });
                await _context.CarCategories.AddAsync(new CarCategory { Name = "Compact" });
                await _context.CarCategories.AddAsync(new CarCategory { Name = "Midsize" });
                await _context.CarCategories.AddAsync(new CarCategory { Name = "Fullsize" });
                await _context.CarCategories.AddAsync(new CarCategory { Name = "Luxury" });
                await _context.SaveChangesAsync();
            }

            if (!_context.Cars.Any())
            {
                await _context.Cars.AddAsync(new Car { Brand = "Ford", Model = "Fiesta", Year = 2020, Color = "Blue", DailyPrice = 40, CategoryId = 1 });
                await _context.Cars.AddAsync(new Car { Brand = "Toyota", Model = "Corolla", Year = 2019, Color = "Black", DailyPrice = 50, CategoryId = 2 });
                await _context.Cars.AddAsync(new Car { Brand = "Honda", Model = "Accord", Year = 2018, Color = "White", DailyPrice = 60, CategoryId = 3 });
                await _context.Cars.AddAsync(new Car { Brand = "Mercedes", Model = "S-Class", Year = 2021, Color = "Silver", DailyPrice = 100, CategoryId = 5 });
                await _context.Cars.AddAsync(new Car { Brand = "BMW", Model = "X5", Year = 2020, Color = "Black", DailyPrice = 90, CategoryId = 5 });
                await _context.Cars.AddAsync(new Car { Brand = "Audi", Model = "A4", Year = 2019, Color = "Red", DailyPrice = 70, CategoryId = 4 });
                await _context.Cars.AddAsync(new Car { Brand = "Volkswagen", Model = "Golf", Year = 2019, Color = "White", DailyPrice = 50, CategoryId = 2 });
                await _context.Cars.AddAsync(new Car { Brand = "Renault", Model = "Clio", Year = 2020, Color = "Blue", DailyPrice = 45, CategoryId = 1 });
                await _context.Cars.AddAsync(new Car { Brand = "Hyundai", Model = "i20", Year = 2018, Color = "Silver", DailyPrice = 35, CategoryId = 1 });
                await _context.Cars.AddAsync(new Car { Brand = "Kia", Model = "Sportage", Year = 2019, Color = "Black", DailyPrice = 55, CategoryId = 3 });
                await _context.Cars.AddAsync(new Car { Brand = "Subaru", Model = "Outback", Year = 2021, Color = "Green", DailyPrice = 80, CategoryId = 4 });
                await _context.Cars.AddAsync(new Car { Brand = "Peugeot", Model = "208", Year = 2017, Color = "Red", DailyPrice = 40, CategoryId = 1 });
                await _context.Cars.AddAsync(new Car { Brand = "Fiat", Model = "500", Year = 2019, Color = "Yellow", DailyPrice = 45, CategoryId = 1 });
                await _context.Cars.AddAsync(new Car { Brand = "Opel", Model = "Astra", Year = 2018, Color = "Gray", DailyPrice = 50, CategoryId = 2 });

                await _context.SaveChangesAsync();
            }
        }
    }
}
