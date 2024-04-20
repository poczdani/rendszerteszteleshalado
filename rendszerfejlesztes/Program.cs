using AutorentAPI.Controllers;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace AutorentClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Autorent Teszt Felület");
            Console.WriteLine("1. Autók Listázása");
            Console.WriteLine("2. Autó Foglalása");
            Console.Write("Válasszon menüpontot: ");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || (choice < 1 || choice > 2))
            {
                Console.WriteLine("Érvénytelen választás. Kérem, válasszon 1 vagy 2 közül.");
                Console.Write("Válasszon menüpontot: ");
            }

            switch (choice)
            {
                case 1:
                    await ListCars();
                    break;
                case 2:
                    await RentCar();
                    break;
            }
        }

        static async Task ListCars()
        {
            // Autók listázása API hívás
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/api/autorent/");
               var response = await client.GetAsync("cars");
               if (response.IsSuccessStatusCode)
               {
                  var cars = await response.Content.ReadAsAsync<IEnumerable<Car>>();
                   foreach (var car in cars)
                    {
                        Console.WriteLine($"ID: {car.Id}, Márka: {car.Brand}, Modell: {car.Model}, Napi ár: {car.DailyPrice}");
                    }
                }
                else
                {
                    Console.WriteLine("Hiba történt az autók lekérdezése közben.");
                }
            }
        }

        static async Task RentCar()
        {
            // Autó foglalása API hívás
            Console.Write("Autó ID: ");
            int carId = int.Parse(Console.ReadLine());

            Console.Write("Kezdő dátum (ÉÉÉÉ-HH-NN): ");
            DateTime fromDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Vég dátum (ÉÉÉÉ-HH-NN): ");
            DateTime toDate = DateTime.Parse(Console.ReadLine());

            var rentalInfo = new RentalInfo { CarId = carId, FromDate = fromDate, ToDate = toDate };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/api/autorent/");
                var response = await client.PostAsJsonAsync("rentals", rentalInfo);
                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(message);
                }
                else
                {
                    Console.WriteLine("Hiba történt az autó foglalása közben.");
                }
            }
        }
    }
}