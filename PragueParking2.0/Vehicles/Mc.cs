using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Mc : Vehicle
    {
        private byte Hight { get; } = (byte)Hights.Mc;
        private byte Size { get; } = (byte)Sizes.Mc;
        private string Type { get; } = "-Mc";

        public Mc(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
