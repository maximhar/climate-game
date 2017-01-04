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
        private double k;
        // Exponential coefficient
        private double k2;
        // Offset (shifts the function left/right, but leaves slope shape the same)
        private double o;
        private UInt32 startAge;
        private UInt32 endAge;

        public string Name { get; }

        public ExponentialModifier(String name, double k, double k2, double o, UInt32 startAge = 0, UInt32 endAge = UInt32.MaxValue)
        {
            Name = Name;
            this.k = k;
            this.k2 = k2;
            this.o = o;
            this.startAge = startAge;
            this.endAge = endAge;
        }

        public Generation ModifyGeneration(Generation gen)
        {
            double rate = (gen.Age >= startAge && gen.Age < endAge) ? 
                k * Math.Exp(k2 * (gen.Age - o)) : .0;
            double change = gen.Count * rate;
            return gen.AddCount(change);
        }
    }
}
