using System;

namespace Jasily.DependencyInjection.AwaiterAdapter
{
    public interface ITaskAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        IAwaitableAdapter AwaitableAdapter { get; }
    }

    /// <summary>
    /// provide interface to get <see cref="ITaskAdapter"/> from <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITaskAdapter<T> : ITaskAdapter
    {
    }
}
