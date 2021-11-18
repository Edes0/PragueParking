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
        internal override byte Height { get; } = Settings.HeightBike;
        internal override byte Size { get; } = Settings.SizeBike;
        internal override string StringType { get; } = "Bike";
        internal override decimal Price
        {
            get
            {
                TimeSpan freeTime = TimeSpan.FromMinutes(Settings.PriceFree);
                double span = (DateTime.Now - ArriveTime - freeTime).TotalHours;
                decimal priceClass = Settings.PriceBike;

                if (span <= 0) return 0;
                return priceClass * (decimal)span;
            }
        }

        public Bike()
        {
        }
        public Bike(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}