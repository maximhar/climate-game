using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClimateGame.Names;

namespace ClimateGame
{
    class ExponentialModifier : IPopulationModifier
    {
        // Linear coefficient
        public DependentVariable<double> K { get; set; }
        // Exponential coefficient
        public DependentVariable<double> K2 { get; set; }
        // Offset (shifts the function left/right, but leaves slope shape the same)
        public DependentVariable<double> O { get; set; }

        public AgeRange Range { get; }

        public string Name { get; }

        public ExponentialModifier(String name, double k, double k2, double o, AgeRange ageRange)
        {
            Name = Name;
            K = World.Instance.DependencyManager.CreateDouble(Mix(name, ParamK), k);
            K2 = World.Instance.DependencyManager.CreateDouble(Mix(name, ParamK2), k2);
            O = World.Instance.DependencyManager.CreateDouble(Mix(name, ParamO), o);
            Range = ageRange;
        }

        public Generation ModifyGeneration(Generation gen)
        {
            double rate = Range.Contains(gen.Age) ? 
                K * Math.Exp(K2 * (gen.Age - O)) : .0;
            double change = gen.Count * rate;
            return gen.AddCount(change);
        }
    }
}
