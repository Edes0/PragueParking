using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using static PragueParking2._0.ParkingHouse.JsonCreationConverter<PragueParking2._0.Vehicles.Vehicle>;

namespace PragueParking2._0
{
    class ParkingHouse
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

                    JsonSync(parkingSpotArray);

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

                        JsonSync(parkingSpotArray);
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
            string parkingSpotArrayJson = JsonConvert.SerializeObject(aParkingSpotArray, Formatting.Indented);
            string path = @"../../../Datafiles/Datafile.json";
            File.WriteAllText(path, parkingSpotArrayJson);

            parkingSpotArrayJson = File.ReadAllText(path);

            //parkingSpotArray = JsonConvert.DeserializeObject<ParkingSpot[]>(parkingSpotArrayJson);
            parkingSpotArray = JsonConvert.DeserializeObject<ParkingSpot[]>(parkingSpotArrayJson, new VehicleConverter());

        }
        public abstract class JsonCreationConverter<T> : JsonConverter
        {
            protected abstract T Create(Type objectType, JObject jObject);

            public override bool CanConvert(Type objectType)
            {
                return typeof(T) == objectType;
            }

            public override object ReadJson(JsonReader reader, Type objectType,
                object existingValue, JsonSerializer serializer)
            {
                try
                {
                    var jObject = JObject.Load(reader);
                    var target = Create(objectType, jObject);
                    serializer.Populate(jObject.CreateReader(), target);
                    return target;
                }
                catch (JsonReaderException)
                {
                    return null;
                }
            }
            public override void WriteJson(JsonWriter writer, object value,
                JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
            public class VehicleConverter : JsonCreationConverter<Vehicle>
            {
                protected override Vehicle Create(Type objectType, JObject jObject)
                {
                    switch ((Constants.VehicleType)jObject["Type"].Value<int>())
                    {
                        case Constants.VehicleType.Car:
                            return new Car();

                        case Constants.VehicleType.Mc:
                            return new Mc();

                        case Constants.VehicleType.Bike:
                            return new Bike();

                        case Constants.VehicleType.Bus:
                            return new Bus();
                    }
                    return null;
                }
            }
        }
    }
}


