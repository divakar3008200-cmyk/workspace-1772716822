using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using dotnetapp;
using dotnetapp.Models;
using System.Linq;
using System.Collections.Generic;

namespace dotnetapp.Tests
{
    [TestFixture]
    public class TransportationTests
    {
        private StringWriter consoleOutput;
        private TextWriter originalConsoleOut;

        [SetUp]
        public void Setup()
        {
            // Reset in-memory storage and counters
            typeof(Program).GetField("Vehicles").SetValue(null, new List<Vehicle>());
            typeof(Program).GetField("Trips").SetValue(null, new List<Trip>());
            typeof(Program).GetField("VehicleCounter").SetValue(null, 1);
            typeof(Program).GetField("TripCounter").SetValue(null, 1);

            originalConsoleOut = Console.Out;
            consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(originalConsoleOut);
            typeof(Program).GetField("Vehicles").SetValue(null, new List<Vehicle>());
            typeof(Program).GetField("Trips").SetValue(null, new List<Trip>());
        }

        [Test, Order(1)]
        public void Test_Vehicle_Class_Should_Exist()
        {
            var assembly = typeof(Vehicle).Assembly;
            Type t = assembly.GetType("dotnetapp.Models.Vehicle");
            Assert.IsNotNull(t);
        }

        [Test, Order(2)]
        public void Test_Trip_Class_Should_Exist()
        {
            var assembly = typeof(Trip).Assembly;
            Type t = assembly.GetType("dotnetapp.Models.Trip");
            Assert.IsNotNull(t);
        }

        [Test, Order(3)]
        public void Test_Vehicle_Properties_Should_Exist()
        {
            Type t = typeof(Vehicle);
            Assert.IsNotNull(t.GetProperty("VehicleID"));
            Assert.IsNotNull(t.GetProperty("Make"));
            Assert.IsNotNull(t.GetProperty("Model"));
            Assert.IsNotNull(t.GetProperty("Year"));
            Assert.IsNotNull(t.GetProperty("LicensePlate"));
            Assert.IsNotNull(t.GetProperty("Capacity"));
            Assert.IsNotNull(t.GetProperty("VehicleType"));
            Assert.IsNotNull(t.GetProperty("IsAvailable"));
            Assert.IsNotNull(t.GetProperty("Mileage"));
        }

        [Test, Order(4)]
        public void Test_Trip_Properties_Should_Exist()
        {
            Type t = typeof(Trip);
            Assert.IsNotNull(t.GetProperty("TripID"));
            Assert.IsNotNull(t.GetProperty("VehicleID"));
            Assert.IsNotNull(t.GetProperty("Origin"));
            Assert.IsNotNull(t.GetProperty("Destination"));
            Assert.IsNotNull(t.GetProperty("DepartureTime"));
            Assert.IsNotNull(t.GetProperty("ArrivalTime"));
            Assert.IsNotNull(t.GetProperty("DistanceKm"));
            Assert.IsNotNull(t.GetProperty("Fare"));
            Assert.IsNotNull(t.GetProperty("Status"));
        }

        [Test, Order(5)]
        public void Test_Program_Methods_Exist()
        {
            Type p = typeof(Program);
            Assert.IsNotNull(p.GetMethod("CreateVehicle"));
            Assert.IsNotNull(p.GetMethod("ListVehicles"));
            Assert.IsNotNull(p.GetMethod("UpdateVehicle"));
            Assert.IsNotNull(p.GetMethod("DeleteVehicle"));
            Assert.IsNotNull(p.GetMethod("CreateTrip"));
            Assert.IsNotNull(p.GetMethod("ListTrips"));
            Assert.IsNotNull(p.GetMethod("UpdateTrip"));
            Assert.IsNotNull(p.GetMethod("DeleteTrip"));
            Assert.IsNotNull(p.GetMethod("AssignTripToVehicle"));
        }

        private void ProvideConsoleInput(string input)
        {
            var sr = new StringReader(input);
            Console.SetIn(sr);
        }

        [Test, Order(6)]
        public void Test_CreateVehicle_Inserts_Record()
        {
            string inputs = "Toyota\nCorolla\n2015\nABC123\n4\nCar\ntrue\n120000\n";
            ProvideConsoleInput(inputs);
            MethodInfo mi = typeof(Program).GetMethod("CreateVehicle");
            mi.Invoke(null, null);

            var vehicles = (List<Vehicle>)typeof(Program).GetField("Vehicles").GetValue(null);
            Assert.AreEqual(1, vehicles.Count);
            Assert.AreEqual(1, vehicles[0].VehicleID);
            Assert.AreEqual("Toyota", vehicles[0].Make);
        }

        [Test, Order(7)]
        public void Test_CreateTrip_With_Nonexistent_Vehicle_Aborts()
        {
            string inputs = "1\nNewYork\nBoston\n2025-01-01 08:00\n2025-01-01 12:00\n300\n50\nScheduled\n";
            ProvideConsoleInput(inputs);
            MethodInfo mi = typeof(Program).GetMethod("CreateTrip");
            mi.Invoke(null, null);

            var trips = (List<Trip>)typeof(Program).GetField("Trips").GetValue(null);
            Assert.AreEqual(0, trips.Count);
            StringAssert.Contains("Vehicle not found", consoleOutput.ToString());
        }

        [Test, Order(8)]
        public void Test_CreateTrip_After_Vehicle_Creation()
        {
            ProvideConsoleInput("Ford\nTransit\n2018\nTRN456\n12\nVan\ntrue\n80000\n");
            typeof(Program).GetMethod("CreateVehicle").Invoke(null, null);

            ProvideConsoleInput("1\nCityA\nCityB\n2025-05-01 09:00\n2025-05-01 11:00\n150\n30\nScheduled\n");
            typeof(Program).GetMethod("CreateTrip").Invoke(null, null);

            var trips = (List<Trip>)typeof(Program).GetField("Trips").GetValue(null);
            Assert.AreEqual(1, trips.Count);
            Assert.AreEqual(1, trips[0].TripID);
            Assert.AreEqual(1, trips[0].VehicleID);
        }

        [Test, Order(9)]
        public void Test_UpdateVehicle_Changes_Data()
        {
            ProvideConsoleInput("Honda\nCivic\n2016\nHON123\n4\nCar\ntrue\n60000\n");
            typeof(Program).GetMethod("CreateVehicle").Invoke(null, null);

            ProvideConsoleInput("1\nToyota\n\n\n\n\n\n\n\n");
            typeof(Program).GetMethod("UpdateVehicle").Invoke(null, null);

            var vehicles = (List<Vehicle>)typeof(Program).GetField("Vehicles").GetValue(null);
            Assert.AreEqual("Toyota", vehicles[0].Make);
        }

        [Test, Order(10)]
        public void Test_DeleteVehicle_Removes_Associated_Trips()
        {
            ProvideConsoleInput("Volvo\nBusX\n2020\nBUS111\n40\nBus\ntrue\n20000\n");
            typeof(Program).GetMethod("CreateVehicle").Invoke(null, null);

            ProvideConsoleInput("1\nA\nB\n2025-06-01 06:00\n2025-06-01 08:00\n100\n20\nScheduled\n");
            typeof(Program).GetMethod("CreateTrip").Invoke(null, null);

            ProvideConsoleInput("1\n");
            typeof(Program).GetMethod("DeleteVehicle").Invoke(null, null);

            var vehicles = (List<Vehicle>)typeof(Program).GetField("Vehicles").GetValue(null);
            var trips = (List<Trip>)typeof(Program).GetField("Trips").GetValue(null);
            Assert.AreEqual(0, vehicles.Count);
            Assert.AreEqual(0, trips.Count);
        }

        [Test, Order(11)]
        public void Test_UpdateTrip_Changes_Data()
        {
            ProvideConsoleInput("Nissan\nNV200\n2019\nNV200\n6\nVan\ntrue\n50000\n");
            typeof(Program).GetMethod("CreateVehicle").Invoke(null, null);
            ProvideConsoleInput("1\nStart\nEnd\n2025-07-01 07:00\n2025-07-01 09:00\n80\n15\nScheduled\n");
            typeof(Program).GetMethod("CreateTrip").Invoke(null, null);

            ProvideConsoleInput("1\nNewStart\n\n\n\n\n\n\n\n");
            typeof(Program).GetMethod("UpdateTrip").Invoke(null, null);

            var trips = (List<Trip>)typeof(Program).GetField("Trips").GetValue(null);
            Assert.AreEqual("NewStart", trips[0].Origin);
        }

        [Test, Order(12)]
        public void Test_DeleteTrip_Removes_Trip()
        {
            ProvideConsoleInput("Tesla\nModelX\n2021\nTES123\n5\nCar\ntrue\n10000\n");
            typeof(Program).GetMethod("CreateVehicle").Invoke(null, null);
            ProvideConsoleInput("1\nP\nQ\n2025-08-01 10:00\n2025-08-01 12:00\n200\n40\nScheduled\n");
            typeof(Program).GetMethod("CreateTrip").Invoke(null, null);

            ProvideConsoleInput("1\n");
            typeof(Program).GetMethod("DeleteTrip").Invoke(null, null);

            var trips = (List<Trip>)typeof(Program).GetField("Trips").GetValue(null);
            Assert.AreEqual(0, trips.Count);
        }

        [Test, Order(13)]
        public void Test_AssignTripToVehicle_Updates_Trip()
        {
            ProvideConsoleInput("Isuzu\nElf\n2017\nISZ777\n3\nTruck\ntrue\n30000\n");
            typeof(Program).GetMethod("CreateVehicle").Invoke(null, null);
            ProvideConsoleInput("1\nX\nY\n2025-09-01 09:00\n2025-09-01 11:00\n50\n10\nScheduled\n");
            typeof(Program).GetMethod("CreateTrip").Invoke(null, null);

            ProvideConsoleInput("1\n1\n");
            typeof(Program).GetMethod("AssignTripToVehicle").Invoke(null, null);

            var trips = (List<Trip>)typeof(Program).GetField("Trips").GetValue(null);
            Assert.AreEqual(1, trips[0].VehicleID);
        }

        [Test, Order(14)]
        public void Test_CreateVehicle_MinCapacity()
        {
            ProvideConsoleInput("Mini\nTiny\n2000\nMIN1\n1\nCar\ntrue\n0\n");
            typeof(Program).GetMethod("CreateVehicle").Invoke(null, null);
            var vehicles = (List<Vehicle>)typeof(Program).GetField("Vehicles").GetValue(null);
            Assert.AreEqual(1, vehicles[0].Capacity);
        }

        [Test, Order(15)]
        public void Test_Invalid_Number_Input_Prompts_Again()
        {
            string inputs = "MakeX\nModelY\nabc\n2010\nPLT9\n4\nCar\ntrue\n10000\n";
            ProvideConsoleInput(inputs);
            typeof(Program).GetMethod("CreateVehicle").Invoke(null, null);

            var vehicles = (List<Vehicle>)typeof(Program).GetField("Vehicles").GetValue(null);
            Assert.AreEqual(1, vehicles.Count);
            StringAssert.Contains("Enter a number between", consoleOutput.ToString());
        }

        [Test, Order(16)]
        public void Test_Models_Folder_Contains_Classes()
        {
            var assembly = typeof(Program).Assembly;
            var types = assembly.GetTypes().Where(t => t.Namespace == "dotnetapp.Models").ToList();
            Assert.IsTrue(types.Any(t => t.Name == "Vehicle"));
            Assert.IsTrue(types.Any(t => t.Name == "Trip"));
        }
    }
}
