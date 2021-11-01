using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using System;

namespace PragueParking2._0
{
    partial class ParkingHouse
    {
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
                    switch ((VehicleType)jObject["Type"].Value<int>())
                    {
                        case VehicleType.Car:
                            return new Car();

                        case VehicleType.Mc:
                            return new Mc();

                        case VehicleType.Bike:
                            return new Bike();

                        case VehicleType.Bus:
                            return new Bus();
                    }
                    return null;
                }
            }
        }
    }
}


