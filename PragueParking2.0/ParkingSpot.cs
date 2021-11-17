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
                Height = Settings.HightParkingHigh;
            else Height = Settings.HightParkingLow;
        }
        internal ParkingSpot(byte aNumber, byte highRoof)
        {
            Number = aNumber;

            if (Number <= highRoof) Height = Settings.HightParkingHigh;
            else Height = Settings.HightParkingLow;
        }
        public override string ToString()
        {
            return $"ParkingSpot[{Number}]";
        }
        internal void AddVehicle(Vehicle vehicle)
        {
            vehicle.Park(this);

            if (vehicle.IsBig()) AvailableSize -= Size;
            if (vehicle.IsSmall() || vehicle.IsTiny()) AvailableSize -= vehicle.Size;
        }
        internal void RemoveVehicle(Vehicle vehicle)
        {
            vehicle.CheckOut(this);

            if (vehicle.IsSmall() || vehicle.IsTiny()) AvailableSize += vehicle.Size;
            if (vehicle.IsBig()) AvailableSize += Size;
        }
        internal void Clear()
        {
            VehicleList.Clear();
            AvailableSize = Size;
        }
        internal void Reserve(ParkingSpot parkingSpot)
        {
            parkingSpot.AvailableSize -= Size;
        }
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
        internal bool ParkingSpotAvailable(Vehicle vehicle)
        {
            if (!vehicle.IsBig() && vehicle.FitSize(this) && vehicle.FitHight(this)) return true;
            if (vehicle.IsBig() && IsFree() && vehicle.FitHight(this)) return true;
            return false;
        }
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
        internal bool IsFree()
        {
            if (AvailableSize == Size) return true;
            return false;
        }
        internal bool IsReserved()
        {
            if (AvailableSize == 0 && VehicleList.Count() == 0) return true;
            return false;
        }
        internal bool IsFull()
        {
            if (AvailableSize == 0) return true;
            return false;
        }
        internal bool IsHigh()
        {
            if (Height >= Settings.HightParkingHigh) return true;
            return false;
        }
        internal bool IsLow()
        {
            if (Height <= Settings.HightParkingLow) return true;
            return false;
        }
        internal string GetTicketInfo()
        {
            foreach (Vehicle vehicle in VehicleList)
                return vehicle.GetVehicleInfo();
            return "----";
        }
        internal decimal GetVehiclePrices()
        {
            decimal totalPrice = 0;

            foreach (Vehicle vehicle in VehicleList)
            {
                totalPrice += vehicle.GetPrice();
            }
            return totalPrice;
        }
        internal bool HaveHighVehicle()
        {
            foreach (Vehicle vehicle in VehicleList)
                if (vehicle.IsHigh()) return true;
            return false;
        }
        internal bool PossibleToShrink(byte newSize)
        {
            if (newSize < Size - AvailableSize) return false;
            return true;
        }
    }
}
