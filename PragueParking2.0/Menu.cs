using PragueParking2._0.Vehicles;
using Spectre.Console;
using System;
using System.Text.RegularExpressions;

namespace PragueParking2._0
{
    class Menu
    {
        /*
        TODO: Fixa så ParkingHouseSize uppdaterar settings så man slipper omstart
        TODO: Exception handling
        TODO: Kommentera
        EXTRA
        TODO: Optimera move bus
        TODO: Fixa en output promt
        TODO: Gör en ticket tabell
        */
        Settings settings = Settings.JsonSettingsRead();
        ParkingHouse parkingHouse = new ParkingHouse();
        /// <summary>
        /// Start is the Main Menu and will let you access all other menus
        /// </summary>
        /// <returns>True if continue program, false if not</returns>
        public bool Start()
        {
            Chores();

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .PageSize(11)
              .AddChoices(new[] { "Park Vehicle", "Remove Vehicle", "Move Vehicle", "Search Vehicle", "Clear Parking", "Optimize all", "Show Tickets", "Update", "Settings", "Exit Program", "X" }));

            switch (userInput)
            {
                case "Park Vehicle":
                    ParkMenu();
                    return true;

                case "Remove Vehicle":
                    Console.WriteLine(RemoveMenu());
                    return true;

                case "Move Vehicle":
                    Console.WriteLine(MoveMenu());
                    return true;

                case "Search Vehicle":
                    Console.WriteLine(SearchVehicleMenu());
                    return true;

                case "Clear Parking":
                    RemoveAllMenu();
                    return true;

                case "Optimize all":
                    OptimizeMenu();
                    return true;

                case "Show Tickets":
                    ShowTicketList();
                    return true;

                case "Update":
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
        /// <summary>
        /// Standard tasks to run every time you come back to main menu
        /// </summary>
        private void Chores()
        {
            Console.SetWindowSize(160, 42);

            settings = Settings.JsonSettingsRead();

            Settings.JsonSettingsRead();

            parkingHouse.Chores();

            ShowParkingGrid();
        }
        /// <summary>
        /// Menu to access all vehicles to park
        /// </summary>
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
        /// <summary>
        /// Creates vehicle type depending on input from ParkMenu
        /// </summary>
        /// <param name="aType"></param>
        /// <param name="aRegistrationNumber"></param>
        /// <param name="aVehicle"></param>
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
        /// <summary>
        /// Menu for to remove vehicles
        /// </summary>
        /// <returns>Returns message depending on success or not</returns>
        private string RemoveMenu()
        {
            ValidateRegistrationNumber(out string RegistrationNumber);

            Console.Clear();

            if (parkingHouse.RemoveVehicle(RegistrationNumber)) return $"Vehicle {RegistrationNumber} has checked out.";

            return $"Vehicle ({RegistrationNumber}) is not parked here.";
        }
        /// <summary>
        /// Menu for to moves vehicles
        /// </summary>
        /// <returns>Returns message depending on success or not</returns>
        private string MoveMenu()
        {
            if (!parkingHouse.SearchVehicle(AskRegistrationNumber(), out Vehicle vehicle, out ParkingSpot parkingSpot))
            {
                Console.Clear();
                return "Vehicle is not parked here.";
            }
            Console.Write("Enter new parkingspot number: ");

            bool parseSuccess = Byte.TryParse(Console.ReadLine(), out byte newParkingSpot);

            Console.Clear();

            if (!parseSuccess && newParkingSpot <= 0 && newParkingSpot > Settings.SizeParkingHouse) return "Enter a valid digit.";
            else if (parkingHouse.MoveVehicle(vehicle, (byte)(newParkingSpot - 1), parkingSpot)) return "Your vehicle is moved to parking spot: " + newParkingSpot;
            else return "Parking spot is unavailable.";
        }
        /// <summary>
        /// Menu to search for vehicle in parking house
        /// </summary>
        /// <returns>Returns message depending on success or not</returns>
        private string SearchVehicleMenu()
        {

            if (parkingHouse.SearchVehicle(AskRegistrationNumber(), out Vehicle vehicle, out ParkingSpot parkingSpot))
            {
                Console.Clear();
                return $"{vehicle.StringType} ({vehicle.RegNum}) is parked at parkingspot: {vehicle.Pspot + 1}.";
            }
            else
            {
                Console.Clear();
                return "Vehicle is not parked here.";
            }
        }
        /// <summary>
        /// Menu to removes all parkings for parking house
        /// </summary>
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
        /// <summary>
        /// Menu to optimize parking house
        /// </summary>
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
        /// <summary>
        /// Asks for registration number and returns string ToUpper.
        /// </summary>
        /// <returns></returns>
        private string AskRegistrationNumber()
        {
            Console.Write("Enter registration number: ");
            string registrationNumber = Console.ReadLine();

            return registrationNumber.ToUpper();
        }
        /// <summary>
        /// Validates regisration number with regex
        /// </summary>
        /// <param name="aRegistrationNumber"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Menu of settings
        /// </summary>
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
        /// <summary>
        /// Settings for parkingspots
        /// </summary>
        private void ParkingSpotSettings()
        {
            Console.WriteLine("Parking Spot");

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
     .AddChoices(new[] { "Size", "Height", "Back" }));

            switch (userInput)
            {
                case "Size":
                    Console.WriteLine(ParkingSpotSizeSettingsMenu());
                    break;

                case "Height":
                    Console.WriteLine(ParkingSpotHighRoofSettingsMenu());
                    break;

                case "Back":
                    Console.Clear();
                    return;
            }
        }
        /// <summary>
        /// Settings for parkingspots size
        /// </summary>
        /// <returns>returns if possible or no</returns>
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

            byte difference = (byte)(Settings.SizeParkingSpot - newSize);

            Settings.ChangeParkingSpotSize(newSize);
            Settings.JsonSettingsWrite(settings);

            parkingHouse.ChangeAvailableSizeAll(difference);

            return "New size confirmed";
        }
        /// <summary>
        /// Settings for parkingspots Height
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string ParkingSpotHighRoofSettingsMenu()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Settings for parking house
        /// </summary>
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
        /// <summary>
        /// Settings for parking house size
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Settings for parking house high roof division
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Settings for vehicles
        /// </summary>
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
                    Console.Clear();
                    break;
            }
        }
        /// <summary>
        /// Settings for vehicle bus
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void BusSettings()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Settings for vehicle bike
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void BikeSettings()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Settings for vehicle mc
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void McSettings()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Settings for vehicle car
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void CarSettings()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Prints ticket list
        /// </summary>
        private void ShowTicketList()
        {
            Console.Clear();
            AnsiConsole.Write(parkingHouse.GetTicketList());
            Console.WriteLine("To continue, press any key...");
            Console.ReadKey();
            Console.Clear();
        }
        /// <summary>
        /// Prints parking grid
        /// </summary>
        private void ShowParkingGrid()
        {
            AnsiConsole.Write(new Columns(parkingHouse.GetParkingGrid()));
        }
    }
}