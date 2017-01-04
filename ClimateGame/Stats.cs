using System;

namespace ClimateGame
{
    internal class Stats
    {
        public double Population { get; set; }

        internal void Print()
        {
            Console.WriteLine($"World population: {Math.Round(Population)}");
        }
    }
}