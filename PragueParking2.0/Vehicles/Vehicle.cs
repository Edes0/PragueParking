using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    public abstract class Vehicle
    {
        public string RegNum { get; set; }
        public DateTime ParkTime { get; } = DateTime.Now;
        public byte Size { get; internal set; }

        public Vehicle(string aRegistrationNumber)
        {
        }
    }
}