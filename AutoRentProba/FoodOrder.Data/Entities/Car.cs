using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.Data.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int DailyPrice { get; set; }

        // Foreign key
        public int CategoryId { get; set; }

        // Navigation property
        public virtual CarCategory Category { get; set; }
    }
}
