using JetBrains.Annotations;

namespace System.Collections.Generic
{
    internal static class EnumerableExtensions
    {
        public static void ForEach<T>([NotNull] this IEnumerable<T> source, [NotNull] Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var item in source) action(item);
        }

        public static void ForEach<T>([NotNull] this IEnumerable<T> source, [NotNull] Action<int, T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var index = 0;
            foreach (var item in source) action(index++, item);
        }

        public static void CopyTo<T>([NotNull] this IEnumerable<T> source, [NotNull] T[] array, int startIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));

            var index = 0;
            foreach (var item in source)
            {
                array[startIndex + index++] = item;
            }
        }

        public static void CopyTo<T>([NotNull] this IEnumerable<T> source, [NotNull] Array array, int startIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Rank != 1) throw new ArgumentException("array rank is NOT 1.");
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));

            var index = 0;
            try
            {
                foreach (var item in source)
                {
                    array.SetValue(item, startIndex + index++);
                }
            }
            catch (ArrayTypeMismatchException e)
            {
                throw new ArgumentException(e.Message, e);
            }
        }

        public static IEnumerable<T> AsReadOnly<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            foreach (var item in source) yield return item;
        }
    }
}