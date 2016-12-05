using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace System
{
    public static class CharExtensions
    {
        public static string Repeat(this char ch, int count) => count == 0 ? string.Empty : new string(ch, count);

        public static bool IsEnglishChar(this char ch) => ('a' <= ch && ch <= 'z') || ('A' <= ch && ch <= 'Z');

        #region get string

        public static string GetString([NotNull] this char[] array) => new string(array);

        public static string GetString([NotNull] this IEnumerable<char> array) => new string(array.ToArray());

        #endregion
    }
}