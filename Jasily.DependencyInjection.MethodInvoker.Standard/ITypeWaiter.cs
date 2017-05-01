using System;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface ITypeWaiter
    {
        IAwaiterAdapter AwaiterAdapter { get; }

        object GetResult(object instance);
    }

    /// <summary>
    /// provide interface to get <see cref="ITypeWaiter"/> from <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITypeWaiter<T> : ITypeWaiter
    {
        TResult GetResult<TAwaiter, TResult>(T instance);
    }
}
