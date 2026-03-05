using System;

namespace dotnetapp.Models
{
    public class Trip
    {
        public int TripID { get; set; }
        public int VehicleID { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public double DistanceKm { get; set; }
        public double Fare { get; set; }
        public string Status { get; set; }
    }
}