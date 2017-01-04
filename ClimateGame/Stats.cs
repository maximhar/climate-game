using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            int columnsMax = Math.Min(80, Generations.Count);
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
            for (int i = rowsMax-1; i >= 0; i--)
            {
                for (int j = columnsMax-1; j >= 0; j--)
                {
                    if ((columnValues[j] / columnGens[j]) >= i)
                        row.Append("X");
                    else
                        row.Append(" ");
                }
                Console.WriteLine(row.ToString());
                row.Clear();
            }
            row.Append("0");
            row.Append(' ', columnsMax);
            row.Append(Generations.OrderBy(g => g.YearOfBirth).First().Age);
            Console.WriteLine(row.ToString());
        }
    }
}