using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Car : Vehicle
    {
        internal override byte Hight { get; } = (byte)Hights.Car;
        internal override byte Size { get; } = (byte)Sizes.Car;
        private string Type { get; } = "-C";
        public Car(string aRegistrationNumber) : base(aRegistrationNumber)
        {

        }
    }
}
