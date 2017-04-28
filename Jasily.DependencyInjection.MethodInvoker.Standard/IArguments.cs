using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IArguments<T>
    {
        void SetValue(string key, T value);

        T AddOrUpdate(string key, Func<string, T> addValueFactory, Func<string, T, T> updateValueFactory);

        T AddOrUpdate(string key, T addValue, Func<string, T, T> updateValueFactory);

        void Clear();

        bool ContainsKey(string key);

        T GetOrAdd(string key, T value);

        T GetOrAdd(string key, Func<string, T> valueFactory);

        bool TryAdd(string key, T value);

        bool TryGetValue(string key, out T value);

        bool TryRemove(string key, out T value);

        bool TryUpdate(string key, T newValue, T comparisonValue);
    }
}
