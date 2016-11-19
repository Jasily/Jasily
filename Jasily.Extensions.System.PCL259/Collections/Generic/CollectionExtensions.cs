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
    }
}