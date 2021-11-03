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
        // Checks if a small vehicle can park. Checks all parkings spots with lower roof first.
        internal bool SmallParkingAvailable(Vehicle vehicle) // BUGGAR
        {

            byte hight = (byte)Hights.ParkingHigh;

            // Parks vehicle at parkings with lower hight if possible.
            IEnumerable<ParkingSpot> parkingsWithLowerHight =
                    from parkingSpot in ParkingSpotArray
                    where parkingSpot.Hight < hight
                    select parkingSpot;

            foreach (ParkingSpot parkingSpot in parkingsWithLowerHight)
            {
                if (parkingSpot.ParkingSpotAvailable(parkingSpot, vehicle))
                {
                    parkingSpot.AddVehicle(vehicle, parkingSpot);

                    JsonWrite(ParkingSpotArray);

                    return true;
                }
            }

            // Parks vehicle at parkings with matching available size if possible.
            IEnumerable<ParkingSpot> parkingsWithMatchingAvailableSize =
                    from parkingSpot in ParkingSpotArray
                    where parkingSpot.AvailableSize < vehicle.Size
                    select parkingSpot;

            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
            {
                if (parkingSpot.ParkingSpotAvailable(parkingSpot, vehicle))
                {
                    parkingSpot.AddVehicle(vehicle, parkingSpot);

                    JsonWrite(ParkingSpotArray);

                    return true;
                }
            }

            // Parks vehicle at any parking.
            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
            {
                if (parkingSpot.ParkingSpotAvailable(parkingSpot, vehicle))
                {
                    parkingSpot.AddVehicle(vehicle, parkingSpot);

                    JsonWrite(ParkingSpotArray);

                    return true;
                }
            }


            return false;
        }
        // Checks if a big vehicle can park.
        internal bool BigParkingAvailable(Vehicle vehicle)
        {
            if (ParkingsInARowCalculator(vehicle, out byte parkingSpotCounter, out decimal counterLimit, out byte parkingsInARow))
            {
                ParkingSpot parkingSpot = ParkingSpotArray[parkingSpotCounter - (byte)counterLimit];

                parkingSpot.AddVehicle(vehicle, parkingSpot);

                AddReservedSpots(parkingSpot, (byte)counterLimit, parkingSpotCounter);

                JsonWrite(ParkingSpotArray);

                return true;
            }
            return false;
        }
        private bool ParkingsInARowCalculator(Vehicle vehicle, out byte aParkingSpotCounter, out decimal aCounterLimit, out byte aParkingsInARow) // OUT decimal från denna???? Kvar eller ej?
        {
            byte parkingsInARowCounter = 0;
            byte parkingSpotCounter = 0;

            foreach (ParkingSpot aParkingSpot in ParkingSpotArray)
            {
                parkingSpotCounter += 1;

                if (aParkingSpot.BigParkingSpotAvailable(aParkingSpot, vehicle))
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
        private void CounterLimitCalculator(byte aVehicleSize, byte aParkingSpotSize, out byte counterLimit)
        {
            decimal aCounterLimit = aVehicleSize / aParkingSpotSize;
            aCounterLimit = Math.Ceiling(aCounterLimit);

            counterLimit = (byte)aCounterLimit;
        }
        //internal bool MoveVehiclePossible(Vehicle vehicle, byte aNewParkingSpot, ParkingSpot oldParkingSpot) // Ska fixa två parkingersplatser  
        //{
        //    ParkingSpot newParkingSpot = ParkingSpotArray[aNewParkingSpot];

        //        if (vehicle.IsSmallVehicle(vehicle))
        //        {
        //        oldParkingSpot.RemoveVehicle(vehicle, oldParkingSpot);
        //        newParkingSpot.AddVehicle(vehicle, newParkingSpot);

        //        }
        //        else if (vehicle.IsBigVehicle(vehicle))
        //        {
        //            byte parkingsInARowCounter = 0;
        //            byte parkingSpotCounter = 0;

        //            parkingSpotCounter += 1;

        //            if (parkingSpot.BigParkingSpotAvailable(parkingSpot, vehicle))
        //            {
        //                parkingsInARowCounter += 1;

        //                CounterLimitCalculator(vehicle.Size, parkingSpot.Size, out decimal counterLimit);

        //                if (parkingsInARowCounter == counterLimit)
        //                {
        //                    byte reservedSpots = (byte)(vehicle.Size / parkingSpot.Size);

        //                    targetParkingSpot.AddVehicle(vehicle, targetParkingSpot);
        //                    AddReservedSpots(vehicle, targetParkingSpot);
        //                    parkingSpot.RemoveVehicle(vehicle, parkingSpot);
        //                    //parkingSpot.ClearReservedSpots(parkingSpot, reservedSpots, ParkingSpotArray);

        //                    JsonWrite(ParkingSpotArray);

        //                    return true;
        //                }
        //            }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Error: Vehicle is neither big or small.");
        //        return false;
        //    }
        //}
        internal bool VehicleExistInParkingHouse(string registrationNumber, out Vehicle aVehicle, out ParkingSpot aParkingSpot)
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
                parkingSpot.Clear(parkingSpot);
            }
            JsonWrite(ParkingSpotArray);
        }
        internal bool RemoveVehicle(string aRegNum)  // CANT REMOVE BIG VEHICLE. FIX TOMORROW :D:D:D:D:D:D
        {
            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
            {
                if (parkingSpot.VehicleExist(aRegNum, out Vehicle vehicle))
                {
                    if (vehicle.IsSmallVehicle(vehicle))
                    {
                        parkingSpot.RemoveVehicle(vehicle, parkingSpot);
                    }
                    else if (vehicle.IsBigVehicle(vehicle))
                    {
                        if (ParkingsInARowCalculator(vehicle, out byte parkingSpotCounter, out decimal counterLimit, out byte parkingSpotsInaRow))
                        {
                            parkingSpot.RemoveVehicle(vehicle, parkingSpot);
                            ClearReservedSpots(parkingSpot, (byte)counterLimit, parkingSpotCounter);
                        }
                    }
                    JsonWrite(ParkingSpotArray);
                    return true;
                }
            }
            return false;
        }
        private void ClearReservedSpots(ParkingSpot parkingSpot, byte aCounterLimit, byte aParkingSpotCounter) // Parking spot.. Linq efter specifika?
        {
            for (int i = 1; i < aCounterLimit; i++)
            {
                parkingSpot.Clear(ParkingSpotArray[aParkingSpotCounter - i]);
            }
        }
        private void AddReservedSpots(ParkingSpot parkingSpot, byte aCounterLimit, byte aParkingSpotCounter)// Parking spot.. Linq efter specifika?
        {
            for (int i = 1; i < (aCounterLimit); i++)
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
            Console.Clear();

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
        public void Optimize()
        {
            throw new System.NotImplementedException();
        }// Not done
        public void SearchVehicle()
        {
            throw new System.NotImplementedException();
        }// Not done
        public void UpdateTickets()
        {
            throw new System.NotImplementedException();
        }// Not done
        public void CalculatePrice()
        {
            throw new System.NotImplementedException();
        }// Not done
        public void Prints()
        {
            throw new System.NotImplementedException();
        }
    }
}