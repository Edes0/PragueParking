using System;
using System.Text.RegularExpressions;
using PragueParking2._0.Vehicles;
using Spectre.Console;

namespace PragueParking2._0
{
    class Menu
    {
        /*
        
        Tankar.. Fixa en find metod? Vad kan jag förbättra med mina metoder. 
        Parkinghouse letar efter parkeringplatser medans parkeringsplatserna gör det dom gör. Menyn gör också det den gör.

        Kan jag samla metoder något?

       Gör jag samma typ av metoder olika?


        TODO: Gör klart alla metoder
        TODO: Slitta ut methoder mellan ParkingHouse och ParkingSpot
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

            // parkingHouse.JsonWrite(parkingHouse.parkingSpotArray);
            parkingHouse.JsonRead();

            parkingHouse.PrintParkingGrid();

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
              .AddChoices(new[] { "Park vehicle", "Remove Vehicle", "Move Vehicle", "Search Vehicle", "Clear Parking", "Exit Program" }));

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
                    return true;

                case "Clear Parking":
                    RemoveAllMenu();
                    return true;

                case "Exit":
                    break;
            }
            return false;
        }

        private void MoveMenu()
        {
            Console.Write("Enter registration number: ");
            string registrationNumber = Console.ReadLine();
        }

        private void RemoveAllMenu()
        {
            Console.WriteLine("Are you sure?\n");

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()

              .AddChoices(new[] { "Yes", "No" }));

            switch (userInput)
            {
                case "Yes":
                    parkingHouse.RemoveAllParkings();
                    break;

                case "No":
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

                    if (parkingHouse.CheckVehicleParkingAvailable(car))
                    {
                        Console.WriteLine("Your " + userInput + " is parked at parkingspot: " + car.Pspot + ".");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No available spots, try to use the Optimize function.");
                        break;
                    }

                case "Mc":

                    CreateVehicle("Mc", RegistrationNumber, out Vehicle mc);

                    if (parkingHouse.CheckVehicleParkingAvailable(mc))
                    {
                        Console.WriteLine("Your " + userInput + " is parked at parkingspot: " + mc.Pspot + ".");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No available spots, try to use the Optimize function.");
                        break;
                    }
                case "Bike":

                    CreateVehicle("Bike", RegistrationNumber, out Vehicle bike);


                    if (parkingHouse.CheckVehicleParkingAvailable(bike))
                    {
                        Console.WriteLine("Your " + userInput + " is parked at parkingspot: " + bike.Pspot + ".");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No available spots, try to use the Optimize function.");
                        break;
                    }

                case "Bus":

                    CreateVehicle("Bus", RegistrationNumber, out Vehicle bus);

                    if (parkingHouse.CheckBigParkingAvailable(bus))
                    {
                        Console.WriteLine("Your " + userInput + " is parked at parkingspot: " + bus.Pspot + ".");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No available spots, try to use the Optimize function.");
                        break;
                    }

                case "Back":
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
            return false;
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
    }
}


