using System;
using JetBrains.Annotations;

namespace Jasily.Extensions.System
{
    public static class ComparableExtensions
    {
        public static T Max<T>([NotNull] this T first, T second) where T : IComparable<T>
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            return first.CompareTo(second) < 0 ? second : first;
        }

        public static T Min<T>([NotNull] this T first, T second) where T : IComparable<T>
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            return first.CompareTo(second) < 0 ? first : second;
        }

        /// <summary>
        /// return self in [lower, upper) 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static bool IsBetween<T>([NotNull] this T self, T lower, T upper) where T : IComparable<T>
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.CompareTo(lower) >= 0 && self.CompareTo(upper) < 0;
        }
    }
}