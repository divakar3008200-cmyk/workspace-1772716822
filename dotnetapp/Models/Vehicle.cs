using System;

namespace dotnetapp.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public int Capacity { get; set; }
        public string VehicleType { get; set; }
        public bool IsAvailable { get; set; }
        public double Mileage { get; set; }
    }
}