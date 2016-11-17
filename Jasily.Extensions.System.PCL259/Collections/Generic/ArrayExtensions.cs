using JetBrains.Annotations;

namespace System.Collections.Generic
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CheckRange<T>([NotNull] this T[] array, int startIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (startIndex < 0 || startIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void CheckRange<T>([NotNull] this T[] array, int startIndex, int count)
        {
            CheckRange(array, startIndex);
            if (count < 0 || count > array.Length) throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > array.Length) throw new ArgumentException();
        }
    }
}