using System.Collections.Generic;

namespace CarRent.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        // Navigation property
        public virtual List<Rental> Rentals { get; set; }
    }
}
