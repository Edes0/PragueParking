using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static PragueParking2._0.ParkingHouse.JsonCreationConverter<PragueParking2._0.Vehicles.Vehicle>;

namespace PragueParking2._0
{
    partial class ParkingHouse
    {
        internal byte HighRoof { get; set; } = Settings.SizeParkingHouseHighRoof;
        private byte Size { get; set; } = Settings.SizeParkingHouse;
        ParkingSpot[] ParkingSpotArray { get; set; }
        internal ParkingHouse()
        {
            ParkingSpot[] ParkingSpotArray = new ParkingSpot[Size];

            for (int i = 0; i < Size; i++)
            {
                ParkingSpotArray[i] = new ParkingSpot((byte)i, HighRoof);
            }
        }
        internal void AddSomeVehicles()
        {
            Car car5 = new Car("CAGR257123");
            ParkingSpotArray[1].AddVehicle(car5);

            Car car6 = new Car("CAFR257123");
            ParkingSpotArray[2].AddVehicle(car6);

            Car car54 = new Car("CSAR257123");
            ParkingSpotArray[3].AddVehicle(car54);

            Car car23 = new Car("CAWR257123");
            ParkingSpotArray[4].AddVehicle(car23);

            Car car75 = new Car("CAHR257123");
            ParkingSpotArray[5].AddVehicle(car75);

            Car car236 = new Car("CQAR257123");
            ParkingSpotArray[6].AddVehicle(car236);

            Car car1 = new Car("CASGR257123");
            ParkingSpotArray[7].AddVehicle(car1);

            Car car2 = new Car("CWQE257123");
            ParkingSpotArray[8].AddVehicle(car2);

            Bike bike = new Bike("BIKE153623");
            ParkingSpotArray[51].AddVehicle(bike);

            Mc mc = new Mc("MC12323");
            ParkingSpotArray[51].AddVehicle(mc);

            Bike bike1 = new Bike("BIKE4222");
            ParkingSpotArray[52].AddVehicle(bike1);

            Bike bike2 = new Bike("BIKE3563");
            ParkingSpotArray[53].AddVehicle(bike2);

            Mc mc1 = new Mc("BIKE12962");
            ParkingSpotArray[53].AddVehicle(mc1);

            Bike bike3 = new Bike("BIKE3201");
            ParkingSpotArray[54].AddVehicle(bike3);

            Bike bike4 = new Bike("BIKE1924");
            ParkingSpotArray[55].AddVehicle(bike4);

            Mc mc2 = new Mc("MC1282");
            ParkingSpotArray[56].AddVehicle(mc1);

            Mc mc3 = new Mc("MC1722");
            ParkingSpotArray[57].AddVehicle(mc1);

            Mc mc4 = new Mc("MC1622");
            ParkingSpotArray[58].AddVehicle(mc1);

            Mc mc5 = new Mc("MMC1522");
            ParkingSpotArray[59].AddVehicle(mc1);

            Mc mc6 = new Mc("mCC1242");
            ParkingSpotArray[61].AddVehicle(mc1);

            Mc mc7 = new Mc("BMCC11222");
            ParkingSpotArray[64].AddVehicle(mc1);

            Mc mc8 = new Mc("MCCKE1222");
            ParkingSpotArray[66].AddVehicle(mc1);

            JsonDatafilWrite(ParkingSpotArray);
        }
        internal bool ParkVehicle(Vehicle vehicle)
        {        
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

                    JsonDatafilWrite(ParkingSpotArray);

                    return true;
                }
            }
            if (vehicle.IsTiny() || vehicle.IsSmall())
            {
                // Parks vehicle at parkings with lower hight if possible.
                IEnumerable<ParkingSpot> parkingsWithLowerHight =
                        from parkingSpot in ParkingSpotArray
                        where parkingSpot.Hight < Settings.HightParkingHigh
                        select parkingSpot;

                foreach (ParkingSpot parkingSpot in parkingsWithLowerHight)
                {
                    if (parkingSpot.ParkingSpotAvailable(vehicle))
                    {
                        parkingSpot.AddVehicle(vehicle);

                        JsonDatafilWrite(ParkingSpotArray);

                        return true;
                    }
                }
                // Parks vehicle at any parking.
                foreach (ParkingSpot anyParkingSpot in ParkingSpotArray)
                {
                    if (anyParkingSpot.ParkingSpotAvailable(vehicle))
                    {
                        anyParkingSpot.AddVehicle(vehicle);

                        JsonDatafilWrite(ParkingSpotArray);

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

                    JsonDatafilWrite(ParkingSpotArray);

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
        internal bool MoveVehicle(Vehicle vehicle, byte aNewParkingSpot, ParkingSpot oldParkingSpot) // Could still optimize big vehicle move. Remove first then add if can't move
        {
            ParkingSpot newParkingSpot = ParkingSpotArray[aNewParkingSpot];

            if (vehicle.IsSmall() || vehicle.IsTiny())
            {
                if (newParkingSpot.ParkingSpotAvailable(vehicle))
                {
                    oldParkingSpot.RemoveVehicle(vehicle);
                    newParkingSpot.AddVehicle(vehicle);

                    JsonDatafilWrite(ParkingSpotArray);

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

                    JsonDatafilWrite(ParkingSpotArray);

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
            JsonDatafilWrite(ParkingSpotArray);
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

                    JsonDatafilWrite(ParkingSpotArray);
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
        internal void JsonDatafilRead()
        {
            string path = @"../../../Datafiles/Datafile.json";

            string ParkingSpotArrayJson = File.ReadAllText(path);

            ParkingSpotArray = JsonConvert.DeserializeObject<ParkingSpot[]>(ParkingSpotArrayJson, new VehicleConverter());

        }
        internal void JsonDatafilWrite(ParkingSpot[] aParkingSpotArray)
        {
            string path = @"../../../Datafiles/Datafile.json";

            string ParkingSpotArrayJson = JsonConvert.SerializeObject(aParkingSpotArray, Formatting.Indented, new VehicleConverter());

            File.WriteAllText(path, ParkingSpotArrayJson);
        }
        internal void JsonSettingsRead(Settings settings)
        {
            string path = @"../../../Datafiles/Settings.json";

            string SettingsJson = File.ReadAllText(path);

            settings = JsonConvert.DeserializeObject<Settings>(SettingsJson);
        }
        internal void JsonSettingsWrite(Settings settings)
        {
            string path = @"../../../Datafiles/Settings.json";

            string settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);

            File.WriteAllText(path, settingsJson);
        }
        public void Optimize() //Kan möjligen dela upp dessa eftersom det kan bli stora flyttar på samma gång.
        {
            while (OptimizeSize3())
            {
            }
            while (OptimizeSize2())
            {
            }
            //while (OptimizeCars()) Kraschar av någon anledning.
            //{
            //}
        }
        private bool OptimizeSize3()
        {
            foreach (ParkingSpot parkingSpotOut in ParkingSpotArray)
            {
                if (parkingSpotOut.AvailableSize == 3)
                {
                    foreach (ParkingSpot parkingSpotIn in ParkingSpotArray)
                    {
                        if (parkingSpotIn.AvailableSize == 1)
                        {
                            Vehicle vehicle = parkingSpotOut.VehicleList[0];

                            MoveVehicle(vehicle, parkingSpotIn.Number, parkingSpotOut);

                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private bool OptimizeSize2()
        {
            foreach (ParkingSpot parkingSpotOut in ParkingSpotArray)
            {
                if (parkingSpotOut.AvailableSize == 2)
                {
                    foreach (ParkingSpot parkingSpotIn in ParkingSpotArray)
                    {
                        if (parkingSpotIn.AvailableSize == 2 && parkingSpotIn.Number != parkingSpotOut.Number)
                        {
                            if (parkingSpotOut.VehicleList.Count == 1)
                            {
                                Vehicle vehicle = parkingSpotOut.VehicleList[0];

                                MoveVehicle(vehicle, parkingSpotIn.Number, parkingSpotOut);
                                return true;
                            }
                            else
                            {
                                Vehicle vehicle = parkingSpotOut.VehicleList[0];
                                Vehicle vehicle1 = parkingSpotOut.VehicleList[1];

                                MoveVehicle(vehicle, parkingSpotIn.Number, parkingSpotOut);
                                MoveVehicle(vehicle1, parkingSpotIn.Number, parkingSpotOut);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private bool OptimizeCars()
        {
            foreach (ParkingSpot parkingSpotOut in ParkingSpotArray)
            {
                if (parkingSpotOut.IsFull() && parkingSpotOut.VehicleList[0].StringType == "Car")
                {
                    foreach (ParkingSpot parkingSpotIn in ParkingSpotArray)
                    {
                        if (parkingSpotIn.IsFree() && parkingSpotIn.IsLow(HighRoof))
                        {
                            Vehicle vehicle = parkingSpotOut.VehicleList[0];

                            MoveVehicle(vehicle, parkingSpotIn.Number, parkingSpotOut);

                            return true;
                        }
                    }
                }
            }
            return false;
        } // Gör så att det kraschar. Förstår inte varför denna kraschar med inte de andra optimize som i princip är samma.
        public void UpdateTickets()
        {
            throw new System.NotImplementedException();
        }// Behövs den? Visa tickets kanske
        public void CalculatePrice()
        {
            throw new System.NotImplementedException();
        }// Behövs den?

    }
}
