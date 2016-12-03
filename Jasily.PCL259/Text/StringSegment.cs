using System;
using System.Collections;
using System.Collections.Generic;
using Jasily.Interfaces;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public struct StringSegment : IInitializedValueType, IEnumerable<char>, IEquatable<StringSegment>
    {
        public StringSegment([NotNull] string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            this.String = str;
            this.StartIndex = 0;
            this.Count = str.Length;
        }

        public StringSegment([NotNull] string str, int startIndex)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));

            this.String = str;
            this.StartIndex = startIndex;
            this.Count = str.Length - startIndex;
        }

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

        public bool IsInitialized => this.String != null;

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private void ThrowIfInvalid()
        {
            if (!this.IsInitialized) throw new InvalidOperationException($"{nameof(StringSegment)} is uninitialized.");
        }

        public override string ToString()
        {
            this.ThrowIfInvalid();
            return this.String.Substring(this.StartIndex, this.Count);
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<char> IEnumerable<char>.GetEnumerator() => this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public struct Enumerator : IEnumerator<char>
        {
            private readonly StringSegment segment;
            private int offset;

            public Enumerator(StringSegment segment)
            {
                segment.ThrowIfInvalid(nameof(segment));
                this.segment = segment;
                this.offset = -1;
            }

            public bool MoveNext() => this.segment.Count > ++this.offset;

            public void Reset() => this.offset = -1;

            public char Current => this.segment.String[this.segment.StartIndex + this.offset];

            object IEnumerator.Current => this.Current;

            public void Dispose() { }
        }

        #region check

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Check(int startIndex)
        {
            if (startIndex < 0 || startIndex > this.Count) throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Check(int startIndex, int count)
        {
            if (startIndex < 0 || startIndex > this.Count) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || count > this.Count) throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > this.Count) throw new ArgumentOutOfRangeException();
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
            this.Check(startIndex);
            this.ThrowIfInvalid();
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
            this.Check(startIndex, count);
            this.ThrowIfInvalid();
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
            if (count < 0 || count > this.Count) throw new ArgumentOutOfRangeException(nameof(count));
            this.ThrowIfInvalid();
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
            var str = other as string;
            if (str != null) return this.Equals(str);
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
            this.ThrowIfInvalid();
            return this.InternalIndexOf(value, 0, this.Count, comparison);
        }

        [Pure]
        public int IndexOf([NotNull] string value, int startIndex,
            StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            this.Check(startIndex);
            this.ThrowIfInvalid();
            return this.InternalIndexOf(value, startIndex, this.Count - startIndex, comparison);
        }

        [Pure]
        public int IndexOf([NotNull] string value, int startIndex, int count,
            StringComparison comparison = StringComparison.Ordinal)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            this.Check(startIndex, count);
            this.ThrowIfInvalid();
            return this.InternalIndexOf(value, startIndex, count, comparison);
        }

        [Pure]
        public bool Contains([NotNull] string value, StringComparison comparisonType = StringComparison.Ordinal)
            => this.IndexOf(value, comparisonType) > -1;

        [Pure]
        public bool Contains([NotNull] string value, int startIndex,
            StringComparison comparisonType = StringComparison.Ordinal)
            => this.IndexOf(value, startIndex, comparisonType) > -1;

        [Pure]
        public bool Contains([NotNull] string value, int startIndex, int length,
            StringComparison comparisonType = StringComparison.Ordinal)
            => this.IndexOf(value, startIndex, length, comparisonType) > -1;

        #endregion
    }
}