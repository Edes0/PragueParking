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
        internal override byte Hight { get; } = (byte)Hights.Bus;
        internal override byte Size { get; } = (byte)Sizes.Bus;
        internal override string StringType { get; } = "Bus";

        public Bus()
        {
        }
        public Bus(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
