using PragueParking2._0;
using PragueParking2._0.Vehicles;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;


namespace PragueParkingProgram
{
    class Program
    {
        ParkingHouse parkingHouse = new ParkingHouse();
        
        static void Main()
        {
            bool exit = false;

            while (!exit)
            {
                ParkingHouse.Menu();
                Console.ReadLine();

            }
        }
    }
}