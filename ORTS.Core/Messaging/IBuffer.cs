using System.Collections.Generic;

namespace ORTS.Core.Messaging
{
    public interface IBuffer<T> : IEnumerable<T>
    {
        void Add(T item);
        int Count();
        void Clear();
    }
}
