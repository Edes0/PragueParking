using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0
{
    class ParkingSpot
    {
        public List<Vehicle> VehicleList = new List<Vehicle>();
        public byte Number { get; set; }
        private byte Size { get; } = (byte)Sizes.ParkingSpot;
        private byte Hight { get; set; }
        public byte AvailableSize { get; set; }

        public ParkingSpot(byte aNumber)
        {
            Number = aNumber;

            if (aNumber <= 50) // TODO: Magic nr
            {
                Hight = (byte)Hights.ParkingHigh;
            }
            else
            {
                Hight = (byte)Hights.ParkingLow;
            }
        }
        public bool CheckVehicleSize(Vehicle vehicle)
        {
            if (vehicle.Size > AvailableSize)
            {
                Console.WriteLine("Not done");
            }
            else
            {
                if (AvailableSize >= vehicle.Size)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckVehicleInParkingSpot(string aRegistrationNumber, out byte aIndex)
        {
            foreach (Vehicle vehicle in VehicleList)
            {
                if (vehicle.RegNum == aRegistrationNumber)
                {
                    aIndex = Number;
                    return true;
                }
            }
            aIndex = 0;
            return false;
        }
    }
}
