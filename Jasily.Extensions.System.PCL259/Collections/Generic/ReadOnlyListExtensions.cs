using System;
using System.Collections.Generic;
using Jasily.Core;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Collections.Generic
{
    /// <summary>
    /// extension method for <see cref="IReadOnlyList{T}"/>.
    /// </summary>
    public static class ReadOnlyListExtensions
    {
        /// <summary>
        /// like python index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetByIndex<T>([NotNull] this IReadOnlyList<T> source, int index)
        {
            if (source == null) throw new ArgumentOutOfRangeException(nameof(source));            
            if (index < 0) index += source.Count;
            if (index < 0 || index >= source.Count) throw new ArgumentOutOfRangeException(nameof(index));
            return source[index];
        }

        /// <summary>
        /// like python index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetByIndex<T>([NotNull] this IReadOnlyList<T> source, int begin, int end)
        {
            if (source == null) throw new ArgumentOutOfRangeException(nameof(source));
            if (begin < 0) begin += source.Count;
            if (end < 0) end += source.Count;
            begin = Math.Max(0, Math.Min(begin, source.Count));
            end = Math.Max(0, Math.Min(end, source.Count));
            if (end <= begin) return Empty<T>.Enumerable;
           
            IEnumerable<T> Iterator()
            {
                while (begin < end) yield return source[begin++];
            }
            return Iterator();
        }
    }
}