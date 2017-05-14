using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Jasily.Core;
using Jasily.Extensions.System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Linq
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// CancellationToken support for enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [PublicAPI]
        public static IEnumerable<T> EnumerateWithToken<T>([NotNull] this IEnumerable<T> source, CancellationToken token)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            IEnumerable<T> Iterator()
            {
                token.ThrowIfCancellationRequested();
                using (var enumerator = source.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                        token.ThrowIfCancellationRequested();
                    }
                }
            }

            return Iterator();
        }

        #region edit enumerable

        #region add

        [PublicAPI]
        public static IEnumerable<T> Append<T>([NotNull] this IEnumerable<T> source, T item, Position position = Position.End)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            switch (position)
            {
                case Position.Begin:
                    IEnumerable<T> StartIterator()
                    {
                        yield return item;
                        foreach (var z in source) yield return z;
                    }
                    return StartIterator();

                case Position.End:
                    IEnumerable<T> EndIterator()
                    {
                        foreach (var z in source) yield return z;
                        yield return item;
                    }
                    return EndIterator();

                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }
        }
        
        public static IEnumerable<T> Append<T>([NotNull] this IEnumerable<T> source, T item, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            IEnumerable<T> Iterator()
            {
                using (var itor = source.GetEnumerator())
                {
                    var i = 0;
                    while (i < index && itor.MoveNext())
                    {
                        yield return itor.Current;
                        i++;
                    }
                    yield return item;
                    while (itor.MoveNext()) yield return itor.Current;
                }
            }

            return Iterator();
        }

        #endregion

        #region insert or set

        public static IEnumerable<T> Insert<T>([NotNull] this IEnumerable<T> source, int index, T item)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            IEnumerable<T> Iterator()
            {
                var i = 0;
                using (var itor = source.GetEnumerator())
                {
                    while (i < index && itor.MoveNext())
                    {
                        yield return itor.Current;
                        i++;
                    }

                    if (i == index)
                    {
                        yield return item;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException(
                            $"source only contains {i} element. cannot insert into index <{index}>.");
                    }

                    while (itor.MoveNext())
                    {
                        yield return itor.Current;
                    }
                }
            };

            return Iterator();
        }

        public static IEnumerable<T> Set<T>([NotNull] this IEnumerable<T> source, int index, T item)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            IEnumerable<T> Iterator()
            {
                var i = 0;
                using (var itor = source.GetEnumerator())
                {
                    while (i < index && itor.MoveNext())
                    {
                        yield return itor.Current;
                        i++;
                    }

                    if (i == index && itor.MoveNext())
                    {
                        yield return item;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException(
                            $"source only contains {i} element. cannot set index <{index}>.");
                    }

                    while (itor.MoveNext())
                    {
                        yield return itor.Current;
                    }
                }
            };

            return Iterator();
        }

        #endregion

        #region join

        public static IEnumerable<T> JoinWith<T>(this IEnumerable<T> source, T spliter)
        {
            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) yield break;
                while (true)
                {
                    yield return itor.Current;
                    if (!itor.MoveNext()) yield break;
                    yield return spliter;
                }
            }
        }

        public static IEnumerable<T> JoinWith<T>(this IEnumerable<T> source, Func<T> spliterFunc)
        {
            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) yield break;
                while (true)
                {
                    yield return itor.Current;
                    if (!itor.MoveNext()) yield break;
                    yield return spliterFunc();
                }
            }
        }

        public static IEnumerable<T> JoinWith<T>(this IEnumerable<T> source, Action action)
        {
            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) yield break;
                while (true)
                {
                    yield return itor.Current;
                    if (!itor.MoveNext()) yield break;
                    action();
                }
            }
        }

        #endregion

        #endregion

        public static T Index<T>([NotNull] this IEnumerable<T> source, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            if (source is IList<T> list)
            {
                if (list.Count <= index) throw new IndexOutOfRangeException();
                return list[index];
            }

            if (source is IReadOnlyList<T> list2)
            {
                if (list2.Count <= index) throw new IndexOutOfRangeException();
                return list2[index];
            }

            using (var itor = source.Skip(index).GetEnumerator())
            {
                if (!itor.MoveNext()) throw new IndexOutOfRangeException();
                return itor.Current;
            }
        }

        private static IEnumerable<T> AppendToStartIterator<T>([NotNull] IEnumerable<T> source, T value)
        {
            yield return value;
            foreach (var item in source) yield return item;
        }

        public static IEnumerable<IEnumerable<TSource>> SplitChunks<TSource>([NotNull] this IEnumerable<TSource> source, int chunkSize)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (chunkSize < 1) throw new ArgumentOutOfRangeException(nameof(chunkSize), chunkSize, "must > 0.");
            
            if (chunkSize == 1)
            {
                foreach (var item in source) yield return new[] { item };
            }
            else // chunkSize > 1
            {
                var count = chunkSize - 1; // > 0
                using (var itor = source.GetEnumerator())
                {
                    while (itor.MoveNext())
                    {
                        var cur = itor.Current;
                        yield return AppendToStartIterator(itor.TakeIterator(count), cur);
                    }
                }
            }
        }

        #region null <=> empty

        [NotNull]
        public static IEnumerable<T> EmptyIfNull<T>([CanBeNull] this IEnumerable<T> enumerable)
            => enumerable ?? Enumerable.Empty<T>();

        [NotNull]
        public static T[] EmptyIfNull<T>([CanBeNull] this T[] array) => array ?? (T[])Enumerable.Empty<T>();

        [CanBeNull]
        public static T[] NullIfEmpty<T>([CanBeNull] this T[] item) => item == null || item.Length == 0 ? null : item;

        [CanBeNull]
        public static List<T> NullIfEmpty<T>([CanBeNull] this List<T> item) => item == null || item.Count == 0 ? null : item;

        #endregion

        #region random

        public static T RandomTake<T>([NotNull] this T[] source, [NotNull] Random random, bool throwIfEmpty = false)
            => (source as IList<T>).RandomTake(random, throwIfEmpty);

        public static T RandomTake<T>([NotNull] this List<T> source, [NotNull] Random random, bool throwIfEmpty = false)
            => (source as IList<T>).RandomTake(random, throwIfEmpty);

        public static T RandomTake<T>([NotNull] this IList<T> source, [NotNull] Random random, bool throwIfEmpty = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            switch (source.Count)
            {
                case 0:
                    if (throwIfEmpty) throw new InvalidOperationException();
                    return default(T);
                case 1: return source[0];
            }
            return source[random.Next(source.Count)];
        }

        public static T RandomTake<T>([NotNull] this IReadOnlyList<T> source, [NotNull] Random random, bool throwIfEmpty = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            switch (source.Count)
            {
                case 0:
                    if (throwIfEmpty) throw new InvalidOperationException();
                    return default(T);
                case 1: return source[0];
            }
            return source[random.Next(source.Count)];
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static T RandomTake<T>([NotNull] this IEnumerable<T> source, [NotNull] Random random, bool throwIfEmpty = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));

            switch (source)
            {
                case IList<T> li:
                    return li.RandomTake(random);

                case IReadOnlyList<T> li:
                    return li.RandomTake(random);

                default:
                    break;
            }

            var c = source.TryGetCount();
            if (c < 0)
            {
                return source.ToArray().RandomTake(random);
            }

            switch (c)
            {
                case 0:
                    if (throwIfEmpty) throw new InvalidOperationException();
                    return default(T);
                case 1:
                    return source.First();
                default:
                    return source.Skip(random.Next(c)).First();
            }
        }

        public static IEnumerable<T> RandomSort<T>([NotNull] this IEnumerable<T> source, [NotNull] Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            return (source as IList<T>)?.RandomSort(random) ??
                   (source as ICollection<T>)?.RandomSort(random) ??
                   (source.ToArray()).RandomSort(random);
        }

        public static IEnumerable<T> RandomSort<T>([NotNull] this ICollection<T> source, [NotNull] Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            var count = source.Count;
            if (count == 0) yield break;
            if (count == 1)
            {
                yield return source.First();
                yield break;
            }
            var array = Enumerable.Range(0, count).ToArray();
            var cache = new List<T>(count);
            using (var itor = source.GetEnumerator())
            {
                while (count > 0)
                {
                    var index = random.Next(count);
                    while (index >= cache.Count)
                    {
                        if (!itor.MoveNext()) throw new InvalidOperationException("source was changed");
                        cache.Add(itor.Current);
                    }
                    yield return cache[array[index]];
                    array[index] = array[count - 1];
                    count--;
                }
            }
        }

        public static IEnumerable<T> RandomSort<T>([NotNull] this IList<T> source, [NotNull] Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            var count = source.Count;
            if (count == 0) yield break;
            if (count == 1)
            {
                yield return source[0];
                yield break;
            }
            var array = Enumerable.Range(0, count).ToArray();
            while (count > 0)
            {
                var index = random.Next(count);
                yield return source[array[index]];
                array[index] = array[count - 1];
                count--;
            }
        }

        #endregion

        #region ignore Exception

        /// <summary>
        /// ignore some Exception from MoveNext() of Enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> IgnoreException<T, TException>([NotNull] this IEnumerable<T> source)
            where TException : Exception
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var itor = source.GetEnumerator())
            {
                while (true)
                {
                    bool moveNext;
                    try
                    {
                        moveNext = itor.MoveNext();
                    }
                    catch (TException)
                    {
                        continue;
                    }
                    if (moveNext)
                    {
                        yield return itor.Current;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }

        /// <summary>
        /// ignore some Exception from MoveNext() of Enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="source"></param>
        /// <param name="exceptionFilter"></param>
        /// <returns></returns>
        public static IEnumerable<T> IgnoreException<T, TException>([NotNull] this IEnumerable<T> source,
            [NotNull] Func<TException, bool> exceptionFilter)
            where TException : Exception
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (exceptionFilter == null) throw new ArgumentNullException(nameof(exceptionFilter));

            using (var itor = source.GetEnumerator())
            {
                while (true)
                {
                    bool moveNext;
                    try
                    {
                        moveNext = itor.MoveNext();
                    }
                    catch (TException error) when (exceptionFilter(error))
                    {
                        continue;
                    }
                    if (moveNext)
                    {
                        yield return itor.Current;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }

        #endregion

        public static int CopyToArray<T>([NotNull] this IEnumerable<T> source, [NotNull] T[] array, int arrayIndex, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (arrayIndex > array.Length || count > array.Length - arrayIndex) throw new ArgumentException();

            var i = arrayIndex;
            foreach (var item in source.Take(count))
            {
                array[i++] = item;
            }
            return i - arrayIndex;
        }

        public static IEnumerable<T> TakeLast<T>([NotNull] this IEnumerable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) return Empty<T>.Array;

            switch (source)
            {
                case T[] array:
                    return array.Length < count ? array.AsReadOnly() : array.Skip(count - array.Length);

                case List<T> list:
                    return list.Count < count ? list.AsReadOnly() : list.Skip(count - list.Count);

                default:
                    break;
            }

            var q = new Queue<T>(count);
            foreach (var item in source)
            {
                if (q.Count == count)
                    q.Dequeue();
                q.Enqueue(item);
            }
            return q.AsReadOnly();
        }

        public static IEnumerable<T> Pipe<T>([NotNull] this IEnumerable<T> source, [NotNull]  Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));
            foreach (var element in source)
            {
                action(element);
                yield return element;
            }
        }
    }
}