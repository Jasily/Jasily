using System;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInstanceMethodInvoker<in T>
    {
        /// <summary>
        /// invoke instance method.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> is null.</exception>
        /// <returns></returns>
        object Invoke(T instance, OverrideArguments arguments);
    }

    /// <summary>
    /// you can cast <see cref="IInstanceMethodInvoker{T}"/> to <see cref="IInstanceMethodInvoker{T, TResult}"/> to resolve result boxing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IInstanceMethodInvoker<in T, out TResult>
    {
        /// <summary>
        /// invoke instance method.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> is null.</exception>
        /// <returns></returns>
        TResult Invoke(T instance, OverrideArguments arguments);
    }
}
