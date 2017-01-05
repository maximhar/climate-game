using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClimateGame.Names;

namespace ClimateGame
{
    class ConstantCreator : IPopulationCreator
    {
        public DependentVariable<double> K { get; set; }
        public AgeRange Range { get; }

        public string Name { get; }

        public ConstantCreator(string name, double k, AgeRange ageRange)
        {
            Name = name;
            K = World.Instance.DependencyManager.CreateDouble(Mix(name, ParamK), k);
            Range = ageRange;
        }

        public Generation CreateGeneration(Generation gen)
        {
            double rate = Range.Contains(gen.Age) ? K : .0;
            double change = gen.Count * rate;
            return new Generation(World.Instance.Year, change);
        }
    }
}
