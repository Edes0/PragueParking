using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0
{
    public class Settings
    {
        public byte SizeBike { get; set; }
        public byte SizeMc { get; set; }
        public byte SizeCar { get; set; }
        public byte SizeBus { get; set; }
        public byte SizeParkingSpot { get; set; }
        public byte SizeParkingHouse { get; set; }
        public byte SizeParkingHouseHighRoof { get; set; }
        public byte SizeTableColumns { get; set; }

        public byte HightBike { get; set; }
        public byte HightMc { get; set; }
        public byte HightCar { get; set; }
        public byte HightBus { get; set; }
        public byte HightParkingLow { get; set; }
        public byte HIghtParkingHigh { get; set; }

        public Settings()
        {

        }
    }
}
