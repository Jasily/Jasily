using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using Jasily.Extensions.System.Text;

namespace Jasily.Extensions.System
{
    /// <summary>
    /// Extension methods for <see langword="byte"/>[].
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// To readonly MemoryStream
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="buffer"/> is null.</exception>
        public static MemoryStream ToMemoryStream([NotNull] this byte[] buffer)
        {
            return new MemoryStream(buffer, false);
        }

        /// <summary>
        /// Get <see langword="string"/> use specified encoding.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if one of arguments is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">throw from <see cref="Encoding.GetString(byte[], int, int)"/></exception>
        /// <exception cref="ArgumentException">throw from <see cref="Encoding.GetString(byte[], int, int)"/></exception>
        /// <exception cref="DecoderFallbackException">throw from <see cref="Encoding.GetString(byte[], int, int)"/></exception>
        public static string GetString([NotNull] this byte[] bytes, [NotNull] Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// Get <see langword="string"/> use Utf-8 encoding.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if one of arguments is null.</exception>
        public static string GetString([NotNull] this byte[] bytes) => bytes.GetString(Encoding.UTF8);

        public static string GetHexString([NotNull] this byte[] value) => BitConverter.ToString(value);

        public static string GetHexString([NotNull] this byte[] value, int startIndex)
        {
            return BitConverter.ToString(value, startIndex);
        }

        public static string GetHexString([NotNull] this byte[] value, int startIndex, int length)
        {
            return BitConverter.ToString(value, startIndex, length);
        }
    }
}