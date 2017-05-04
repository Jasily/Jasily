using JetBrains.Annotations;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.AwaiterAdapter
{
    /// <summary>
    /// extension methods for the module interfaces.
    /// </summary>
    public static class InternalExtensions
    {
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

            var taskWaiter = serviceProvider.GetTaskAdapter(instance.GetType());
            return taskWaiter.AwaitableAdapter.IsAwaitable ? taskWaiter.AwaitableAdapter.GetResult(instance) : instance;
        }

        /// <summary>
        /// if <paramref name="instance"/> is awaitable, return result of awaitable (null if void).
        /// if <paramref name="instance"/> is NOT awaitable, return <paramref name="instance"/>.
        /// </summary>
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
                while (result != instance && instance != null)
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

        /// <summary>
        /// wait any object with `await`.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        [PublicAPI]
        public static AsyncObject<T> Async<T>([NotNull] this IServiceProvider serviceProvider, T instance)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            return new AsyncObject<T>(instance, serviceProvider.GetTaskAdapter<T>());
        }
    }
}
