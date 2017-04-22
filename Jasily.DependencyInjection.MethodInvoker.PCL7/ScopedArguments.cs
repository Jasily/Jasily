using System;
using System.Collections.Concurrent;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal class ScopedArguments<T> : IScopedArguments<T>
    {
        public ConcurrentDictionary<string, T> Data { get; }
            = new ConcurrentDictionary<string, T>(StringComparer.OrdinalIgnoreCase);
    }
}
