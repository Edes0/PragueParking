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
        internal override byte Hight { get; } = (byte)Hights.Car;
        internal override byte Size { get; } = (byte)Sizes.Car;
        internal override string StringType { get; } = "Car";
        //internal override decimal Price
        //{
        //    get { return Price; }
        //    set
        //    {
        //        TimeSpan freeTime = TimeSpan.FromMinutes((double)Prices.Free);
        //        double span = (DateTime.Now - ParkTime - freeTime).TotalHours;
        //        decimal priceClass = (decimal)Prices.Car;
        //        decimal cost = priceClass * (decimal)span;
        //    }
        //}

        public Car()
        {

        }
        public Car(string aRegistrationNumber) : base(aRegistrationNumber)
        {

        }
    }
}
