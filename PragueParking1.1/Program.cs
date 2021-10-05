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
            //Parking[1] = "ABC123-C";    //-----------------------------testing
            //Parking[2] = "AAA111-MC/AAA222-MC";
            //Parking[3] = "APA123-C";

            //Parking[1] = "ABC123-MC";    //-----------------------------testing
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
                        stackMc();
                        break;

                    case 7:
                        Restart();
                        break;

                    default:
                        Error();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
            }
        }
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
                        ParkMC();
                        break;

                    case 0:
                        Console.Clear();
                        Menu();
                        break;

                    default:
                        Error();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
            }
        }
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


                if (plateIsOk(new_Reg))
                {
                    new_Reg += "-C"; // CAR IDENTIFIER

                    for (int i = 1; i < Parking.Length; i++)
                    {
                        if (Parking[i] == null)
                        {
                            Parking[i] = new_Reg;
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("______________________________");
                            Console.WriteLine();
                            ParkTimes[i] = DateTime.Now;
                            Console.WriteLine("Parked at time: " + ParkTimes[i]);
                            Console.WriteLine("Vehicle (" + Parking[i] + ") is registred.\nProceed to parking space " + i + ".");
                            Console.WriteLine("______________________________");
                            Console.WriteLine();
                            Console.ResetColor();
                            break;
                        }
                        else if (Parking.Contains(new_Reg))
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Vehicle (" + new_Reg + ") is already registered");
                            Console.WriteLine();
                            Console.ResetColor();
                            break;
                        }
                        else if (!Parking.Contains(null))
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("No slot available");
                            Console.WriteLine();
                            Console.ResetColor();
                            break;
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Error: Requirement 4-10 digits [A-Z / 0-9]");
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
        static void ParkMC()
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

                if (plateIsOk(new_Reg))
                {
                    new_Reg += "-MC";

                    if (Search(new_Reg))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Vehicle (" + new_Reg + ") is already registered");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                    else if (!Parking.Contains(null))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("No slot available");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                    else
                    {
                        for (int i = 1; i < Parking.Length; i++)
                        {
                            if (isAloneMc(i))
                            {
                                Parking[i] += "/" + new_Reg;   // ABC123-MC/ABC321-MC
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine("______________________________");
                                Console.WriteLine();
                                ParkTimes[i+1] = DateTime.Now;
                                Console.WriteLine("Parked at time: " + ParkTimes[i+1]);
                                Console.WriteLine("Vehicle (" + new_Reg + ") is registred.\nProceed to parking space " + i + ".");
                                Console.WriteLine();
                                Console.WriteLine("______________________________");
                                Console.WriteLine();
                                Console.ResetColor();
                                break;
                            }
                        }
                        if (!Search(new_Reg))
                        {
                            for (int i = 1; i < Parking.Length; i++)
                            {
                                if (Parking[i] == null)
                                {
                                    Parking[i] = new_Reg;
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine("______________________________");
                                    Console.WriteLine();
                                    ParkTimes[i] = DateTime.Now;
                                    Console.WriteLine("Parked at time: " + ParkTimes[i]);
                                    Console.WriteLine("Vehicle (" + new_Reg + ") is registred.\nProceed to parking space " + i + ".");
                                    Console.WriteLine();
                                    Console.WriteLine("______________________________");
                                    Console.WriteLine();
                                    Console.ResetColor();
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Error();
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.ResetColor();
            }
        }
        static void CheckOut()
        {
            Console.Clear();
            Console.WriteLine("-Check out-");
            Console.WriteLine();
            Console.Write("Enter licence plate number: ");


            try
            {
                string old_Reg = Console.ReadLine().ToUpper();

                if (Search(old_Reg))
                {
                    Console.Clear();

                    int index = findIndex(old_Reg);

                    if (isTwoMc(index))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Vehicle (" + old_Reg + ") is checked out.");
                        Console.WriteLine();
                        Console.ResetColor();
                        Parking[index] = Parking[index].Replace(old_Reg + "-MC", "");
                        Parking[index] = Parking[index].Replace("/", "");

                    }
                    else
                    {
                        DateTime timeNow = DateTime.Now;
                        TimeSpan timeDiff = timeNow.Subtract(ParkTimes[index]);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Parking duration: " + timeDiff.ToString(@"hh\:mm\:ss"));
                        Console.WriteLine("Vehicle (" + old_Reg + ") is checked out.");
                        Console.WriteLine();
                        Console.ResetColor();

                        
                        Console.WriteLine();
                        Console.WriteLine(); 
                        Parking[index] = null;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Vehicle (" + old_Reg + ") is not parked here.");
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.ResetColor();
            }
        }
        static void Move()
        {

            Console.Clear();
            Console.WriteLine("-Move-");
            Console.WriteLine();
            Console.Write("Enter licence plate number: ");

            try
            {
                string old_Reg = Console.ReadLine().ToUpper();

                if (Search(old_Reg))
                {

                    Console.Clear();
                    Console.WriteLine("-Move-");
                    Console.WriteLine();
                    Console.Write("Enter new parking spot: ");

                    int index = findIndex(old_Reg);
                    int n = int.Parse(Console.ReadLine());

                    if (isTwoMc(index) && Parking[n] == null) // MOVE 2MC TO NULL
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("______________________________");
                        Console.WriteLine();
                        Parking[n] = old_Reg + "-MC";
                        Parking[index] = Parking[index].Replace(old_Reg + "-MC", "");
                        Parking[index] = Parking[index].Replace("/", "");
                        Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                        Console.WriteLine("______________________________");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                    else if (isAloneMc(index) && Parking[n] == null) // MOVE MC TO NULL
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("______________________________");
                        Console.WriteLine();
                        Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                        Parking[n] = old_Reg + "-MC";
                        Parking[index] = null;
                        Console.WriteLine("______________________________");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                    else if (isTwoMc(index) && isAloneMc(n)) // MOVE 2MC TO MC
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("______________________________");
                        Console.WriteLine();
                        Console.WriteLine();
                        Parking[n] += "/" + old_Reg + "-MC";
                        Parking[index] = Parking[index].Replace(old_Reg + "-MC", "");
                        Parking[index] = Parking[index].Replace("/", "");
                        Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                        Console.WriteLine("______________________________");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                    else if (isAloneMc(index) && isAloneMc(n)) // MOVE MC TO MC
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("______________________________");
                        Console.WriteLine();
                        Parking[n] += "/" + old_Reg + "-MC";
                        Parking[index] = null;
                        Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                        Console.WriteLine("______________________________");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                    else if (isCar(index) && Parking[n] == null) // MOVE C TO NULL
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("______________________________");
                        Console.WriteLine();
                        Parking[n] = old_Reg + "-C";
                        Parking[index] = null;
                        Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                        Console.WriteLine("______________________________");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                    else if (Parking[n] == Parking[index]) // SAME SPOT ERROR
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Vehicle (" + Parking[index] + ") is already parked here");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Cannot move to this spot");
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                }
                else // !EXIST ERROR
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Vehicle (" + old_Reg + ") is not parked here.");
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
        static void Search()
        {
            Console.Clear();

            Console.WriteLine("-Search-");
            Console.WriteLine();
            Console.Write("Enter licence plate number: ");

            try
            {

                string old_Reg = Console.ReadLine().ToUpper();

                int index = findIndex(old_Reg);

                if (Search(old_Reg))
                {
                    Console.Clear();
                    Console.WriteLine("Vehicle (" + old_Reg + ") is parked at " + index);
                    Console.WriteLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Vehicle (" + old_Reg + ") is not parked here.");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
            }
        }
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
        }
        static int findIndex(string regnum)
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
        }
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
        }
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
        }
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
        }
        static bool plateIsOk(string regnum)
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
        }
        static void stackMc()
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
                            Console.WriteLine("______________________________");
                            Console.WriteLine();
                            Console.WriteLine("Vehicle (" + Parking[firstMcFound] + ") is moved to parking spot " + firstMcFound);
                            Console.WriteLine();
                            Console.WriteLine("______________________________");
                            Console.WriteLine();
                            Console.ResetColor();
                            Parking[y] = null;
                            break;
                        }
                    }
                }
            }
        }
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
        }
        static void Error()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Clear();
            Console.WriteLine("Error: Requirement 4-10 digits [A-Z / 0-9]");
            Console.WriteLine();
            Console.ResetColor();
        }
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
        }
    }

}