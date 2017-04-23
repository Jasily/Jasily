using System;
using System.Collections.Concurrent;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class ScopedArguments<T> : IScopedArguments<T>
    {
        public ConcurrentDictionary<string, T> Data { get; }
            = new ConcurrentDictionary<string, T>(StringComparer.OrdinalIgnoreCase);
    }
}
