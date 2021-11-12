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

            if (Number <= highRoof)
            {
                Hight = Settings.HightParkingHigh;
            }
            else
            {
                Hight = Settings.HightParkingLow;
            }
        }
        public override string ToString()
        {
            return "ParkingSpot[" + this.Number + "]";
        }
        internal void AddVehicle(Vehicle vehicle)
        {
            vehicle.Park(this);

            if (vehicle.IsBig())
            {
                AvailableSize -= Size;
            }
            else if (vehicle.IsSmall() || vehicle.IsTiny())
            {
                AvailableSize -= vehicle.Size;
            }
            else
            {
                throw new Exception("Error: Vehicle is neither big or small vehicle, change vehicle size");
            }
        }
        internal void RemoveVehicle(Vehicle vehicle)
        {
            vehicle.CheckOut(this);

            if (vehicle.IsSmall() || vehicle.IsTiny())
            {
                AvailableSize += vehicle.Size;
            }
            else if (vehicle.IsBig())
            {
                AvailableSize += Size;
            }
            else
            {
                throw new Exception("Error: Vehicle is neither big nor small nor tiny vehicle, change vehicle size");
            }
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
            if (vehicle.IsSmall() && vehicle.FitSize(this) && vehicle.FitHight(this))
            {
                return true;
            }
            if (vehicle.IsBig() && IsFree() && vehicle.FitHight(this))
            {
                return true;
            }
            return false;
        }
        internal string GetparkingSpotInfo(ParkingSpot parkingSpot)
        {
            var parkingSpotNumber = $"{parkingSpot.AvailableSize}";

            if (VehicleList.Count == 0 && AvailableSize == Size)
            {
                return $"[b]{"   "}[/]\n[darkgreen]{parkingSpotNumber}[/]";
                // FREE
            }
            else if (VehicleList.Count == 0 && AvailableSize == 0)
            {
                return $"[b]{"   "}[/]\n[maroon]{parkingSpotNumber}[/]";
                // RES 
            }
            else if (AvailableSize == 3)
            {
                return $"[b]{"   "}[/]\n[yellow3_1]{parkingSpotNumber}[/]";
            }
            else if (AvailableSize == 2)
            {
                return $"[b]{"   "}[/]\n[gold1]{parkingSpotNumber}[/]";

            }
            else if (AvailableSize == 1)
            {
                return $"[b]{"   "}[/]\n[darkorange3_1]{parkingSpotNumber}[/]";
            }
            else if (AvailableSize == 0)
            {
                return $"[b]{"   "}[/]\n[maroon]{parkingSpotNumber}[/]";
            }
            else
            { 
                // throw new Exception ("Parking spot has invalid size.");    When code done
                // Code to put in vehicles in panel / Not used
                var vehicleType = $"{parkingSpot.VehicleList[0].StringType}";
                return $"[b]{"Error"}[/]\n[maroon]{parkingSpotNumber}[/]";
            }

        }
        internal bool IsFree()
        {
            if (AvailableSize == Size)
            {
                return true;
            }
            return false;
        }
        internal bool IsFull()
        {
            if (AvailableSize == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal bool IsHigh(byte highRoof)
        {
            if (Hight > highRoof)
            {
                return true;
            }
            return false;
        }
        internal bool IsLow(byte highRoof)
        {
            if (Hight < highRoof)
            {
                return true;
            }
            return false;
        }

    }
}
