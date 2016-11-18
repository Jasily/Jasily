using System;

namespace Jasily
{
    /// <summary>
    /// current code path should never reachable
    /// </summary>
    public sealed class ReachableException : Exception
    {
        public ReachableException()
        {
        }

        public ReachableException(string message)
            : base(message)
        {
        }
    }
}