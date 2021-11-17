﻿using PragueParking2._0.Vehicles;
using Spectre.Console;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PragueParking2._0
{
    class Menu
    {
        /*
        TODO: Exception handling
        TODO: Kommentera

        EXTRA
        TODO: Optimera move bus
        TODO: Fixa en output promt
        TODO: Gör en ticket tabell
        */
        Settings settings = Settings.JsonSettingsRead();
        ParkingHouse parkingHouse = new ParkingHouse();

        public bool Start()
        {
            Chores();

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .PageSize(11)
              .AddChoices(new[] { "Park vehicle", "Remove Vehicle", "Move Vehicle", "Search Vehicle", "Clear Parking", "Optimize all", "Show Tickets", "Update", "Settings", "Exit Program", "X" }));

            switch (userInput)
            {
                case "Park vehicle":
                    ParkMenu();
                    return true;

                case "Remove Vehicle":
                    RemoveMenu();
                    return true;

                case "Move Vehicle":
                    MoveMenu();
                    return true;

                case "Search Vehicle":
                    SearchVehicleMenu();
                    return true;

                case "Clear Parking":
                    RemoveAllMenu();
                    return true;

                case "Optimize all":
                    OptimizeMenu();
                    return true;

                case "Show Tickets":
                    Console.Clear();
                    ShowTicketList();
                    Console.WriteLine("To continue, press any key...");
                    Console.ReadKey();
                    Console.Clear();
                    return true;

                case "Update":
                    Console.Clear();
                    parkingHouse.Update();
                    return true;

                case "Settings":
                    SettingsMenu();
                    return true;

                case "Exit":
                    break;

                case "X":
                    parkingHouse.AddSomeVehicles();
                    Console.Clear();
                    return true;
            }
            return false;
        }
        private void Chores()
        {
            Console.SetWindowSize(160, 42);

            settings = Settings.JsonSettingsRead();

            Settings.JsonSettingsRead();

            parkingHouse.Chores();

            ShowParkingGrid();
        }
        private void ParkMenu()
        {
            if (ValidateRegistrationNumber(out string RegistrationNumber))
            {
                string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
              .AddChoices(new[] { "Car", "Mc", "Bike", "Bus", "Back" }));

                CreateVehicle(userInput, RegistrationNumber, out Vehicle vehicle);

                switch (userInput)
                {
                    case "Back":
                        Console.Clear();
                        break;

                    default:
                        if (parkingHouse.ParkVehicle(vehicle))
                        {
                            Console.Clear();
                            Console.WriteLine($"{vehicle.StringType} ({vehicle.RegNum}) is parked at parkingspot: {vehicle.Pspot + 1}");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("No available spots, try to use the optimize function.");
                        }
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Registration number not allowed, try again.");
            }
        }
        private void CreateVehicle(string aType, string aRegistrationNumber, out Vehicle aVehicle)
        {
            if (aType == "Car")
            {
                Car car = new Car(aRegistrationNumber);
                aVehicle = car;
            }
            else if (aType == "Mc")
            {
                Mc mc = new Mc(aRegistrationNumber);
                aVehicle = mc;
            }
            else if (aType == "Bike")
            {
                Bike bike = new Bike(aRegistrationNumber);
                aVehicle = bike;
            }
            else if (aType == "Bus")
            {
                Bus bus = new Bus(aRegistrationNumber);
                aVehicle = bus;
            }
            else
            {
                Car Error = new Car("Error: Vehicle type not found.");
                aVehicle = Error;
            }

        }
        private void RemoveMenu()
        {
            ValidateRegistrationNumber(out string RegistrationNumber);

            Console.Clear();

            if (parkingHouse.RemoveVehicle(RegistrationNumber))
            {
                Console.WriteLine("Your vehicle (" + RegistrationNumber + ") has checked out.");
            }
            else
            {
                Console.WriteLine("Vehicle (" + RegistrationNumber + ") is not parked here.");
            }
        }
        private void MoveMenu()
        {
            if (parkingHouse.SearchVehicle(AskRegistrationNumber(), out Vehicle vehicle, out ParkingSpot parkingSpot))
            {
                Console.Write("Enter new parkingspot number: ");
                bool parseSuccess = Byte.TryParse(Console.ReadLine(), out byte newParkingSpot);

                Console.Clear();

                if (parseSuccess && newParkingSpot < 0 && newParkingSpot > Settings.SizeParkingHouse)
                {
                    if (parkingHouse.MoveVehicle(vehicle, (byte)(newParkingSpot - 1), parkingSpot))
                    {
                        Console.Clear();
                        Console.WriteLine("Your vehicle is moved to parking spot: " + newParkingSpot);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Parking spot is unavailable.");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Enter a valid digit.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Vehicle is not parked here.");
            }
        }
        private void SearchVehicleMenu()
        {

            if (parkingHouse.SearchVehicle(AskRegistrationNumber(), out Vehicle vehicle, out ParkingSpot parkingSpot))
            {
                Console.Clear();
                Console.WriteLine(vehicle.StringType + " (" + vehicle.RegNum + ") is parked at parkingspot: " + (vehicle.Pspot + 1) + ".");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Vehicle is not parked here.");
            }
        }
        private void RemoveAllMenu()
        {
            Console.WriteLine("Do you want to remove all vehicles in the parking?\n");

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()

              .AddChoices(new[] { "Yes", "No" }));

            Console.Clear();

            switch (userInput)
            {
                case "Yes":
                    parkingHouse.RemoveAllParkings();
                    break;

                case "No":
                    break;
            }
        }
        private void OptimizeMenu()
        {
            Console.WriteLine("Do you want to move all parked vehicles to optimized spots?\n");

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()

              .AddChoices(new[] { "Yes", "No" }));

            Console.Clear();

            switch (userInput)
            {
                case "Yes":
                    parkingHouse.Optimize();
                    break;

                case "No":
                    Console.Clear();
                    break;
            }
        }
        private string AskRegistrationNumber()
        {
            Console.Write("Enter registration number: ");
            string registrationNumber = Console.ReadLine();

            return registrationNumber.ToUpper();
        }
        private bool ValidateRegistrationNumber(out string aRegistrationNumber)
        {
            Console.Write("Enter your registration plate number: ");
            string registrationNumber = Console.ReadLine();

            registrationNumber = registrationNumber.ToUpper();

            Regex regex = new Regex(@"^[0-9A-Z]+$");
            Regex regexLength = new Regex(@"^.{4,10}$");

            if (regex.IsMatch(registrationNumber) && regexLength.IsMatch(registrationNumber))
            {
                aRegistrationNumber = registrationNumber;

                if (parkingHouse.SearchVehicle(registrationNumber, out Vehicle vehicle, out ParkingSpot parkingSpot))
                {
                    Console.WriteLine(vehicle.StringType + " (" + vehicle.RegNum + ") already exist in parking house");
                    return false;
                }
                return true;
            }
            Console.WriteLine("Registration number requirement. Symbols: A-Z and 0-9, length: 4-10");

            aRegistrationNumber = null;
            return false;
        }
        private void SettingsMenu()
        {
            Console.WriteLine("Settings");

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
     .AddChoices(new[] { "Vehicles", "Parking spot", "Parking house", "Back" }));

            switch (userInput)
            {
                case "Vehicles":
                    VehicleSettings();
                    break;

                case "Parking spot":
                    ParkingSpotSettings();
                    break;

                case "Parking house":
                    ParkingHouseSettings();
                    break;

                case "Back":
                    Console.Clear();
                    return;

            }
        }
        private void ParkingSpotSettings()
        {
            Console.WriteLine("Parking Spot");

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
     .AddChoices(new[] { "Size", "Hight", "Available size", "Back" }));

            switch (userInput)
            {
                case "Size":
                    Console.WriteLine(ParkingSpotSizeSettingsMenu());
                    break;

                case "High roof":
                    Console.WriteLine(ParkingSpotHighRoofSettingsMenu());
                    break;     
                
                case "Available size":
                    Console.WriteLine(ParkingSpotAvailableSizeSettingsMenu());
                    break;

                case "Back":
                    Console.Clear();
                    return;
            }
        }
        private string ParkingSpotSizeSettingsMenu()
        {
            byte size = Settings.SizeParkingSpot;

            Console.WriteLine($"Current size: {size}");
            Console.Write("Enter new size: ");

            bool parseSuccess = Byte.TryParse(Console.ReadLine(), out byte newSize);

            Console.Clear();

            if (!parseSuccess) return $"{newSize} is not a valid value, try again";
            if (newSize < size)
                if (!parkingHouse.PossibleToShrinkParkingSpotSize(newSize)) return "Vehicle in the way";

            Settings.ChangeParkingSpotSize(newSize);
            Settings.JsonSettingsWrite(settings);

            return "New size confirmed";
        }
        private string ParkingSpotHighRoofSettingsMenu()
        {
            throw new NotImplementedException();
        }
        private string ParkingSpotAvailableSizeSettingsMenu()
        {
            throw new NotImplementedException();
        }
        private void ParkingHouseSettings()
        {
            Console.WriteLine("Parking House");

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
     .AddChoices(new[] { "Size", "High roof", "Back" }));

            switch (userInput)
            {
                case "Size":
                    Console.WriteLine(ParkingHouseSizeSettingsMenu());
                    break;

                case "High roof":
                    Console.WriteLine(ParkingHouseHighRoofSettingsMenu());
                    break;

                case "Back":
                    Console.Clear();
                    return;
            }
        }
        private string ParkingHouseSizeSettingsMenu()
        {
            byte size = Settings.SizeParkingHouse;

            Console.WriteLine($"Current size: {size}");
            Console.Write("Enter new size: ");

            bool parseSuccess = Byte.TryParse(Console.ReadLine(), out byte newSize);

            Console.Clear();

            if (!parseSuccess) return $"{newSize} is not a valid value, try again";
            if (newSize < size)
                if (!parkingHouse.PossibleToShrinkSize(newSize)) return "Vehicle in the way";

            Settings.ChangeParkingHouseSize(newSize);
            Settings.JsonSettingsWrite(settings);

            return "New size confirmed, restart program";
        }
        private string ParkingHouseHighRoofSettingsMenu()
        {
            byte highRoof = Settings.SizeParkingHouseHighRoof;

            Console.WriteLine($"Current size: {highRoof}");
            Console.Write("Enter new value: ");

            bool parseSuccess = Byte.TryParse(Console.ReadLine(), out byte newValue);

            Console.Clear();

            if (!parseSuccess) return $"{newValue} is not a valid value, try again";
            if ((newValue < highRoof))
                if (!parkingHouse.PossibleToLowerHighRoof(newValue, highRoof)) return "High vehicle in the way";

                Settings.ChangeParkingHouseHighRoof(newValue);
                Settings.JsonSettingsWrite(settings);

                return "New value confirmed";
        }
        private void VehicleSettings()
        {
            Console.WriteLine("Vehicles");

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
     .AddChoices(new[] { "Car", "Mc", "Bike", "Bus", "Back" }));

            switch (userInput)
            {
                case "Car":
                    CarSettings();
                    break;

                case "Mc":
                    McSettings();
                    break;

                case "Bike":
                    BikeSettings();
                    break;

                case "Bus":
                    BusSettings();
                    break;

                case "Back":
                    break;
            }
        }
        private void BusSettings()
        {
            throw new NotImplementedException();
        }
        private void BikeSettings()
        {
            throw new NotImplementedException();
        }
        private void McSettings()
        {
            throw new NotImplementedException();
        }
        private void CarSettings()
        {
            throw new NotImplementedException();
        }
        private void ShowTicketList()
        {
            AnsiConsole.Write(parkingHouse.GetTicketList());
        }
        private void ShowParkingGrid()
        {
            AnsiConsole.Write(new Columns(parkingHouse.GetParkingGrid()));
        }
    }
}
