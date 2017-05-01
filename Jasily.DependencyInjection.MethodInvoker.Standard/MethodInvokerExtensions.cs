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
        /// <param name="serviceProvider"></param>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">
        /// throw if <paramref name="serviceProvider"/> 
        /// or <paramref name="instance"/> 
        /// or <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object InvokeInstanceMethod<T>([NotNull] this IServiceProvider serviceProvider,
            [NotNull] MethodInfo method, [NotNull] T instance, OverrideArguments arguments = default(OverrideArguments))
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (method == null) throw new ArgumentNullException(nameof(method));

            using (var scope = serviceProvider.CreateScope())
            {
                var factory = scope.ServiceProvider.GetService<IMethodInvokerFactory<T>>();
                if (factory == null) throw new InvalidOperationException("before invoke method, call `UseMethodInvoker()` by `IServiceCollection`.");
                return factory.GetInstanceMethodInvoker(method).Invoke(instance, scope.ServiceProvider, arguments);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceProvider"/> or <paramref name="method"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object InvokeStaticMethod<T>([NotNull] this IServiceProvider serviceProvider,
            [NotNull] MethodInfo method, OverrideArguments arguments = default(OverrideArguments))
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (method == null) throw new ArgumentNullException(nameof(method));

            using (var scope = serviceProvider.CreateScope())
            {
                var factory = scope.ServiceProvider.GetService<IMethodInvokerFactory<T>>();
                if (factory == null) throw new InvalidOperationException("before invoke method, call `UseMethodInvoker()` by `IServiceCollection`.");
                return factory.GetStaticMethodInvoker(method).Invoke(scope.ServiceProvider, arguments);
            }
        }

        public static object InvokeConstructor<T>([NotNull] this IServiceProvider serviceProvider,
            [NotNull] ConstructorInfo constructor, OverrideArguments arguments = default(OverrideArguments))
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (constructor == null) throw new ArgumentNullException(nameof(constructor));

            using (var scope = serviceProvider.CreateScope())
            {
                var factory = scope.ServiceProvider.GetService<IMethodInvokerFactory<T>>();
                if (factory == null)
                    throw new InvalidOperationException("before invoke constructor, call `UseMethodInvoker()` by `IServiceCollection`.");
                return factory.GetConstructorInvoker(constructor).Invoke(scope.ServiceProvider, arguments);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">
        /// throw if <paramref name="factory"/>
        /// or <paramref name="instance"/> 
        /// or <paramref name="method"/>
        /// or <paramref name="serviceProvider"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object InvokeInstanceMethod<T>([NotNull] this IMethodInvokerFactory<T> factory,
            [NotNull] MethodInfo method, [NotNull] T instance, [NotNull] IServiceProvider serviceProvider,
            OverrideArguments arguments = default(OverrideArguments))
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            using (var scope = serviceProvider.CreateScope())
            {
                return factory.GetInstanceMethodInvoker(method).Invoke(instance, scope.ServiceProvider, arguments);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">
        /// throw if <paramref name="factory"/> 
        /// or <paramref name="method"/> 
        /// or <paramref name="serviceProvider"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object InvokeStaticMethod<T>([NotNull] this IMethodInvokerFactory<T> factory,
            [NotNull] MethodInfo method, [NotNull] IServiceProvider serviceProvider,
            OverrideArguments arguments = default(OverrideArguments))
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            using (var scope = serviceProvider.CreateScope())
            {
                return factory.GetStaticMethodInvoker(method).Invoke(scope.ServiceProvider, arguments);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="constructor"></param>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException">
        /// throw if <paramref name="factory"/> 
        /// or <paramref name="constructor"/> 
        /// or <paramref name="serviceProvider"/> is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object InvokeConstructor<T>([NotNull] this IMethodInvokerFactory<T> factory,
            [NotNull] ConstructorInfo constructor, [NotNull] IServiceProvider serviceProvider,
            OverrideArguments arguments = default(OverrideArguments))
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (constructor == null) throw new ArgumentNullException(nameof(constructor));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            using (var scope = serviceProvider.CreateScope())
            {
                return factory.GetConstructorInvoker(constructor).Invoke(scope.ServiceProvider, arguments);
            }
        }

        /// <summary>
        /// if <paramref name="instance"/> is awaitable, return result of awaitable (null if void).
        /// if <paramref name="instance"/> is NOT awaitable, return <paramref name="instance"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object GetValueOrAwaitableResult([NotNull] this IServiceProvider serviceProvider, [NotNull] object instance)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var taskWaiter = (ITypeWaiter) serviceProvider.GetService(typeof(ITypeWaiter<>).MakeGenericType(instance.GetType()));
            if (taskWaiter == null)
            {
                throw new InvalidOperationException("before invoke method, call `UseMethodInvoker()` by `IServiceCollection`.");
            }
            if (taskWaiter.AwaiterAdapter.IsAwaitable)
            {
                return taskWaiter.GetResult(instance);
            }
            return instance;
        }

        /// <summary>
        /// if <paramref name="instance"/> is awaitable, return result of awaitable (null if void).
        /// if <paramref name="instance"/> is NOT awaitable, return <paramref name="instance"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="instance"></param>
        /// <param name="recursive"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public static object GetValueOrAwaitableResult([NotNull] this IServiceProvider serviceProvider, [NotNull] object instance,
            bool recursive)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            if (recursive)
            {
                object result = null;
                while (result != instance)
                {
                    result = instance;
                    instance = serviceProvider.GetValueOrAwaitableResult(instance);
                }
                return instance;
            }
            else
            {
                return serviceProvider.GetValueOrAwaitableResult(instance);
            }
        }

        #region default(OverrideArguments)

        /// <summary>
        /// invoke instance method.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static TResult Invoke<T, TResult>([NotNull] this IInstanceMethodInvoker<T, TResult> invoker,
            [NotNull] T instance, [NotNull] IServiceProvider serviceProvider)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return invoker.Invoke(instance, serviceProvider, default(OverrideArguments));
        }

        /// <summary>
        /// invoke instance method.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static object Invoke<T>([NotNull] this IInstanceMethodInvoker<T> invoker,
            [NotNull] T instance, [NotNull] IServiceProvider serviceProvider)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return invoker.Invoke(instance, serviceProvider, default(OverrideArguments));
        }

        /// <summary>
        /// invoke static method.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static TResult Invoke<TResult>([NotNull] this IStaticMethodInvoker<TResult> invoker, [NotNull] IServiceProvider serviceProvider)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return invoker.Invoke(serviceProvider, default(OverrideArguments));
        }

        /// <summary>
        /// invoke static method.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="instance"/> or <paramref name="serviceProvider"/> is null.</exception>
        /// <returns></returns>
        public static object Invoke([NotNull] this IStaticMethodInvoker invoker, [NotNull] IServiceProvider serviceProvider)
        {
            if (invoker == null) throw new ArgumentNullException(nameof(invoker));
            return invoker.Invoke(serviceProvider, default(OverrideArguments));
        }

        #endregion
    }
}
