using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System.Linq
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
        public static IEnumerable<T> NotNull<T>([NotNull] this IEnumerable<T> source) where T : class
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
    }
}
