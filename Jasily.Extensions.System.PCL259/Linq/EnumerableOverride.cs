using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery

namespace System.Linq
{
    public static class EnumerableOverride
    {
        #region skip

        public static IEnumerable<T> Skip<T>([NotNull] this T[] source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            for (var i = count; i < source.Length; i++)
            {
                yield return source[i];
            }
        }

        public static IEnumerable<T> Skip<T>([NotNull] this List<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            for (var i = count; i < source.Count; i++)
            {
                yield return source[i];
            }
        }

        #endregion

        #region orderby

        private static class C<T>
        {
            public static readonly Func<T, T> ReturnSelf = z => z;
        }

        public static IOrderedEnumerable<T> OrderBy<T>([NotNull] this IEnumerable<T> source,
            [NotNull] IComparer<T> comparer)
            => source.OrderBy(C<T>.ReturnSelf, comparer);

        public static IOrderedEnumerable<T> OrderBy<T>([NotNull] this IEnumerable<T> source,
            [NotNull] Comparison<T> comparison)
            => source.OrderBy(C<T>.ReturnSelf, Comparer<T>.Create(comparison));

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>([NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, [NotNull] Comparison<TKey> comparison)
            => source.OrderBy(keySelector, Comparer<TKey>.Create(comparison));

        public static IOrderedEnumerable<T> OrderByDescending<T>([NotNull] this IEnumerable<T> source,
            [NotNull] IComparer<T> comparer)
            => source.OrderByDescending(C<T>.ReturnSelf, comparer);

        public static IOrderedEnumerable<T> OrderByDescending<T>([NotNull] this IEnumerable<T> source,
            [NotNull] Comparison<T> comparison)
            => source.OrderByDescending(C<T>.ReturnSelf, Comparer<T>.Create(comparison));

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            [NotNull] this IEnumerable<TSource> source, [NotNull] Func<TSource, TKey> keySelector,
            [NotNull] Comparison<TKey> comparison)
            => source.OrderByDescending(keySelector, Comparer<TKey>.Create(comparison));

        #endregion

        #region first or default

        public static T? FirstOrNull<T>([NotNull] this IEnumerable<T> source)
            where T : struct
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            foreach (var item in source) return item;
            return null;
        }

        #endregion

        #region any

        public static bool Any([NotNull] this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var itor = source.GetEnumerator();
            using (itor as IDisposable) return itor.MoveNext();
        }

        #endregion

        #region count

        /// <summary>
        /// return -1 if cannot get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [PublicAPI]
        public static int TryGetCount<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return (source as ICollection<T>)?.Count ??
                   (source as ICollection)?.Count ??
                   (source as IReadOnlyCollection<T>)?.Count ?? -1;
        }

        /// <summary>
        /// return -1 if cannot get.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [PublicAPI]
        public static int TryGetCount([NotNull] this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return (source as ICollection)?.Count ?? -1;
        }

        [PublicAPI]
        public static int Count([NotNull] this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            
            if (source is ICollection collection) return collection.Count;

            var count = 0;
            var itor = source.GetEnumerator();
            using (itor as IDisposable) while (itor.MoveNext()) count++;
            return count;
        }

        [PublicAPI]
        public static long LongCount([NotNull] this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var collection = source as ICollection;
            if (collection != null) return collection.Count;

            var count = 0L;
            var itor = source.GetEnumerator();
            using (itor as IDisposable) while (itor.MoveNext()) count++;
            return count;
        }

        #endregion

        #region sum

        public static long LongSum([NotNull] this IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Aggregate(0L, (current, item) => current + item);
        }

        #endregion

        #region min & max

        public static T MaxOrDefault<T>([NotNull] IEnumerable<T> source, T @default) where T : IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) return @default;
                var item = itor.Current;
                while (itor.MoveNext())
                {
                    item = item.Max(itor.Current);
                }
                return item;
            }
        }

        public static T MinOrDefault<T>([NotNull] IEnumerable<T> source, T @default) where T : IComparable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var itor = source.GetEnumerator())
            {
                if (!itor.MoveNext()) return @default;
                var item = itor.Current;
                while (itor.MoveNext())
                {
                    item = item.Min(itor.Current);
                }
                return item;
            }
        }

        #endregion
    }
}