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

            // VG UPPGIFTERNA
            //Parking[1] = "ABC123-C";    //----------------------------- Helps with testing
            //Parking[2] = "AAA111-MC/AAA222-MC";
            //Parking[3] = "APA123-C";

            //Parking[1] = "ABC123-MC";    //---------------------------- Helps with testing
            //Parking[2] = "AAA111-MC";
            //Parking[3] = "APA123-MC";
            //Parking[4] = "AAA222-MC";
            //Parking[5] = "AAA333-MC";

            while (true)
            {
                Menu();
            }
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
            Console.WriteLine();
            Console.Write("Input digit: ");



            try
            {
                int userInput = int.Parse(Console.ReadLine());

                switch (userInput)
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
                int userInput = int.Parse(Console.ReadLine());

                switch (userInput)
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

                string new_Reg = Console.ReadLine().ToUpper();

                new_Reg = new_Reg.Replace(" ", "");

                if (PlateIsOk(new_Reg)) //---------------------------- Check if licence registration plate is correct format
                {
                    new_Reg += "-C"; // Car identifier

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
                                Parking[i] = new_Reg;
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
                string new_Reg = Console.ReadLine().ToUpper();

                new_Reg = new_Reg.Replace(" ", "");

                Console.Clear();

                if (PlateIsOk(new_Reg)) //---------------------------- Check if licence registration plate is in correct format
                {
                    new_Reg += "-MC"; //---------------------------- Mc identifier

                    if (Search(new_Reg)) //---------------------------- No duplicates
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
                            if (isAloneMc(i)) //---------------------------- Looking for an alone MC, if finds, then do double parking
                            {
                                Parking[i] += "/" + new_Reg;   // Format is: ABC123-MC/ABC321-MC
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine("______________________________\n");
                                ParkTimes[i + 1] = DateTime.Now;
                                Console.WriteLine("Parked at time: " + ParkTimes[i + 1]);
                                Console.WriteLine("Vehicle (" + new_Reg + ") is registred.\nProceed to parking space " + i + ".\n");
                                Console.WriteLine("______________________________\n");
                                Console.ResetColor();
                                break;
                            }
                        }
                        if (!Search(new_Reg)) //---------------------------- If no double parking, find null instead
                        {
                            for (int i = 1; i < Parking.Length; i++)
                            {
                                if (Parking[i] == null)
                                {
                                    Parking[i] = new_Reg;
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine("______________________________\n");
                                    ParkTimes[i] = DateTime.Now;
                                    Console.WriteLine("Parked at time: " + ParkTimes[i]);
                                    Console.WriteLine("Vehicle (" + new_Reg + ") is registred.\nProceed to parking space " + i + ".\n");
                                    Console.WriteLine("______________________________\n");
                                    Console.ResetColor();
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
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message + "\n");
                Console.ResetColor();
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
                string old_Reg = Console.ReadLine().ToUpper();

                if (Search(old_Reg)) //---------------------------- Searching for plate in Parking array
                {
                    Console.Clear();

                    int index = FindIndex(old_Reg); //---------------------------- Finds old_Reg index in Parking array

                    if (isTwoMc(index)) //---------------------------- Check out with double parking
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
                string old_Reg = Console.ReadLine().ToUpper();

                if (Search(old_Reg)) //---------------------------- Seaching for old_Reg in Parking array
                {

                    Console.Clear();
                    Console.WriteLine("-Move-");
                    Console.WriteLine();
                    Console.Write("Enter new parking spot: ");

                    int index = FindIndex(old_Reg);
                    int n; bool parseSuccess = int.TryParse(Console.ReadLine(), out n);

                    if (parseSuccess)
                    {
                        if (isTwoMc(index) && Parking[n] == null) //---------------------------- Two mc --> null
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("______________________________\n");
                            Parking[n] = old_Reg + "-MC";
                            Parking[index] = Parking[index].Replace(old_Reg + "-MC", "");
                            Parking[index] = Parking[index].Replace("/", "");
                            Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                            Console.WriteLine("______________________________\n");
                            Console.ResetColor();
                        }
                        else if (isAloneMc(index) && Parking[n] == null) //---------------------------- Mc --> null
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("______________________________\n");
                            Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                            Parking[n] = old_Reg + "-MC";
                            Parking[index] = null;
                            Console.WriteLine("______________________________\n");
                            Console.ResetColor();
                        }
                        else if (isTwoMc(index) && isAloneMc(n)) //---------------------------- Two mc --> mc
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("______________________________\n");
                            Parking[n] += "/" + old_Reg + "-MC";
                            Parking[index] = Parking[index].Replace(old_Reg + "-MC", "");
                            Parking[index] = Parking[index].Replace("/", "");
                            Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                            Console.WriteLine("______________________________\n");
                            Console.ResetColor();
                        }
                        else if (isAloneMc(index) && isAloneMc(n)) //---------------------------- Mc --> mc
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("______________________________\n");
                            Parking[n] += "/" + old_Reg + "-MC";
                            Parking[index] = null;
                            Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                            Console.WriteLine("______________________________\n");
                            Console.ResetColor();
                        }
                        else if (isCar(index) && Parking[n] == null) //---------------------------- C --> null
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("______________________________\n");
                            Parking[n] = old_Reg + "-C";
                            Parking[index] = null;
                            Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                            Console.WriteLine("______________________________\n");
                            Console.ResetColor();
                        }
                        else if (Parking[n] == Parking[index]) //---------------------------- Move to same spot error
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Vehicle (" + Parking[index] + ") is already parked here\n");
                            Console.ResetColor();
                        }
                        else //---------------------------- Moving to non avaliable spot error
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Cannot move to this spot\n");
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
        } //-------------------------------------- Moving vehicle from index to index in Parking array
        static void Search()
        {
            Console.Clear();

            Console.WriteLine("-Search-");
            Console.WriteLine();
            Console.Write("Enter licence plate number: ");

            try
            {

                string old_Reg = Console.ReadLine().ToUpper();

                int index = FindIndex(old_Reg);

                if (Search(old_Reg))
                {
                    Console.Clear();
                    Console.WriteLine("Vehicle (" + old_Reg + ") is parked at " + index + "\n");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Vehicle (" + old_Reg + ") is not parked here.\n");
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message + "\n");
            }
        } //------------------------------------ Looking for vehicle in Parking array
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
                else if (isAloneMc(i))
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
                if (isAloneMc(i)) // ONE MC
                {
                    int firstMcFound = i;

                    for (int y = (i + 1); y < Parking.Length; y++)
                    {

                        if (isAloneMc(y)) // ONE MC
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

            int userInput = int.Parse(Console.ReadLine());

            if (userInput == 2)
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
        static bool Search(string regnum)
        {
            regnum.ToUpper();

            for (int i = 1; i < Parking.Length; i++)
            {
                if (Parking[i] != null && Parking[i].Contains(regnum))
                {
                    return true;
                }
            }
            return false;
        } //----------------------- Searching for vehicle registration plate in Parking array
        static int FindIndex(string regnum)
        {
            regnum.ToUpper();

            for (int i = 1; i < Parking.Length; i++)
            {
                if (Parking[i] != null && Parking[i].Contains(regnum))
                {
                    int Index = i;
                    return Index;
                }
            }
            return 0;
        } //--------------------- Searching for index of vehicle in Parking array
        static bool isAloneMc(int index)
        {
            if (Parking[index] != null && !Parking[index].Contains("/") && Parking[index].Contains("-MC")) // ONE MC
            {
                return true;
            }
            else
            {
                return false;
            }
        }  //----------------------- Identify alone MC in Parking array index
        static bool isTwoMc(int index)
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
        static bool isCar(int index)
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
    }

}