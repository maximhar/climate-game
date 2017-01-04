using System;

namespace ClimateGame
{
    public class DependentVariable<T> : ITickable
    {
        private Func<DependentVariable<T>, T> source;
        private bool evaluated = false;

        public string Name { get; }
        public T LastValue { get; private set; }

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
    }
}