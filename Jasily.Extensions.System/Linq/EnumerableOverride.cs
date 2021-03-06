﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery

namespace Jasily.Extensions.System.Linq
{
    public static class EnumerableOverride
    {
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

            if (source is ICollection collection) return collection.Count;

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