using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Jasily.Collections.Generic
{
    public static class IndexValuePair
    {
        public static IndexValuePair<T> Create<T>(int index, T value) => new IndexValuePair<T>(index, value);

        private static IEnumerable<IndexValuePair<T>> CreateBy<T>([NotNull] this T[] array, int startIndex, int count)
        {
            for (var i = startIndex; i < count; i++)
            {
                yield return Create(i, array[i]);
            }
        }

        public static IEnumerable<IndexValuePair<T>> EnumerateIndex<T>([NotNull] this T[] array, int startIndex = 0)
        {
            array.CheckRange(startIndex);
            return CreateBy(array, startIndex, array.Length);
        }

        public static IEnumerable<IndexValuePair<T>> EnumerateIndex<T>([NotNull] this T[] array, int startIndex, int count)
        {
            array.CheckRange(startIndex, count);
            return CreateBy(array, startIndex, count);
        }

        public static IEnumerable<IndexValuePair<T>> EnumerateIndex<T>([NotNull] this IEnumerable<T> source)
            => source.Select((z, i) => Create(i, z));
    }

    public struct IndexValuePair<T>
    {
        public IndexValuePair(int index, T value)
        {
            this.Index = index;
            this.Value = value;
        }

        public int Index { get; }

        public T Value { get; }
    }
}