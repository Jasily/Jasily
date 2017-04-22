using System.Collections.Concurrent;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IArguments<T>
    {
        ConcurrentDictionary<string, T> Data { get; }
    }
}
