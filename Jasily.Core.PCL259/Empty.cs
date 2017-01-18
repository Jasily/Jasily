using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using E = System.Linq.Enumerable;

namespace Jasily.Core
{
    public static class Empty<T>
    {
        /// <summary>
        /// get a empty array.
        /// </summary>
        [NotNull]
        public static T[] Array { get; } = (T[])E.Empty<T>();

        /// <summary>
        /// get a empty array.
        /// </summary>
        [NotNull]
        public static IEnumerable<T> Enumerable { get; } = E.Empty<T>();

        static Empty()
        {
            Debug.Assert(Array != null);
            Debug.Assert(Enumerable != null);
        }
    }
}