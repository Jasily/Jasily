using System;
using System.Text;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.Text
{
    /// <summary>
    /// Extension methods for <see cref="Encoding"/>.
    /// </summary>
    public static class EncodingExtensions
    {
#if PCL259
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if one of arguments is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">throw from <see cref="Encoding.GetString(byte[], int, int)"/></exception>
        /// <exception cref="ArgumentException">throw from <see cref="Encoding.GetString(byte[], int, int)"/></exception>
        /// <exception cref="DecoderFallbackException">throw from <see cref="Encoding.GetString(byte[], int, int)"/></exception>
        public static string GetString([NotNull] this Encoding encoding, [NotNull] byte[] bytes)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            return encoding.GetString(bytes, 0, bytes.Length);
        }
#endif
    }
}