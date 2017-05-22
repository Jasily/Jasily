using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Jasily.Core;
using Jasily.Extensions.System.Linq;
using Jasily.Extensions.System.Text;
using JetBrains.Annotations;

namespace Jasily.Extensions.System
{
    /// <summary>
    /// useful extension methods for string.
    /// </summary>
    public static class StringExtensions
    {
        #region encoding & decoding

        /// <summary>
        /// use utf-8 encoding convert string to byte[].
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [PublicAPI]
        [NotNull]
        public static byte[] GetBytes([NotNull] this string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            return Encoding.UTF8.GetBytes(s);
        }

        /// <summary>
        /// use <paramref name="encoding"/> encoding convert string to byte[].
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [PublicAPI]
        [NotNull]
        public static byte[] GetBytes([NotNull] this string s, [NotNull] Encoding encoding)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            return encoding.GetBytes(s);
        }

        #endregion

        public static string Repeat([CanBeNull] this string str, int count)
        {
            // make sure raise Exception whatever str is null or not.
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            return str == null ? null : (count == 0 ? string.Empty : string.Concat(Enumerable.Repeat(str, count)));
        }

        #region between

        [NotNull]
        public static string Between([NotNull] this string str, int leftIndex, int rightIndex)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (rightIndex == leftIndex) return string.Empty;
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

        #region ensure

        /// <summary>
        /// ensure length of string large or equals the <paramref name="length"/>.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="paddingChar"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        [PublicAPI]
        [NotNull]
        public static string EnsureLength([NotNull] this string str, int length, char paddingChar, Position position)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (length < 0) throw new ArgumentOutOfRangeException();

            if (str.Length >= length) return str;
            var padding = new string(paddingChar, length - str.Length);
            switch (position)
            {
                case Position.Begin:
                    return padding + str;
                case Position.End:
                    return str + padding;
                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }
        }

        /// <summary>
        /// ensure string startswith <paramref name="value"/>. if NOT, insert it.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        [PublicAPI]
        [NotNull]
        public static string EnsureStart([NotNull] this string str, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return str.StartsWith(value, comparisonType) ? str : value + str;
        }

        /// <summary>
        /// ensure string endswith <paramref name="value"/>. if NOT, append it.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        [PublicAPI]
        [NotNull]
        public static string EnsureEnd([NotNull] this string str, string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return str.EndsWith(value, comparisonType) ? str : str + value;
        }

        #endregion

        #region substring

        [NotNull]
        public static string Take([NotNull] this string str, int count)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (count > str.Length) return str;
            return str.Substring(0, count);
        }

        #endregion

        #region replace

        public static string Replace([NotNull] this string str, [NotNull] string oldValue, [NotNull] string newValue,
            StringComparison comparisonType, int maxCount = int.MaxValue)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (oldValue == null) throw new ArgumentNullException(nameof(oldValue));
            if (oldValue.Length == 0) throw new ArgumentException("Argument is empty", nameof(oldValue));
            if (newValue == null) throw new ArgumentNullException(nameof(newValue));
            if (maxCount < 1) throw new ArgumentOutOfRangeException(nameof(maxCount));

            if (str.Length < oldValue.Length) return str;
            if (comparisonType == StringComparison.Ordinal) return str.Replace(oldValue, newValue);

            var sb = new StringBuilder();
            var ptr = 0;
            for (int index, count = 0; count < maxCount && (index = str.IndexOf(oldValue, ptr, comparisonType)) > 0; count++)
            {
                sb.Append(str, ptr, index - ptr);
                sb.Append(newValue);
                ptr = index + oldValue.Length;
                count++;
            }
            return sb.Append(str, ptr).ToString();
        }

        public static string ReplaceFirst([NotNull] this string str, [NotNull] string oldValue,
            [NotNull] string newValue, StringComparison comparisonType)
            => Replace(str, oldValue, newValue, comparisonType, 1);

        public static string ReplaceStart([NotNull] this string str, [NotNull] string oldValue,
            [NotNull] string newValue,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (oldValue == null) throw new ArgumentNullException(nameof(oldValue));
            if (oldValue.Length == 0) throw new ArgumentException("Argument is empty", nameof(oldValue));
            if (newValue == null) throw new ArgumentNullException(nameof(newValue));

            if (str.Length < oldValue.Length) return str;
            var count = 0;
            for (; str.StartsWith(oldValue.Length * count, oldValue, comparisonType); count++)
            {
            }
            switch (count)
            {
                case 0:
                    return str;
                case 1:
                    return newValue + str.Substring(oldValue.Length);
                default:
                    return string.Concat(Enumerable.Repeat(newValue, count)
                        .Append(str.Substring(oldValue.Length * count), Position.End));
            }
        }

        /// <summary>
        /// replace char by index.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string ReplaceChar([NotNull] this string str, char value, int index)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (index < 0 || index > str.Length) throw new ArgumentOutOfRangeException(nameof(index));
            var array = str.ToCharArray();
            array[index] = value;
            return new string(array);
        }

        #endregion

        #region start or end

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="value"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool StartsWith([NotNull] this string str, int startIndex, [NotNull] string value,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length == 0) return true;
            return string.Compare(str, startIndex, value, 0, value.Length, comparisonType) == 0;
        }

        #endregion

        #region contains

        public static bool Contains([NotNull] this string str, char value)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return str.IndexOf(value) > -1;
        }

        public static bool Contains([NotNull] this string str, char value, int startIndex)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return str.IndexOf(value, startIndex) > -1;
        }

        public static bool Contains([NotNull] this string str, char value, int startIndex, int count)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return str.IndexOf(value, startIndex, count) > -1;
        }

        public static bool Contains([NotNull] this string str, [NotNull] string value,
            StringComparison comparisonType)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return str.IndexOf(value, comparisonType) > -1;
        }

        public static bool Contains([NotNull] this string str, [NotNull] string value, int startIndex,
            StringComparison comparisonType)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return str.IndexOf(value, startIndex, comparisonType) > -1;
        }

        public static bool Contains([NotNull] this string str, [NotNull] string value, int startIndex, int count,
            StringComparison comparisonType)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return str.IndexOf(value, startIndex, count, comparisonType) > -1;
        }

        #endregion

        #region split & join

        public static string[] Split([NotNull] this string str, [NotNull] string separator,
            StringSplitOptions options = StringSplitOptions.None)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (separator == null) throw new ArgumentNullException(nameof(separator));
            return str.Split(new[] { separator }, options);
        }

        public static string[] Split([NotNull] this string str, [NotNull] string separator, int count,
            StringSplitOptions options = StringSplitOptions.None)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (separator == null) throw new ArgumentNullException(nameof(separator));
            return str.Split(new[] { separator }, count, options);
        }

        /// <summary>
        /// if set includeSeparator = true, return [ block, separator, block, separator, ... ].
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <param name="comparisonType"></param>
        /// <param name="options"></param>
        /// <param name="includeSeparator"></param>
        /// <returns></returns>
        public static string[] Split([NotNull] this string str, [NotNull] string separator, StringComparison comparisonType,
            StringSplitOptions options = StringSplitOptions.None, bool includeSeparator = false)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (separator == null) throw new ArgumentNullException(nameof(separator));
            if (separator.Length == 0) throw new ArgumentException(nameof(separator));

            var rets = new List<string>();
            var ptr = 0;
            while (true)
            {
                var index = str.IndexOf(separator, ptr, comparisonType);
                if (index < 0)
                {
                    rets.Add(str.Substring(ptr));
                    break;
                }
                rets.Add(str.Substring(ptr, index - ptr));
                if (includeSeparator)
                {
                    rets.Add(str.Substring(index, separator.Length));
                }
                ptr = index + separator.Length;
            }
            return options == StringSplitOptions.None ? rets.ToArray() : rets.Where(z => !string.IsNullOrEmpty(z)).ToArray();
        }

        public static string[] SplitWhen([NotNull] this string text, [NotNull] Func<char, bool> separator,
            StringSplitOptions options = StringSplitOptions.None)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (separator == null) throw new ArgumentNullException(nameof(separator));
            return SplitWhen(text, (c, i) => separator(c), options);
        }

        public static string[] SplitWhen([NotNull] this string text, [NotNull] Func<char, int, bool> separator,
            StringSplitOptions options = StringSplitOptions.None)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (separator == null) throw new ArgumentNullException(nameof(separator));

            var rets = new List<string>();
            var start = 0;
            var ptr = 0;
            for (; ptr < text.Length; ptr++)
            {
                if (separator(text[ptr], ptr))
                {
                    if (ptr == start)
                    {
                        rets.Add(string.Empty);
                    }
                    else
                    {
                        rets.Add(text.Substring(start, ptr - start));
                        start = ptr;
                    }
                }
            }
            if (ptr > start + 1)
            {
                rets.Add(text.Substring(start, ptr - start));
            }
            return options == StringSplitOptions.None ? rets.ToArray() : rets.Where(z => z.Length > 0).ToArray();
        }

        /// <summary>
        /// use '\r\n' or '\n' to split text.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] SplitLines([NotNull] this string str, StringSplitOptions options = StringSplitOptions.None)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            return str.Split(new[] { "\r\n", "\n" }, options);
        }

        /// <summary>
        /// return first line from text
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstLine([NotNull] this string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.IndexOf('\n');
            return index == -1
                ? str
                : str.Substring(0, index > 0 && str[index - 1] == '\r' ? index - 1 : index);
        }

        /// <summary>
        /// use Environment.NewLine to join texts.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string JoinLines([NotNull] this IEnumerable<string> values)
            => string.Join(Environment.NewLine, values);

        #endregion

        #region trim

        public static string TrimStart([NotNull] this string str, params string[] trimStrings)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            if (trimStrings == null || trimStrings.Length == 0)
                return str;

            if (trimStrings.Any(z => z.IsNullOrEmpty()))
                throw new ArgumentException();
            
            var startIndex = 0;
            for (string start; (start = trimStrings.FirstOrDefault(z => str.StartsWith(startIndex, z))) != null;)
            {
                startIndex += start.Length;
            }
            return str.Substring(startIndex);
        }

        #endregion

        #region common part

        public static string CommonStart(this string[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Length == 0) return string.Empty;
            if (source.Length == 1) return source[0];

            var exp = source[0];
            var end = source.Min(z => z.Length); // length
            foreach (var item in source.Skip(1))
            {
                for (var i = 0; i < end; i++)
                {
                    if (item[i] != exp[i])
                    {
                        end = i;
                        break;
                    }
                }
            }
            return exp.Substring(0, end);
        }

        public static string CommonEnd(this string[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Length == 0) return string.Empty;
            if (source.Length == 1) return source[0];

            var exp = source[0];
            var len = source.Select(z => z.Length).Min();
            foreach (var item in source.Skip(1))
            {
                for (var i = 0; i < len; i++)
                {
                    if (exp[exp.Length - i - 1] != item[item.Length - i - 1])
                    {
                        len = i;
                        break;
                    }
                }
            }
            return exp.Substring(exp.Length - len);
        }

        #endregion

        #region after/before first and last

        public static string AfterFirst([NotNull] this string str, [NotNull] string spliter,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(spliter)) throw new ArgumentException(nameof(spliter));

            var index = str.IndexOf(spliter, comparisonType);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterFirst([NotNull] this string str, char spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.IndexOf(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterFirst([NotNull] this string str, params char[] spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.LastIndexOfAny(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }

        public static string AfterLast([NotNull] this string str, [NotNull] string spliter,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(spliter)) throw new ArgumentException(nameof(spliter));

            var index = str.LastIndexOf(spliter, comparisonType);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterLast([NotNull] this string str, char spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.LastIndexOf(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }
        public static string AfterLast([NotNull] this string str, params char[] spliter)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            var index = str.LastIndexOfAny(spliter);
            return index < 1 ? str : str.Substring(index + 1);
        }

        public static string BeforeFirst([NotNull] this string str, [NotNull] string spliter,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(spliter)) throw new ArgumentException(nameof(spliter));

            var index = str.IndexOf(spliter, comparisonType);
            if (index < 0) return str;
            return index == 0 ? string.Empty : str.Substring(0, index);
        }
        public static string BeforeFirst([NotNull] this string str, [NotNull] string[] spliters,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (spliters == null) throw new ArgumentNullException(nameof(spliters));
            if (spliters.Length == 0 || spliters.Any(string.IsNullOrEmpty)) throw new ArgumentException(nameof(spliters));

            var indexs = spliters.Select(z => str.IndexOf(z, comparisonType)).Where(z => z >= 0).ToArray();
            if (indexs.Length == 0) return str;
            var index = indexs.Min();
            return index == 0 ? string.Empty : str.Substring(0, index);
        }

        public static string BeforeLast([NotNull] this string str, [NotNull] string spliter,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (spliter == null) throw new ArgumentNullException(nameof(spliter));

            var index = str.LastIndexOf(spliter, comparisonType);
            return index <= 0 ? string.Empty : str.Substring(0, index);
        }

        #endregion

        #region static method to extension method

        /// <summary>
        /// return String.IsNullOrEmpty(text)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty([CanBeNull] this string value) => string.IsNullOrEmpty(value);

        /// <summary>
        /// return String.IsNullOrWhiteSpace(text)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace([CanBeNull] this string value) => string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// return String.Concat(texts)
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string ConcatAsString([NotNull] this IEnumerable<string> texts)
            => string.Concat(texts);

        /// <summary>
        /// return String.Join(spliter, texts)
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string JoinAsString([NotNull] this IEnumerable<string> texts, string spliter)
            => string.Join(spliter, texts);

        #endregion

        #region null <=> empty

        [NotNull]
        public static string EmptyIfNull([CanBeNull] this string str) => str ?? string.Empty;

        [CanBeNull]
        public static string NullIfEmpty<T>([CanBeNull] this string str) => string.IsNullOrEmpty(str) ? null : str;

        #endregion

        /// <summary>
        /// since unicode code point can large then char.MaxValue, we need int to save it.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] ToUnicodeChars([NotNull] this string str)
        {
            var indexs = StringInfo.ParseCombiningCharacters(str);
            if (indexs.Length == 0) return Empty<int>.Array;
            var chars = new int[indexs.Length];
            for (var i = 0; i < indexs.Length; i++)
            {
                chars[i] = char.ConvertToUtf32(str, indexs[i]);
            }
            return chars;
        }
    }
}