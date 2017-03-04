using System.Collections;
using System.Collections.Generic;

namespace Jasily.Interfaces.Collections.Generic
{
    public interface IQueue<T> : IEnumerable<T>, ICollection
    {
        void Enqueue(T item);

        T Dequeue();

        T Peek();

        void Clear();
    }
}