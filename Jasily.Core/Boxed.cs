using System;
using JetBrains.Annotations;

namespace Jasily.Core
{
    public class Boxed<T>
    {
        public T Value { get; set; }

        [ThreadStatic] private static Boxed<T> _cachedInstance;

        [NotNull]
        public static Boxed<T> Acquire(T value = default (T))
        {
            var box = _cachedInstance ?? new Boxed<T>();
            _cachedInstance = null;
            box.Value = value;
            return box;
        }

        public static void Release([CanBeNull] Boxed<T> val)
        {
            if (val != null)
            {
                val.Value = default(T);
                _cachedInstance = val;
            }
        }
    }
}