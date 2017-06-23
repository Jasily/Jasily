using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Jasily.Core
{
    /// <summary>
    /// reuseable empty instances.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Empty<T>
    {
        /// <summary>
        /// get a empty array.
        /// </summary>
        [NotNull]
        [PublicAPI]
        public static T[] Array { get; }

        /// <summary>
        /// get a empty array (without type cast).
        /// </summary>
        [NotNull]
        [PublicAPI]
        public static IEnumerable<T> Enumerable { get; }

        static Empty()
        {
            Array = (T[])System.Linq.Enumerable.Empty<T>();
            Debug.Assert(Array != null);
            Enumerable = Array;
        }
    }
}