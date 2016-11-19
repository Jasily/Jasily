using System.Collections.ObjectModel;
using System.Diagnostics;
using JetBrains.Annotations;

namespace System.Collections.Generic
{
    public static class ListExtensions
    {
        public static IReadOnlyList<T> AsReadOnly<T>([NotNull] this IList<T> list) => new ReadOnlyCollection<T>(list);

        /// <summary>
        /// return a simple synchronized list which copy from http://referencesource.microsoft.com/.
        /// if you want to use better solution, use System.Collections.Concurrent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> AsSynchronized<T>([NotNull] IList<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            return new SynchronizedList<T>(list);
        }

        private class SynchronizedList<T> : IList<T>, ICollection
        {
            private readonly IList<T> list;

            internal SynchronizedList(IList<T> list)
            {
                Debug.Assert(list != null);
                this.list = list;
                this.SyncRoot = (list as ICollection)?.SyncRoot ?? new object();
            }

            public void CopyTo(Array array, int index)
            {
                lock (this.SyncRoot)
                {
                    this.list.CopyTo(array, index);
                }
            }

            public int Count
            {
                get
                {
                    lock (this.SyncRoot)
                    {
                        return this.list.Count;
                    }
                }
            }

            public bool IsSynchronized => true;

            public object SyncRoot { get; }

            public bool IsReadOnly => this.list.IsReadOnly;

            public void Add(T item)
            {
                lock (this.SyncRoot)
                {
                    this.list.Add(item);
                }
            }

            public void Clear()
            {
                lock (this.SyncRoot)
                {
                    this.list.Clear();
                }
            }

            public bool Contains(T item)
            {
                lock (this.SyncRoot)
                {
                    return this.list.Contains(item);
                }
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                lock (this.SyncRoot)
                {
                    this.list.CopyTo(array, arrayIndex);
                }
            }

            public bool Remove(T item)
            {
                lock (this.SyncRoot)
                {
                    return this.list.Remove(item);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                lock (this.SyncRoot)
                {
                    return this.list.GetEnumerator();
                }
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                lock (this.SyncRoot)
                {
                    return this.list.GetEnumerator();
                }
            }

            public T this[int index]
            {
                get
                {
                    lock (this.SyncRoot)
                    {
                        return this.list[index];
                    }
                }
                set
                {
                    lock (this.SyncRoot)
                    {
                        this.list[index] = value;
                    }
                }
            }

            public int IndexOf(T item)
            {
                lock (this.SyncRoot)
                {
                    return this.list.IndexOf(item);
                }
            }

            public void Insert(int index, T item)
            {
                lock (this.SyncRoot)
                {
                    this.list.Insert(index, item);
                }
            }

            public void RemoveAt(int index)
            {
                lock (this.SyncRoot)
                {
                    this.list.RemoveAt(index);
                }
            }
        }
    }
}