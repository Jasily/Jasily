namespace Jasily.Interfaces.Collections.Generic
{
    public interface IQueue<T>
    {
        void Enqueue(T item);

        T Dequeue();

        T Peek();

        int Count { get; }

        void Clear();
    }
}