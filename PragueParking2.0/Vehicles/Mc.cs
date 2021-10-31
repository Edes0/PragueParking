using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Mc : Vehicle
    {
        public override VehicleType Type
        {
            get { return VehicleType.Mc; }
        }
        internal override byte Hight { get; } = (byte)Hights.Mc;
        internal override byte Size { get; } = (byte)Sizes.Mc;

        public Mc()
        {
        }
        public Mc(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
