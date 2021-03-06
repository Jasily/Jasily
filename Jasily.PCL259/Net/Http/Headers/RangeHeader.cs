﻿
namespace Jasily.Net.Http.Headers
{
    public struct RangeHeader
    {
        public long From { get; private set; }

        public long? To { get; private set; }

        public RangeHeader(long from, long to)
            : this(from)
        {
            this.To = to;
        }

        public RangeHeader(long from)
            : this()
        {
            this.From = from;
        }

        public override string ToString() => this.Build();

        public static RangeHeader? TryParse(string rangeHeader)
        {
            if (!rangeHeader.StartsWith("bytes="))
                return null;

            var range = rangeHeader.Substring(6);
            if (range.Length == 0)
                return null;

            var array = range.Split(new[] { '-' }, 2);
            if (array.Length != 2)
                return null;

            int from;
            if (string.IsNullOrWhiteSpace(array[1]))
            {
                if (int.TryParse(array[0], out from))
                    return new RangeHeader(from);
            }
            else
            {
                if (int.TryParse(array[0], out from) && int.TryParse(array[0], out var to))
                    return new RangeHeader(from, to);
            }

            return null;
        }

        public string Build() => $"bytes={this.From}-{this.To}";
    }
}