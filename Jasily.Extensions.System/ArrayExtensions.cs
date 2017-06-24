using System;
using System.Runtime.CompilerServices;
using Jasily.Core;
using JetBrains.Annotations;

namespace Jasily.Extensions.System
{
    /// <summary>
    /// 
    /// </summary>
    public static class ArrayExtensions
    {
        public static T[] ToArray<T>([NotNull] this T[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var array = new T[source.Length];
            source.CopyTo(array, 0);
            return array;
        }

        [NotNull]
        public static T[] ForEach<T>([NotNull] this T[] source, [NotNull] Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            for (var i = 0; i < source.Length; i++)
            {
                action(source[i]);
            }
            return source;
        }

        [NotNull]
        public static T[] ForEach<T>([NotNull] this T[] source, [NotNull] Action<int, T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            for (var i = 0; i < source.Length; i++)
            {
                action(i, source[i]);
            }
            return source;
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
        // ReSharper disable once UnusedMethodReturnValue.Global
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
        // ReSharper disable once UnusedMethodReturnValue.Global
        public static ArraySegment<T> ToSegment<T>([NotNull] this T[] array, int offset, int count)
            => new ArraySegment<T>(array, offset, count);

        public static TDest[] ToArray<TSource, TDest>([NotNull] this TSource[] array) where TSource : TDest
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) return Empty<TDest>.Array;
            var newArray = new TDest[array.Length];
            Array.Copy(array, newArray, array.Length);
            return newArray;
        }

        public static TDest[] CastToArray<TSource, TDest>([NotNull] this TSource[] array)
            where TSource : class 
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) return Empty<TDest>.Array;
            var newArray = new TDest[array.Length];
            for (var i = 0; i < array.Length; i++)
            {
                newArray[i] = (TDest)(object)array[i];
            }
            return newArray;
        }

        public static TOutput[] ConvertToArray<TInput, TOutput>([NotNull] this TInput[] array,
            [NotNull] Func<TInput, TOutput> converter)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            if (array.Length == 0) return Empty<TOutput>.Array;
            var newArray = new TOutput[array.Length];
            for (var i = 0; i < array.Length; i++)
            {
                newArray[i] = converter(array[i]);
            }
            return newArray;
        }

        #region ensure range in array

        /// <summary>
        /// Check whether 
        /// [<paramref name="index"/>, <paramref name="index"/> + <paramref name="length"/>] 
        /// in range of <paramref name="array"/>.
        /// If not, raise some <see cref="Exception"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="arrayName"></param>
        /// <param name="indexName"></param>
        /// <param name="lengthName"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">throw if <paramref name="index"/> or <paramref name="length"/> less then 0</exception>
        /// <exception cref="ArgumentException">throw if <paramref name="index"/> + <paramref name="length"/> greater than <paramref name="array"/>.Length.</exception>
        public static void EnsureInArrayRange<T>([NotNull] this T[] array, int index, int length,
            [CanBeNull] string arrayName = null, [CanBeNull] string indexName = null, [CanBeNull] string lengthName = null)
        {
            if (array == null) throw new ArgumentNullException(arrayName ?? nameof(array));
            if (index < 0) throw new ArgumentOutOfRangeException(indexName ?? nameof(index));
            if (length < 0) throw new ArgumentOutOfRangeException(lengthName ?? nameof(length));
            if (index + length > array.Length) throw new ArgumentException();
        }

        #endregion
    }
}