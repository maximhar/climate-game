using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class ModifierCollection : IPopulationModifier
    {
        private List<IPopulationModifier> modifiers = new List<IPopulationModifier>();
        private bool nonNegative;

        public string Name { get; }

        public ModifierCollection(String name, bool nonNegative, IEnumerable<IPopulationModifier> modifiers = null)
        {
            Name = name;
            this.nonNegative = nonNegative;

            if(modifiers != null)
            {
                this.modifiers.AddRange(modifiers);
            }
        }

        public IPopulationModifier this[int i]
        {
            get
            {
                return modifiers[i];
            }
            set
            {
                modifiers[i] = value;
            }
        }

        public void AddModifier(IPopulationModifier modifier)
        {
            modifiers.Add(modifier);
        }

        public void RemoveModifier(IPopulationModifier modifier)
        {
            modifiers.Remove(modifier);
        }

        public Generation ModifyGeneration(Generation gen)
        {
            double accDelta = 0;

            foreach(var modifier in modifiers)
            {
                var newGen = modifier.ModifyGeneration(gen);
                double delta = newGen.Count - gen.Count;
                accDelta += delta;
            }

            gen = gen.AddCount(accDelta);

            if (nonNegative)
                gen = gen.SetCount(Math.Max(0, gen.Count));

            return gen;
        }
    }
}
