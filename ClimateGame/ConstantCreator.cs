using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class ConstantCreator : IPopulationCreator
    {
        private double k;
        private UInt32 startAge;
        private UInt32 endAge;

        public string Name { get; }

        public ConstantCreator(string name, double k, UInt32 startAge = 0, UInt32 endAge = UInt32.MaxValue)
        {
            Name = name;
            this.k = k;
            this.startAge = startAge;
            this.endAge = endAge;
        }

        public Generation CreateGeneration(Generation gen)
        {
            double rate = (gen.Age >= startAge && gen.Age < endAge) ? k : .0;
            Int64 change = (Int64)Math.Round(gen.Count * rate);
            return new Generation(World.Instance.Year, change);
        }
    }
}
