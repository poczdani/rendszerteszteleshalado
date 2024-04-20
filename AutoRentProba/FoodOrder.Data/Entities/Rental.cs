using System;

namespace CarRent.Data.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public DateTime RentStart { get; set; }
        public DateTime RentEnd { get; set; }
    }
}
