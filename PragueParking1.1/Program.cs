using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;


namespace PragueParkingProgram
{
    class Program
    {
        static string[] Parking = new string[101];

        static void Main(string[] args)
        {

            // TODO: Better headlines 
            // TODO: TICKET
            // TODO: I MAIN. PARKING TICKET EFTER 00

            // VG UPPGIFTERNA
            Parking[1] = "ABC123-C";    //-----------------------------testing
            Parking[2] = "AAA111-MC/AAA222-MC";
            Parking[3] = "APA123-C";

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
            Console.WriteLine("[6] Restart");
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

                var hasNumber = new Regex(@"[0-9]");
                var hasChar = new Regex(@"[a-zA-Z]");
                bool hasLength = new_Reg.Length < 4 || new_Reg.Length <= 10;
                bool plateIsOk = hasNumber.IsMatch(new_Reg) && hasChar.IsMatch(new_Reg) && hasLength;


                //Console.WriteLine("hasnumber : " + hasNumber.IsMatch(new_Reg));//------------REGEX / BOOL CHECKER
                //Console.WriteLine("hasChar: " + hasChar.IsMatch(new_Reg));
                //Console.WriteLine("hasLength: " + hasLength);
                //Console.WriteLine();
                //Console.WriteLine("Plate :" + plateIsOk);

                //Console.ReadLine();

                new_Reg += "-C"; // CAR IDENTIFIER

                if (plateIsOk)
                {
                    for (int i = 1; i < Parking.Length; i++)
                    {
                        if (Parking[i] == null && !Parking.Contains(new_Reg))
                        {
                            Parking[i] = new_Reg;
                            Console.Clear();
                            Console.WriteLine("Vehicle (" + Parking[i] + ") is registred.\nProceed to parking space " + i + ".");
                            Console.WriteLine();
                            DateTime x = DateTime.Now; //TICKET BRO
                            // SKAPA TICKET HÄR
                            break;
                        }
                        else if (Parking.Contains(new_Reg))
                        {
                            Console.Clear();
                            Console.WriteLine("Vehicle (" + new_Reg + ") is already registered");
                            Console.WriteLine();
                            break;
                        }
                        else if (!Parking.Contains(null))
                        {
                            Console.Clear();
                            Console.WriteLine("No slot available");
                            Console.WriteLine();
                            break;
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Error: Requirement 4-10 digits [A-Z / 0-9]");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
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

                var hasNumber = new Regex(@"[0-9]");
                var hasChar = new Regex(@"[a-zA-Z]");
                bool hasLength = new_Reg.Length < 4 || new_Reg.Length <= 10;
                bool plateIsOk = hasNumber.IsMatch(new_Reg) && hasChar.IsMatch(new_Reg) && hasLength;


                //Console.WriteLine("hasnumber : " + hasNumber.IsMatch(new_Reg)); ------------- REGEX / BOOL CHECKER
                //Console.WriteLine("hasChar: " + hasChar.IsMatch(new_Reg));
                //Console.WriteLine("hasLength: " + hasLength);
                //Console.WriteLine();
                //Console.WriteLine("Plate :" + plateIsOk);

                //Console.ReadLine();
                new_Reg += "-MC";

                Console.Clear();

                if (plateIsOk)
                {
                    if (Search(new_Reg))
                    {
                        Console.WriteLine("Vehicle (" + new_Reg + ") is already registered");
                        Console.WriteLine();
                    }
                    else if (!Parking.Contains(null))
                    {
                        Console.WriteLine("No slot available");
                        Console.WriteLine();
                    }
                    else
                    {
                        for (int i = 1; i < Parking.Length; i++)
                        {
                            if (Parking[i] != null && Parking[i].Contains("-MC") && !Parking[i].Contains("/"))
                            {
                                Parking[i] += "/" + new_Reg;   // ABC123-MC/ABC321-MC
                                Console.WriteLine("Vehicle (" + new_Reg + ") is registred.\nProceed to parking space " + i + ".");
                                Console.WriteLine();
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
                                    Console.WriteLine("Vehicle (" + new_Reg + ") is registred.\nProceed to parking space " + i + ".");
                                    Console.WriteLine();
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
                Console.WriteLine(ex.Message);
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

                    if (Parking[index].Contains("/")) // 2 MC
                    {
                        Console.WriteLine("Vehicle (" + old_Reg + ") is checked out.");
                        Console.WriteLine();
                        Parking[index] = Parking[index].Replace(old_Reg + "-MC", "");
                        Parking[index] = Parking[index].Replace("/", "");

                    }
                    else
                    {
                        Console.WriteLine("Vehicle (" + old_Reg + ") is checked out.");
                        Console.WriteLine();
                        Parking[index] = null;
                    }
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

                    if (Parking[index].Contains("/") && Parking[n] == null) // MOVE 2MC TO NULL
                    {
                        Console.Clear();
                        Parking[n] = old_Reg + "-MC";
                        Parking[index] = Parking[index].Replace(old_Reg + "-MC", "");
                        Parking[index] = Parking[index].Replace("/", "");
                        Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                        Console.WriteLine();
                    }
                    else if (Parking[index].Contains("-MC") && Parking[n] == null) // MOVE MC TO NULL
                    {
                        Console.Clear();
                        Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                        Parking[n] = old_Reg + "-MC";
                        Parking[index] = null;
                        Console.WriteLine();
                    }
                    else if (Parking[n] != null && Parking[n].Contains("-MC") && !Parking[n].Contains("/") && Parking[index].Contains("/")) // MOVE 2MC TO MC
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Parking[n] += "/" + old_Reg + "-MC";
                        Parking[index] = Parking[index].Replace(old_Reg + "-MC", "");
                        Parking[index] = Parking[index].Replace("/", "");
                        Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                        Console.WriteLine();
                    }
                    else if (Parking[index] != null && !Parking[index].Contains("/") && !Parking[n].Contains("/") && Parking[index].Contains("-MC") && Parking[n].Contains("-MC")) // MOVE MC TO MC
                    {
                        Console.Clear();
                        Parking[n] += "/" + old_Reg + "-MC";
                        Parking[index] = null;
                        Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                        Console.WriteLine();
                    }
                    else if (Parking[index].Contains("-C") && Parking[n] == null) // MOVE C TO NULL
                    {
                        Console.Clear();
                        Parking[n] = old_Reg + "-C";
                        Parking[index] = null;
                        Console.WriteLine("Vehicle (" + old_Reg + ") is moved to parking spot " + n);
                        Console.WriteLine();
                    }
                    else if (Parking[n] == Parking[index]) // SAME SPOT ERROR
                    {
                        Console.Clear();
                        Console.WriteLine("Vehicle (" + Parking[index] + ") is already parked here");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Cannot move to this spot");
                        Console.WriteLine();
                    }
                }
                else // !EXIST ERROR
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
                    Console.Write(i + ": Empty \t");
                    n++;
                }
                else
                {
                    Console.Write(i + ": " + Parking[i] + "\t");
                    n++;
                }

            }
            Console.WriteLine("\n\n... Press key to continue ...");
            Console.ReadKey();
            Console.Clear();
        }
        static void Error()
        {
            Console.Clear();
            Console.WriteLine("Error: Requirement 4-10 digits [A-Z / 0-9]");
            Console.WriteLine();
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