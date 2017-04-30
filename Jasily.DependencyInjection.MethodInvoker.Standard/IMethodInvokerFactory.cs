using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IMethodInvokerFactory<in T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IInstanceMethodInvoker<T> GetInstanceMethodInvoker([NotNull] MethodInfo method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IStaticMethodInvoker GetStaticMethodInvoker([NotNull] MethodInfo method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructor"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="constructor"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IStaticMethodInvoker GetConstructorInvoker([NotNull] ConstructorInfo constructor);
    }
}
