using System;
using System.Collections.Generic;
using System.Linq;
using Jasily.Extensions.System;
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

        public static IEnumerable<IndexValuePair<T>> EnumerateWithIndex<T>([NotNull] this T[] array, int offset = 0)
        {
            array.CheckRange(offset);
            return CreateBy(array, offset, array.Length);
        }

        public static IEnumerable<IndexValuePair<T>> EnumerateWithIndex<T>([NotNull] this T[] array, int offset, int count)
        {
            array.CheckRange(offset, count);
            return CreateBy(array, offset, count);
        }

        public static IEnumerable<IndexValuePair<T>> EnumerateWithIndex<T>([NotNull] this IEnumerable<T> source)
            => source.Select((z, i) => Create(i, z));
    }

    public struct IndexValuePair<T> : IEquatable<IndexValuePair<T>>
    {
        public IndexValuePair(int index, T value)
        {
            this.Index = index;
            this.Value = value;
        }

        public int Index { get; }

        public T Value { get; }

        public bool Equals(IndexValuePair<T> other)
        {
            return this.Index == other.Index && EqualityComparer<T>.Default.Equals(this.Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is IndexValuePair<T> pair && this.Equals(pair);
        }

        public override int GetHashCode()
        {
            return this.Index.GetHashCode() ^ EqualityComparer<T>.Default.GetHashCode(this.Value);
        }
    }
}