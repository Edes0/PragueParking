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
        internal bool IsSmallVehicle(Vehicle vehicle)
        {
            byte parkingSpotSize = (byte)Sizes.ParkingSpot;

            if (vehicle.Size <= parkingSpotSize)
            {
                return true;
            }
            return false;
        }
        internal bool IsBigVehicle(Vehicle vehicle)
        {
            byte parkingSpotSize = (byte)Sizes.ParkingSpot;

            if (vehicle.Size > parkingSpotSize)
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
    }
}