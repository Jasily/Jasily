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

            if (source.Length == 0) return (T[])Enumerable.Empty<T>();
            var ret = new T[source.Length];
            Array.Copy(source, ret, source.Length);
            return ret;
        }

        public static T[] ToArray<T>([NotNull] this T[] source, int count) => source.ToArray(0, count);

        public static T[] ToArray<T>([NotNull] this T[] source, int offset, int count)
        {
            source.CheckRange(offset, count);

            count = Math.Min(count, source.Length);
            if (count == 0) return (T[]) Enumerable.Empty<T>();
            var ret = new T[count];
            Array.Copy(source, offset, ret, 0, count);
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

        #region any and all

        public static bool Any<TSource>([NotNull] this TSource[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Length > 0;
        }

        public static bool Any<TSource>([NotNull] this TSource[] source, [NotNull] Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            for (var i = 0; i < source.Length; i++)
            {
                if (predicate(source[i])) return true;
            }
            return false;
        }

        #endregion
    }
}