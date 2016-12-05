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

        #region to

        public static T[] ToArray<T>([NotNull] this T[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var array = new T[source.Length];
            source.CopyTo(array, 0);
            return array;
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

        #region any

        public static bool Any([NotNull] this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.GetEnumerator().MoveNext();
        }

        #endregion
    }
}