using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace System.IO
{
    public static class TextReaderExtensions
    {
        public static int ReadBlock([NotNull] this TextReader reader, [NotNull] char[] buffer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            return InternalReadBlock(reader, buffer);
        }

        private static int InternalReadBlock([NotNull] TextReader reader, [NotNull] char[] buffer)
            => reader.ReadBlock(buffer, 0, buffer.Length);

        /// <summary>
        /// read next char if not end.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static char? ReadChar([NotNull] this TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var n = reader.Read();
            return n == -1 ? (char?)null : (char)n;
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<string> EnumerateLines([NotNull] this TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null) yield break;
                yield return line;
            }
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<char> EnumerateChars([NotNull] this TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            while (true)
            {
                var ch = reader.Read();
                if (ch == -1) yield break;
                yield return (char)ch;
            }
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<char[]> EnumerateBlocks([NotNull] this TextReader reader, int maxBlockSize, bool reuseBuffer = false)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            int readed;
            var buffer = new char[maxBlockSize];
            while ((readed = InternalReadBlock(reader, buffer)) > 0)
            {
                if (readed == maxBlockSize)
                {
                    yield return reuseBuffer ? buffer : buffer.ToArray();
                }
                else
                {
                    yield return buffer.Take(readed).ToArray();
                }
            }
        }
    }
}