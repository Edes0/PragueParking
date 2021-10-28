using Newtonsoft.Json;
using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PragueParking2._0
{
    class ParkingHouse
    {
        public byte Size { get; } = (byte)Sizes.ParkingHouse;
        public ParkingSpot[] parkingSpotArray = new ParkingSpot[(int)Sizes.ParkingHouse];

        public ParkingHouse()
        {
            for (int i = 1; i < Size + 1; i++)
            {
                parkingSpotArray[i] = new ParkingSpot((byte)i);
            }
            JsonSync();
        }
        public void Menu()
        {
            Console.WriteLine("Enter registration plate");
            Car car = new Car(Console.ReadLine());
            GetFreeParking(car);
            ParkingSpot.CheckVehicleSize(car); // VAD I HELA FITTAN JÄVLA HORA JÄVLA FITTKUK JÄVLA SLYNMAMMAKNULLARE
        }
        public byte GetFreeParking(Vehicle aVehicle)
        {
            foreach (ParkingSpot parkingSpot in parkingSpotArray)
            {
                if (parkingSpot.AvailableSize >= aVehicle.Size)
                {
                    return parkingSpot.Number;
                }
            }
            return 0;
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

        public void ParkVehicle()
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
        public void JsonSync()
        {
            for (int i = 1; i < Size + 1; i++)
            {
                string parkingSpotJson = JsonConvert.SerializeObject(parkingSpotArray[i]);
                File.WriteAllText(@"C:\Users\sjogr_v8becyz\source\repos\PragueParking\PragueParking2.0\Datafiles\Datafile.json", parkingSpotJson);
                parkingSpotJson = File.ReadAllText(@"C:\Users\sjogr_v8becyz\source\repos\PragueParking\PragueParking2.0\Datafiles\Datafile.json");
                ParkingSpot parkingspot = JsonConvert.DeserializeObject<ParkingSpot>(parkingSpotJson);
                parkingSpotArray[i] = parkingspot;
            }
        }
    }
}
