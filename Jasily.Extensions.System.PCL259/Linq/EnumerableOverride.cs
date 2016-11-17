using System.Collections.Generic;
using JetBrains.Annotations;

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

        public static IEnumerable<T> Skip<T>([NotNull] this IList<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            for (var i = count; i < source.Count; i++)
            {
                yield return source[i];
            }
        }

        #endregion

        #region to

        public static T[] ToArray<T>([NotNull] this T[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var ret = new T[source.Length];
            Array.Copy(source, ret, source.Length);
            return ret;
        }

        public static T[] ToArray<T>([NotNull] this T[] source, int count) => source.ToArray(0, count);

        public static T[] ToArray<T>([NotNull] this T[] source, int startIndex, int count)
        {
            source.CheckRange(startIndex, count);

            count = Math.Min(count, source.Length);
            var ret = new T[count];
            Array.Copy(source, startIndex, ret, 0, count);
            return ret;
        }

        public static List<T> ToList<T>([NotNull] this IEnumerable<T> source, int capacity)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var list = new List<T>(capacity);
            list.AddRange(source);
            return list;
        }

        #endregion

        #region orderby

        public static IOrderedEnumerable<T> OrderBy<T>([NotNull] this IEnumerable<T> source,
            [NotNull] IComparer<T> comparer)
            => source.OrderBy(z => z, comparer);

        public static IOrderedEnumerable<T> OrderBy<T>([NotNull] this IEnumerable<T> source,
            [NotNull] Comparison<T> comparison)
            => source.OrderBy(z => z, Comparer<T>.Create(comparison));

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>([NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector, [NotNull] Comparison<TKey> comparison)
            => source.OrderBy(keySelector, Comparer<TKey>.Create(comparison));

        public static IOrderedEnumerable<T> OrderByDescending<T>([NotNull] this IEnumerable<T> source,
            [NotNull] IComparer<T> comparer)
            => source.OrderByDescending(z => z, comparer);

        public static IOrderedEnumerable<T> OrderByDescending<T>([NotNull] this IEnumerable<T> source,
            [NotNull] Comparison<T> comparison)
            => source.OrderByDescending(z => z, Comparer<T>.Create(comparison));

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
    }
}