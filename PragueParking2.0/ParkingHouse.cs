using Newtonsoft.Json;
using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PragueParking2._0
{
    class ParkingHouse
    {
        private byte HighRoof { get; } = (byte)Sizes.ParkingHouseHighRoof;
        private byte Size { get; } = (byte)Sizes.ParkingHouse;
        internal ParkingSpot[] parkingSpotArray { get; set; } = new ParkingSpot[(int)Sizes.ParkingHouse];
        public ParkingHouse()
        {
            for (int i = 0; i < Size; i++)
            {
                parkingSpotArray[i] = new ParkingSpot((byte)i, HighRoof);
            }
        }
        public bool ParkVehicle(Vehicle aVehicle)
        {
            foreach (ParkingSpot parkingSpot in parkingSpotArray)
            {
                if (parkingSpot.CheckVehicleParkingAvailable(aVehicle, parkingSpot))
                {
                    return true;
                }
            }
            return false;
        }
        public bool ParkBigVehicle(Vehicle aVehicle)
        {
            if (CheckBusParkingAvailable(aVehicle, out ParkingSpot aParkingSpot))
            {
                return true;
            }
            return false;
        }
        public bool CheckBusParkingAvailable(Vehicle aVehicle, out ParkingSpot parkingSpot)
        {
            byte counter = 0;

            foreach (ParkingSpot aParkingSpot in parkingSpotArray)
            {
                if (aParkingSpot.Hight > aVehicle.Hight && aParkingSpot.AvailableSize == aParkingSpot.Size)
                {
                    parkingSpot = aParkingSpot;
                    counter += 1;
                    CounterLimitCalculator(aVehicle.Size, aParkingSpot.Size, out decimal aCounterLimit);

                    if (counter == aCounterLimit)
                    {
                       // parkingSpotArray[parkingSpot.Number-aCounterLimit]
                        return true;
                    }
                }
                else
                {
                    counter = 0;
                }
            }
            parkingSpot = null;
            return false;
        }

        private void CounterLimitCalculator(byte aVehicleSize, byte aParkingSpotSize, out decimal aCounterLimit)
        {
            if (true)
            {
                aCounterLimit = aVehicleSize / aParkingSpotSize;
                aCounterLimit = Math.Ceiling(aCounterLimit);
            }
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

        public void Exit()
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
        public void JsonSync(ParkingSpot[] aParkingSpotArray)
        {
            //for (int i = 0; i < Size; i++)
            //{
            string parkingSpotArrayJson = JsonConvert.SerializeObject(aParkingSpotArray, Formatting.Indented);
            Console.WriteLine(parkingSpotArrayJson);
            //File.WriteAllText(@"../../../Datafiles/Datafile.json",parkingSpotArrayJson);
            //parkingSpotJson = File.ReadAllText(@"../../../Datafiles/Datafile.json");
            //ParkingSpot parkingspot = JsonConvert.DeserializeObject<ParkingSpot>(parkingSpotJson);
            //parkingSpotArray[i] = parkingspot;
            //}
        }
    }
}
