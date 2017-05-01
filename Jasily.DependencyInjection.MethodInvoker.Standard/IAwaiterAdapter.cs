using System;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IAwaiterAdapter
    {
        bool IsAwaitable { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        /// <returns></returns>
        object GetResult(object instance);
    }

    public interface IAwaiterAdapter<in TAwaiter, out TResult> : IAwaiterAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">throw is not IsAwaitable.</exception>
        /// <returns></returns>
        TResult GetResult(TAwaiter instance);
    }
}
