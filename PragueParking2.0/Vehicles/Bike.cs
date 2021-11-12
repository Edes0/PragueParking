using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Bike : Vehicle
    {
        public override VehicleType Type
        {
            get { return VehicleType.Bike; }
        }
        internal override byte Hight { get; } = Settings.HightBike;
        internal override byte Size { get; } = Settings.SizeBike;
        internal override string StringType { get; } = "Bike";
        //internal override decimal Price
        //{
        //    get { return Price; }
        //    set
        //    {

        //        TimeSpan freeTime = TimeSpan.FromMinutes((double)Prices.Free);
        //        double span = (DateTime.Now - ParkTime - freeTime).TotalHours;
        //        decimal priceClass = (decimal)Prices.Bike;
        //        decimal cost = priceClass * (decimal)span;
        //    }
        //}
        public Bike()
        {
        }
        public Bike(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
