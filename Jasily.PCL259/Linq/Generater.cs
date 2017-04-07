using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Linq
{
    public static class Generater
    {
        public static IEnumerable<T> Create<T>(int count) where T : new()
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            for (var i = 0; i < count; i++) yield return new T();
        }

        public static IEnumerable<T> Create<T>([NotNull] Func<T> func, int count)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            for (var i = 0; i < count; i++) yield return func();
        }

        public static IEnumerable<T> Create<T>([NotNull] Func<int, T> func, int count)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            for (var i = 0; i < count; i++) yield return func(i);
        }

        public static IEnumerable<int> Forever()
        {
            // value type is better then object.
            while (true) yield return 0;
        }
    }
}