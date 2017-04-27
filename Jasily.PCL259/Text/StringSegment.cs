using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public struct StringSegment : IEnumerable<char>, IEquatable<StringSegment>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public StringSegment([NotNull] string str)
        {
            this.String = str ?? throw new ArgumentNullException(nameof(str));
            this.StartIndex = 0;
            this.Count = str.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public StringSegment([NotNull] string str, int startIndex)
        {
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));

            this.String = str ?? throw new ArgumentNullException(nameof(str));
            this.StartIndex = startIndex;
            this.Count = str.Length - startIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public StringSegment([NotNull] string str, int startIndex, int count)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count + startIndex > str.Length) throw new ArgumentOutOfRangeException(nameof(count));

            this.String = str;
            this.StartIndex = startIndex;
            this.Count = count;
        }

        public string String { get; }

        public int StartIndex { get; }

        public int Count { get; }

        [CanBeNull]
        public override string ToString() => this.String?.Substring(this.StartIndex, this.Count);

        public Enumerator GetEnumerator()
        {
            this.EnsureNotNull();
            return new Enumerator(this);
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator() => this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public struct Enumerator : IEnumerator<char>
        {
            private readonly StringSegment segment;
            private int offset;

            internal Enumerator(StringSegment segment)
            {
                this.segment = segment;
                this.offset = -1;
            }

            public bool MoveNext()
            {
                this.segment.EnsureNotNull();
                return this.segment.Count > ++this.offset;
            }

            public void Reset() => this.offset = -1;

            public char Current => this.segment.String[this.segment.StartIndex + this.offset];

            object IEnumerator.Current => this.Current;

            public void Dispose() { }
        }

        #region check

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        // ReSharper disable once PureAttributeOnVoidMethod
        [Pure]
        public void EnsureNotNull()
        {
            if (this.String == null) throw new InvalidOperationException($"string of {nameof(StringSegment)} is null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void EnsureRange(int startIndex)
        {
            this.EnsureNotNull();
            if (startIndex < 0 || startIndex > this.Count) throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void EnsureRange(int startIndex, int count)
        {
            this.EnsureRange(startIndex);
            if (count < 0 || startIndex + count > this.Count) throw new ArgumentOutOfRangeException(nameof(count));
        }

        #endregion

        #region sub StringSegment

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [Pure]
        public StringSegment SubSegment(int startIndex)
        {
            this.EnsureRange(startIndex);
            return new StringSegment(this.String, this.StartIndex + startIndex, this.Count - startIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [Pure]
        public StringSegment SubSegment(int startIndex, int count)
        {
            this.EnsureRange(startIndex, count);
            return new StringSegment(this.String, this.StartIndex + startIndex, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [Pure]
        public StringSegment Take(int count)
        {
            this.EnsureNotNull();
            if (count < 0 || count > this.Count) throw new ArgumentOutOfRangeException(nameof(count));
            return new StringSegment(this.String, this.StartIndex, count);
        }

        #endregion

        #region equals

        public override int GetHashCode()
        {
            if (this.String == null || this.Count == 0) return 0;
            return this.String.GetHashCode() ^ this.StartIndex ^ this.Count;
        }

        [Pure]
        public override bool Equals(object other)
        {
            if (other is string str) return this.Equals(str);
            var range = other as StringSegment?;
            return range != null && this.Equals(range.Value);
        }

        [Pure]
        public bool Equals(StringSegment other) => Equals(this, other);

        [Pure]
        public bool Equals(StringSegment other, StringComparison comparison) => Equals(this, other, comparison);

        [Pure]
        public bool Equals(string other) => Equals(this, other);

        [Pure]
        public bool Equals(string other, StringComparison comparison) => Equals(this, other, comparison);

        [Pure]
        public static bool operator ==(StringSegment first, StringSegment second) => Equals(first, second);

        [Pure]
        public static bool operator !=(StringSegment first, StringSegment second) => !(first == second);

        [Pure]
        public static bool operator ==(StringSegment first, string second) => Equals(first, second);

        [Pure]
        public static bool operator !=(StringSegment first, string second) => !(first == second);

        [Pure]
        public static bool operator ==(string second, StringSegment first) => Equals(first, second);

        [Pure]
        public static bool operator !=(string second, StringSegment first) => !(first == second);

        [Pure]
        public static bool Equals(StringSegment first, StringSegment second, StringComparison comparison = StringComparison.Ordinal)
        {
            if (first.String == null) return second.String == null;
            if (second.String == null) return false;
            return first.Count == second.Count && 0 == string.Compare(
                first.String, first.StartIndex,
                second.String, second.StartIndex,
                first.Count, comparison);
        }

        [Pure]
        public static bool Equals(StringSegment first, string second, StringComparison comparison = StringComparison.Ordinal)
        {
            if (first.String == null) return second == null;
            if (second == null) return false;
            return first.Count == second.Length && 0 == string.Compare(
                first.String, first.StartIndex,
                second, 0,
                first.Count, comparison);
        }

        #endregion

        #region index & contains

        private int InternalIndexOf([NotNull] string value, int startIndex, int count, StringComparison comparison)
            => this.String.IndexOf(value, this.StartIndex + startIndex, count, comparison) - this.StartIndex;

        [Pure]
        public int IndexOf([NotNull] string value, StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            this.EnsureNotNull();
            return this.InternalIndexOf(value, 0, this.Count, comparison);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [Pure]
        public int IndexOf([NotNull] string value, int startIndex,
            StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            this.EnsureRange(startIndex);
            return this.InternalIndexOf(value, startIndex, this.Count - startIndex, comparison);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [Pure]
        public int IndexOf([NotNull] string value, int startIndex, int count,
            StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            this.EnsureRange(startIndex, count);
            return this.InternalIndexOf(value, startIndex, count, comparison);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [Pure]
        public bool Contains([NotNull] string value, StringComparison comparisonType = StringComparison.Ordinal)
            => this.IndexOf(value, comparisonType) > -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [Pure]
        public bool Contains([NotNull] string value, int startIndex,
            StringComparison comparisonType = StringComparison.Ordinal)
            => this.IndexOf(value, startIndex, comparisonType) > -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [Pure]
        public bool Contains([NotNull] string value, int startIndex, int length,
            StringComparison comparisonType = StringComparison.Ordinal)
            => this.IndexOf(value, startIndex, length, comparisonType) > -1;

        #endregion
    }
}