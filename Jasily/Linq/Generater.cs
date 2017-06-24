using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Jasily.Linq
{
    public static class Generater
    {
        /// <summary>
        /// Create specified number objects by <see langword="new"/> T();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <returns></returns>
        [NotNull, ItemNotNull]
        public static IEnumerable<T> Create<T>(int count) where T : new()
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            IEnumerable<T> Iterator()
            {
                for (var i = 0; i < count; i++) yield return new T();
            }

            return Iterator();
        }

        /// <summary>
        /// Create specified number objects by <paramref name="factory"/>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> Create<T>([NotNull] Func<T> factory, int count)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            IEnumerable<T> Iterator()
            {
                for (var i = 0; i < count; i++) yield return factory();
            }

            return Iterator();
        }

        /// <summary>
        /// Create specified number objects by <paramref name="factory"/>(<see langword="int"/>);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> Create<T>([NotNull] Func<int, T> factory, int count)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            IEnumerable<T> Iterator()
            {
                for (var i = 0; i < count; i++) yield return factory(i);
            }

            return Iterator();
        }

        /// <summary>
        /// Return a forever enumerate, all elements are zero.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<int> Forever()
        {
            // value type is better then object.
            while (true) yield return 0;
        }
    }
}