using JetBrains.Annotations;

namespace System
{
    public static class StringExtensions
    {
        #region Between

        [NotNull]
        public static string Between([NotNull] this string str, int leftIndex, int rightIndex)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (rightIndex < leftIndex) throw new ArgumentException();
            return str.Substring(leftIndex, rightIndex - leftIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        [NotNull]
        public static string Between([NotNull] this string str, char left, char right) => Between(str, 0, left, right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
        [NotNull]
        public static string Between([NotNull] this string str, int startIndex, char left, char right)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var leftIndex = str.IndexOf(left, startIndex);
            if (leftIndex < 0) return string.Empty;
            startIndex = leftIndex + 1;
            if (startIndex == str.Length) return string.Empty;
            var rightIndex = str.IndexOf(right, startIndex);
            if (rightIndex < 0) return string.Empty;
            return str.Substring(startIndex, rightIndex - startIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.ArgumentException"></exception>
        [NotNull]
        public static string Between([NotNull] this string str, [NotNull] string left, [NotNull] string right,
            StringComparison comparisonType = StringComparison.Ordinal) => Between(str, 0, left, right, comparisonType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="T:System.ArgumentException"></exception>
        [NotNull]
        public static string Between([NotNull] this string str, int startIndex, [NotNull] string left, [NotNull] string right,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var leftIndex = str.IndexOf(left, startIndex, comparisonType);
            if (leftIndex < 0) return string.Empty;
            startIndex = leftIndex + left.Length;
            if (startIndex == str.Length) return string.Empty;
            var rightIndex = str.IndexOf(right, startIndex, comparisonType);
            if (rightIndex < 0) return string.Empty;
            return str.Substring(startIndex, rightIndex - startIndex);
        }

        #endregion
    }
}