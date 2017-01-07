using System;
using System.Collections;
using System.Collections.Generic;

namespace ClimateGame
{
    public class ValueHistory<T> : IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>
    {
        private int capacity;
        private List<T> values;

        public int Count => values.Count;

        public T this[int index] => values[index];

        public ValueHistory(int capacity)
        {
            this.capacity = capacity;
            values = new List<T>(capacity);
        }

        public void Add(T value)
        {
            values.Insert(0, value);
            if (values.Count > capacity)
                values.RemoveAt(values.Count - 1);
        }

        public IEnumerator<T> GetEnumerator() => values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();
    }
}