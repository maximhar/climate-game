using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class ConstantModifier : IPopulationModifier
    {
        private double k;
        private UInt32 startAge;
        private UInt32 endAge;

        public string Name { get; }

        public ConstantModifier(string name, double k, UInt32 startAge = 0, UInt32 endAge = UInt32.MaxValue)
        {
            Name = name;
            this.k = k;
            this.startAge = startAge;
            this.endAge = endAge;
        }

        public Generation ModifyGeneration(Generation gen)
        {
            double rate = (gen.Age >= startAge && gen.Age < endAge) ? k : .0;
            Int64 change = (Int64) Math.Round(gen.Count * rate);
            return gen.AddCount(change);
        }
    }
}
