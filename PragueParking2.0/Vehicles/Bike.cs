using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Bike : Vehicle
    {
        internal override byte Hight { get; } = (byte)Hights.Bike;
        internal override byte Size { get; } = (byte)Sizes.Bike;
        private string Type { get; } = "-B"; // Lägg i utrskrift

        public Bike(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
