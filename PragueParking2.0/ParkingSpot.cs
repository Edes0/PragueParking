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

        internal void RemoveVehicle(Vehicle vehicle, ParkingSpot parkingSpot)
        {
            if (vehicle.IsSmallVehicle(vehicle))
            {
                parkingSpot.VehicleList.Remove(vehicle);
                parkingSpot.AvailableSize += vehicle.Size;
            }
            else if (vehicle.IsBigVehicle(vehicle))
            {
                parkingSpot.VehicleList.Remove(vehicle);
                parkingSpot.AvailableSize += parkingSpot.Size;
            }
            else
            {
                // Ändra detta
                Console.WriteLine("Error: Vehicle size and parkingspot size is the same");
            }
        }
        internal void AddVehicle(Vehicle vehicle, ParkingSpot parkingSpot)
        {
            if (vehicle.IsBigVehicle(vehicle)) // HIGHT
            {
                VehicleList.Add(vehicle);
                AvailableSize -= Size;
                vehicle.Pspot = Number;
            }
            else if (vehicle.IsSmallVehicle(vehicle))
            {
                VehicleList.Add(vehicle);
                AvailableSize -= vehicle.Size;
                vehicle.Pspot = Number;
            }
            else
            { // Ändra detta
                Console.WriteLine("Error: Vehicle size and parkingspot size is the same");
            }
        }
        internal void Clear(ParkingSpot parkingSpot)
        {
            parkingSpot.VehicleList.Clear();
            parkingSpot.AvailableSize = Size;
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
        internal bool ParkingSpotAvailable(ParkingSpot aParkingSpot, Vehicle aVehicle)
        {
            if (aParkingSpot.AvailableSize >= aVehicle.Size)
            {
                return true;
            }
            return false;
        }
        internal bool BigParkingSpotAvailable(ParkingSpot aParkingSpot, Vehicle aVehicle)
        {
            if (aParkingSpot.Hight > aVehicle.Hight && aParkingSpot.AvailableSize == Size)
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
            else if (VehicleList.Count == 0)
            {
                return $"[b]{"   "}[/]\n[maroon]{parkingSpotNumber}[/]";
                // RES 
            }
            else
            {
                // var vehicleType = $"{parkingSpot.VehicleList[0].StringType}";
                return $"[b]{"   "}[/]\n[maroon]{parkingSpotNumber}[/]";
                // parkingSpot.VehicleList[0].StringType
            }
        }
    }
}
