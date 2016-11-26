using JetBrains.Annotations;

namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static void AddIfNotDefault<T>([NotNull] this ICollection<T> source, T item)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (!EqualityComparer<T>.Default.Equals(default(T), item)) source.Add(item);
        }

        public static void AddRange<T>([NotNull] this ICollection<T> source, [NotNull] IEnumerable<T> items)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items) source.Add(item);
        }
    }
}