using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract partial class Vehicle
    {
        [JsonProperty]
        internal string RegNum { get; set; }
        [JsonProperty]
        internal byte Pspot { get; set; }
        [JsonProperty]
        internal abstract byte Size { get; }
        [JsonProperty]
        internal abstract byte Hight { get; }
        [JsonProperty]
        internal abstract string StringType { get; }
        [JsonProperty]
        private DateTime ParkTime { get; } = DateTime.Now;
        [JsonProperty]
        public abstract VehicleType Type { get; }

        protected Vehicle()
        {
        }
        protected Vehicle(string registrationNumber)
        {
            RegNum = registrationNumber;
        }
        internal void Park(ParkingSpot parkingSpot)
        {
            parkingSpot.VehicleList.Add(this);
            Pspot = parkingSpot.Number;

            if (IsBigVehicle())
            {
                parkingSpot.AvailableSize -= parkingSpot.Size;
            }
            else if (IsSmallVehicle())
            {
                parkingSpot.AvailableSize -= Size;
            }
            else
            {
                throw new Exception("Error: Vehicle is neither big or small vehicle, change vehicle size");
            }
        }
        internal void CheckOut(ParkingSpot parkingSpot)
        {
            parkingSpot.VehicleList.Remove(this);

            if (IsSmallVehicle())
            {
                parkingSpot.AvailableSize += Size;
            }
            else if (IsBigVehicle())
            {
                parkingSpot.AvailableSize += parkingSpot.AvailableSize;
            }
            else
            {
                throw new Exception("Error: Vehicle is neither big or small vehicle, change vehicle size");
            }

        }
        internal bool IsSmallVehicle()
        {
            byte parkingSize = (byte)Sizes.ParkingSpot;

            if (Size <= parkingSize)
            {
                return true;
            }
            return false;
        }
        internal bool IsBigVehicle()
        {
            byte parkingSpotSize = (byte)Sizes.ParkingSpot;

            if (Size > parkingSpotSize)
            {
                return true;
            }
            return false;
        }
        internal bool IsHighVehicle(Vehicle vehicle, ParkingSpot parkingSpot)
        {
            if (vehicle.Hight > parkingSpot.Hight)
            {
                return true;
            }
            return false;
        }
        internal bool IsLowVehicle(Vehicle vehicle, ParkingSpot parkingSpot)
        {
            if (vehicle.Hight < parkingSpot.Hight)
            {
                return true;
            }
            return false;
        }
        internal bool VehicleFitSize(ParkingSpot parkingSpot)
        {
            if (parkingSpot.AvailableSize >= Size)
            {
                return true;
            }
            return false;
        }
        internal bool VehicleFitHight(ParkingSpot parkingSpot)
        {
            if (parkingSpot.Hight > Hight)
            {
                return true;
            }
            return false;
        }

    }
}