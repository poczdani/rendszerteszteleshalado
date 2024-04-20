namespace AutorentApi.Models
{
    public class Car
    {
        public int CarID { get; set; }
        public int CategoryID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int DailyPrice { get; set; }
    }
}
