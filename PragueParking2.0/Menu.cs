using System;
using System.Text.RegularExpressions;
using PragueParking2._0.Vehicles;
using Spectre.Console;

namespace PragueParking2._0
{
    class Menu
    {

        ParkingHouse parkingHouse = new ParkingHouse();
        public bool Start()
        {
            Console.SetWindowSize(160, 40);

            parkingHouse.JsonRead();

            parkingHouse.PrintParkingGrid();

            string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
              .AddChoices(new[] { "Park vehicle", "Remove Vehicle", "Move Vehicle", "Search Vehicle", "Exit Program" }));

            switch (userInput)
            {
                case "Park vehicle":
                    ParkMenu();
                    return true;

                case "Remove Vehicle":
                    return true;

                case "Move Vehicle":
                    return true;

                case "Search Vehicle":
                    return true;

                case "Exit":
                    break;
            }
            return false;
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
                        Console.Clear();
                        Console.WriteLine("Your " + userInput + " is parked at parkingspot: " + car.Pspot);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No available spots, try to use the Optimize function");
                        break;
                    }

                case "Mc":

                    CreateVehicle("Mc", RegistrationNumber, out Vehicle mc);

                    if (parkingHouse.CheckVehicleParkingAvailable(mc))
                    {
                        Console.Clear();
                        Console.WriteLine("Your " + userInput + " is parked at parkingspot: " + mc.Pspot);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No available spots, try to use the Optimize function");
                        break;
                    }
                case "Bike":

                    CreateVehicle("Bike", RegistrationNumber, out Vehicle bike);


                    if (parkingHouse.CheckVehicleParkingAvailable(bike))
                    {
                        Console.Clear();
                        Console.WriteLine("Your " + userInput + " is parked at parkingspot: " + bike.Pspot);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No available spots, try to use the Optimize function");
                        break;
                    }

                case "Bus":

                    CreateVehicle("Bus", RegistrationNumber, out Vehicle bus);

                    if (parkingHouse.CheckBigParkingAvailable(bus))
                    {
                        Console.Clear();
                        Console.WriteLine("Your " + userInput + " is parked at parkingspot: " + bus.Pspot);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No available spots, try to use the Optimize function");
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
                Car Error = new Car("Error: Vehicle type not found");
                aVehicle = Error;
            }

        }
    }
}


