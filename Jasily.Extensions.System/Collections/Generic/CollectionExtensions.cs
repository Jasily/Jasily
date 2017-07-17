using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Add if <paramref name="item"/> is not equals <see langword="default"/>(T).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        public static void AddIfNotDefault<T>([NotNull] this ICollection<T> source, T item)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (!EqualityComparer<T>.Default.Equals(default(T), item)) source.Add(item);
        }

        /// <summary>
        /// Add <paramref name="items"/> to <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        public static void AddRange<T>([NotNull] this ICollection<T> source, [NotNull] IEnumerable<T> items)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items) source.Add(item);
        }
    }
}