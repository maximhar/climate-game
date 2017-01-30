using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class Program
    {
        static void Main(string[] args)
        {
            World.Instance.Initialize();
            while(true)
            {
                World.Instance.Tick();
                PrintCommands();
                string command = Console.ReadLine();
                while (command.Trim() != string.Empty)
                {
                    ExecuteCommand(command);
                    command = Console.ReadLine();
                }
            }
        }

        private static void PrintCommands()
        {
            Console.WriteLine("Possible commands:");
            Console.WriteLine("  GE %GDP: Set government expenditure");
            Console.WriteLine("  GT %GDP: Set government taxation");
        }

        private static void ExecuteCommand(string command)
        {
            string[] words = command.Split(' ');

            if (words.Length == 2)
            {
                if (words[0] == "GE")
                {
                    double governmentExpenditure = double.Parse(words[1]);
                    World.Instance.Government.Expenditure = governmentExpenditure / 100;
                    Console.WriteLine("Government expenditure set to {0:0.0%}.", 
                        World.Instance.Government.Expenditure);
                }
                if (words[0] == "GT")
                {
                    double governmentTaxation = double.Parse(words[1]);
                    World.Instance.Government.Taxation = governmentTaxation / 100;
                    Console.WriteLine("Government taxation set to {0:0.0%}.", 
                        World.Instance.Government.Taxation);
                }
            }
            else
            {
                Console.WriteLine("Invalid command length");
            }
        }
    }
}
