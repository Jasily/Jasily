using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStaticMethodInvoker
    {
        /// <summary>
        /// invoke static method.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        object Invoke([NotNull] IServiceProvider serviceProvider, OverrideArguments arguments);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IStaticMethodInvoker<TResult> CastTo<TResult>();
    }

    /// <summary>
    /// you can cast <see cref="IStaticMethodInvoker"/> to <see cref="IStaticMethodInvoker{TResult}"/> to resolve result boxing.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IStaticMethodInvoker<out TResult>
    {
        /// <summary>
        /// invoke static method.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        TResult Invoke([NotNull] IServiceProvider serviceProvider, OverrideArguments arguments);
    }
}
