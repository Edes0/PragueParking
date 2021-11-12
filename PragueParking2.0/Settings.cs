using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings
    {
        [JsonProperty]
        public static byte SizeBike { get; set; }
        [JsonProperty]
        public static byte SizeMc { get; set; }
        [JsonProperty]
        public static byte SizeCar { get; set; }
        [JsonProperty]
        public static byte SizeBus { get; set; }
        [JsonProperty]
        public static byte SizeParkingSpot { get; set; }
        [JsonProperty]
        public static byte SizeParkingHouse { get; set; }
        [JsonProperty]
        public static byte SizeParkingHouseHighRoof { get; set; }
        [JsonProperty]
        public static byte SizeTableColumns { get; set; }

        [JsonProperty]
        public static byte HightBike { get; set; }
        [JsonProperty]
        public static byte HightMc { get; set; }
        [JsonProperty]
        public static byte HightCar { get; set; }
        [JsonProperty]
        public static byte HightBus { get; set; }
        [JsonProperty]
        public static byte HightParkingLow { get; set; }
        [JsonProperty]
        public static byte HightParkingHigh { get; set; }


        public Settings()
        {

        }
    }
}
