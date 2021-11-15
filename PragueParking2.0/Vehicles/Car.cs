using Microsoft.VisualBasic;
using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Car : Vehicle
    {
        public override VehicleType Type
        {
            get { return VehicleType.Car; }
        }
        internal override byte Hight { get; } = Settings.HightCar;
        internal override byte Size { get; } = Settings.SizeCar;
        internal override string StringType { get; } = "Car";
        internal override decimal Price
        {
            get
            {
                TimeSpan freeTime = TimeSpan.FromMinutes(Settings.PriceFree);
                double span = (DateTime.Now - ArriveTime - freeTime).TotalHours;
                decimal priceClass = Settings.PriceCar;

                if (span <= 0) return 0;
                return priceClass * (decimal)span;
            }
        }

        public Car()
        {

        }
        public Car(string aRegistrationNumber) : base(aRegistrationNumber)
        {

        }
    }
}
