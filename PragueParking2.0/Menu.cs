using System;
using System.Text.RegularExpressions;
using PragueParking2._0.Vehicles;
using Spectre.Console;

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
        */
        ParkingHouse parkingHouse = new ParkingHouse();
        public bool Start()
        {
            Console.SetWindowSize(160, 40);

            //parkingHouse.JsonWrite(parkingHouse.ParkingSpotArray);

            parkingHouse.JsonRead();

            parkingHouse.PrintParkingGrid();

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
              .AddChoices(new[] { "Park vehicle", "Remove Vehicle", "Move Vehicle", "Search Vehicle", "Clear Parking", "Optimize", "Exit Program" }));

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

                case "Exit":
                    break;
            }
            return false;
        }

        private void OptimizeMenu()
        {
            Console.WriteLine("Do you want to move all parked vehicles to optimized spots?\n");

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()

              .AddChoices(new[] { "Yes", "No" }));

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

        private void SearchVehicleMenu()
        {
            if (parkingHouse.VehicleExistInParkingHouse(AskRegistrationNumber(), out Vehicle vehicle, out ParkingSpot parkingSpot))
            {
                PrintSearchFound(vehicle, parkingSpot);
            }
            else
            {
                PrintVehicleNotFound(vehicle);
            }
        }
        private void MoveMenu()
        {
            if (parkingHouse.VehicleExistInParkingHouse(AskRegistrationNumber(), out Vehicle vehicle, out ParkingSpot parkingSpot))
            {
                // FRÅGA CLAES OM DET ÄR VÄRT ATT SKAPA EN METOD FÖ DETTA
                Console.Write("Enter new parkingspot number: ");
                bool parseSuccess = Byte.TryParse(Console.ReadLine(), out byte newParkingSPot);

                if (parseSuccess)
                {
                    if (parkingHouse.MoveVehiclePossible(vehicle, newParkingSPot, parkingSpot))
                    {
                        Console.WriteLine("Your vehicle is moved to parking spot: " + newParkingSPot);
                    }
                    else
                    {
                        Console.WriteLine("Parking spot is unavailable.");
                    }
                }
                else
                {
                    Console.WriteLine("Enter a valid digit.");
                }
            }
            else
            {
                PrintVehicleNotFound(vehicle);
            }
        }
        private void RemoveAllMenu()
        {
            Console.WriteLine("Do you want to remove all vehicles in the parking?\n");

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()

              .AddChoices(new[] { "Yes", "No" }));

            switch (userInput)
            {
                case "Yes":
                    parkingHouse.RemoveAllParkings();
                    break;

                case "No":
                    Console.Clear();
                    break;
            }
        }
        private void RemoveMenu()
        {
            CheckRegistrationNumber(out string RegistrationNumber);

            if (parkingHouse.RemoveVehicle(RegistrationNumber))
            {
                Console.WriteLine("Your vehicle has checked out.");
            }
            else
            {
                Console.WriteLine("Vehicle with registration number (" + RegistrationNumber + ") is not parked here.");
            }
        }
        private void ParkMenu()
        {
            CheckRegistrationNumber(out string RegistrationNumber);

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
              .AddChoices(new[] { "Car", "Mc", "Bike", "Bus", "Back" }));

            switch (userInput)
            {
                case "Car":

                    CreateVehicle("Car", RegistrationNumber, out Vehicle car);

                    if (parkingHouse.SmallParkingAvailable(car))
                    {
                        PrintVehicleIsParked(car);
                        break;
                    }
                    else
                    {
                        PrintNoSpotAvailable();
                        break;
                    }

                case "Mc":

                    CreateVehicle("Mc", RegistrationNumber, out Vehicle mc);

                    if (parkingHouse.SmallParkingAvailable(mc))
                    {
                        PrintVehicleIsParked(mc);
                        break;
                    }
                    else
                    {
                        PrintNoSpotAvailable();
                        break;
                    }
                case "Bike":

                    CreateVehicle("Bike", RegistrationNumber, out Vehicle bike);


                    if (parkingHouse.SmallParkingAvailable(bike))
                    {
                        PrintVehicleIsParked(bike);
                        break;
                    }
                    else
                    {
                        PrintNoSpotAvailable();
                        break;
                    }

                case "Bus":

                    CreateVehicle("Bus", RegistrationNumber, out Vehicle bus);

                    if (parkingHouse.BigParkingAvailable(bus))
                    {
                        PrintVehicleIsParked(bus);
                        break;
                    }
                    else
                    {
                        PrintNoSpotAvailable();
                        break;
                    }

                case "Back":
                    Console.Clear();
                    break;

            }
        }
        private bool CheckRegistrationNumber(out string aRegistrationNumber)
        {
            Console.Write("Enter your registration plate number: ");
            string RegistrationNumber = Console.ReadLine();

            Regex regex = new Regex(@"^[[A-Z]\d-]{4,10}$");

            if (regex.IsMatch(RegistrationNumber))
            {
                aRegistrationNumber = RegistrationNumber;
                return true;
            }
            aRegistrationNumber = RegistrationNumber;
            return false; // IN I SKAPA BIl
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
        private string PrintVehicleIsParked(Vehicle vehicle)
        {
            return vehicle.StringType + " (" + vehicle.RegNum + ") is parked at parkingspot: " + (vehicle.Pspot + 1) + ".";
        }
        private string PrintNoSpotAvailable()
        {
            return "No available spots, try to use the optimize function.";
        }
        private string AskRegistrationNumber()
        {
            Console.Write("Enter registration number: ");
            string registrationNumber = Console.ReadLine();

            return registrationNumber.ToUpper();
        }
        private string PrintSearchFound(Vehicle vehicle, ParkingSpot parkingSpot)
        {
            return vehicle.StringType + " (" + vehicle.RegNum + ") is parked at parkingspot: " + parkingSpot.Number + 1 ;
        }
        private string PrintVehicleNotFound(Vehicle vehicle)
        {
            return vehicle.StringType + " (" + vehicle.RegNum + ") is not parked here.";
        }
    }
}


