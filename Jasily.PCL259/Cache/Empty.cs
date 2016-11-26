using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Jasily.Cache
{
    public static class Empty<T>
    {
        [NotNull]
        public static readonly T[] Array = (T[])Enumerable.Empty<T>();

        static Empty()
        {
            Debug.Assert(Array != null);
        }
    }
}