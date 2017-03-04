using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Jasily.Interfaces.Collections.Generic
{
    public static class QueueExtensions
    {
        public static IQueue<T> AsQueue<T>([NotNull] this Queue<T> queue)
        {
            if (queue == null) throw new ArgumentNullException(nameof(queue));
            return new QueueWrapper<T>(queue);
        }

        private class QueueWrapper<T> : IQueue<T>
        {
            private readonly Queue<T> queue;

            public QueueWrapper(Queue<T> queue)
            {
                Debug.Assert(queue != null);
                this.queue = queue;
            }

            public IEnumerator<T> GetEnumerator() => this.queue.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public void CopyTo(Array array, int index) => this.queue.CopyTo(array, index);

            public void Clear() => this.queue.Clear();

            public void Enqueue(T item) => this.queue.Enqueue(item);

            public T Dequeue() => this.queue.Dequeue();

            public T Peek() => this.queue.Peek();

            public int Count => this.queue.Count;

            public bool IsSynchronized => ((ICollection) this.queue).IsSynchronized;

            public object SyncRoot => ((ICollection) this.queue).SyncRoot;
        }
    }
}