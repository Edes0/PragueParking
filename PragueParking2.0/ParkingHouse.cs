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
        private ParkingSpot[] ParkingSpotArray { get; set; } = new ParkingSpot[(int)Sizes.ParkingHouse];
        internal ParkingHouse()
        {
            for (int i = 0; i < Size; i++)
            {
                ParkingSpotArray[i] = new ParkingSpot((byte)i, HighRoof);
            }
        }
        internal bool CheckVehicleParkingAvailable(Vehicle aVehicle)
        {
            Console.Clear();

            byte hight = (byte)Hights.ParkingHigh;

            IEnumerable<ParkingSpot> parkingsWithLowerHight =
                    from parkingSpot in ParkingSpotArray
                    where parkingSpot.Hight < hight
                    select parkingSpot;

            foreach (ParkingSpot parkingSpot in parkingsWithLowerHight)
            {
                if (parkingSpot.ParkingSpotIsAvailable(parkingSpot, aVehicle))
                {
                    JsonWrite(ParkingSpotArray);
                    return true;
                }
            }
            foreach (ParkingSpot aParkingSpot in ParkingSpotArray)
            {
                if (aParkingSpot.ParkingSpotIsAvailable(aParkingSpot, aVehicle))
                {
                    JsonWrite(ParkingSpotArray);
                    return true;
                }
            }
            return false;
        }
        internal bool CheckBigParkingAvailable(Vehicle aVehicle)
        {
            byte parkingsInARowCounter = 0;
            byte parkingSpotCounter = 0;

            Console.Clear();

            foreach (ParkingSpot aParkingSpot in ParkingSpotArray)
            {
                parkingSpotCounter += 1;

                if (aParkingSpot.BigParkingSpotIsAvailable(aParkingSpot, aVehicle))
                {
                    parkingsInARowCounter += 1;

                    CounterLimitCalculator(aVehicle.Size, aParkingSpot.Size, out decimal aCounterLimit);

                    if (parkingsInARowCounter == aCounterLimit)
                    {
                        ParkingSpot parkingSpot = ParkingSpotArray[parkingSpotCounter - (byte)aCounterLimit];

                        aParkingSpot.ReservSpotsForBigVehicle(parkingSpot, aVehicle, parkingSpotCounter, (byte)aCounterLimit, ParkingSpotArray);

                        JsonWrite(ParkingSpotArray);

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
        public void MoveParkingSpot()
        {
            throw new System.NotImplementedException();
        }
        //public bool MoveVehicle(string aRegistrationNumber, byte aTargetParkingSpot)
        //{// small vehicle

        //    ParkingSpot targetParkingSpot = ParkingSpotArray[aTargetParkingSpot];

        //    foreach (ParkingSpot parkingSpot in ParkingSpotArray)
        //    {
        //        if (parkingSpot.CheckVehicleInParkingSpot(aRegistrationNumber, out Vehicle aVehicle);)
        //        {
        //            parkingSpot.
        //        }
                
        //    {

        //            // big vehicle
        //            //else if (aVehicle.Size > parkingSpot.Size && parkingSpot.AvailableSize == parkingSpot.Size)
        //            //{

        //            //}
        //        }
        //    }
        //}
        public void RemoveAllParkings()
        {
            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
            {
                parkingSpot.ClearParking(parkingSpot);
            }
            Console.Clear();
            JsonWrite(ParkingSpotArray);
        }
        public bool RemoveVehicle(string aRegNum)
        {
            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
            {
                if (parkingSpot.CheckVehicleInParkingSpot(aRegNum, out Vehicle aVehicle))
                {
                    if (parkingSpot.RemoveSmallVehicle(aVehicle, parkingSpot))
                    {
                        Console.Clear();
                        JsonWrite(ParkingSpotArray);

                        return true;
                    }
                    else
                    {
                        parkingSpot.VehicleList.Remove(aVehicle);
                        byte reservedSpots = (byte)(aVehicle.Size / parkingSpot.Size);

                        parkingSpot.RemoveBigVehicle(parkingSpot, reservedSpots, ParkingSpotArray);

                        Console.Clear();
                        JsonWrite(ParkingSpotArray);

                        return true;
                    }
                }
            }
            return false;
        }
        internal void PrintParkingGrid()
        {
            var box = new List<Panel>();
            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
            {
                box.Add(
                    new Panel(parkingSpot.GetparkingSpotInfo(parkingSpot))
                        .Header($"{parkingSpot.Number + 1}")
                        .RoundedBorder());

            }
            // Render all cards in columns
            AnsiConsole.Write(new Columns(box));
        }
        internal void JsonWrite(ParkingSpot[] aParkingSpotArray)
        {
            string path = @"../../../Datafiles/Datafile.json";

            string ParkingSpotArrayJson = JsonConvert.SerializeObject(aParkingSpotArray, Formatting.Indented, new VehicleConverter());

            File.WriteAllText(path, ParkingSpotArrayJson);
        }
        public void JsonRead()
        {
            string path = @"../../../Datafiles/Datafile.json";

            string ParkingSpotArrayJson = File.ReadAllText(path);

            ParkingSpotArray = JsonConvert.DeserializeObject<ParkingSpot[]>(ParkingSpotArrayJson, new VehicleConverter());

        }
    }
}