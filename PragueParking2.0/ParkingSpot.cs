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
        internal byte Size { get; set; } = (byte)Sizes.ParkingSpot;
        [JsonProperty]
        internal byte Hight { get; set; }
        [JsonProperty]
        internal byte AvailableSize { get; set; } = (byte)Sizes.ParkingSpot;
        [JsonProperty]
        internal List<Vehicle> VehicleList { get; set; } = new List<Vehicle>();
        public ParkingSpot()
        {

        }
        internal ParkingSpot(byte aNumber, byte aHighRoof)
        {
            Number = aNumber;

            if (Number <= aHighRoof)
            {
                Hight = (byte)Hights.ParkingHigh;
            }
            else
            {
                Hight = (byte)Hights.ParkingLow;
            }
        }
        public override string ToString()
        {
            return "ParkingSpot[" + this.Number + "]";
        }

        internal void RemoveVehicle(Vehicle vehicle) // Make vehicle remove it self
        {
                vehicle.CheckOut(this);
        }
        internal void AddVehicle(Vehicle vehicle)
        {
                vehicle.Park(this);
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
            if (vehicle.IsSmallVehicle() && vehicle.VehicleFitSize(this) && vehicle.VehicleFitHight(this))
            {
                return true;
            }
            if (vehicle.IsBigVehicle() && ParkingIsFree() && vehicle.VehicleFitHight(this))
            {
                return true;
            }
            return false;
        }
        private bool ParkingIsFree()
        {
            if (AvailableSize == Size)
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
                // Code to put in vehicles in panel / Not used
                var vehicleType = $"{parkingSpot.VehicleList[0].StringType}";
                return $"[b]{parkingSpot.VehicleList[0].StringType}[/]\n[maroon]{parkingSpotNumber}[/]";
            }

        }
        internal void MoveVehicles(ParkingSpot parkingSpot)
        {
            parkingSpot.VehicleList.Add(VehicleList[0]);
        }

    }
}
