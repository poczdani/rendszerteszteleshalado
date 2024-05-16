using System.Collections.Generic;

namespace CarRent.Data.Entities
{
    public class CarCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property
        public List<Car> Cars { get; set; }
    }
}
