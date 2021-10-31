using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PragueParking2._0.Vehicles
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Vehicle
    {
        [JsonProperty]
        internal string RegNum { get; set; }
        [JsonProperty]
        internal byte Pspot { get; set; }
        [JsonProperty]
        internal abstract byte Size { get; }
        [JsonProperty]
        internal abstract byte Hight { get; }
        [JsonProperty]
        private DateTime ParkTime { get; } = DateTime.Now;

        protected Vehicle(string aRegistrationNumber)
        {
            RegNum = aRegistrationNumber;
        }
        // OPTIONAL: Försökt att få det att fungera
        //[JsonConverter(typeof(Vehicle))]
        //abstract class Base
        //{
        //    public int ObjType { get; set; }
        //    public int Id { get; set; }
        //}

        //class Car : Base
        //{
        //    public string Foo { get; set; }
        //}

        //class Mc : Base
        //{
        //    public string Bar { get; set; }
        //}

        //class Bike : Base
        //{
        //    public string Bar { get; set; }
        //}

        //class Bus : Base
        //{
        //    public string Bar { get; set; }
        //}

        public override string ToString()
        {
            return this.RegNum;
        }
    }
}