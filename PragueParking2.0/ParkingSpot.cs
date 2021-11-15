using Newtonsoft.Json;
using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
using System;
using System.Collections.Generic;

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
        internal byte Hight { get; set; }
        [JsonProperty]
        internal byte AvailableSize { get; set; } = Settings.SizeParkingSpot;
        [JsonProperty]
        internal List<Vehicle> VehicleList { get; set; } = new List<Vehicle>();
        public ParkingSpot()
        {

        }
        internal ParkingSpot(byte aNumber, byte highRoof)
        {
            Number = aNumber;

            if (Number <= highRoof) Hight = Settings.HightParkingHigh;
            else Hight = Settings.HightParkingLow;
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
            if (vehicle.IsSmall() && vehicle.FitSize(this) && vehicle.FitHight(this)) return true;
            if (vehicle.IsBig() && IsFree() && vehicle.FitHight(this)) return true;
            return false;
        }
        internal string GetparkingSpotInfo(ParkingSpot parkingSpot)
        {
            var parkingSpotNumber = $"{parkingSpot.AvailableSize}";

            if (parkingSpot.IsFree()) return $"[b]{"   "}[/]\n[darkgreen]{parkingSpotNumber}[/]";
            if (AvailableSize == 3) return $"[b]{"   "}[/]\n[yellow3_1]{parkingSpotNumber}[/]";
            if (AvailableSize == 2) return $"[b]{"   "}[/]\n[gold1]{parkingSpotNumber}[/]";
            if (AvailableSize == 1) return $"[b]{"   "}[/]\n[darkorange3_1]{parkingSpotNumber}[/]";
            if (parkingSpot.IsFull()) return $"[b]{"   "}[/]\n[maroon]{parkingSpotNumber}[/]";

            throw new Exception("Parking spot has invalid size. Implement new size above");

            //if (VehicleList.Count == 0 && parkingSpot.IsFull()) return $"[b]{"RES "}[/]\n[maroon]{parkingSpotNumber}[/]"; // If you want to show reserved spots
        }
        internal bool IsFree()
        {
            if (AvailableSize == Size) return true;
            return false;
        }
        internal bool IsFull()
        {
            if (AvailableSize == 0) return true;
            return false;
        }
        internal bool IsHigh()
        {
            if (Hight >= Settings.HightParkingHigh) return true;
            return false;
        }
        internal bool IsLow()
        {
            if (Hight <= Settings.HightParkingLow) return true;
            return false;
        }
        internal string GetTicketInfo()
        {
            foreach (Vehicle vehicle in VehicleList)
            {
                return $"{vehicle.StringType} ({vehicle.RegNum}) arrived: {vehicle.ArriveTime:dd/MMM/yyyy HH:mm} with total price: {vehicle.Price:0.00} CZK";
            }
            return "----";
        }
    }
}
