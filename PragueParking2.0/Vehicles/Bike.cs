using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Bike : Vehicle
    {
        public override Constants.VehicleType Type
        {
            get { return Constants.VehicleType.Bike; }
        }
        internal override byte Hight { get; } = (byte)Hights.Bike;
        internal override byte Size { get; } = (byte)Sizes.Bike;
        //public override ClassDiscriminatorEnum Type => ClassDiscriminatorEnum.Bike;

        public Bike(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
