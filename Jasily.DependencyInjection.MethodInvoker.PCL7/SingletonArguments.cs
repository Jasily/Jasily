using System;
using System.Collections.Concurrent;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal class SingletonArguments<T> : ISingletonArguments<T>
    {
        public ConcurrentDictionary<string, T> Data { get; }
            = new ConcurrentDictionary<string, T>(StringComparer.OrdinalIgnoreCase);
    }
}
