namespace CarRent.Core.Models.Car
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int DailyPrice { get; set; }
        public CarCategoryDto Category { get; set; }
    }

    public class CreateCarDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }

    public class CarCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CarOrderDto
    {
        internal int DailyPrice;

        public int CarId { get; set; }
        public string CarName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
