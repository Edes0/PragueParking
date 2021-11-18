using Newtonsoft.Json;
using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PragueParking2._0
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ParkingSpot
    {
        [JsonProperty]
        internal byte Number { get; set; }
        [JsonProperty]
        internal byte Size { get; set; } = Settings.SizeParkingSpot;
        [JsonProperty]
        internal byte Height { get; set; }
        [JsonProperty]
        internal byte AvailableSize { get; set; } = Settings.SizeParkingSpot;
        [JsonProperty]
        internal List<Vehicle> VehicleList { get; set; } = new List<Vehicle>();
        public ParkingSpot()
        {
            if (Number <= Settings.SizeParkingHouseHighRoof)
                Height = Settings.HeightParkingHigh;
            else Height = Settings.HeightParkingLow;
        }
        internal ParkingSpot(byte aNumber, byte highRoof)
        {
            Number = aNumber;

            if (Number <= highRoof) Height = Settings.HeightParkingHigh;
            else Height = Settings.HeightParkingLow;
        }
        public override string ToString()
        {
            return $"ParkingSpot[{Number}]";
        }
        /// <summary>
        /// Adds vehicle in parkingspot
        /// </summary>
        /// <param name="vehicle"></param>
        internal void AddVehicle(Vehicle vehicle)
        {
            vehicle.Park(this);

            if (vehicle.IsBig()) AvailableSize -= Size;
            if (vehicle.IsSmall() || vehicle.IsTiny()) AvailableSize -= vehicle.Size;
        }
        /// <summary>
        /// Removes vehicle in parkingspot
        /// </summary>
        /// <param name="vehicle"></param>
        internal void RemoveVehicle(Vehicle vehicle)
        {
            vehicle.CheckOut(this);

            if (vehicle.IsSmall() || vehicle.IsTiny()) AvailableSize += vehicle.Size;
            if (vehicle.IsBig()) AvailableSize += Size;
        }
        /// <summary>
        /// Clear parking
        /// </summary>
        internal void Clear()
        {
            VehicleList.Clear();
            AvailableSize = Size;
        }
        /// <summary>
        /// Reserve parking
        /// </summary>
        /// <param name="parkingSpot"></param>
        internal void Reserve(ParkingSpot parkingSpot)
        {
            parkingSpot.AvailableSize -= Size;
        }
        /// <summary>
        /// Checks if vehicle is in parkingspot
        /// </summary>
        /// <param name="aRegistrationNumber"></param>
        /// <param name="aVehicle"></param>
        /// <returns></returns>
        internal bool VehicleExist(string aRegistrationNumber, out Vehicle aVehicle)
        {
            foreach (Vehicle vehicle in VehicleList)
            {
                if (vehicle.RegNum == aRegistrationNumber)
                {
                    aVehicle = vehicle;
                    return true;
                }
            }
            aVehicle = null;
            return false;
        }
        /// <summary>
        /// Check if parkingspot is available for parking
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        internal bool ParkingSpotAvailable(Vehicle vehicle)
        {
            if (!vehicle.IsBig() && vehicle.FitSize(this) && vehicle.FitHeight(this)) return true;
            if (vehicle.IsBig() && IsFree() && vehicle.FitHeight(this)) return true;
            return false;
        }
        /// <summary>
        /// Gets info from parkingspot to print
        /// </summary>
        /// <param name="parkingSpot"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal string GetparkingSpotInfo(ParkingSpot parkingSpot)
        {
            var availableSize = $"{parkingSpot.AvailableSize}";

            if (parkingSpot.IsFree() || AvailableSize >= 4) return $"[b]{"   "}[/]\n[darkgreen]{availableSize}[/]";
            if (AvailableSize == 3) return $"[b]{"   "}[/]\n[yellow3_1]{availableSize}[/]";
            if (AvailableSize == 2) return $"[b]{"   "}[/]\n[gold1]{availableSize}[/]";
            if (AvailableSize == 1) return $"[b]{"   "}[/]\n[darkorange3_1]{availableSize}[/]";
            if (parkingSpot.IsFull()) return $"[b]{"   "}[/]\n[maroon]{availableSize}[/]";

            throw new Exception("Error: Parking spot has invalid size");

            //if (VehicleList.Count == 0 && parkingSpot.IsFull()) return $"[b]{"RES "}[/]\n[maroon]{parkingSpotNumber}[/]"; // If you want to show reserved spots
        }
        /// <summary>
        /// Checks if parking is free
        /// </summary>
        /// <returns></returns>
        internal bool IsFree()
        {
            if (AvailableSize == Size) return true;
            return false;
        }
        /// <summary>
        /// Checks if parking is reserved
        /// </summary>
        /// <returns></returns>
        internal bool IsReserved()
        {
            if (AvailableSize == 0 && VehicleList.Count() == 0) return true;
            return false;
        }
        /// <summary>
        /// Checks if parking is full
        /// </summary>
        /// <returns></returns>
        internal bool IsFull()
        {
            if (AvailableSize == 0) return true;
            return false;
        }
        /// <summary>
        /// Checks if parking is high
        /// </summary>
        /// <returns></returns>
        internal bool IsHigh()
        {
            if (Height >= Settings.HeightParkingHigh) return true;
            return false;
        }
        /// <summary>
        /// Check if parking is low
        /// </summary>
        /// <returns></returns>
        internal bool IsLow()
        {
            if (Height <= Settings.HeightParkingLow) return true;
            return false;
        }
        /// <summary>
        /// Gets ticket info at parking
        /// </summary>
        /// <returns></returns>
        internal string GetTicketInfo()
        {
            foreach (Vehicle vehicle in VehicleList)
                return vehicle.GetVehicleInfo();
            return "----";
        }
        /// <summary>
        /// Gets vehicle prices from parking
        /// </summary>
        /// <returns></returns>
        internal decimal GetVehiclePrices()
        {
            decimal totalPrice = 0;

            foreach (Vehicle vehicle in VehicleList)
            {
                totalPrice += vehicle.GetPrice();
            }
            return totalPrice;
        }
        /// <summary>
        /// Checks if parking have high vehicle
        /// </summary>
        /// <returns></returns>
        internal bool HaveHighVehicle()
        {
            foreach (Vehicle vehicle in VehicleList)
                if (vehicle.IsHigh()) return true;
            return false;
        }
        /// <summary>
        /// Checks if parkingspot is possible to shrink size
        /// </summary>
        /// <param name="newSize"></param>
        /// <returns></returns>
        internal bool PossibleToShrink(byte newSize)
        {
            if (newSize < Size - AvailableSize) return false;
            return true;
        }
    }
}