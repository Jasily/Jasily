using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class EnumerableOverride
    {
        #region skip

        public static IEnumerable<T> Skip<T>([NotNull] this T[] source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            for (var i = count; i < source.Length; i++)
            {
                yield return source[i];
            }
        }

        public static IEnumerable<T> Skip<T>([NotNull] this IList<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            for (var i = count; i < source.Count; i++)
            {
                yield return source[i];
            }
        }

        #endregion

        public static T[] ToArray<T>([NotNull] this T[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var ret = new T[source.Length];
            Array.Copy(source, ret, source.Length);
            return ret;
        }

        public static T[] ToArray<T>([NotNull] this T[] source, int count) => source.ToArray(0, count);

        public static T[] ToArray<T>([NotNull] this T[] source, int startIndex, int count)
        {
            source.CheckRange(startIndex, count);

            count = Math.Min(count, source.Length);
            var ret = new T[count];
            Array.Copy(source, startIndex, ret, 0, count);
            return ret;
        }
    }
}