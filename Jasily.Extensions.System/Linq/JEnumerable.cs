using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Linq
{
    /// <summary>
    /// extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static partial class JEnumerable
    {
        /// <summary>
        /// filter by whether reference equals null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="source"/> is null.</exception>
        /// <returns></returns>
        public static IEnumerable<T> NotNull<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            IEnumerable<T> Iterator()
            {
                foreach (var item in source)
                {
                    if (!ReferenceEquals(item, null)) yield return item;
                }
            }

            return Iterator();
        }

        /// <summary>
        /// If any item in <paramref name="source"/> is null, return <see langword="true"/>; 
        /// otherwise, return <see langword="false"/>.
        /// (If <paramref name="source"/> is empty, also return <see langword="false"/>.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool AnyIsNull<T>([NotNull] this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            foreach (var item in source)
            {
                if (ReferenceEquals(item, null)) return true;
            }
            return false;
        }
    }
}
