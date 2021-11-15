using PragueParking2._0.Vehicles;
using Spectre.Console;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PragueParking2._0
{
    class Menu
    {
        /*
        
        TODO: Gör klart alla metoder
        TODO: Exception handling
        TODO: Kommentera
        TODO: Dela ParkingHouse 2.0 och ParkingHouse 2.1

        TODO: Gör klart VG-uppgifter
        TODO: Exception handling igen
        TODO: Kommentera igen


        EXTRA
        TODO: Optimera move bus
        TODO: Fixa en output promt
        */
        Settings settings = new Settings();
        ParkingHouse parkingHouse = new ParkingHouse();

        public bool Start()
        {

            Chores();

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
              .AddChoices(new[] { "Park vehicle", "Remove Vehicle", "Move Vehicle", "Search Vehicle", "Clear Parking", "Optimize", "Print tickets", "Update", "Settings", "Exit Program", "X" }));

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

                case "Optimize":
                    OptimizeMenu();
                    return true;

                case "Update":
                    Console.Clear();
                    parkingHouse.Update();
                    return true;

                case "Settings":
                    SettingsMenu();
                    Console.Clear();
                    return true;

                case "Print tickets":
                    Console.Clear();
                    parkingHouse.GetTickets();
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
            Console.SetWindowSize(160, 40);

            // For debugging.
            //parkingHouse.JsonWrite(parkingHouse.ParkingSpotArray);

            //parkingHouse.JsonSettingsWrite(settings);

            CultureInfo.CurrentCulture = new CultureInfo("cs-CZ");

            Settings.JsonSettingsRead(settings);

            parkingHouse.JsonDatafilRead();

            parkingHouse.PrintParkingGrid();
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
                    break;

            }
        }
        private void ParkingSpotSettings()
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
                    ParkingHouseSizeSettingsMenu();
                    break;

                case "High roof":
                    ParkingHouseHighRoofSettingsMenu();
                    break;

                case "Back":
                    break;
            }
        }
        private void ParkingHouseHighRoofSettingsMenu()
        {
            byte highRoof = Settings.SizeParkingHouse;

            Console.Write($"Current value: {highRoof}\n" +
                "Enter new value: ");

            bool parseSuccess = Byte.TryParse(Console.ReadLine(), out byte newValue);

            Console.Clear();

            if (parseSuccess)
            {
                Settings.ChangeParkingHouseHighRoof(newValue);
                Settings.JsonSettingsWrite(settings);
                Console.WriteLine("New value confirmed");
                return;
            }
            Console.WriteLine($"{newValue} is not a valid value, try again");
        }
        private void ParkingHouseSizeSettingsMenu()
        {
            byte size = Settings.SizeParkingHouse;

            Console.Write($"Current size: {size}\n" +
                "Enter new size(cannot be lower than current: ");

            bool parseSuccess = Byte.TryParse(Console.ReadLine(), out byte newSize);

            Console.Clear();

            if (parseSuccess && newSize > size)
            {
                Settings.ChangeParkingHouseSize(newSize);
                Settings.JsonSettingsWrite(settings);
                Console.WriteLine("New size confirmed");
                return;
            }
            Console.WriteLine($"{newSize} is not a valid value, try again");
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
    }
}

