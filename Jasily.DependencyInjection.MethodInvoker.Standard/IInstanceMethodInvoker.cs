using System;
using JetBrains.Annotations;

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
        /// <param name="serviceProvider"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        object Invoke([NotNull] T instance, [NotNull]  IServiceProvider serviceProvider, OverrideArguments arguments);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        IInstanceMethodInvoker<T, TResult> CastTo<TResult>();
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
        /// <param name="serviceProvider"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        TResult Invoke(T instance, [NotNull]  IServiceProvider serviceProvider, OverrideArguments arguments);
    }
}
