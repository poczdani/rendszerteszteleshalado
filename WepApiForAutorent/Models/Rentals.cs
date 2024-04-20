namespace WepApiForAutorent.Models
{
    public class Rentals
    {
        public int RentalID { get; set; }
        public string UserID { get; set; }
        public string CarID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
