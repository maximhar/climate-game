using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClimateGame.Names;

namespace ClimateGame
{
    class PopulationAspect : IAspect
    {
        private Population population;
        private ModifierCollection popModifiers;
        private ConstantCreator popCreator;

        private AgeRange childRange = new AgeRange(0, 18);
        private AgeRange workingRange = new AgeRange(18, 65);
        private AgeRange elderlyRange = new AgeRange(60);

        private DependentVariable<double> childCount;
        private DependentVariable<double> workingCount;
        private DependentVariable<double> elderlyCount;

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

            popModifiers = new ModifierCollection(PopulationModifiers, true,
                new List<IPopulationModifier>
                {
                     new ExponentialModifier(Cancer, -0.105, 0.08, 100.5, new AgeRange(0)),
                     new ExponentialModifier(Heart, -0.093, 0.12, 97.0, new AgeRange(0)),
                     new ExponentialModifier(Respiratory, -0.055, 0.09, 105.0, new AgeRange(0)),
                     new ExponentialModifier(Nervous, -0.031, 0.10, 105.0, new AgeRange(0)),
                     new ConstantModifier(RoadAccidents, -0.00004, new AgeRange(18)),
                     new ConstantModifier(WorkAccidents, -0.000005, workingRange),
                     new ConstantModifier(Childhood, -0.005, new AgeRange(0, 5)),
                     new ConstantModifier(Crime, -0.00001, new AgeRange(18))
                });

            popCreator = new ConstantCreator(Birth, (double)(0.5*2.1) / (40 - 15), new AgeRange(15, 40));
            
            childCount = World.Instance.DependencyManager.CreateDouble(ChildPopulation, 0);
            childCount.AttachSource(dv => population.Generations.Where(g => childRange.Contains(g.Age)).Select(g => g.Count).Sum());

            workingCount = World.Instance.DependencyManager.CreateDouble(WorkingPopulation, 0);
            workingCount.AttachSource(dv => population.Generations.Where(g => workingRange.Contains(g.Age)).Select(g => g.Count).Sum());

            elderlyCount = World.Instance.DependencyManager.CreateDouble(ElderlyPopulation, 0);
            elderlyCount.AttachSource(dv => population.Generations.Where(g => elderlyRange.Contains(g.Age)).Select(g => g.Count).Sum());
        }

        public void Tick()
        {
            population.Create(popCreator);
            population.Modify(popModifiers);

            World.Instance.Stats.Population = population.Count;
        }

    }
}
