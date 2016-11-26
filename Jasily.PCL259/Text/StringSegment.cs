using System;
using Jasily.Interfaces;
using JetBrains.Annotations;

namespace Jasily.Text
{
    public struct StringSegment : IInitializedValueType
    {
        public StringSegment(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            this.String = str;
            this.StartIndex = 0;
            this.Count = str.Length;
        }

        public StringSegment(string str, int startIndex)
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

        public override string ToString()
        {
            this.ThrowIfInvalid(nameof(StringSegment));
            return this.String.Substring(this.StartIndex, this.Count);
        }
    }
}