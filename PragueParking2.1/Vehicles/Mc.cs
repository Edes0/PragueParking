using PragueParking2._0.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    class Mc : Vehicle
    {
        public override VehicleType Type
        {
            get { return VehicleType.Mc; }
        }
        internal override byte Hight { get; } = Settings.HightMc;
        internal override byte Size { get; } = Settings.SizeMc;
        internal override string StringType { get; } = "Mc";
        internal override decimal Price
        {
            get
            {
                TimeSpan freeTime = TimeSpan.FromMinutes(Settings.PriceFree);
                double span = (DateTime.Now - ArriveTime - freeTime).TotalHours;
                decimal priceClass = Settings.PriceMc;

                if (span <= 0) return 0;
                return priceClass * (decimal)span;
            }
        }

        public Mc()
        {
        }
        public Mc(string aRegistrationNumber) : base(aRegistrationNumber)
        {
        }
    }
}
