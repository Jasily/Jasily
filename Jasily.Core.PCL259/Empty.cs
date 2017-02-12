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
        public static T[] Array { get; }

        /// <summary>
        /// get a empty array.
        /// </summary>
        [NotNull]
        public static IEnumerable<T> Enumerable { get; }

        static Empty()
        {
            Array = (T[])E.Empty<T>();
            Debug.Assert(Array != null);
            Enumerable = Array;
        }
    }
}