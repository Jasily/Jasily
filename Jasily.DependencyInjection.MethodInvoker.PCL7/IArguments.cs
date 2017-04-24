using System;
using System.Collections.Concurrent;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IArguments<T>
    {
        bool TryGetValue(string key, out T value);
    }
}
