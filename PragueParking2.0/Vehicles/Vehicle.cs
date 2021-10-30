using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    public abstract class Vehicle
    {
        internal string RegNum { get; set; }
        private DateTime ParkTime { get; } = DateTime.Now;

        internal abstract byte Size { get; } 
        internal abstract byte Hight { get; }
        internal byte Pspot { get; set; }

        protected Vehicle(string aRegistrationNumber)
        {
            RegNum = aRegistrationNumber;
        }
        public override string ToString()
        {
            return this.RegNum;
        }
    }
}