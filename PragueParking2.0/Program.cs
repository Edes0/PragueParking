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
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            while (menu.Start())
            {
                
            }
            Console.WriteLine("Program ended");
            Console.ReadKey();
        }
    }
}