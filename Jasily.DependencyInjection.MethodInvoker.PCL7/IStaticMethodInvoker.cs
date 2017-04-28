using System;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStaticMethodInvoker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        object Invoke(OverrideArguments arguments);

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
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        TResult Invoke(OverrideArguments arguments);
    }
}
