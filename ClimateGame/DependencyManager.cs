using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClimateGame
{
    class DependencyManager : ITickable
    {

        private Dictionary<string, DependentVariable<double>> dictDouble = new Dictionary<string, DependentVariable<double>>();

        public DependentVariable<double> CreateDouble(string name, double initial = 0)
        {
            var dict = dictDouble;
            if (!dict.ContainsKey(name))
            {
                var dv = new DependentVariable<double>(name, initial);
                dict.Add(name, dv);
            }
            else
            {
                throw new DependentVariableAlreadyExistsException(name);
            }
            return dict[name];
        }

        public DependentVariable<double> GetDouble(string name)
        {
            var dict = dictDouble;
            if (!dict.ContainsKey(name))
            {
                throw new DependentVariableNotFoundException(name);
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

    class DependentVariableAlreadyExistsException : Exception
    {
        public DependentVariableAlreadyExistsException()
        {
        }

        public DependentVariableAlreadyExistsException(string message) : base(message)
        {
        }

        public DependentVariableAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    class DependentVariableNotFoundException : Exception
    {
        public DependentVariableNotFoundException()
        {
        }

        public DependentVariableNotFoundException(string message) : base(message)
        {
        }

        public DependentVariableNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
