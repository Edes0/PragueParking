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
        // SET PRIVATE WHEN DONE
        public ParkingSpot[] ParkingSpotArray { get; set; } = new ParkingSpot[(int)Sizes.ParkingHouse];
        internal ParkingHouse()
        {
            for (int i = 0; i < Size; i++)
            {
                ParkingSpotArray[i] = new ParkingSpot((byte)i, HighRoof);
            }
        }
        internal bool ParkVehicle(Vehicle vehicle)
        {
            byte hight = (byte)Hights.ParkingHigh;

            if (vehicle.IsTiny())
            {
                // Parks vehicle at parkings with matching available size if possible.   Fixa så att inte car också kommer med..
                IEnumerable<ParkingSpot> parkingsWithMatchingAvailableSize =
                        from parkingSpot in ParkingSpotArray
                        where parkingSpot.AvailableSize == vehicle.Size
                        select parkingSpot;

                foreach (ParkingSpot parkingSpot in parkingsWithMatchingAvailableSize)
                {
                    parkingSpot.AddVehicle(vehicle);

                    JsonWrite(ParkingSpotArray);

                    return true;
                }
            }
            if (vehicle.IsTiny() || vehicle.IsSmall())
            {
                // Parks vehicle at parkings with lower hight if possible.
                IEnumerable<ParkingSpot> parkingsWithLowerHight =
                        from parkingSpot in ParkingSpotArray
                        where parkingSpot.Hight < hight
                        select parkingSpot;

                foreach (ParkingSpot parkingSpot in parkingsWithLowerHight)
                {
                    if (parkingSpot.ParkingSpotAvailable(vehicle))
                    {
                        parkingSpot.AddVehicle(vehicle);

                        JsonWrite(ParkingSpotArray);

                        return true;
                    }
                }
                // Parks vehicle at any parking.
                foreach (ParkingSpot parkingSpot in ParkingSpotArray)
                {
                    if (parkingSpot.ParkingSpotAvailable(vehicle))
                    {
                        parkingSpot.AddVehicle(vehicle);

                        JsonWrite(ParkingSpotArray);

                        return true;
                    }
                }
            }
            else if (vehicle.IsBig())
            {
                if (BigParkingAvailable(vehicle, out byte newParkingSpotIndex, out byte counterLimit))
                {
                    ParkingSpot parkingSpot = ParkingSpotArray[newParkingSpotIndex];

                    parkingSpot.AddVehicle(vehicle);

                    AddReservedSpots(parkingSpot, counterLimit);

                    JsonWrite(ParkingSpotArray);

                    return true;
                }
                return false;
            }
            else
            {
                throw new Exception("Vehicle is neither big nor small, change vehicle size or parking size");
            }
            return false;
        }
        private bool BigParkingAvailable(Vehicle vehicle, out byte newParkingSpotIndex, out byte aCounterLimit)
        {
            byte parkingsInARowCounter = 0;
            byte parkingSpotCounter = 0;

            foreach (ParkingSpot aParkingSpot in ParkingSpotArray)
            {
                parkingSpotCounter += 1;

                if (aParkingSpot.ParkingSpotAvailable(vehicle))
                {
                    parkingsInARowCounter += 1;

                    CounterLimitCalculator(vehicle.Size, aParkingSpot.Size, out byte counterLimit);

                    if (parkingsInARowCounter == counterLimit)
                    {
                        aCounterLimit = counterLimit;
                        newParkingSpotIndex = (byte)(parkingSpotCounter - counterLimit);
                        return true;
                    }
                }
                else
                {
                    parkingsInARowCounter = 0;
                }
            }
            aCounterLimit = 0;
            newParkingSpotIndex = 0;
            return false;
        }
        private bool BigParkingAvailableSpecfic(Vehicle vehicle, ParkingSpot newParkingSpot, out byte aCounterLimit)
        {
            CounterLimitCalculator(vehicle.Size, newParkingSpot.Size, out byte counterLimit);

            byte parkingsInARowCounter = 0;

            for (int i = newParkingSpot.Number; i < newParkingSpot.Number + counterLimit; i++)
            {
                if (ParkingSpotArray[i].ParkingSpotAvailable(vehicle)) // (newParkingSpot.Number >= vehicle.Pspot && vehicle.Pspot !< vehicle.Pspot + counterLimit +1)
                {
                    parkingsInARowCounter += 1;

                    if (parkingsInARowCounter == counterLimit)
                    {
                        aCounterLimit = counterLimit;
                        return true;
                    }
                }
            }
            aCounterLimit = 0;
            return false;
        }
        private void CounterLimitCalculator(byte aVehicleSize, byte aParkingSpotSize, out byte counterLimit)
        {
            decimal aCounterLimit = aVehicleSize / aParkingSpotSize;
            aCounterLimit = Math.Ceiling(aCounterLimit);

            counterLimit = (byte)aCounterLimit;
        }
        internal bool MoveVehicle(Vehicle vehicle, byte aNewParkingSpot, ParkingSpot oldParkingSpot)
        {
            ParkingSpot newParkingSpot = ParkingSpotArray[aNewParkingSpot - 1];

            if (vehicle.IsSmall() || vehicle.IsTiny())
            {
                if (newParkingSpot.ParkingSpotAvailable(vehicle))
                {
                    oldParkingSpot.RemoveVehicle(vehicle);
                    newParkingSpot.AddVehicle(vehicle);

                    JsonWrite(ParkingSpotArray);

                    return true;
                }
                else
                {
                    return false;
                }

            }
            else if (vehicle.IsBig())
            {
                if (BigParkingAvailableSpecfic(vehicle, newParkingSpot, out byte counterLimit))
                {
                    oldParkingSpot.RemoveVehicle(vehicle);
                    ClearReservedSpots(oldParkingSpot, counterLimit);
                    newParkingSpot.AddVehicle(vehicle);
                    AddReservedSpots(newParkingSpot, counterLimit);

                    JsonWrite(ParkingSpotArray);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new Exception("Vehicle is neither big, small nor tiny, change vehicle size or parking size");
            }
        }
        internal bool SearchVehicle(string registrationNumber, out Vehicle aVehicle, out ParkingSpot aParkingSpot)
        {
            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
            {
                if (parkingSpot.VehicleExist(registrationNumber, out Vehicle vehicle))
                {
                    aParkingSpot = parkingSpot;
                    aVehicle = vehicle;
                    return true;
                }
            }
            aParkingSpot = null;
            aVehicle = null;
            return false;
        }
        internal void RemoveAllParkings()
        {
            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
            {
                parkingSpot.Clear();
            }
            JsonWrite(ParkingSpotArray);
        }
        internal bool RemoveVehicle(string aRegNum)
        {
            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
            {
                if (parkingSpot.VehicleExist(aRegNum, out Vehicle vehicle))
                {
                    parkingSpot.RemoveVehicle(vehicle);

                    CounterLimitCalculator(vehicle.Size, parkingSpot.Size, out byte counterLimit);
                    ClearReservedSpots(parkingSpot, counterLimit);

                    JsonWrite(ParkingSpotArray);
                    return true;
                }
            }
            return false;
        }
        private void ClearReservedSpots(ParkingSpot parkingSpot, byte aCounterLimit)
        {
            for (int i = 1; i < aCounterLimit; i++) // kolla här.
            {
                ParkingSpotArray[parkingSpot.Number + aCounterLimit - i].Clear();
            }
        }
        private void AddReservedSpots(ParkingSpot parkingSpot, byte aCounterLimit)
        {
            for (int i = 1; i < aCounterLimit; i++)
            {
                parkingSpot.Reserve(ParkingSpotArray[parkingSpot.Number + i]);
            }
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
        public void Optimize() // NOT DONE
        {
            var ParkingSpotsNeedOptimization3and1 =
            from parkingSpot3 in ParkingSpotArray
            where parkingSpot3.AvailableSize == 3
            from parkingSpot1 in ParkingSpotArray
            where parkingSpot1.AvailableSize == 1
            select new
            {
                parkingSpot3,
                parkingSpot1,
                vehicle0 = parkingSpot1.VehicleList[0],
                vehicle1 = parkingSpot1.VehicleList[1]
            };

            foreach (var pair in ParkingSpotsNeedOptimization3and1)
            {
                if (pair.vehicle0.Size == 1)
                {
                    Vehicle vehicle = pair.vehicle0;
                    ParkingSpot parkingSpot = pair.parkingSpot3;

                    pair.parkingSpot3.AddVehicle(vehicle);
                    Console.WriteLine(vehicle.RegNum + "was moved to spot " + pair.parkingSpot3.Number);
                    pair.parkingSpot1.RemoveVehicle(vehicle);

                    JsonWrite(ParkingSpotArray);
                }
                else if (pair.vehicle1.Size == 1)
                {
                    Vehicle vehicle = pair.vehicle1;

                    pair.parkingSpot3.AddVehicle(vehicle);
                    Console.WriteLine(vehicle.RegNum + "was moved to spot " + pair.parkingSpot3.Number);
                    pair.parkingSpot1.RemoveVehicle(vehicle);

                    JsonWrite(ParkingSpotArray);
                }
            }
            //    var ParkingSpotsNeedOptimization2and2 =
            //    from parkingSpot3 in ParkingSpotArray
            //    where parkingSpot3.AvailableSize == 2
            //    from parkingSpot1 in ParkingSpotArray
            //    where parkingSpot1.AvailableSize == 2
            //    select new { parkingSpot3, parkingSpot1 };

            //    foreach (var pair in ParkingSpotsNeedOptimization2and2) // UNION??? NO DUPLICATES
            //    {
            //        ParkingSpot.JoinParkings2and2(pair);
            //    }
        }
        public void UpdateTickets()
        {
            throw new System.NotImplementedException();
        }// Not done
        public void CalculatePrice()
        {
            throw new System.NotImplementedException();
        }// Not done
    }
}