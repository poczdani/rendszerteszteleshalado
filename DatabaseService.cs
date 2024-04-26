using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using AutorentApi.Models;
using Dapper;
using WepApiForAutorent.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutoRent.API.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = "Data Source=autorent.db";
        }

        public List<Car> GetAllCars()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var cars = connection.Query<Car>("SELECT * FROM cars").ToList();
                return cars;
            }
        }

        public Car GetCarById(int carId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var car = connection.QueryFirstOrDefault<Car>("SELECT * FROM cars WHERE car_id = @CarId", new { CarId = carId });
                return car;
            }
        }

        public void AddRental(Rentals rental)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("INSERT INTO rentals (user_id, car_id, from_date, to_date, created) VALUES (@UserId, @CarId, @FromDate, @ToDate, @Created)", rental);
            }
        }

       

    }
}