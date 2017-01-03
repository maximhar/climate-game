using System;

namespace ClimateGame
{
    internal class Stats
    {
        public Int64 Population { get; set; }

        internal void Print()
        {
            Console.WriteLine($"World population: {Population}");
        }
    }
}