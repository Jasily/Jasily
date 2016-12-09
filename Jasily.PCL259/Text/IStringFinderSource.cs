using System;

namespace Jasily.Text
{
    public interface IStringFinderSource
    {
        string OriginalString { get; }

        StringComparison Comparison { get; }

        int StartIndex { get; }
    }
}