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

        public static IQueue<T> AsQueue<T>([NotNull] this LinkedList<T> linkedList)
        {
            if (linkedList == null) throw new ArgumentNullException(nameof(linkedList));
            return new LinkedListWrapper<T>(linkedList);
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

        private class LinkedListWrapper<T> : IQueue<T>, IReadOnlyCollection<T>
        {
            private readonly LinkedList<T> list;

            public LinkedListWrapper(LinkedList<T> linkedList)
            {
                Debug.Assert(linkedList != null);
                this.list = linkedList;
            }

            public void CopyTo(Array array, int index) => ((ICollection)this.list).CopyTo(array, index);

            public int Count => this.list.Count;

            public bool IsSynchronized => ((ICollection)this.list).IsSynchronized;

            public object SyncRoot => ((ICollection) this.list).SyncRoot;

            public void Enqueue(T item) => this.list.AddLast(item);

            public T Dequeue()
            {
                if (this.Count == 0) throw new InvalidOperationException("Queue is empty.");
                var item = this.list.First.Value;
                this.list.RemoveFirst();
                return item;
            }

            public T Peek()
            {
                if (this.Count == 0) throw new InvalidOperationException("Queue is empty.");
                return this.list.First.Value;
            }

            public void Clear() => this.list.Clear();

            public IEnumerator<T> GetEnumerator() => this.list.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}