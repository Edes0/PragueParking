using Newtonsoft.Json;
using PragueParking2._0.Enums;
using PragueParking2._0.Vehicles;
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

            if (aNumber <= aHighRoof)
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
        internal bool CheckVehicleInParkingSpot(string aRegistrationNumber, out byte aIndex)
        {
            foreach (Vehicle vehicle in VehicleList)
            {
                if (vehicle.RegNum == aRegistrationNumber)
                {
                    aIndex = Number;
                    return true;
                }
            }
            aIndex = 0;
            return false;
        }
    }
}
