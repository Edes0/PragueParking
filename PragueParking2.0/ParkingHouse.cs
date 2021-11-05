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
            if (vehicle.IsSmallVehicle())
            {
                byte hight = (byte)Hights.ParkingHigh;

                // Parks vehicle at parkings with matching available size if possible.   Fixa så att inte car också kommer med..
                IEnumerable<ParkingSpot> parkingsWithMatchingAvailableSize =
                        from parkingSpot in ParkingSpotArray
                        where parkingSpot.AvailableSize == vehicle.Size && parkingSpot.VehicleList.Count() > 0
                        select parkingSpot;

                foreach (ParkingSpot parkingSpot in ParkingSpotArray)
                {
                    if (parkingSpot.ParkingSpotAvailable(vehicle))
                    {
                        parkingSpot.AddVehicle(vehicle);

                        JsonWrite(ParkingSpotArray);

                        return true;
                    }
                }
                
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
            else if (vehicle.IsBigVehicle())
            {
                if (BigParkingAvailable(vehicle, out byte parkingSpotCounter, out decimal counterLimit, out byte parkingsInARow))
                {
                    ParkingSpot parkingSpot = ParkingSpotArray[parkingSpotCounter - (byte)counterLimit];

                    parkingSpot.AddVehicle(vehicle);

                    AddReservedSpots(parkingSpot, (byte)counterLimit, parkingSpotCounter);

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
        private bool BigParkingAvailable(Vehicle vehicle, out byte aParkingSpotCounter, out decimal aCounterLimit, out byte aParkingsInARow)
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
                        aParkingSpotCounter = parkingSpotCounter;
                        aCounterLimit = counterLimit;
                        aParkingsInARow = parkingsInARowCounter;

                        return true;
                    }
                }
                else
                {
                    parkingsInARowCounter = 0;
                }
            }
            aParkingSpotCounter = 0;
            aCounterLimit = 0;
            aParkingsInARow = 0;
            return false;
        }
        private bool BigParkingAvailableSpecfic(Vehicle vehicle, out byte aParkingSpotCounter, out decimal aCounterLimit, out byte aParkingsInARow)
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
                        aParkingSpotCounter = parkingSpotCounter;
                        aCounterLimit = counterLimit;
                        aParkingsInARow = parkingsInARowCounter;

                        return true;
                    }
                }
            }
            aParkingSpotCounter = 0;
            aCounterLimit = 0;
            aParkingsInARow = 0;
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

            if (vehicle.IsSmallVehicle())
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
            else if (vehicle.IsBigVehicle())
            {
                if (BigParkingAvailableSpecfic(vehicle, out byte parkingSpotCounter, out decimal counterLimit, out byte parkingsInARow))
                {
                    newParkingSpot.AddVehicle(vehicle);
                    AddReservedSpots(newParkingSpot, (byte)counterLimit, aNewParkingSpot);
                    oldParkingSpot.RemoveVehicle(vehicle);
                    ClearReservedSpots(oldParkingSpot, (byte)counterLimit);

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
                throw new Exception("Vehicle is neither big nor small, change vehicle size or parking size");
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
        internal bool RemoveVehicle(string aRegNum) // Buggar när tar bort större fordon. Tog bort i före counter limit, la till den igen
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
        } // BUGGING WHEN REMOVING BUS. Doesnt remove last available space
        private void ClearReservedSpots(ParkingSpot parkingSpot, byte aCounterLimit)
        {
            for (int i = 1; i < aCounterLimit; i++) // kolla här.
            {
                ParkingSpotArray[parkingSpot.Number + aCounterLimit - i].Clear();
            }
        }
        private void AddReservedSpots(ParkingSpot parkingSpot, byte aCounterLimit, byte aParkingSpotCounter)
        {
            for (int i = 1; i < aCounterLimit; i++)
            {
                parkingSpot.Reserve(ParkingSpotArray[aParkingSpotCounter - i]);
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
            where parkingSpot3.AvailableSize < 3
            from parkingSpot1 in ParkingSpotArray
            where parkingSpot1.AvailableSize < 1
            select new { parkingSpot3, parkingSpot1 };

            foreach (var pair in ParkingSpotsNeedOptimization3and1)
            {
                ParkingSpot.JoinParkings3and1(pair);
            }

            var ParkingSpotsNeedOptimization2and2 =
            from parkingSpot3 in ParkingSpotArray
            where parkingSpot3.AvailableSize < 2
            from parkingSpot1 in ParkingSpotArray
            where parkingSpot1.AvailableSize < 2
            select new { parkingSpot3, parkingSpot1 };

            foreach (var pair in ParkingSpotsNeedOptimization2and2) // UNION??? NO DUPLICATES
            {
                ParkingSpot.JoinParkings3and1(pair);
            }
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