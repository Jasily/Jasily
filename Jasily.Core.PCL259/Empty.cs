using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Jasily.Core
{
    /// <summary>
    /// empty 
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
            Array = (T[])System.Linq.Enumerable.Empty<T>();
            Debug.Assert(Array != null);
            Enumerable = Array;
        }
    }
}