namespace CarRent.Data.Entities
{
    public class RentCar
    {
        public int Id { get; set; }
        public int RentalId { get; set; } // Az aktuális kölcsönzés azonosítója
        public int CarId { get; set; } // Az autó azonosítója
        public int Quantity { get; set; } // Az autók száma
        public decimal DailyPrice { get; set; } // Az autó napi kölcsönzési ára

        public Rental Rental { get; set; } // Kapcsolat a kölcsönzéssel
        public Car Car { get; set; } // Kapcsolat az autóval
    }
}
