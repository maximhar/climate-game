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
            Console.WriteLine($"World population: {Math.Round(Population)}");
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
            var dm = World.Instance.DependencyManager;
            Console.WriteLine($"Birth rate: {dm.GetDouble(Mix(Birth, ParamK)).LastValue}");
            Console.WriteLine($"Cancer rate: {dm.GetDouble(Mix(Cancer, ParamO)).LastValue}");
            Console.WriteLine($"Heart rate: {dm.GetDouble(Mix(Heart, ParamO)).LastValue}");
            Console.WriteLine($"Respiratory rate: {dm.GetDouble(Mix(Respiratory, ParamO)).LastValue}");
            Console.WriteLine($"Nervous rate: {dm.GetDouble(Mix(Nervous, ParamO)).LastValue}");
            Console.WriteLine($"Year: {World.Instance.Year}");

        }
    }
}