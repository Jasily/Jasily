using System;
using System.Collections.Concurrent;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class SingletonArguments<T> : ISingletonArguments<T>
    {
        public ConcurrentDictionary<string, T> Data { get; }
            = new ConcurrentDictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        public bool TryGetValue(string key, out T value)
        {
            return this.Data.TryGetValue(key, out value);
        }
    }
}
