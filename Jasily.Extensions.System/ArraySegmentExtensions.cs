using System;

namespace Jasily.Extensions.System
{
    public static class ArraySegmentExtensions
    {
        /// <summary>
        /// return true if segment.Array is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static bool IsDefault<T>(this ArraySegment<T> segment) => segment.Array == null;

        /// <summary>
        /// raise InvalidOperationException if segment.Array is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="segment"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ThrowIfDefault<T>(this ArraySegment<T> segment)
        {
            if (segment.Array == null)
            {
                throw new InvalidOperationException("Array of ArraySegment is null");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="segment"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T Index<T>(this ArraySegment<T> segment, int index)
        {
            segment.ThrowIfDefault();
            if (index < 0 || index >= segment.Count) throw new ArgumentOutOfRangeException(nameof(index));
            return segment.Array[segment.Offset + index];
        }
    }
}