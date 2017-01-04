using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class ExponentialModifier : IPopulationModifier
    {
        // Linear coefficient
        public double K { get; set; }
        // Exponential coefficient
        public double K2 { get; set; }
        // Offset (shifts the function left/right, but leaves slope shape the same)
        public double O { get; set; }

        public UInt32 StartAge { get; set; }
        public UInt32 EndAge { get; set; }

        public string Name { get; }

        public ExponentialModifier(String name, double k, double k2, double o, UInt32 startAge = 0, UInt32 endAge = UInt32.MaxValue)
        {
            Name = Name;
            K = k;
            K2 = k2;
            O = o;
            StartAge = startAge;
            EndAge = endAge;
        }

        public Generation ModifyGeneration(Generation gen)
        {
            double rate = (gen.Age >= StartAge && gen.Age < EndAge) ? 
                K * Math.Exp(K2 * (gen.Age - O)) : .0;
            double change = gen.Count * rate;
            return gen.AddCount(change);
        }
    }
}
