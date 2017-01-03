using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class ExponentialModifier : IPopulationModifier
    {
        private double k;
        private double k2;
        private UInt32 startAge;
        private UInt32 endAge;

        public string Name { get; }

        public ExponentialModifier(String name, double k, double k2, UInt32 startAge = 0, UInt32 endAge = UInt32.MaxValue)
        {
            Name = Name;
            this.k = k;
            this.k2 = k2;
            this.startAge = startAge;
            this.endAge = endAge;
        }

        public Generation ModifyGeneration(Generation gen)
        {
            double rate = (gen.Age >= startAge && gen.Age < endAge) ? 
                k * Math.Exp(k2 * gen.Age - startAge) : .0;
            Int64 change = (Int64)Math.Round(gen.Count * rate);
            return gen.AddCount(change);
        }
    }
}
