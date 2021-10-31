using Microsoft.VisualBasic;
using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Car : Vehicle
    {
        public override Constants.VehicleType Type
        {
            get { return Constants.VehicleType.Car; }
        }
        internal override byte Hight { get; } = (byte)Hights.Car;
        internal override byte Size { get; } = (byte)Sizes.Car;
        //public override ClassDiscriminatorEnum Type => ClassDiscriminatorEnum.Car;

        public Car(string aRegistrationNumber) : base(aRegistrationNumber)
        {

        }
    }
}
