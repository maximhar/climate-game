﻿using System;
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
                Console.ReadLine();
            }
        }
    }
}