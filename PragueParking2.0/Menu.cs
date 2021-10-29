﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using PragueParking2._0.Vehicles;
using Spectre.Console;

namespace PragueParking2._0
{
    class Menu
    {
        ParkingHouse parkingHouse = new ParkingHouse();
        public void Start()
        {
            bool exit = false;

            while (!exit)
            {
                var table = new Table();
                table.AddColumn("PRAGUE PARKING V2");
                table.Expand();
                AnsiConsole.Write(table);
                string userInput = AnsiConsole.Prompt(new SelectionPrompt<string>()
                  .AddChoices(new[] { "Park vehicle", "Remove Vehicle", "Move Vehicle", "Search Vehicle", "Exit Program" }));

                switch (userInput)
                {
                    case "Park vehicle":
                        ParkMenu();
                        break;

                    case "Remove Vehicle":
                        break;

                    case "Move Vehicle":
                        break;

                    case "Search Vehicle":
                        break;

                    case "Exit":
                        exit = true;
                        break;
                }
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

                    if (parkingHouse.ParkVehicle(car))
                    {
                        Console.Clear();
                        Console.WriteLine("Your " + userInput + " is parked at parkingspot: " + car.Pspot);
                        parkingHouse.JsonSync(parkingHouse.parkingSpotArray);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No available spots, try to use the Optimize function");
                        break;
                    }

                //case "Mc":
                //    CreateVehicle("Mc", RegistrationNumber, out Vehicle mc);
                //    break;

                //case "Bike":
                //    CreateVehicle("Bike", RegistrationNumber, out Vehicle bike);
                //    break;

                case "Bus":
                    CreateVehicle("Bus", RegistrationNumber, out Vehicle bus);

                    if (parkingHouse.ParkBigVehicle(bus))
                    {
                        Console.Clear();
                        Console.WriteLine("Your " + userInput + " is parked at parkingspot: " + bus.Pspot);
                        parkingHouse.JsonSync(parkingHouse.parkingSpotArray);
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
                    Console.WriteLine("Error: Not yet done");
                    Mc mc = new Mc(aRegistrationNumber);
                    aVehicle = mc;
                }
                else if (aType == "Bike")
                {
                    Bike bike = new Bike(aRegistrationNumber);
                    aVehicle = bike;
                    Console.WriteLine("Error: Not yet done");
                }
                else if (aType == "Bus")
                {
                    Bus bus = new Bus(aRegistrationNumber);
                    aVehicle = bus;
                    Console.WriteLine("Error: Not yet done");
                }
                else
                {
                    Car Error = new Car("Error: Vehicle type not found");
                    aVehicle = Error;
                }

            }
        }
    }

