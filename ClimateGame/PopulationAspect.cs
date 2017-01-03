using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class PopulationAspect : IAspect
    {
        private Population population;
        private ModifierCollection popModifiers;
        private ConstantCreator popCreator;

        public void Initialize()
        {
            population = new Population(
                new List<Generation>
                {
                    new Generation(1999, 1000000),
                    new Generation(1998, 1000000),
                    new Generation(1997, 1000000),
                    new Generation(1996, 1000000)
                });

            popModifiers = new ModifierCollection("Modifiers", true,
                new List<IPopulationModifier>
                {
                    new ConstantModifier("Death", -0.2, 40)
                });

            popCreator = new ConstantCreator("Births", (double)(0.5*2.1) / (40 - 15), 15, 40);
        }

        public void Tick()
        {
            population.Create(popCreator);
            population.Modify(popModifiers);

            World.Instance.Stats.Population = population.Count;
        }

    }
}
