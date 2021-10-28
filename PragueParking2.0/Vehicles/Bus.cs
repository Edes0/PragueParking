using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Bus : Vehicle
    {
        private byte Hight { get; } = (byte)Hights.Bus;
        private byte Size { get; } = (byte)Sizes.Bus;
        private string Type { get; } = "-Bus";

        public Bus(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
