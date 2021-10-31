using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using static PragueParking2._0.ParkingHouse.JsonCreationConverter<PragueParking2._0.Vehicles.Vehicle>;

namespace PragueParking2._0
{
    partial class ParkingHouse
    {
        private byte HighRoof { get; } = (byte)Sizes.ParkingHouseHighRoof;
        private byte Size { get; } = (byte)Sizes.ParkingHouse;
        private ParkingSpot[] parkingSpotArray { get; set; } = new ParkingSpot[(int)Sizes.ParkingHouse];
        internal ParkingHouse()
        {
            for (int i = 0; i < Size; i++)
            {
                parkingSpotArray[i] = new ParkingSpot((byte)i, HighRoof);
            }
        }
        internal bool CheckVehicleParkingAvailable(Vehicle aVehicle)
        {
            foreach (ParkingSpot aParkingSpot in parkingSpotArray)
            {
                if (aParkingSpot.AvailableSize >= aVehicle.Size)
                {
                    aParkingSpot.AvailableSize -= aVehicle.Size;
                    aVehicle.Pspot = aParkingSpot.Number;
                    aParkingSpot.VehicleList.Add(aVehicle);

                    JsonWrite(parkingSpotArray);

                    return true;
                }
            }
            return false;
        }
        internal bool CheckBigParkingAvailable(Vehicle aVehicle)
        {
            byte parkingsInARowCounter = 0;
            byte parkingSpotCounter = 0;

            foreach (ParkingSpot aParkingSpot in parkingSpotArray)
            {
                parkingSpotCounter += 1;

                if (aParkingSpot.Hight > aVehicle.Hight && aParkingSpot.AvailableSize == aParkingSpot.Size)
                {

                    parkingsInARowCounter += 1;

                    CounterLimitCalculator(aVehicle.Size, aParkingSpot.Size, out decimal aCounterLimit);

                    if (parkingsInARowCounter == aCounterLimit)
                    {
                        ParkingSpot parkingSpot = parkingSpotArray[parkingSpotCounter - (byte)aCounterLimit];

                        for (int i = 1; i < aCounterLimit; i++)
                        {

                            parkingSpotArray[parkingSpotCounter - i].AvailableSize -= parkingSpot.AvailableSize;
                        }

                        parkingSpot.AvailableSize -= parkingSpot.AvailableSize;
                        parkingSpot.VehicleList.Add(aVehicle);

                        JsonWrite(parkingSpotArray);

                        return true;
                    }
                }
                else
                {
                    parkingsInARowCounter = 0;
                }
            }
            return false;
        }

        internal void CounterLimitCalculator(byte aVehicleSize, byte aParkingSpotSize, out decimal aCounterLimit)
        {
            aCounterLimit = aVehicleSize / aParkingSpotSize;
            aCounterLimit = Math.Ceiling(aCounterLimit);
        }
        public void Optimize()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAllParkings()
        {
            throw new System.NotImplementedException();
        }

        public void SearchVehicle()
        {
            throw new System.NotImplementedException();
        }
        public void UpdateTickets()
        {
            throw new System.NotImplementedException();
        }

        public void CalculatePrice()
        {
            throw new System.NotImplementedException();
        }

        public void Prints()
        {
            throw new System.NotImplementedException();
        }

        public void MoveVehicle()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveVehicle()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateDataFil()
        {
            throw new System.NotImplementedException();
        }

        public void MoveParkingSpot()
        {
            throw new System.NotImplementedException();
        }
        internal void PrintParkingGrid()
        {
            var table = new Table();
            const int cols = 6;
            int n = 0;

            foreach (ParkingSpot parkingSpot in parkingSpotArray)
            {
                if (n >= cols && n % cols == 0)
                {
                    // New row
                    n = 0;
                }
                else if (parkingSpot.VehicleList.Count == 0 && parkingSpot.AvailableSize == parkingSpot.Size)
                {
                    table.AddColumn("Empty");
                    AnsiConsole.Write(table);
                    n++;
                }
                else if (parkingSpot.VehicleList.Count == 0 && parkingSpot.AvailableSize != parkingSpot.Size)
                {                             
                    table.AddColumn("-");
                    AnsiConsole.Write(table);
                    n++;
                }
                else
                {
                    n++;
                    foreach (Vehicle vehicle in parkingSpot.VehicleList)
                    {
                        table.AddColumn(vehicle.StringType);
                        AnsiConsole.Write(table);
                    }
                }
            }
            table.Expand();
            AnsiConsole.Write(table);
        }
        internal void JsonWrite(ParkingSpot[] aParkingSpotArray)
        {
            string path = @"../../../Datafiles/Datafile.json";

            string parkingSpotArrayJson = JsonConvert.SerializeObject(aParkingSpotArray, Formatting.Indented, new VehicleConverter());

            File.WriteAllText(path, parkingSpotArrayJson);
        }
        public void JsonRead()
        {
            string path = @"../../../Datafiles/Datafile.json";

            string parkingSpotArrayJson = File.ReadAllText(path);

            parkingSpotArray = JsonConvert.DeserializeObject<ParkingSpot[]>(parkingSpotArrayJson, new VehicleConverter());

        }
    }
}