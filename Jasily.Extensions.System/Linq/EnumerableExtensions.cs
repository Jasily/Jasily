using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Jasily.Core;
using Jasily.Extensions.System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Linq
{
    /// <summary>
    /// extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Try direct get count if <paramref name="source"/> is 
        /// <see cref="ICollection{T}"/> or <see cref="ICollection"/> or <see cref="IReadOnlyCollection{T}"/>.
        /// This function do not enumerate any elements from <paramref name="source"/>.
        /// return -1 if cannot get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int TryDirectGetCount<T>([NotNull] this IEnumerable<T> source)
        {
            Debug.Assert(source != null);
            return (source as ICollection<T>)?.Count ??
                   (source as ICollection)?.Count ??
                   (source as IReadOnlyCollection<T>)?.Count ?? -1;
        }

        /// <summary>
        /// Try direct get count if <paramref name="source"/> is 
        /// <see cref="ICollection"/>.
        /// This function do not enumerate any elements from <paramref name="source"/>.
        /// return -1 if cannot get.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int TryDirectGetCount([NotNull] this IEnumerable source)
        {
            Debug.Assert(source != null);
            return (source as ICollection)?.Count ?? -1;
        }

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

        #region random take

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T InternalRandomTake<T>([NotNull] this IList<T> source, [NotNull] Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            switch (source.Count)
            {
                case 0: throw new InvalidOperationException();
                case 1: return source[0];
            }
            return source[random.Next(source.Count)];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T InternalRandomTake<T>([NotNull] this IReadOnlyList<T> source, [NotNull] Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            switch (source.Count)
            {
                case 0: throw new InvalidOperationException();
                case 1: return source[0];
            }
            return source[random.Next(source.Count)];
        }

        /// <summary>
        /// Random take one element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        public static T RandomTake<T>([NotNull] this T[] source, [NotNull] Random random)
        {
            return InternalRandomTake((IList<T>) source, random);
        }

        /// <summary>
        /// Random take one element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        public static T RandomTake<T>([NotNull] this List<T> source, [NotNull] Random random)
        {
            return InternalRandomTake((IList<T>)source, random);
        }

        /// <summary>
        /// Random take one element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        public static T RandomTake<T>([NotNull] this IList<T> source, [NotNull] Random random)
        {
            return InternalRandomTake(source, random);
        }

        /// <summary>
        /// Random take one element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        public static T RandomTake<T>([NotNull] this IReadOnlyList<T> source, [NotNull] Random random)
        {
            return InternalRandomTake(source, random);
        }

        /// <summary>
        /// Random take one element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty.</exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static T RandomTake<T>([NotNull] this IEnumerable<T> source, [NotNull] Random random)
        {
            switch (source)
            {
                case IList<T> li:
                    return li.RandomTake(random);

                case IReadOnlyList<T> li:
                    return li.RandomTake(random);
            }

            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));

            var c = source.TryDirectGetCount();
            return c < 0
                ? source.ToArray().RandomTake(random)
                : (c > 1 ? source.Skip(random.Next(c)) : source).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T InternalRandomTakeOrDefault<T>([NotNull] this IList<T> source, [NotNull] Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            switch (source.Count)
            {
                case 0: return default(T);
                case 1: return source[0];
            }
            return source[random.Next(source.Count)];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T InternalRandomTakeOrDefault<T>([NotNull] this IReadOnlyList<T> source, [NotNull] Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));
            switch (source.Count)
            {
                case 0: return default(T);
                case 1: return source[0];
            }
            return source[random.Next(source.Count)];
        }

        /// <summary>
        /// Random take one element or <see langword="default"/>(T) if <paramref name="source"/> is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        public static T RandomTakeOrDefault<T>([NotNull] this T[] source, [NotNull] Random random)
        {
            return InternalRandomTakeOrDefault((IList<T>)source, random);
        }

        /// <summary>
        /// Random take one element or <see langword="default"/>(T) if <paramref name="source"/> is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        public static T RandomTakeOrDefault<T>([NotNull] this List<T> source, [NotNull] Random random)
        {
            return InternalRandomTakeOrDefault((IList<T>)source, random);
        }

        /// <summary>
        /// Random take one element or <see langword="default"/>(T) if <paramref name="source"/> is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        public static T RandomTakeOrDefault<T>([NotNull] this IList<T> source, [NotNull] Random random)
        {
            return InternalRandomTakeOrDefault(source, random);
        }

        /// <summary>
        /// Random take one element or <see langword="default"/>(T) if <paramref name="source"/> is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        public static T RandomTakeOrDefault<T>([NotNull] this IReadOnlyList<T> source, [NotNull] Random random)
        {
            return InternalRandomTakeOrDefault(source, random);
        }

        /// <summary>
        /// Random take one element or <see langword="default"/>(T) if <paramref name="source"/> is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">one of arguments is null.</exception>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static T RandomTakeOrDefault<T>([NotNull] this IEnumerable<T> source, [NotNull] Random random)
        {
            switch (source)
            {
                case IList<T> li:
                    return li.RandomTakeOrDefault(random);

                case IReadOnlyList<T> li:
                    return li.RandomTakeOrDefault(random);
            }

            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));

            var c = source.TryDirectGetCount();
            return c < 0 
                ? source.ToArray().RandomTake(random) 
                : (c > 1 ? source.Skip(random.Next(c)) : source).FirstOrDefault();
        }

        #endregion

        #region random order

        /// <summary>
        /// Order by random.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        [NotNull, Pure, SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> OrderBy<T>([NotNull] this IEnumerable<T> source, [NotNull] Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));

            IEnumerable<T> InternalOrderBy(IList<T> list)
            {
                var count = list.Count;
                if (count == 0) yield break;
                if (count == 1)
                {
                    yield return list[0];
                    yield break;
                }
                var array = Enumerable.Range(0, count).ToArray();
                while (count > 0)
                {
                    var index = random.Next(count);
                    yield return list[array[index]];
                    array[index] = array[count - 1];
                    count--;
                }
            }

            switch (source)
            {
                case List<T> l:
                    return InternalOrderBy(l);
                case T[] a:
                    return InternalOrderBy(a);
                default:
                    return InternalOrderBy(source.ToArray());
            }
        }

        #endregion

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

        #region override linq function for special type

        #region skip

        /// <summary>
        /// Override <see cref="Enumerable.Skip"/> for <see cref="Array"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if one of arguments is null.</exception>
        [NotNull]
        public static IEnumerable<T> Skip<T>([NotNull] this T[] source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Length < count) return source;

            IEnumerable<T> Iterator()
            {
                for (var i = count; i < source.Length; i++)
                {
                    yield return source[i];
                }
            }

            return Iterator();
        }

        /// <summary>
        /// Override <see cref="Enumerable.Skip"/> for <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if one of arguments is null.</exception>
        [NotNull]
        public static IEnumerable<T> Skip<T>([NotNull] this List<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Count < count) return source;

            IEnumerable<T> Iterator()
            {
                for (var i = count; i < source.Count; i++)
                {
                    yield return source[i];
                }
            }

            return Iterator();
        }

        #endregion

        #endregion

        [NotNull]
        public static IEnumerable<T> TakeLast<T>([NotNull] this IEnumerable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) return Empty<T>.Array;

            // ReSharper disable once PossibleMultipleEnumeration
            switch (source)
            {
                case T[] array:
                    return array.Skip(count - array.Length);

                case List<T> list:
                    return list.Skip(count - list.Count);

                default:
                    var q = new Queue<T>(count);
                    // ReSharper disable once PossibleMultipleEnumeration
                    foreach (var item in source)
                    {
                        if (q.Count == count)
                            q.Dequeue();
                        q.Enqueue(item);
                    }
                    return q.AsReadOnly();
            }
        }

        #region for some array op

        /// <summary>
        /// Copy elements to <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="count">the max count to copy.</param>
        /// <returns>how many element was copyed.</returns>
        /// <exception cref="ArgumentNullException">throw if one of arguments is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static int CopyToArray<T>([NotNull] this IEnumerable<T> source, [NotNull] T[] array, int arrayIndex, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            array.EnsureInArrayRange(arrayIndex, count, nameof(array), nameof(arrayIndex), nameof(count));

            var i = arrayIndex;
            foreach (var item in source.Take(count))
            {
                array[i++] = item;
            }
            return i - arrayIndex;
        }

        #endregion

        #region for some linq missing method

        /// <summary>
        /// Invoke <paramref name="action"/> on each element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if one of argument is null.</exception>
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

        #endregion

        #region for reduce delegate create.

        /// <summary>
        /// Filter item by whether reference equals null.
        /// Same as <code>.Where(x => !ReferenceEquals(x, <see langword="null"/>))</code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="source"/> is null.</exception>
        public static IEnumerable<T> NotNull<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            IEnumerable<T> Iterator()
            {
                foreach (var item in source)
                {
                    if (!ReferenceEquals(item, null)) yield return item;
                }
            }

            return Iterator();
        }

        /// <summary>
        /// If any item in <paramref name="source"/> is null, return <see langword="true"/>; 
        /// otherwise, return <see langword="false"/>.
        /// (If <paramref name="source"/> is empty, also return <see langword="false"/>.)
        /// Same as <code>.Any(x => ReferenceEquals(x, <see langword="null"/>))</code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="source"/> is null.</exception>
        public static bool AnyIsNull<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            foreach (var item in source)
            {
                if (ReferenceEquals(item, null)) return true;
            }
            return false;
        }

        #endregion
    }
}