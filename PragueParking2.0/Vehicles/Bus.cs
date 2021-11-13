using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Bus : Vehicle
    {
        public override VehicleType Type
        {
            get { return VehicleType.Bus; }
        }
        internal override byte Hight { get; } = Settings.HightBus;
        internal override byte Size { get; } = Settings.SizeBus;
        internal override string StringType { get; } = "Bus";
        internal override decimal Price
        {
            get
            {
                TimeSpan freeTime = TimeSpan.FromMinutes(Settings.PriceFree);
                double span = (DateTime.Now - ArriveTime - freeTime).TotalHours;
                decimal priceClass = Settings.PriceBus;
                decimal cost = priceClass * (decimal)span;

                if (cost < 0)
                {
                    return 0;
                }
                return cost;
            }
        }

        public Bus()
        {
        }
        public Bus(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
