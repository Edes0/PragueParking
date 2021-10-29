using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Mc : Vehicle
    {
        internal override byte Hight { get; } = (byte)Hights.Mc;
        internal override byte Size { get; } = (byte)Sizes.Mc;
        private string Type { get; } = "-Mc";

        public Mc(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
