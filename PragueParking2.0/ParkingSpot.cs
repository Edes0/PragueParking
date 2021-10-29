using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0
{
    public class ParkingSpot
    {
        internal List<Vehicle> VehicleList { get; set; } = new List<Vehicle>();
        internal byte Number { get; set; }
        internal byte Size { get; } = (byte)Sizes.ParkingSpot;
        internal byte Hight { get; set; }
        internal byte AvailableSize { get; set; } = (byte)Sizes.ParkingSpot;

        internal ParkingSpot(byte aNumber, byte aHighRoof)
        {
            Number = aNumber;

            if (aNumber <= aHighRoof) // TODO: Magic nr
            {
                Hight = (byte)Hights.ParkingHigh;
            }
            else
            {
                Hight = (byte)Hights.ParkingLow;
            }
        }
        internal bool CheckVehicleParkingAvailable(Vehicle aVehicle, ParkingSpot aParkingSpot)
        {
            if (aParkingSpot.AvailableSize >= aVehicle.Size)
            {
                aParkingSpot.AvailableSize -= aVehicle.Size;
                aVehicle.Pspot = aParkingSpot.Number;
                aParkingSpot.VehicleList.Add(aVehicle); // PROBLEM MED SIZE
                return true;
            }
            return false;
        }
        internal bool CheckHight(Vehicle aVehicle, ParkingSpot aParkingSpot)
        {
            if (aParkingSpot.Hight > aVehicle.Hight)
            {
                return true;
            }
            return false;
        }
        internal bool CheckVehicleInParkingSpot(string aRegistrationNumber, out byte aIndex)
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
