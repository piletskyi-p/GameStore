using System.Collections.Generic;
using GameStore.Dal.Interfaces;

namespace GameStore.Dal.Filter
{
    public abstract class Pipeline<T>
    {
        protected readonly List<IFilter<T>> Filters = new List<IFilter<T>>();

        public Pipeline<T> Register(IFilter<T> filter)
        {
            Filters.Add(filter);
            return this;
        }

        public abstract T Process(T input);
    }
}