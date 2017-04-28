using System;
using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IMethodInvokerFactory<in T>
    {
        object InvokeInstanceMethod(MethodInfo method, T instance, OverrideArguments arguments = default(OverrideArguments));

        object InvokeStaticMethod(MethodInfo method, OverrideArguments arguments = default(OverrideArguments));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IInstanceMethodInvoker<T> GetInstanceMethodInvoker(MethodInfo method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        IStaticMethodInvoker GetStaticMethodInvoker(MethodInfo method);
    }
}
