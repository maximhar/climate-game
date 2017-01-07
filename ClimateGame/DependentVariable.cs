using System;
using System.Collections.Generic;

namespace ClimateGame
{
    public class DependentVariable<T> : ITickable
    {
        private Func<DependentVariable<T>, T> source;
        private bool evaluated = false;

        public string Name { get; }
        public T LastValue { get; private set; }

        public ValueHistory<T> History { get; set; }

        public DependentVariable(string name, T initial)
        {
            Name = name;
            LastValue = initial;
        }

        public void AttachSource(Func<DependentVariable<T>, T>  sourceFunc)
        {
            source = sourceFunc;
        }

        public T Evaluate()
        {
            if (!evaluated && source != null)
            {
                LastValue = source(this);
                evaluated = true;

                AddToHistory(LastValue);
            }

            return LastValue;
        }

        public void Tick()
        {
            evaluated = false;
        }

        public static implicit operator T(DependentVariable<T> dv)
        {
            return dv.Evaluate();
        }
        private void AddToHistory(T value)
        {
            if (History != null)
            {
                History.Add(value);
            }
        }
    }
}