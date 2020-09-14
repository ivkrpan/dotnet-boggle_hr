using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using BoggleClientCLI.Boggle;
namespace BoggleClientCLI
{
    class Program
    {
        static Boggle.BoggleServiceClient BSC = new BoggleServiceClient();

        private static System.Timers.Timer aTimer;

        static bool ploca_prikzana = false;
        static string[] ploca = { };

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
 
            aTimer = new System.Timers.Timer(1000);

            try
            {
                if (BSC.preostaloVremena() >= 0)
                {
                    aTimer.Elapsed += OnTimedEvent;
                    aTimer.AutoReset = true;
                    aTimer.Enabled = true;

                    Console.WriteLine("Upišite start za početak nove igre!");

                    while (true)
                    {
                        string rijec;
                        rijec = Console.ReadLine().Trim().ToLower();
                        if (rijec.Length > 0)
                            if (ploca_prikzana == true)
                            {
                                if (rijec == "!restart")
                                {
                                    Console.Clear();
                                    BSC.napraviIgru();
                                    ploca_prikzana = false;
                                    continue;
                                }
                                string msg;
                                Console.SetCursorPosition(rijec.Length, Console.CursorTop - 1);
                                msg = BSC.provjeriRijec(rijec.ToLower());
                                Console.Write(" :: {0}\n", msg);
                            }
                            else
                            {
                                if (rijec == "start")
                                {
                                    Console.Clear();
                                    BSC.napraviIgru();
                                }
                            }
                        else
                            Console.SetCursorPosition(0, Console.CursorTop - 1);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Server nije dostupan!");
                Console.ReadLine();
            }
           
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
   
            int status = BSC.preostaloVremena();
            if (status >= 179) ploca_prikzana = false;
            if (status > 0)
            {

                if (ploca_prikzana == false)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine();
                    ploca = BSC.dohvatiSlova();
                    int n = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2}\t{3}\t{4}", ploca[n].ToUpper(), ploca[n+1].ToUpper(), ploca[n+2].ToUpper(), ploca[n+3].ToUpper(), ploca[n + 4].ToUpper());
                        Console.WriteLine();
                        n = n+5;
                    }
                    Console.WriteLine();
                    ploca_prikzana = true;
                }

                int oldx, oldy;
                oldx = Console.CursorLeft;
                oldy = Console.CursorTop;
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Preostalo " + status + " sec.   ");
              
                    Console.SetCursorPosition(oldx, oldy);

                if (status <= 10)
                {
                    Console.Beep();
                };

            }
            else
            {
                if (ploca_prikzana == true)
                {
                    Console.Clear();
                    Console.WriteLine("IGRAČI:");

                    foreach (KeyValuePair<string, string> kvp in BSC.popisRezultata())
                    {
                        Console.WriteLine(kvp.Key);
                        Console.SetCursorPosition(20, Console.CursorTop -1);
                        Console.WriteLine(kvp.Value);
                    }   
                    Console.WriteLine("Upišite start za početak nove igre!");
                    ploca_prikzana = false;
                }
            }

        }
    }
}
