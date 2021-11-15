using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        [JsonProperty]
        public static byte PriceCar { get; set; }
        [JsonProperty]
        public static byte PriceMc { get; set; }
        [JsonProperty]
        public static byte PriceBike { get; set; }
        [JsonProperty]
        public static byte PriceBus { get; set; }
        [JsonProperty]
        public static double PriceFree { get; set; }

        public Settings()
        {

        }
        internal static void JsonSettingsRead(Settings settings)
        {
            string path = @"../../../Datafiles/Settings.json";

            string SettingsJson = File.ReadAllText(path);

            settings = JsonConvert.DeserializeObject<Settings>(SettingsJson);
        }
        internal static void JsonSettingsWrite(Settings settings)
        {
            string path = @"../../../Datafiles/Settings.json";

            string settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);

            File.WriteAllText(path, settingsJson);
        }

        internal static void ChangeParkingHouseSize(byte newSize)
        {
            Settings.SizeParkingHouse = newSize;
        }

        internal static void ChangeParkingHouseHighRoof(byte newValue)
        {
            throw new NotImplementedException();
        }
    }
}
