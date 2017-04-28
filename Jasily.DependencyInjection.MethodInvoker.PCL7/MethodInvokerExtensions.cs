using JetBrains.Annotations;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// extension methods for the module.
    /// </summary>
    public static class MethodInvokerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="provider"/> or <paramref name="instance"/> or <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object InvokeInstanceMethod<T>([NotNull] this IServiceProvider provider,
            [NotNull] T instance, [NotNull] MethodInfo method, OverrideArguments arguments = default(OverrideArguments))
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (method == null) throw new ArgumentNullException(nameof(method));
            
            var factory = provider.GetService<IMethodInvokerFactory<T>>();
            if (factory == null) throw new InvalidOperationException("before invoke method, call `UseMethodInvoker()` by `IServiceCollection`.");
            return factory.GetInstanceMethodInvoker(method).Invoke(instance, arguments);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="provider"/> or <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object InvokeStaticMethod<T>([NotNull] this IServiceProvider provider,
            [NotNull] MethodInfo method, OverrideArguments arguments = default(OverrideArguments))
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (method == null) throw new ArgumentNullException(nameof(method));

            var factory = provider.GetService<IMethodInvokerFactory<T>>();
            if (factory == null) throw new InvalidOperationException("before invoke method, call `UseMethodInvoker()` by `IServiceCollection`.");
            return factory.GetStaticMethodInvoker(method).Invoke(arguments);
        }
    }
}
