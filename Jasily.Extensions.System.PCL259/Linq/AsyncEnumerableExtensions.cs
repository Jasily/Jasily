using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class AsyncEnumerableExtensions
    {
        #region to

        public static Task<T[]> ToArrayAsync<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return Task.Run(() => source.ToArray());
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TKey, TSource>(
            [NotNull] this IEnumerable<TSource> source, [NotNull] Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            return Task.Run(() => source.ToDictionary(keySelector));
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TKey, TSource>(
            [NotNull] this IEnumerable<TSource> source, [NotNull] Func<TSource, TKey> keySelector,
            [NotNull] IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            return Task.Run(() => source.ToDictionary(keySelector, comparer));
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TSource, TElement>(
            [NotNull] this IEnumerable<TSource> source, [NotNull] Func<TSource, TKey> keySelector,
            [NotNull] Func<TSource, TElement> elementSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));
            return Task.Run(() => source.ToDictionary(keySelector, elementSelector));
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TSource, TElement>(
            [NotNull] this IEnumerable<TSource> source, [NotNull] Func<TSource, TKey> keySelector,
            [NotNull] Func<TSource, TElement> elementSelector, [NotNull] IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            return Task.Run(() => source.ToDictionary(keySelector, elementSelector, comparer));
        }

        public static Task<List<T>> ToListAsync<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return Task.Run(() => source.ToList());
        }

        #endregion
    }
}