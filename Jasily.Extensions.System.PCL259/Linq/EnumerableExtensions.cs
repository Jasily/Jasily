using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// CancellationToken support for enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <param name="checkCycle"></param>
        /// <returns></returns>
        [PublicAPI]
        public static IEnumerable<T> EnumerateWithToken<T>([NotNull] this IEnumerable<T> source,
            CancellationToken token, int checkCycle = 30)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (checkCycle <= 0) throw new ArgumentOutOfRangeException(nameof(checkCycle));

            return EnumerateWithTokenIterator(source, token, checkCycle);
        }

        private static IEnumerable<T> EnumerateWithTokenIterator<T>([NotNull] this IEnumerable<T> source,
            CancellationToken token, int checkCycle)
        {
            Debug.Assert(source != null);
            Debug.Assert(checkCycle > 0);

            using (var enumerator = source.GetEnumerator())
            {
                if (checkCycle == 1)
                {
                    while (enumerator.MoveNext())
                    {
                        token.ThrowIfCancellationRequested();
                        yield return enumerator.Current;
                    }
                }
                else
                {
                    while (true)
                    {
                        token.ThrowIfCancellationRequested();
                        for (var i = 0u; i < checkCycle; i++)
                        {
                            if (enumerator.MoveNext()) yield return enumerator.Current;
                            else yield break;
                        }
                    }
                }
            }
        }

        [PublicAPI]
        public static IEnumerable<T> Append<T>([NotNull] this IEnumerable<T> source, T value,
            Position position)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            switch (position)
            {
                case Position.Start:
                    return AppendToStartIterator(source, value);

                case Position.End:
                    return AppendToEndIterator(source, value);

                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }
        }

        private static IEnumerable<T> AppendToStartIterator<T>([NotNull] IEnumerable<T> source, T value)
        {
            yield return value;
            foreach (var item in source) yield return item;
        }

        private static IEnumerable<T> AppendToEndIterator<T>([NotNull] IEnumerable<T> source, T value)
        {
            foreach (var item in source) yield return item;
            yield return value;
        }

        private static IEnumerable<T> AppendToIndexIterator<T>([NotNull] IEnumerable<T> source, T value, int index,
            bool allowEnd)
        {
            using (var itor = source.GetEnumerator())
            {
                var i = 0;
                while (i < index && itor.MoveNext())
                {
                    yield return itor.Current;
                    i++;
                }

                if (i == index)
                {
                    yield return value;
                }
                else if (!allowEnd)
                {
                    yield return value;
                    yield break;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }

                while (itor.MoveNext())
                {
                    yield return itor.Current;
                }
            }
        }

        [PublicAPI]
        public static IEnumerable<T> Append<T>([NotNull] this IEnumerable<T> source, T value, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return index == 0 ? AppendToStartIterator(source, value) : AppendToIndexIterator(source, value, index, false);
        }

        [PublicAPI]
        public static IEnumerable<T> AppendToIndexOrEnd<T>([NotNull] this IEnumerable<T> source, T value, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return index == 0 ? AppendToStartIterator(source, value) : AppendToIndexIterator(source, value, index, true);
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
    }
}