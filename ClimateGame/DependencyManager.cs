using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class DependencyManager : ITickable
    {

        private Dictionary<string, DependentVariable<double>> dictDouble = new Dictionary<string, DependentVariable<double>>();

        public DependentVariable<double> GetDouble(string name, double initial = 0)
        {
            var dict = dictDouble;
            if (!dict.ContainsKey(name))
            {
                var dv = new DependentVariable<double>(name, initial);
                dict.Add(name, dv);
            }
            return dict[name];
        }

        public void Tick()
        {
            List<ITickable> dicts = new List<ITickable>();
            dicts.AddRange(dictDouble.Values);

            foreach(var tickable in dicts)
            {
                tickable.Tick();
            }
        }
    }
}
