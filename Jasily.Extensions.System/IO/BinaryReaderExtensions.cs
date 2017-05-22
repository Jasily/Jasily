using System;
using System.IO;
using JetBrains.Annotations;

namespace Jasily.Extensions.System.IO
{
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="EndOfStreamException"></exception>
        public static byte[] ReadBytesOrThrow([NotNull] this BinaryReader reader, int count)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var buffer = reader.ReadBytes(count);
            if (buffer.Length != count) throw new EndOfStreamException();
            return buffer;
        }
    }
}