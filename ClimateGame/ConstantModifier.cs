using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class ConstantModifier : IPopulationModifier
    {
        public DependentVariable<double> K { get; set; }
        public UInt32 StartAge { get; set; }
        public UInt32 EndAge { get; set; }

        public string Name { get; }

        public ConstantModifier(string name, double k, UInt32 startAge = 0, UInt32 endAge = UInt32.MaxValue)
        {
            Name = name;
            K = World.Instance.DependencyManager.GetDouble(name + ".K", k);
            StartAge = startAge;
            EndAge = endAge;
        }

        public Generation ModifyGeneration(Generation gen)
        {
            double rate = (gen.Age >= StartAge && gen.Age < EndAge) ? K : .0;
            double change = gen.Count * rate;
            return gen.AddCount(change);
        }
    }
}
