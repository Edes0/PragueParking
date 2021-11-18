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
        internal abstract byte Height { get; }
        [JsonProperty]
        internal abstract string StringType { get; }
        [JsonProperty]
        internal DateTime ArriveTime { get; set; }
        [JsonProperty]
        internal abstract decimal Price { get; }
        [JsonProperty]
        public abstract VehicleType Type { get; }
        protected Vehicle()
        {
        }
        protected Vehicle(string registrationNumber)
        {
            ArriveTime = DateTime.Now;
            RegNum = registrationNumber;
        }
        /// <summary>
        /// Parks vehicle
        /// </summary>
        /// <param name="parkingSpot"></param>
        internal void Park(ParkingSpot parkingSpot)
        {
            parkingSpot.VehicleList.Add(this);
            Pspot = parkingSpot.Number;
        }
        /// <summary>
        /// Removes vehicle
        /// </summary>
        /// <param name="parkingSpot"></param>
        internal void CheckOut(ParkingSpot parkingSpot)
        {
            parkingSpot.VehicleList.Remove(this);
        }
        /// <summary>
        /// Checks if vehicle is tiny
        /// </summary>
        /// <returns></returns>
        internal bool IsTiny()
        {
            byte parkingSpotSize = Settings.SizeParkingSpot;

            if (Size < parkingSpotSize) return true;
            return false;
        }
        /// <summary>
        /// Checks if vehicle is small
        /// </summary>
        /// <returns></returns>
        internal bool IsSmall()
        {
            byte parkingSpotSize = Settings.SizeParkingSpot;

            if (Size == parkingSpotSize) return true;
            return false;
        }
        /// <summary>
        /// Checks if vehicle is big
        /// </summary>
        /// <returns></returns>
        internal bool IsBig()
        {
            byte parkingSpotSize = Settings.SizeParkingSpot;

            if (Size > parkingSpotSize) return true;
            return false;
        }
        /// <summary>
        /// Checks if vehicle is high
        /// </summary>
        /// <returns></returns>
        internal bool IsHigh()
        {
            if (Height > Settings.HeightParkingLow) return true;
            return false;
        }
        /// <summary>
        /// Checks if vehicle is low
        /// </summary>
        /// <returns></returns>
        internal bool IsLow()
        {
            if (Height < Settings.HeightParkingLow) return true;
            return false;
        }
        /// <summary>
        /// Checks if vehicle fits size in parkingspot
        /// </summary>
        /// <param name="parkingSpot"></param>
        /// <returns></returns>
        internal bool FitSize(ParkingSpot parkingSpot)
        {
            if (parkingSpot.AvailableSize >= Size) return true;
            return false;
        }
        /// <summary>
        /// Checks if vehicle fits height in parkingspot
        /// </summary>
        /// <param name="parkingSpot"></param>
        /// <returns></returns>
        internal bool FitHeight(ParkingSpot parkingSpot)
        {
            if (parkingSpot.Height > Height) return true;
            return false;
        }
        /// <summary>
        /// Gets vehicle info
        /// </summary>
        /// <returns></returns>
        internal string GetVehicleInfo()
        {
            return $"{Pspot + 1}, {StringType}, {RegNum}, {ArriveTime:dd/MMM/yyyy HH:mm}, {Price:0.00} CZK";
        }
        /// <summary>
        /// Gets vehicle price
        /// </summary>
        /// <returns></returns>
        internal decimal GetPrice()
        {
            return Price;
        }
    }
}