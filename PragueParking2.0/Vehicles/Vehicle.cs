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
        protected DateTime ParkTime { get; } = DateTime.Now;
        [JsonProperty]
        public abstract VehicleType Type { get; }
        //[JsonProperty]
        //internal abstract decimal Price { get; set; }

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
        }
        internal void CheckOut(ParkingSpot parkingSpot)
        {
            parkingSpot.VehicleList.Remove(this);
         }
        internal bool IsTiny()
        {
            byte parkingSpotSize = Settings.SizeParkingSpot;

            if (Size < parkingSpotSize)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal bool IsSmall()
        {
            byte parkingSpotSize = Settings.SizeParkingSpot;

            if (Size <= parkingSpotSize)
            {
                return true;
            }
            return false;
        }
        internal bool IsBig()
        {
            byte parkingSpotSize = Settings.SizeParkingSpot;

            if (Size > parkingSpotSize)
            {
                return true;
            }
            return false;
        }
        internal bool IsHigh(Vehicle vehicle, ParkingSpot parkingSpot)
        {
            if (vehicle.Hight > parkingSpot.Hight)
            {
                return true;
            }
            return false;
        }
        internal bool IsLow (Vehicle vehicle, ParkingSpot parkingSpot)
        {
            if (vehicle.Hight < parkingSpot.Hight)
            {
                return true;
            }
            return false;
        }
        internal bool FitSize(ParkingSpot parkingSpot)
        {
            if (parkingSpot.AvailableSize >= Size)
            {
                return true;
            }
            return false;
        }
        internal bool FitHight(ParkingSpot parkingSpot)
        {
            if (parkingSpot.Hight > Hight)
            {
                return true;
            }
            return false;
        }
        internal void TicketInfo()
        {

        }
    }
}