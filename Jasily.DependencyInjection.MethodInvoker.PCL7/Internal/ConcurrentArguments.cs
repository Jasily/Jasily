using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal class ConcurrentArguments<T> : ISingletonArguments<T>, IScopedArguments<T>
    {
        public ConcurrentDictionary<string, T> Data { get; }
            = new ConcurrentDictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        public T AddOrUpdate(string key, Func<string, T> addValueFactory, Func<string, T, T> updateValueFactory)
        {
            return this.Data.AddOrUpdate(key, addValueFactory, updateValueFactory);
        }

        public T AddOrUpdate(string key, T addValue, Func<string, T, T> updateValueFactory)
        {
            return this.Data.AddOrUpdate(key, addValue, updateValueFactory);
        }

        public void Clear()
        {
            this.Data.Clear();
        }

        public bool ContainsKey(string key)
        {
            return this.Data.ContainsKey(key);
        }

        public T GetOrAdd(string key, T value)
        {
            return this.Data.GetOrAdd(key, value);
        }

        public T GetOrAdd(string key, Func<string, T> valueFactory)
        {
            return this.Data.GetOrAdd(key, valueFactory);
        }

        public void SetValue(string key, T value)
        {
            this.Data[key] = value;
        }

        public bool TryAdd(string key, T value)
        {
            return this.Data.TryAdd(key, value);
        }

        public bool TryGetValue(string key, out T value)
        {
            return this.Data.TryGetValue(key, out value);
        }

        public bool TryRemove(string key, out T value)
        {
            return this.Data.TryRemove(key, out value);
        }

        public bool TryUpdate(string key, T newValue, T comparisonValue)
        {
            return this.Data.TryUpdate(key, newValue, comparisonValue);
        }
    }
}
