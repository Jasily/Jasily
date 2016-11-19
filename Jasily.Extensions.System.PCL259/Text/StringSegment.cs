using JetBrains.Annotations;

namespace System.Text
{
    public struct StringSegment
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

        public bool IsValid() => this.String != null;

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void ThrowIfInvalid()
        {
            if (!this.IsValid()) throw new InvalidOperationException($"{nameof(StringSegment)} is invalid.");
        }
    }
}