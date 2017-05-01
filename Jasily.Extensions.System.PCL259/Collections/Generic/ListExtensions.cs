using System.Collections.ObjectModel;
using System.Diagnostics;
using JetBrains.Annotations;

namespace System.Collections.Generic
{
    /// <summary>
    /// extensions for <see cref="List{T}"/> or <see cref="IList{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// create a <see cref="ReadOnlyCollection{T}"/> list as a readonly wrapper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        [PublicAPI]
        public static IReadOnlyList<T> AsReadOnly<T>([NotNull] this IList<T> list) => new ReadOnlyCollection<T>(list);

        /// <summary>
        /// return a simple synchronized list (use lock).
        /// if you looking for lockfree solution, try namespace `System.Collections.Concurrent`.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        [PublicAPI]
        public static IList<T> AsSynchronized<T>([NotNull] IList<T> list) => new SynchronizedList<T>(list);

        /// <summary>
        /// copy from http://referencesource.microsoft.com/.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class SynchronizedList<T> : IList<T>, ICollection
        {
            private readonly IList<T> _list;

            internal SynchronizedList(IList<T> list)
            {
                this._list = list ?? throw new ArgumentNullException(nameof(list));
                this.SyncRoot = (list as ICollection)?.SyncRoot ?? new object();
            }

            public void CopyTo(Array array, int index)
            {
                lock (this.SyncRoot)
                {
                    this._list.CopyTo(array, index);
                }
            }

            public int Count
            {
                get
                {
                    lock (this.SyncRoot)
                    {
                        return this._list.Count;
                    }
                }
            }

            public bool IsSynchronized => true;

            public object SyncRoot { get; }

            public bool IsReadOnly => this._list.IsReadOnly;

            public void Add(T item)
            {
                lock (this.SyncRoot)
                {
                    this._list.Add(item);
                }
            }

            public void Clear()
            {
                lock (this.SyncRoot)
                {
                    this._list.Clear();
                }
            }

            public bool Contains(T item)
            {
                lock (this.SyncRoot)
                {
                    return this._list.Contains(item);
                }
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                lock (this.SyncRoot)
                {
                    this._list.CopyTo(array, arrayIndex);
                }
            }

            public bool Remove(T item)
            {
                lock (this.SyncRoot)
                {
                    return this._list.Remove(item);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                lock (this.SyncRoot)
                {
                    return this._list.GetEnumerator();
                }
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                lock (this.SyncRoot)
                {
                    return this._list.GetEnumerator();
                }
            }

            public T this[int index]
            {
                get
                {
                    lock (this.SyncRoot)
                    {
                        return this._list[index];
                    }
                }
                set
                {
                    lock (this.SyncRoot)
                    {
                        this._list[index] = value;
                    }
                }
            }

            public int IndexOf(T item)
            {
                lock (this.SyncRoot)
                {
                    return this._list.IndexOf(item);
                }
            }

            public void Insert(int index, T item)
            {
                lock (this.SyncRoot)
                {
                    this._list.Insert(index, item);
                }
            }

            public void RemoveAt(int index)
            {
                lock (this.SyncRoot)
                {
                    this._list.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// copy items from data source, make list equals to data source.
        /// this method is helpful for some list which impl <see cref="Specialized.INotifyCollectionChanged"/> interface like <see cref="ObservableCollection&lt;"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        public static void MakeEqualsTo<T>([NotNull] this IList<T> list, [NotNull] IList<T> dataSource)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
            if (ReferenceEquals(list, dataSource)) throw new InvalidOperationException();

            var comparer = EqualityComparer<T>.Default;
            var listCount = list.Count;
            var dataSourceCount = dataSource.Count;
            var count = Math.Min(listCount, dataSourceCount);
            for (var i = 0; i < count; i++)
            {
                var item = dataSource[i];
                if (!comparer.Equals(item, list[i])) list[i] = item;
            }
            if (listCount > dataSourceCount)
            {
                do
                {
                    list.RemoveAt(listCount - 1);
                } while (--listCount > dataSourceCount);
            }
            else if (listCount < dataSourceCount)
            {
                do
                {
                    list.Add(dataSource[count]);
                } while (++count < dataSourceCount);
            }
        }
    }
}