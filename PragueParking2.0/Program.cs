using PragueParking2._0;
using System;


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