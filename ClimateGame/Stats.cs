using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ClimateGame.Names;

namespace ClimateGame
{
    class Stats
    {
        public double Population { get; set; }
        public List<Generation> Generations { get; set; }

        public void Print()
        {
            Console.Clear();

            PrintPyramid();

            var dm = World.Instance.DependencyManager;
            Console.WriteLine($"Year: {World.Instance.Year}");

            Console.WriteLine($"Birth rate: {dm.GetDouble(Mix(Birth, ParamK)).Evaluate()}");
            Console.WriteLine($"Cancer rate: {dm.GetDouble(Mix(Cancer, ParamO)).Evaluate()}");
            Console.WriteLine($"Heart rate: {dm.GetDouble(Mix(Heart, ParamO)).Evaluate()}");
            Console.WriteLine($"Respiratory rate: {dm.GetDouble(Mix(Respiratory, ParamO)).Evaluate()}");
            Console.WriteLine($"Nervous rate: {dm.GetDouble(Mix(Nervous, ParamO)).Evaluate()}");

            Console.WriteLine($"Children: {Math.Round(dm.GetDouble(ChildPopulation)):N0}");
            Console.WriteLine($"Working: {Math.Round(dm.GetDouble(WorkingPopulation)):N0}");
            Console.WriteLine($"Elderly: {Math.Round(dm.GetDouble(ElderlyPopulation)):N0}");
            Console.WriteLine($"Total: {Math.Round(Population):N0}");

            var gdp = dm.GetDouble(GDP);
            var debt = dm.GetDouble(Debt);
            var inflation = dm.GetDouble(Inflation).Evaluate();
            var employment = dm.GetDouble(Employment).Evaluate();
            var ptc = dm.GetDouble(PTC).Evaluate();
            var pti = dm.GetDouble(PTI).Evaluate();
            var last = gdp.History.Count > 1 ? gdp.History[1] : 0;
            var growth = last != 0 ? (gdp.Evaluate() - last) / last : 0;
            Console.WriteLine("GDP: B${0:N}, Growth: {1:0.00%}", Math.Round(gdp.Evaluate()) / (1000*1000), growth);
            Console.WriteLine("GDP/capita: ${0:N}", Math.Round(1000 * gdp.Evaluate() / Population));
            Console.WriteLine("Debt: B${0:N}", Math.Round(debt.Evaluate()) / (1000 * 1000));
            Console.WriteLine("Inflation: {0:0.0%}, Employment: {1:0.0%}", inflation, employment);
            Console.WriteLine("PTI: {0:0.0%}, PTC: {1:0.0%}", pti, ptc);
        }

        private void PrintPyramid()
        {
            int columnsMax = Math.Min(100, Generations.Count);
            const int rowsMax = 30;
            float gensPerColumn = (float)Generations.Count / columnsMax;
            int popPerRow = (int)Math.Round(Generations.Max(g => g.Count) / (rowsMax-1));

            int[] columnValues = new int[columnsMax+1];
            int[] columnGens = new int[columnsMax + 1];


            {
                int i = 0;
                foreach (var gen in Generations)
                {
                    int currentColumn = (int)(i / gensPerColumn);

                    int rows = (int)Math.Round(gen.Count / popPerRow);
                    columnValues[currentColumn] += rows;
                    columnGens[currentColumn]++;

                    i++;
                }
            }

            StringBuilder row = new StringBuilder();
            row.Append(' ', columnsMax);
            row.Append(Generations.Max(g => g.Count));
            Console.WriteLine(row.ToString());
            row.Clear();
            for (int i = rowsMax-1; i >= 0; i--)
            {
                for (int j = columnsMax-1; j >= 0; j--)
                {
                    if ((columnValues[j] / columnGens[j]) >= i)
                        row.Append("█");
                    else
                        row.Append(" ");
                }
                row.Append('|');
                Console.WriteLine(row.ToString());
                row.Clear();
            }
            row.Append("0");
            row.Append('-', columnsMax-1);
            row.Append(Generations.OrderBy(g => g.YearOfBirth).First().Age);
            Console.WriteLine(row.ToString());
        }
    }
}