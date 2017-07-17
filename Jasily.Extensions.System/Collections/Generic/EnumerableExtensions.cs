using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Just enumerate each items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        public static void EmptyForEach<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            foreach (var _ in source) { }
        }

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
            array.EnsureArrayArguments(startIndex, nameof(array), nameof(startIndex));

            var index = 0;
            foreach (var item in source)
            {
                array[startIndex + index++] = item;
            }
        }

        public static void CopyTo<T>([NotNull] this IEnumerable<T> source, [NotNull] Array array, int startIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            array.EnsureArrayArguments(startIndex, nameof(array), nameof(startIndex));

            var index = 0;
            foreach (var item in source)
            {
                array.SetValue(item, startIndex + index++);
            }
        }

        public static IEnumerable<T> AsReadOnly<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new Enumerable<T>(source);
        }

        private class Enumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> baseEnumerable;

            public Enumerable([NotNull] IEnumerable<T> baseEnumerable)
            {
                Debug.Assert(baseEnumerable != null);
                this.baseEnumerable = baseEnumerable;
            }

            IEnumerator IEnumerable.GetEnumerator() => this.baseEnumerable.GetEnumerator();

            public IEnumerator<T> GetEnumerator() => this.baseEnumerable.GetEnumerator();
        }
    }
}