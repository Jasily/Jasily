using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace System
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// to readonly MemoryStream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream([NotNull] this byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            return new MemoryStream(bytes, false);
        }

        /// <summary>
        /// get string use special encoding
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetString([NotNull] this byte[] bytes, [NotNull] Encoding encoding)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// get string use encoding-utf8
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetString([NotNull] this byte[] bytes) => bytes.GetString(Encoding.UTF8);

        public static string GetHexString([NotNull] this byte[] bytes) => BitConverter.ToString(bytes);

        public static string GetHexString([NotNull] this byte[] bytes, int startIndex) => BitConverter.ToString(bytes, startIndex);

        public static string GetHexString([NotNull] this byte[] bytes, int startIndex, int length)
            => BitConverter.ToString(bytes, startIndex, length);
    }
}