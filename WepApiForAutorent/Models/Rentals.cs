using System.ComponentModel.DataAnnotations;

namespace WepApiForAutorent.Models
{
    public class Rentals
    {
        [Key]
        public int RentalID { get; set; }
        public string UserID { get; set; }
        public string CarID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
