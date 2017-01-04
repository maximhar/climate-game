using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class Population
    {
        private SortedDictionary<UInt32, Generation> generations = new SortedDictionary<uint, Generation>();

        public double Count => generations.Values.Sum(v => v.Count);

        public Population(IEnumerable<Generation> generations = null)
        {
            if (generations != null)
            {
                foreach(var gen in generations)
                    Add(gen);
            }
        }


        public void Create(IPopulationCreator creator)
        {
            var newGenerations = generations.Select(p => creator.CreateGeneration(p.Value)).ToList();

            foreach (var gen in newGenerations)
                Add(gen);

            Prune();
        }

        public void Modify(IPopulationModifier modifier)
        {
            var newGenerations = generations.Select(p => modifier.ModifyGeneration(p.Value)).ToList();
                
            foreach(var gen in newGenerations)
                Replace(gen);

            Prune();
        }

        private void Prune()
        {
            var emptyGenerations = generations.Where(p => p.Value.Count < 1).Select(p => p.Key).ToList();

            foreach(var key in emptyGenerations)
            {
                generations.Remove(key);
            }
        }
        
        private void Add(Generation gen)
        {
            if (generations.ContainsKey(gen.YearOfBirth))
                generations[gen.YearOfBirth] = generations[gen.YearOfBirth].AddCount(gen.Count);
            else
                generations.Add(gen.YearOfBirth, gen);
        }

        private void Replace(Generation gen)
        {
            generations[gen.YearOfBirth] = gen;
        }
    }
}
