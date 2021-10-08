using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;


namespace PragueParkingProgram
{
    class Program
    {
        static string[] Parking = new string[101];
        static DateTime[] ParkTimes = new DateTime[101];




        static void Main(string[] args)
        {
            // Prague Parking1.1
            // TODO: Better headlines 
            // TODO: TICKET
            // TODO: I MAIN. PARKING TICKET EFTER 00
            // TODO: Använd split

            // VG UPPGIFTERNA
            //----------------------------- Helps with testing
            Parking[2] = "AAA111-MC/AAA222-MC";
            Parking[3] = "AAA333-MC/AAA444-MC";
            Parking[4] = "APA123-MC";

            //Parking[1] = "ABC123-MC";    //---------------------------- Helps with testing
            //Parking[2] = "AAA111-MC";
            //Parking[3] = "APA123-MC";
            //Parking[4] = "AAA222-MC";
            //Parking[5] = "AAA333-MC";


            do
            {
                Menu();
            } while (Exist(1));
        }
        static void Menu()
        {
            Console.WriteLine("-Prague Parking-");
            Console.WriteLine();
            Console.WriteLine("[1] Park vehicle");
            Console.WriteLine("[2] Check out vehicle");
            Console.WriteLine("[3] Move vehicle");
            Console.WriteLine("[4] Search vehicle");
            Console.WriteLine("[5] Parking overview");
            Console.WriteLine("[6] Optimize");
            Console.WriteLine("[7] Restart");
            Console.WriteLine("[0] Exit");

            Console.WriteLine();
            Console.Write("Input digit: ");



            try
            {
                int regnummer = int.Parse(Console.ReadLine());

                switch (regnummer)
                {
                    case 1:
                        Park();
                        break;

                    case 2:
                        CheckOut();
                        break;

                    case 3:
                        Move();
                        break;

                    case 4:
                        Search();
                        break;

                    case 5:
                        Show();
                        break;

                    case 6:
                        StackMc();
                        break;

                    case 7:
                        Restart();
                        break;

                    case 0:
                        Exit(-1);
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Clear();
                        Console.WriteLine("Enter a valid number\n");
                        Console.ResetColor();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message + "\n");
                Console.ResetColor();
            }
        } //-------------------------------------- Switch menu --> Park || Checkout || Move || Search || Show || StackMc || Restart
        static void Park()
        {
            Console.Clear();
            Console.WriteLine("-Park-");
            Console.WriteLine();
            Console.WriteLine("[1] Car");
            Console.WriteLine("[2] Motorcycle");
            Console.WriteLine("[0] Back");
            Console.WriteLine();
            Console.Write("Input digit: ");

            try
            {
                int regnummer = int.Parse(Console.ReadLine());

                switch (regnummer)
                {
                    case 1:
                        Console.Clear();
                        ParkCar();
                        break;

                    case 2:
                        Console.Clear();
                        ParkMc();
                        break;

                    case 0:
                        Console.Clear();
                        Menu();
                        break;

                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Enter a valid number\n");
                        Console.ResetColor();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message + "\n");
                Console.ResetColor();
            }

        } //-------------------------------------- Switch menu --> ParkCar || ParkMc
        static void ParkCar()
        {
            Console.Clear();
            Console.WriteLine("-Park Car-");
            Console.WriteLine();
            Console.Write("Enter licence plate number: ");

            try
            {

                string new_Reg = Console.ReadLine().ToUpper(); /// TODO: GÖR TILL METOD BOY

                new_Reg = new_Reg.Replace(" ", "");  /// TODO: GÖR TILL METOD BOY

                if (PlateIsOk(new_Reg)) //---------------------------- Check if licence registration plate is correct format
                {

                    if (Parking.Contains(new_Reg)) //---------------------------- No duplicates
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Vehicle (" + new_Reg + ") is already registered\n");
                        Console.ResetColor();
                    }
                    else if (!Parking.Contains(null)) //---------------------------- When parking already full
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("No slot available\n");
                        Console.ResetColor();
                    }
                    else
                    {
                        for (int i = 1; i < Parking.Length; i++)
                        {
                            if (Parking[i] == null) //---------------------------- Find null index in Parking array and parks car
                            {
                                Parking[i] = new_Reg + "-C"; // Car identifier
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine("______________________________\n");
                                ParkTimes[i] = DateTime.Now;
                                Console.WriteLine("Parked at time: " + ParkTimes[i]);
                                Console.WriteLine("Vehicle (" + Parking[i] + ") is registred.\nProceed to parking space " + i + ".");
                                Console.WriteLine("______________________________\n");
                                Console.ResetColor();
                                break;
                            }
                        }
                    }
                }
                else //---------------------------- Wrong plate format
                {
                    Error();
                }
            }
            catch (Exception ex) //---------------------------- Error
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message + "\n");
                Console.ResetColor();
            }
        } //----------------------------------- Parking Car in Parking array
        static void ParkMc()
        {
            Console.Clear();
            Console.WriteLine("-Park Motorcycle-");
            Console.WriteLine();
            Console.Write("Enter licence plate number: ");

            try
            {
                string new_Reg = Console.ReadLine().ToUpper();  /// SAMMA HÄR

                new_Reg = new_Reg.Replace(" ", ""); /// samma här

                Console.Clear();   // in i samma metod

                if (PlateIsOk(new_Reg)) //---------------------------- Check if licence registration plate is in correct format
                {

                    if (Search(new_Reg, out int index)) //---------------------------- No duplicates
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Vehicle (" + new_Reg + ") is already registered\n");
                        Console.ResetColor();
                    }
                    else if (!Parking.Contains(null)) //---------------------------- When parking already full
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("No slot available\n");
                        Console.ResetColor();
                    }
                    else //---------------------------- If everything is fine so far
                    {

                        for (int i = 1; i < Parking.Length; i++)
                        {
                            if (IsOneMc(i)) //---------------------------- Looking for an alone MC, if finds, then do double parking
                            {
                                Parking[i] += "/" + new_Reg + "-MC";  // Format is: ABC123-MC/ABC321-MC
                                ParkTimes[i] = DateTime.Now;

                                PrintVehicleParkedMassage(new_Reg, i);
                                break;
                            }
                        }
                        if (!Search(new_Reg, out index)) //---------------------------- If no double parking, find null instead
                        {
                            for (int i = 1; i < Parking.Length; i++)
                            {
                                if (Parking[i] == null)
                                {
                                    Parking[i] = new_Reg + "-MC";
                                    ParkTimes[i] = DateTime.Now;
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                                    PrintVehicleParkedMassage(new_Reg, i);
                                    break;
                                }
                            }
                        }
                    }
                }
                else //---------------------------- Wrong plate format
                {
                    Error();
                }
            }
            catch (Exception ex) //---------------------------- Wrong format error
            {
                PrintExErrorMessage(ex.Message);
            }
        } //------------------------------------ Parking Mc in Parking array
        static void CheckOut()
        {
            Console.Clear();
            Console.WriteLine("-Check out-");
            Console.WriteLine();
            Console.Write("Enter licence plate number: ");


            try
            {
                string old_Reg = Console.ReadLine().ToUpper();   // samma här

                int index;

                if (Search(old_Reg, out index)) //---------------------------- Searching for plate in Parking array
                {
                    Console.Clear();

                    if (IsTwoMc(index)) //---------------------------- Check out with double parking
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Vehicle (" + old_Reg + ") is checked out.\n");
                        Console.ResetColor();
                        Parking[index] = Parking[index].Replace(old_Reg + "-MC", "");
                        Parking[index] = Parking[index].Replace("/", "");

                    }
                    else //---------------------------- Checkout
                    {
                        DateTime timeNow = DateTime.Now;
                        TimeSpan timeDiff = timeNow.Subtract(ParkTimes[index]);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Parking duration: " + timeDiff.ToString(@"hh\:mm\:ss"));
                        Console.WriteLine("Vehicle (" + old_Reg + ") is checked out.\n");
                        Console.ResetColor();
                        Parking[index] = null;
                    }
                }
                else //---------------------------- Didnt find old_Reg in Parking array
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Vehicle (" + old_Reg + ") is not parked here.\n");
                    Console.ResetColor();
                }
            }
            catch (Exception ex) //---------------------------- Error
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Clear();
                Console.WriteLine(ex.Message + "\n");
                Console.ResetColor();
            }
        } //---------------------------------- Checking out vehicle and changes value of index to null
        static void Move()
        {

            Console.Clear();
            Console.WriteLine("-Move-");
            Console.WriteLine();
            Console.Write("Enter licence plate number: ");

            try
            {
                string old_Reg = Console.ReadLine().ToUpper(); // samma här

                if (Search(old_Reg, out int index)) //---------------------------- Seaching for old_Reg in Parking array
                {

                    Console.Clear();
                    Console.WriteLine("-Move-");
                    Console.WriteLine();
                    Console.Write("Enter new parking spot: ");

                    bool parseSuccess = int.TryParse(Console.ReadLine(), out int n);

                    if (parseSuccess)
                    {
                        if (!ParkingIndexIsFull(n)) // Sätt IsTwoMc här också
                        {
                            string vehicleType = VehicleIdentifier(old_Reg);

                            if (vehicleType == "-MC") // Ta bort och sätt den i över
                            {
                                if (IsOneMc(n))
                                {
                                    string[] vehicle = Parking[index].Split("/");

                                    if (vehicle[0] == old_Reg + "-MC")
                                    {
                                        Parking[index] = vehicle[1];
                                        Parking[n] = Parking[n] + "/" + vehicle[0];

                                        PrintVehicleMovedMessage(old_Reg, n); //---------------------------- Prints out move message
                                    }
                                    else if (vehicle[1] == old_Reg + "-MC")
                                    {
                                        Parking[index] = vehicle[0];
                                        Parking[n] = Parking[n] + "/" + vehicle[1];

                                        PrintVehicleMovedMessage(old_Reg, n); //---------------------------- Prints out move message
                                    }
                                }
                                else
                                {
                                    string[] vehicle = Parking[index].Split("/");

                                    if (vehicle[0] == old_Reg + "-MC")
                                    {
                                        Parking[index] = vehicle[1];
                                        Parking[n] = vehicle[0];

                                        PrintVehicleMovedMessage(old_Reg, n); //---------------------------- Prints out move message
                                    }
                                    else if (vehicle[1] == old_Reg + "-MC")
                                    {
                                        Parking[index] = vehicle[0];
                                        Parking[n] = vehicle[1];

                                        PrintVehicleMovedMessage(old_Reg, n); //---------------------------- Prints out move message
                                    }
                                }
                            }
                            else if (vehicleType == "-C")
                            {
                                Parking[n] = Parking[index];
                                Parking[index] = null;

                                PrintVehicleMovedMessage(old_Reg, n); //---------------------------- Prints out move message
                            }
                        }
                        else //---------------------------- Moving to a full Parking spot
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Parking spot full\n");
                            Console.ResetColor();
                        }
                    }
                    else //---------------------------- TryParse failed
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Enter a valid parking spot\n");
                        Console.ResetColor();
                    }
                }
                else //---------------------------- Doesnt find old_Reg in Parking array
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Vehicle (" + old_Reg + ") is not parked here.\n");
                    Console.ResetColor();
                }
            }
            catch (Exception ex) //---------------------------- Error
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message + "\n");
                Console.ResetColor();
            }
        } //-------------------------------------- Moving vehicle from one index to another
        static void Search()
        {
            Console.Clear();
            Console.WriteLine("-Search-");
            Console.WriteLine();
            Console.Write("Enter licence plate number: ");

            string old_Reg = Console.ReadLine().ToUpper();

            if (Search(old_Reg, out int index))
            {
                Console.Clear();
                Console.WriteLine("Vehicle (" + old_Reg + ") is parked at " + index + "\n");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Vehicle (" + old_Reg + ") is not parked here.\n");
            }
        } //------------------------------------ Looking for vehicle in Parking array. 
        static bool Search(string regnum, out int index)
        {
            for (int i = 1; i < Parking.Length; i++)
            {
                if (Parking[i] != null)
                {
                    if (Parking[i].Contains("/"))
                    {

                        string[] vehicle = Parking[i].Split("/");

                        if (vehicle[0] == regnum + "-MC")
                        {
                            index = i;
                            return true;
                        }
                        else if (vehicle[1] == regnum + "-MC")
                        {
                            index = i;
                            return true;
                        }

                    }
                    else if (Parking[i] == regnum + "-MC")
                    {
                        index = i;
                        return true;
                    }
                    else if (Parking[i] == regnum + "-C")
                    {
                        index = i;
                        return true;
                    }

                }
            }
            index = -1;
            return false;
        } //-------- Searching for vehicle registration plate in Parking array. True if vehicle exists and outs index
        static void Show()
        {
            const int cols = 6;
            int n = 1;

            Console.Clear();
            for (int i = 1; i < Parking.Length; i++)
            {

                if (n >= cols && n % cols == 0)
                {
                    Console.WriteLine();
                    n = 1;
                }
                if (Parking[i] == null)
                {
                    Console.Write(i + ": Empty");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(" *");
                    Console.ResetColor();
                    Console.Write("\t");
                    n++;
                }
                else if (IsOneMc(i))
                {
                    Console.Write(i + ": " + Parking[i]);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" *");
                    Console.ResetColor();
                    Console.Write("\t");
                    n++;
                }
                else
                {
                    Console.Write(i + ": " + Parking[i]);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(" *");
                    Console.ResetColor();
                    Console.Write("\t");
                    n++;
                }

            }
            Console.WriteLine("\n\n... Press key to continue ...");
            Console.ReadKey();
            Console.Clear();
        } //-------------------------------------- Printing out overview of Parking
        static void StackMc()
        {
            Console.Clear();
            for (int i = 1; i < Parking.Length; i++)
            {
                if (IsOneMc(i)) // ONE MC
                {
                    int firstMcFound = i;

                    for (int y = (i + 1); y < Parking.Length; y++)
                    {

                        if (IsOneMc(y)) // ONE MC
                        {
                            Parking[firstMcFound] += "/" + Parking[y];   // ABC123-MC/ABC321-MC
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("______________________________\n");
                            Console.WriteLine("Vehicle (" + Parking[firstMcFound] + ") is moved to parking spot " + firstMcFound + "\n");
                            Console.WriteLine("______________________________\n");
                            Console.ResetColor();
                            Parking[y] = null;
                            break;
                        }
                    }
                }
            }
        } //----------------------------------- Stacking MCs together
        static void Restart()
        {
            Console.Clear();
            Console.WriteLine("Are you sure?");
            Console.WriteLine();
            Console.WriteLine("[1] Yes");
            Console.WriteLine("[2] No");
            Console.WriteLine();
            Console.Write("Input digit: ");

            int regnummer = int.Parse(Console.ReadLine());

            if (regnummer == 2)
            {
                Console.Clear();
            }
            else
            {
                Console.Clear();

                int n = 0;

                while (n != 2)
                {
                    n++;
                    foreach (char l in "Restarting")
                    {
                        Thread.Sleep(100);
                        Console.Write(l);
                    }
                    Thread.Sleep(1000);
                    Console.Clear();

                    for (int i = 1; i < Parking.Length; i++)
                    {
                        if (Parking[i] != null)
                        {
                            Parking[i] = null;
                        }
                    }
                }
                Console.Clear();
            }
        } //----------------------------------- Removing all vehicles from Parking
        static void Error()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Clear();
            Console.WriteLine("Error: Requirement 4-10 digits [A-Z / 0-9]\n");
            Console.ResetColor();
        } //------------------------------------- Plate format standard error
        static bool IsOneMc(int index)
        {
            if (Parking[index] != null && !Parking[index].Contains("/") && Parking[index].Contains("-MC")) // ONE MC
            {
                return true;
            }
            else
            {
                return false;
            }
        } //-------------------------- Identify alone MC in Parking array index
        static bool IsTwoMc(int index)
        {
            if (Parking[index] != null && Parking[index].Contains("/") && Parking[index].Contains("-MC"))
            {
                return true;
            }
            else
            {
                return false;
            }
        } //-------------------------- Identify two MC in Parking array index
        static bool IsCar(int index)
        {
            if (Parking[index].Contains("-C"))
            {
                return true;
            }
            else
            {
                return false;
            }
        } //---------------------------- Identify Car in Parking array index
        static bool ParkingIndexIsFull(int index)
        {
            if (Parking[index] == null)
            {
                return false;
            }
            if (Parking[index].Contains("/") || Parking[index].Contains("-C"))
            {
                return true;
            }
            return false;
        }
        static bool PlateIsOk(string regnum)
        {
            var hasNumber = new Regex(@"[0-9]\d{1}");
            var hasChar = new Regex(@"[a-zA-Z]\d{1}");
            bool hasLength = regnum.Length >= 4 && regnum.Length <= 10;

            if (hasNumber.IsMatch(regnum) && hasChar.IsMatch(regnum) && hasLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        } //-------------------- Check if registration plate number is valid
        static string VehicleIdentifier(string regnum)
        {
            for (int i = 1; i < Parking.Length; i++)
            {
                if (Parking[i] != null && Parking[i].Contains(regnum + "-C"))
                {
                    return "-C";
                }
                else if (Parking[i] != null && Parking[i].Contains(regnum + "-MC"))
                {
                    return "-MC";
                }
            }
            return "Error";
        }
        static void PrintVehicleMovedMessage(string regnum, int index)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("______________________________\n");
            Console.WriteLine("Vehicle (" + regnum + ") is moved to parking spot " + index);
            Console.WriteLine("______________________________\n");
            Console.ResetColor();
        }
        static void PrintVehicleParkedMassage(string regnum, int index)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("______________________________\n");
            Console.WriteLine("Parked at time: " + ParkTimes[index]);
            Console.WriteLine("Vehicle (" + regnum + ") is registred.\nProceed to parking space " + index + ".\n");
            Console.WriteLine("______________________________\n");
            Console.ResetColor();
        }
        static void PrintExErrorMessage(string ex)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(ex + "\n");
            Console.ResetColor();
        }
        static bool Exit(int i) // DENNA GÅR ALLTID. Skapa IF grejer
        {
            if (i = 1)
	{
                return true;
	}
            return false;
            
        } //-------------------------------------- returns Exit as false. Ends the program.
    }
}