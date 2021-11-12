using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Bus : Vehicle
    {
        public override VehicleType Type
        {
            get { return VehicleType.Bus; }
        }
        internal override byte Hight { get; } = Settings.HightBus;
        internal override byte Size { get; } = Settings.SizeBus;
        internal override string StringType { get; } = "Bus";
        //internal override decimal Price
        //{
        //    get { return Price; }
        //    set
        //    {

        //        TimeSpan freeTime = TimeSpan.FromMinutes((double)Prices.Free);
        //        double span = (DateTime.Now - ParkTime - freeTime).TotalHours;
        //        decimal priceClass = (decimal)Prices.Bus;
        //        decimal cost = priceClass * (decimal)span;
        //    }
        //}
        public Bus()
        {
        }
        public Bus(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
