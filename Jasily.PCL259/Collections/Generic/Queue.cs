using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Jasily.Interfaces.Collections.Generic;

namespace Jasily.Collections.Generic
{
    public class Queue<T> : IQueue<T>, IReadOnlyCollection<T>
    {
        private Node headNode;
        private Node tailNode;

        private class Node
        {
            public Node nextNode;
            public T value;
        }

        public int Count { get; private set; }

        public void Enqueue(T item)
        {
            var node = new Node { value = item };
            if (this.tailNode == null)
            {
                Debug.Assert(this.headNode == null);
                this.headNode = this.tailNode = node;
            }
            else
            {
                this.tailNode.nextNode = node;
                this.tailNode = node;
            }
            this.Count++;
        }

        public T Dequeue()
        {
            if (this.Count == 0) throw new InvalidOperationException("Queue is empty.");
            Debug.Assert(this.headNode != null);
            Debug.Assert(this.tailNode != null);

            this.Count--;
            var result = this.headNode.value;
            this.headNode = this.headNode.nextNode;
            if (this.headNode == null)
            {
                this.tailNode = null;
            }
            return result;
        }

        public T Peek()
        {
            if (this.Count == 0) throw new InvalidOperationException("Queue is empty.");
            Debug.Assert(this.headNode != null);
            Debug.Assert(this.tailNode != null);
            return this.headNode.value;
        }

        public void Clear()
        {
            this.Count = 0;
            this.headNode = this.tailNode = null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = this.headNode;
            while (node != null)
            {
                yield return node.value;
                node = node.nextNode;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}