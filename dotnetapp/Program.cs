using System;
using System.Collections.Generic;
using System.Linq;
using dotnetapp.Models;

namespace dotnetapp
{
    public class Program
    {
        // In-memory storage
        public static List<Vehicle> Vehicles = new List<Vehicle>();
        public static List<Trip> Trips = new List<Trip>();

        // Auto-increment counters
        public static int VehicleCounter = 1;
        public static int TripCounter = 1;

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("=== Transportation Management ===");
                Console.WriteLine("1. Create Vehicle");
                Console.WriteLine("2. List Vehicles");
                Console.WriteLine("3. Update Vehicle");
                Console.WriteLine("4. Delete Vehicle");
                Console.WriteLine("5. Create Trip");
                Console.WriteLine("6. List Trips");
                Console.WriteLine("7. Update Trip");
                Console.WriteLine("8. Delete Trip");
                Console.WriteLine("9. Assign Trip to Vehicle");
                Console.WriteLine("10. Exit");
                Console.Write("Select option: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int option))
                {
                    Console.WriteLine("Invalid option. Please enter a number.");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        CreateVehicle();
                        break;
                    case 2:
                        ListVehicles();
                        break;
                    case 3:
                        UpdateVehicle();
                        break;
                    case 4:
                        DeleteVehicle();
                        break;
                    case 5:
                        CreateTrip();
                        break;
                    case 6:
                        ListTrips();
                        break;
                    case 7:
                        UpdateTrip();
                        break;
                    case 8:
                        DeleteTrip();
                        break;
                    case 9:
                        AssignTripToVehicle();
                        break;
                    case 10:
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }
            }
        }

        // VEHICLE CRUD
        public static void CreateVehicle()
        {
            Vehicle v = new Vehicle();
            v.VehicleID = VehicleCounter++;

            Console.Write("Make: ");
            v.Make = ReadNonEmptyString();

            Console.Write("Model: ");
            v.Model = ReadNonEmptyString();

            Console.Write("Year: ");
            v.Year = ReadIntInRange(1900, DateTime.Now.Year);

            Console.Write("License Plate: ");
            v.LicensePlate = ReadNonEmptyString();

            Console.Write("Capacity: ");
            v.Capacity = ReadIntInRange(1, 100);

            Console.Write("Vehicle Type (Bus/Van/Car/Truck): ");
            v.VehicleType = ReadNonEmptyString();

            Console.Write("Is Available (true/false): ");
            v.IsAvailable = ReadBool();

            Console.Write("Mileage: ");
            v.Mileage = ReadDoubleNonNegative();

            Vehicles.Add(v);
            Console.WriteLine($"Vehicle {v.VehicleID} created.");
        }

        public static void ListVehicles()
        {
            if (!Vehicles.Any())
            {
                Console.WriteLine("No vehicles found.");
                return;
            }

            foreach (var v in Vehicles)
            {
                Console.WriteLine($"ID: {v.VehicleID}, {v.Year} {v.Make} {v.Model}, Plate: {v.LicensePlate}, Capacity: {v.Capacity}, Type: {v.VehicleType}, Available: {v.IsAvailable}, Mileage: {v.Mileage}");
            }
        }

        public static void UpdateVehicle()
        {
            Console.Write("Vehicle ID to update: ");
            int id = ReadInt();
            var v = Vehicles.FirstOrDefault(x => x.VehicleID == id);
            if (v == null)
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }

            Console.Write($"Make ({v.Make}): ");
            string make = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(make)) v.Make = make;

            Console.Write($"Model ({v.Model}): ");
            string model = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(model)) v.Model = model;

            Console.Write($"Year ({v.Year}): ");
            string yearStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(yearStr) && int.TryParse(yearStr, out int year)) v.Year = year;

            Console.Write($"License Plate ({v.LicensePlate}): ");
            string plate = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(plate)) v.LicensePlate = plate;

            Console.Write($"Capacity ({v.Capacity}): ");
            string capStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(capStr) && int.TryParse(capStr, out int cap)) v.Capacity = cap;

            Console.Write($"Vehicle Type ({v.VehicleType}): ");
            string type = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(type)) v.VehicleType = type;

            Console.Write($"Is Available ({v.IsAvailable}): ");
            string avail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(avail) && bool.TryParse(avail, out bool isAvail)) v.IsAvailable = isAvail;

            Console.Write($"Mileage ({v.Mileage}): ");
            string milStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(milStr) && double.TryParse(milStr, out double mil)) v.Mileage = mil;

            Console.WriteLine("Vehicle updated.");
        }

        public static void DeleteVehicle()
        {
            Console.Write("Vehicle ID to delete: ");
            int id = ReadInt();
            var v = Vehicles.FirstOrDefault(x => x.VehicleID == id);
            if (v == null)
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }

            // Also remove associated trips
            Trips.RemoveAll(t => t.VehicleID == id);
            Vehicles.Remove(v);
            Console.WriteLine("Vehicle deleted.");
        }

        // TRIP CRUD
        public static void CreateTrip()
        {
            Trip t = new Trip();
            t.TripID = TripCounter++;

            Console.Write("Vehicle ID: ");
            int vid = ReadInt();
            var vehicle = Vehicles.FirstOrDefault(x => x.VehicleID == vid);
            if (vehicle == null)
            {
                Console.WriteLine("Vehicle not found. Trip creation aborted.");
                return;
            }
            t.VehicleID = vid;

            Console.Write("Origin: ");
            t.Origin = ReadNonEmptyString();

            Console.Write("Destination: ");
            t.Destination = ReadNonEmptyString();

            Console.Write("Departure Time (yyyy-MM-dd HH:mm): ");
            t.DepartureTime = ReadDateTime();

            Console.Write("Arrival Time (yyyy-MM-dd HH:mm): ");
            t.ArrivalTime = ReadDateTime();

            Console.Write("Distance (km): ");
            t.DistanceKm = ReadDoubleNonNegative();

            Console.Write("Fare: ");
            t.Fare = ReadDoubleNonNegative();

            Console.Write("Status (Scheduled/InProgress/Completed): ");
            t.Status = ReadNonEmptyString();

            Trips.Add(t);
            Console.WriteLine($"Trip {t.TripID} created.");
        }

        public static void ListTrips()
        {
            if (!Trips.Any())
            {
                Console.WriteLine("No trips found.");
                return;
            }

            foreach (var t in Trips)
            {
                Console.WriteLine($"ID: {t.TripID}, VehicleID: {t.VehicleID}, {t.Origin} -> {t.Destination}, Departs: {t.DepartureTime}, Arrives: {t.ArrivalTime}, Distance: {t.DistanceKm}km, Fare: {t.Fare}, Status: {t.Status}");
            }
        }

        public static void UpdateTrip()
        {
            Console.Write("Trip ID to update: ");
            int id = ReadInt();
            var t = Trips.FirstOrDefault(x => x.TripID == id);
            if (t == null)
            {
                Console.WriteLine("Trip not found.");
                return;
            }

            Console.Write($"Origin ({t.Origin}): ");
            string origin = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(origin)) t.Origin = origin;

            Console.Write($"Destination ({t.Destination}): ");
            string dest = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dest)) t.Destination = dest;

            Console.Write($"Departure Time ({t.DepartureTime:yyyy-MM-dd HH:mm}): ");
            string dep = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dep) && DateTime.TryParse(dep, out DateTime dt)) t.DepartureTime = dt;

            Console.Write($"Arrival Time ({t.ArrivalTime:yyyy-MM-dd HH:mm}): ");
            string arr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(arr) && DateTime.TryParse(arr, out DateTime at)) t.ArrivalTime = at;

            Console.Write($"Distance ({t.DistanceKm}): ");
            string dist = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dist) && double.TryParse(dist, out double d)) t.DistanceKm = d;

            Console.Write($"Fare ({t.Fare}): ");
            string fare = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fare) && double.TryParse(fare, out double f)) t.Fare = f;

            Console.Write($"Status ({t.Status}): ");
            string status = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(status)) t.Status = status;

            Console.WriteLine("Trip updated.");
        }

        public static void DeleteTrip()
        {
            Console.Write("Trip ID to delete: ");
            int id = ReadInt();
            var t = Trips.FirstOrDefault(x => x.TripID == id);
            if (t == null)
            {
                Console.WriteLine("Trip not found.");
                return;
            }

            Trips.Remove(t);
            Console.WriteLine("Trip deleted.");
        }

        public static void AssignTripToVehicle()
        {
            Console.Write("Trip ID: ");
            int tid = ReadInt();
            var trip = Trips.FirstOrDefault(x => x.TripID == tid);
            if (trip == null)
            {
                Console.WriteLine("Trip not found.");
                return;
            }

            Console.Write("Vehicle ID: ");
            int vid = ReadInt();
            var vehicle = Vehicles.FirstOrDefault(x => x.VehicleID == vid);
            if (vehicle == null)
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }

            trip.VehicleID = vid;
            Console.WriteLine($"Trip {tid} assigned to Vehicle {vid}.");
        }

        // Helper input methods
        public static string ReadNonEmptyString()
        {
            string s = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(s))
            {
                Console.Write("Value cannot be empty. Enter again: ");
                s = Console.ReadLine();
            }
            return s.Trim();
        }

        public static int ReadInt()
        {
            string s = Console.ReadLine();
            while (!int.TryParse(s, out int val))
            {
                Console.Write("Invalid number. Enter again: ");
                s = Console.ReadLine();
            }
            return int.Parse(s);
        }

        public static int ReadIntInRange(int min, int max)
        {
            string s = Console.ReadLine();
            while (!int.TryParse(s, out int val) || val < min || val > max)
            {
                Console.Write($"Enter a number between {min} and {max}: ");
                s = Console.ReadLine();
            }
            return int.Parse(s);
        }

        public static bool ReadBool()
        {
            string s = Console.ReadLine();
            while (!bool.TryParse(s, out bool val))
            {
                Console.Write("Enter true or false: ");
                s = Console.ReadLine();
            }
            return bool.Parse(s);
        }

        public static double ReadDoubleNonNegative()
        {
            string s = Console.ReadLine();
            while (!double.TryParse(s, out double val) || val < 0)
            {
                Console.Write("Enter a non-negative number: ");
                s = Console.ReadLine();
            }
            return double.Parse(s);
        }

        public static DateTime ReadDateTime()
        {
            string s = Console.ReadLine();
            while (!DateTime.TryParse(s, out DateTime dt))
            {
                Console.Write("Enter date/time in valid format (yyyy-MM-dd HH:mm): ");
                s = Console.ReadLine();
            }
            return DateTime.Parse(s);
        }
    }
}