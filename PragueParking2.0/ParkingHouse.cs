using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using Spectre.Console;
using Spectre.Console.Rendering;
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
        private ParkingSpot[] ParkingSpotArray { get; set; } = new ParkingSpot[Settings.SizeParkingHouse];
        internal ParkingHouse()
        {
            for (int i = 0; i < Size; i++)
                ParkingSpotArray[i] = new ParkingSpot((byte)i, HighRoof);
        }
        /// <summary>
        /// Chores in menu chores. Runs everytime program loops back to main menu
        /// </summary>
        internal void Chores()
        {
            if (ParkingSpotArray == null) JsonDatafilWrite(ParkingSpotArray);

            JsonDatafilRead();
            JsonDatafilWrite(ParkingSpotArray);
        }
        /// <summary>
        /// Looks for parking spot to park vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
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
                // Parks vehicle at parkings with lower Height if possible.
                IEnumerable<ParkingSpot> parkingsWithLowerHeight =
                        from parkingSpot in ParkingSpotArray
                        where parkingSpot.Height < Settings.HeightParkingHigh
                        select parkingSpot;

                foreach (ParkingSpot parkingSpot in parkingsWithLowerHeight)
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
            }
            return false;
        }
        /// <summary>
        /// Checks if parking spot is available for big vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="newParkingSpotIndex"></param>
        /// <param name="aCounterLimit"></param>
        /// <returns></returns>
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

                    byte counterLimit = CounterLimitCalculator(vehicle.Size, aParkingSpot.Size);

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
        /// <summary>
        /// Checks if parking spot is available for big vehicle at specfic spot
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="newParkingSpot"></param>
        /// <param name="aCounterLimit"></param>
        /// <returns></returns>
        private bool BigParkingAvailableSpecfic(Vehicle vehicle, ParkingSpot newParkingSpot, out byte aCounterLimit)
        {
            byte counterLimit = CounterLimitCalculator(vehicle.Size, newParkingSpot.Size);

            byte parkingsInARowCounter = 0;

            for (int i = newParkingSpot.Number; i < newParkingSpot.Number + counterLimit; i++)
            {
                if (ParkingSpotArray[i].ParkingSpotAvailable(vehicle))
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
        /// <summary>
        /// Calculates how many times BigParkingAvailable should count. Depends on how big vehicle is. Calculates how many spots to reserve for big vehicle.
        /// </summary>
        /// <param name="aVehicleSize"></param>
        /// <param name="aParkingSpotSize"></param>
        /// <returns></returns>
        private byte CounterLimitCalculator(byte aVehicleSize, byte aParkingSpotSize)
        {
            decimal aCounterLimit = aVehicleSize / aParkingSpotSize;
            return (byte)Math.Ceiling(aCounterLimit);
        }
        /// <summary>
        /// Moves vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="aNewParkingSpot"></param>
        /// <param name="oldParkingSpot"></param>
        /// <returns></returns>
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
                return false;
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
            }
            return false;
        }
        /// <summary>
        /// Search for vehicle
        /// </summary>
        /// <param name="registrationNumber"></param>
        /// <param name="aVehicle"></param>
        /// <param name="aParkingSpot"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Remove all parkings spots. loops through them
        /// </summary>
        internal void RemoveAllParkings()
        {
            foreach (ParkingSpot parkingSpot in ParkingSpotArray) parkingSpot.Clear();
            JsonDatafilWrite(ParkingSpotArray);
        }
        /// <summary>
        /// Removes vehicle
        /// </summary>
        /// <param name="aRegNum"></param>
        /// <returns></returns>
        internal bool RemoveVehicle(string aRegNum)
        {
            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
            {
                if (parkingSpot.VehicleExist(aRegNum, out Vehicle vehicle))
                {
                    parkingSpot.RemoveVehicle(vehicle);

                    byte counterLimit = CounterLimitCalculator(vehicle.Size, parkingSpot.Size);
                    ClearReservedSpots(parkingSpot, counterLimit);

                    JsonDatafilWrite(ParkingSpotArray);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Clear reserved spots efter removing big vehicle
        /// </summary>
        /// <param name="parkingSpot"></param>
        /// <param name="aCounterLimit"></param>
        private void ClearReservedSpots(ParkingSpot parkingSpot, byte aCounterLimit)
        {
            for (int i = 1; i < aCounterLimit; i++)
                ParkingSpotArray[parkingSpot.Number + aCounterLimit - i].Clear();
        }
        /// <summary>
        /// Add reserved spots after adding big vehicle
        /// </summary>
        /// <param name="parkingSpot"></param>
        /// <param name="aCounterLimit"></param>
        private void AddReservedSpots(ParkingSpot parkingSpot, byte aCounterLimit)
        {
            for (int i = 1; i < aCounterLimit; i++)
                parkingSpot.Reserve(ParkingSpotArray[parkingSpot.Number + i]);
        }
        /// <summary>
        /// Gets parkings grid
        /// </summary>
        /// <returns></returns>
        internal List<Panel> GetParkingGrid()
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
            return box;
        }
        /// <summary>
        /// Reads ParkingSpotArray from Json 
        /// </summary>
        private void JsonDatafilRead()
        {
            string path = @"../../../Datafiles/Datafile.json";

            string ParkingSpotArrayJson = File.ReadAllText(path);

            ParkingSpot[] tempArray = JsonConvert.DeserializeObject<ParkingSpot[]>(ParkingSpotArrayJson, new VehicleConverter());

            if (tempArray.Length >= ParkingSpotArray.Length)
                for (int i = 0; i < ParkingSpotArray.Length; i++)
                {
                    //tempArray[i].Size = Settings.SizeParkingSpot;
                    //tempArray[i].AvailableSize = Settings.SizeParkingSpot;
                    ParkingSpotArray[i] = tempArray[i];
                }


            if (tempArray.Length < ParkingSpotArray.Length)
                for (int i = 0; i < tempArray.Length; i++)
                {
                    //tempArray[i].Size = Settings.SizeParkingSpot;
                    //tempArray[i].AvailableSize = Settings.SizeParkingSpot;
                    ParkingSpotArray[i] = tempArray[i];
                }



        }
        /// <summary>
        /// Write ParkingSpotArray to Json
        /// </summary>
        /// <param name="aParkingSpotArray"></param>
        private void JsonDatafilWrite(ParkingSpot[] aParkingSpotArray)
        {
            string path = @"../../../Datafiles/Datafile.json";

            string ParkingSpotArrayJson = JsonConvert.SerializeObject(aParkingSpotArray, Formatting.Indented, new VehicleConverter());

            File.WriteAllText(path, ParkingSpotArrayJson);
        }
        /// <summary>
        /// Update settings and write ParkingSpotArray to Json
        /// </summary>
        internal void Update()
        {
            Console.Clear();
            Settings.JsonSettingsRead();
            JsonDatafilWrite(ParkingSpotArray);
        }
        /// <summary>
        /// Optimizes all parkings
        /// </summary>
        public void Optimize() //Kan möjligen dela upp dessa eftersom det kan bli stora flyttar på samma gång.
        {
            while (OptimizeSize3(out string printVehicleMoved)) Console.WriteLine(printVehicleMoved);
            while (OptimizeSize2(out string printVehicleMoved)) Console.WriteLine(printVehicleMoved);
            while (OptimizeCars(out string printVehicleMoved)) Console.WriteLine(printVehicleMoved);
        }
        /// <summary>
        /// Optimizes parkings with available space 3 to 1
        /// </summary>
        /// <param name="printVehicleMoved"></param>
        /// <returns></returns>
        private bool OptimizeSize3(out string printVehicleMoved)
        {
            foreach (ParkingSpot parkingSpotOut in ParkingSpotArray)

                if (parkingSpotOut.AvailableSize == 3)

                    foreach (ParkingSpot parkingSpotIn in ParkingSpotArray)

                        if (parkingSpotIn.AvailableSize == 1)
                        {
                            Vehicle vehicle = parkingSpotOut.VehicleList[0];

                            MoveVehicle(vehicle, parkingSpotIn.Number, parkingSpotOut);

                            printVehicleMoved = $"{vehicle.StringType} ({vehicle.RegNum}) moved: {parkingSpotOut.Number} -- > {parkingSpotIn.Number}";

                            return true;
                        }
            printVehicleMoved = null;
            return false;
        }
        /// <summary>
        /// Optimizes parkings with available space 2 to 2
        /// </summary>
        /// <param name="printVehicleMoved"></param>
        /// <returns></returns>
        private bool OptimizeSize2(out string printVehicleMoved)
        {
            foreach (ParkingSpot parkingSpotOut in ParkingSpotArray)

                if (parkingSpotOut.AvailableSize == 2)

                    foreach (ParkingSpot parkingSpotIn in ParkingSpotArray)

                        if (parkingSpotIn.AvailableSize == 2 && parkingSpotIn.Number != parkingSpotOut.Number)
                        {
                            if (parkingSpotOut.VehicleList.Count == 1)
                            {
                                Vehicle vehicle = parkingSpotOut.VehicleList[0];

                                MoveVehicle(vehicle, parkingSpotIn.Number, parkingSpotOut);

                                printVehicleMoved = $"{vehicle.StringType} ({vehicle.RegNum}) moved: {parkingSpotOut.Number} -- > {parkingSpotIn.Number}";

                                return true;
                            }

                            Vehicle vehicle1 = parkingSpotOut.VehicleList[0];
                            Vehicle vehicle2 = parkingSpotOut.VehicleList[1];

                            MoveVehicle(vehicle2, parkingSpotIn.Number, parkingSpotOut);
                            MoveVehicle(vehicle1, parkingSpotIn.Number, parkingSpotOut);

                            printVehicleMoved = $"{vehicle1.StringType} ({vehicle1.RegNum}) moved: {parkingSpotOut.Number} -- > {parkingSpotIn.Number}" +
                                $"{vehicle1.StringType} ({vehicle1.RegNum}) moved: {parkingSpotOut.Number} -- > {parkingSpotIn.Number}";

                            return true;
                        }
            printVehicleMoved = null;
            return false;
        }
        /// <summary>
        /// Optimizes cars
        /// </summary>
        /// <param name="printVehicleMoved"></param>
        /// <returns></returns>
        private bool OptimizeCars(out string printVehicleMoved)
        {
            foreach (ParkingSpot parkingSpotOut in ParkingSpotArray)
            {
                if (parkingSpotOut.IsFull() && parkingSpotOut.IsHigh() && !parkingSpotOut.IsReserved() && parkingSpotOut.VehicleList[0].IsSmall())

                    foreach (ParkingSpot parkingSpotIn in ParkingSpotArray)

                        if (parkingSpotIn.IsFree() && parkingSpotIn.IsLow())
                        {
                            Vehicle vehicle = parkingSpotOut.VehicleList[0];

                            MoveVehicle(vehicle, parkingSpotIn.Number, parkingSpotOut);

                            printVehicleMoved = $"{vehicle.StringType} ({vehicle.RegNum}) moved: {parkingSpotOut.Number} -- > {parkingSpotIn.Number}";
                            return true;
                        }
            }
            printVehicleMoved = null;
            return false;
        }
        /// <summary>
        /// Checks if ParkingSpotArray is possible to shrink without losing vehicles
        /// </summary>
        /// <param name="newSize"></param>
        /// <returns></returns>
        internal bool PossibleToShrinkSize(byte newSize)
        {
            for (int i = newSize; i < ParkingSpotArray.Length; i++)
                if (!ParkingSpotArray[i].IsFree()) return false;
            return true;
        }
        /// <summary>
        /// Checks if ParkingSpotArray is possible to lower without lowering under vehicle height
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="highRoof"></param>
        /// <returns></returns>
        internal bool PossibleToLowerHighRoof(byte newValue, byte highRoof)
        {
            for (int i = newValue; i < ParkingSpotArray.Length; i++)
                foreach (ParkingSpot parkingSpot in ParkingSpotArray)
                    if (ParkingSpotArray[i].HaveHighVehicle() || ParkingSpotArray[i].IsReserved()) return false;
            return true;
        }
        /// <summary>
        /// Gets ticket list
        /// </summary>
        /// <returns></returns>
        internal Table GetTicketList()
        {
            Table table = new Table()
           .Border(TableBorder.DoubleEdge)
           .Title("[yellow]Ticket List[/]")
           .AddColumn(new TableColumn("P-spot"))
           .AddColumn(new TableColumn("Type"))
           .AddColumn(new TableColumn("Reg number"))
           .AddColumn(new TableColumn("Arrive time").Footer("Total:"))
           .AddColumn(new TableColumn("Price").Footer(GetTotalPrice() + " CZK"));

            GetNotFreeParkings(out IEnumerable<ParkingSpot> GetNotFreeParkingSpots);

            foreach (ParkingSpot parkingSpot in GetNotFreeParkingSpots)
                table.AddRow(parkingSpot.GetTicketInfo().Split(", "));

            return table;
        }
        /// <summary>
        /// Gets total price
        /// </summary>
        /// <returns></returns>
        private string GetTotalPrice()
        {
            decimal totalPrice = 0;

            var notFreeParkingSpots =
             from parkingSpot in ParkingSpotArray
             where !parkingSpot.IsFree()
             select parkingSpot;

            foreach (var parkingSpot in notFreeParkingSpots)
                totalPrice += parkingSpot.GetVehiclePrices();

            return $"{totalPrice:0.00}";
        }
        /// <summary>
        /// Gets parkings that are not free
        /// </summary>
        /// <param name="notFreeParkingSpots"></param>
        private void GetNotFreeParkings(out IEnumerable<ParkingSpot> notFreeParkingSpots)
        {
            notFreeParkingSpots =
             from parkingSpot in ParkingSpotArray
             where !parkingSpot.IsFree() && !parkingSpot.IsReserved()
             select parkingSpot;
        }
        /// <summary>
        /// Add preset of vehicles for tests
        /// </summary>
        internal void AddSomeVehicles()
        {
            Mc mc = new Mc("TEST0");
            ParkingSpotArray[50].AddVehicle(mc);

            Mc mc1 = new Mc("TEST1");
            ParkingSpotArray[51].AddVehicle(mc1);

            Mc mc2 = new Mc("TEST2");
            ParkingSpotArray[52].AddVehicle(mc2);

            Mc mc3 = new Mc("TEST3");
            ParkingSpotArray[53].AddVehicle(mc3);

            Mc mc4 = new Mc("TEST4");
            ParkingSpotArray[54].AddVehicle(mc4);

            Mc mc5 = new Mc("TEST5");
            ParkingSpotArray[55].AddVehicle(mc5);

            Mc mc6 = new Mc("TEST6");
            ParkingSpotArray[56].AddVehicle(mc6);

            Mc mc7 = new Mc("TEST7");
            ParkingSpotArray[57].AddVehicle(mc7);

            Mc mc8 = new Mc("TEST8");
            ParkingSpotArray[58].AddVehicle(mc8);

            Mc mc9 = new Mc("TEST9");
            ParkingSpotArray[59].AddVehicle(mc9);

            Bike bik0 = new Bike("TEST01");
            ParkingSpotArray[50].AddVehicle(bik0);

            Bike bik1 = new Bike("TEST01");
            ParkingSpotArray[51].AddVehicle(bik1);

            Bike bik2 = new Bike("TEST02");
            ParkingSpotArray[52].AddVehicle(bik2);

            Bike bik3 = new Bike("TEST03");
            ParkingSpotArray[53].AddVehicle(bik3);

            Bike bik4 = new Bike("TEST04");
            ParkingSpotArray[54].AddVehicle(bik4);

            Bike bik5 = new Bike("TEST05");
            ParkingSpotArray[55].AddVehicle(bik5);

            Bike bik6 = new Bike("TEST06");
            ParkingSpotArray[56].AddVehicle(bik6);

            Bike bik7 = new Bike("TEST07");
            ParkingSpotArray[57].AddVehicle(bik7);

            Bike bik8 = new Bike("TEST08");
            ParkingSpotArray[58].AddVehicle(bik8);

            Bike bik9 = new Bike("TEST09");
            ParkingSpotArray[59].AddVehicle(bik9);

            Bike bik01 = new Bike("TEST0");
            ParkingSpotArray[60].AddVehicle(bik01);

            Bike bik02 = new Bike("TEST1");
            ParkingSpotArray[61].AddVehicle(bik02);

            Bike bik03 = new Bike("TEST2");
            ParkingSpotArray[62].AddVehicle(bik03);

            Bike bik04 = new Bike("TEST3");
            ParkingSpotArray[63].AddVehicle(bik04);

            Bike bik05 = new Bike("TEST4");
            ParkingSpotArray[64].AddVehicle(bik05);

            Bike bik06 = new Bike("TEST5");
            ParkingSpotArray[65].AddVehicle(bik06);

            Bike bik07 = new Bike("TEST6");
            ParkingSpotArray[66].AddVehicle(bik07);

            Bike bik08 = new Bike("TEST7");
            ParkingSpotArray[67].AddVehicle(bik08);

            Bike bik09 = new Bike("TEST8");
            ParkingSpotArray[68].AddVehicle(bik09);

            Bike bik10 = new Bike("TEST9");
            ParkingSpotArray[69].AddVehicle(bik10);

            Mc mc20 = new Mc("TEST0");
            ParkingSpotArray[70].AddVehicle(mc20);

            Mc mc21 = new Mc("TEST1");
            ParkingSpotArray[71].AddVehicle(mc21);

            Mc mc22 = new Mc("TEST2");
            ParkingSpotArray[72].AddVehicle(mc22);

            Mc mc23 = new Mc("TEST3");
            ParkingSpotArray[73].AddVehicle(mc23);

            Mc mc24 = new Mc("TEST4");
            ParkingSpotArray[74].AddVehicle(mc24);

            Mc mc25 = new Mc("TEST5");
            ParkingSpotArray[75].AddVehicle(mc25);

            Mc mc26 = new Mc("TEST6");
            ParkingSpotArray[76].AddVehicle(mc26);

            Mc mc27 = new Mc("TEST7");
            ParkingSpotArray[77].AddVehicle(mc27);

            Mc mc28 = new Mc("TEST8");
            ParkingSpotArray[78].AddVehicle(mc28);

            Mc mc29 = new Mc("TEST9");
            ParkingSpotArray[79].AddVehicle(mc29);

            Mc mc30 = new Mc("TEST0");
            ParkingSpotArray[80].AddVehicle(mc30);

            Mc mc31 = new Mc("TEST1");
            ParkingSpotArray[81].AddVehicle(mc31);

            Mc mc32 = new Mc("TEST2");
            ParkingSpotArray[82].AddVehicle(mc32);

            Mc mc33 = new Mc("TEST3");
            ParkingSpotArray[83].AddVehicle(mc33);

            Mc mc34 = new Mc("TEST4");
            ParkingSpotArray[84].AddVehicle(mc34);

            Mc mc35 = new Mc("TEST5");
            ParkingSpotArray[85].AddVehicle(mc35);

            Mc mc36 = new Mc("TEST6");
            ParkingSpotArray[86].AddVehicle(mc36);

            Mc mc37 = new Mc("TEST7");
            ParkingSpotArray[87].AddVehicle(mc37);

            Mc mc38 = new Mc("TEST8");
            ParkingSpotArray[88].AddVehicle(mc38);

            Mc mc39 = new Mc("TEST9");
            ParkingSpotArray[89].AddVehicle(mc39);

            Car car = new Car("TEST9");
            ParkingSpotArray[1].AddVehicle(car);

            Car car1 = new Car("TEST9");
            ParkingSpotArray[2].AddVehicle(car1);

            Car car2 = new Car("TEST9");
            ParkingSpotArray[3].AddVehicle(car2);

            Car car3 = new Car("TEST9");
            ParkingSpotArray[4].AddVehicle(car3);

            Car car4 = new Car("TEST9");
            ParkingSpotArray[5].AddVehicle(car4);

            Car car5 = new Car("TEST9");
            ParkingSpotArray[6].AddVehicle(car5);

            Car car6 = new Car("TEST9");
            ParkingSpotArray[7].AddVehicle(car6);

            Car car7 = new Car("TEST9");
            ParkingSpotArray[8].AddVehicle(car7);

            Car car8 = new Car("TEST9");
            ParkingSpotArray[9].AddVehicle(car8);

            Car car9 = new Car("TEST9");
            ParkingSpotArray[10].AddVehicle(car9);

            Car car19 = new Car("TEST9");
            ParkingSpotArray[0].AddVehicle(car19);

            JsonDatafilWrite(ParkingSpotArray);
        }
        /// <summary>
        /// Checks if possible to shrink parkingspot size while checking with vehicles
        /// </summary>
        /// <param name="newSize"></param>
        /// <returns></returns>
        internal bool PossibleToShrinkParkingSpotSize(byte newSize)
        {
            foreach (ParkingSpot parkingSpot in ParkingSpotArray)
                if (!parkingSpot.PossibleToShrink(newSize)) return false;
            return true;
        }
        /// <summary>
        /// Changes availablesize after low or high parkingspot size
        /// </summary>
        /// <param name="difference"></param>
        internal void ChangeAvailableSizeAll(byte difference)
        {
            if (difference > 0)
                foreach (ParkingSpot parkingSpot in ParkingSpotArray)
                    parkingSpot.AvailableSize = (byte)(parkingSpot.AvailableSize - difference);

            else
                foreach (ParkingSpot parkingSpot in ParkingSpotArray)
                    parkingSpot.AvailableSize = (byte)(parkingSpot.AvailableSize + difference);

            JsonDatafilWrite(ParkingSpotArray);
        }
    }

}
