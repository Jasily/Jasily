using System;
using System.Reflection;
using Jasily.Extensions.System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.AwaiterAdapter
{
    /// <summary>
    /// extension methods for <see cref="IServiceProvider"/>
    /// </summary>
    public static class ServiceProviderExtensions
    {
        [NotNull]
        public static IAwaitableAdapter GetAwaitableAdapter<T>([NotNull] this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetAwaitableAdapter(typeof(T));
        }

        [NotNull]
        public static IAwaitableAdapter GetAwaitableAdapter([NotNull] this IServiceProvider serviceProvider, [NotNull] Type type)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (type == null) throw new ArgumentNullException(nameof(type));

            var factory = serviceProvider.GetService<IAwaitableAdapterFactory>();
            if (factory == null)
            {
                throw new InvalidOperationException(
                    $"before invoke method, call `{nameof(ServiceCollectionServiceExtensions.UseAwaiterAdapter)}` by `IServiceCollection`.");
            }
            return factory.GetAwaitableAdapter(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        [NotNull]
        public static ITaskAdapter<T> GetTaskAdapter<T>([NotNull] this IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var taskWaiter = serviceProvider.GetService<ITaskAdapter<T>>();
            if (taskWaiter == null)
            {
                throw new InvalidOperationException(
                    $"before invoke method, call `{nameof(ServiceCollectionServiceExtensions.UseAwaiterAdapter)}` by `IServiceCollection`.");
            }
            return taskWaiter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="type"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        [NotNull]
        public static ITaskAdapter GetTaskAdapter([NotNull] this IServiceProvider serviceProvider, [NotNull] Type type)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (type == null) throw new ArgumentNullException(nameof(type));

            var taskWaiter = (ITaskAdapter)serviceProvider.GetService(typeof(ITaskAdapter<>).MakeGenericType(type));
            if (taskWaiter == null)
            {
                throw new InvalidOperationException(
                    $"before invoke method, call `{nameof(ServiceCollectionServiceExtensions.UseAwaiterAdapter)}` by `IServiceCollection`.");
            }
            return taskWaiter;
        }
    }
}