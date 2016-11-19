using JetBrains.Annotations;

namespace System.Collections.Generic
{
    public static class ArrayExtensions
    {
        public static void ForEach<T>([NotNull] this T[] source, [NotNull] Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            for (var i = 0; i < source.Length; i++)
            {
                action(source[i]);
            }
        }

        public static void ForEach<T>([NotNull] this T[] source, [NotNull] Action<int, T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            for (var i = 0; i < source.Length; i++)
            {
                action(i, source[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CheckRange<T>([NotNull] this T[] array, int offset)
            => ToSegment(array, offset);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void CheckRange<T>([NotNull] this T[] array, int offset, int count)
            => ToSegment(array, offset, count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ArraySegment<T> ToSegment<T>([NotNull] this T[] array)
            => new ArraySegment<T>(array);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ArraySegment<T> ToSegment<T>([NotNull] this T[] array, int offset)
            => new ArraySegment<T>(array, offset, array.Length - offset);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static ArraySegment<T> ToSegment<T>([NotNull] this T[] array, int offset, int count)
            => new ArraySegment<T>(array, offset, count);
    }
}