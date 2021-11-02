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
        internal bool CheckHight(Vehicle aVehicle, ParkingSpot aParkingSpot)
        {
            if (aParkingSpot.Hight > aVehicle.Hight)
            {
                return true;
            }
            return false;
        }
        internal bool RemoveSmallVehicle(Vehicle aVehicle, ParkingSpot aParkingSpot)
        {
            if (aVehicle.Size <= aParkingSpot.Size)
            {
                aParkingSpot.VehicleList.Remove(aVehicle);
                aParkingSpot.AvailableSize += aVehicle.Size;

                return true;
            }
            return false;
        }
        internal void RemoveBigVehicle(ParkingSpot aParkingSpot, byte aReservedSpots, ParkingSpot[] aParkingSpotArray)
        {
            for (int i = aParkingSpot.Number; i < aReservedSpots + aParkingSpot.Number; i++)
            {
                aParkingSpotArray[i].AvailableSize = aParkingSpot.Size;
            }
        }
            internal void AddVehicle(Vehicle vehicle, ParkingSpot parkingSpot)
        {
            parkingSpot.VehicleList.Add(vehicle);
        }
        internal void ClearParking(ParkingSpot parkingSpot)
        {
            parkingSpot.VehicleList.Clear();
            parkingSpot.AvailableSize = parkingSpot.Size;
        }
        internal bool CheckVehicleInParkingSpot(string aRegistrationNumber, out Vehicle aVehicle)
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
        internal bool ParkingSpotIsAvailable(ParkingSpot aParkingSpot, Vehicle aVehicle)
        {
            if (aParkingSpot.AvailableSize >= aVehicle.Size)
            {
                aParkingSpot.AvailableSize -= aVehicle.Size;
                aVehicle.Pspot = aParkingSpot.Number;
                aParkingSpot.VehicleList.Add(aVehicle);

                return true;
            }
            return false;
        }
        internal bool BigParkingSpotIsAvailable(ParkingSpot aParkingSpot, Vehicle aVehicle)
        {
            if (aParkingSpot.Hight > aVehicle.Hight && aParkingSpot.AvailableSize == Size)
            {
                return true;
            }
            return false;
        }
        internal void ReservSpotsForBigVehicle(ParkingSpot aParkingSpot, Vehicle aVehicle, byte aParkingSpotCounter, byte aCounterLimit, ParkingSpot[] aParkingSpotArray)
        {
            for (int i = 1; i < aCounterLimit; i++)
            {
                aParkingSpotArray[aParkingSpotCounter - i].AvailableSize -= Size;
            }
            aParkingSpot.AvailableSize -= aParkingSpot.AvailableSize;
            aParkingSpot.VehicleList.Add(aVehicle);
        }

        internal string GetparkingSpotInfo(ParkingSpot parkingSpot)
        {
            var parkingSpotNumber = $"{parkingSpot.AvailableSize}";

            if (parkingSpot.VehicleList.Count == 0 && parkingSpot.AvailableSize == Size)
            {
                return $"[b]{"   "}[/]\n[darkgreen]{parkingSpotNumber}[/]";
                // FREE
            }
            else if (parkingSpot.VehicleList.Count == 0)
            {
                return $"[b]{"   "}[/]\n[maroon]{parkingSpotNumber}[/]";
                // RES 
            }
            else
            {
                var vehicleType = $"{parkingSpot.VehicleList[0].StringType}";
                return $"[b]{"   "}[/]\n[maroon]{parkingSpotNumber}[/]";
                // parkingSpot.VehicleList[0].StringType
            }
        }
    }
}
