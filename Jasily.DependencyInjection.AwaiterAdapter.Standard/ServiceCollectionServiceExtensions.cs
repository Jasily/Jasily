using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jasily.DependencyInjection.AwaiterAdapter.Internal;
using Jasily.DependencyInjection.MemberInjection;
using Jasily.DependencyInjection.MethodInvoker;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.AwaiterAdapter
{
    /// <summary>
    /// extension method for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionServiceExtensions
    {
        /// <summary>
        /// Add <see cref="ITaskAdapter{T}"/> services to <paramref name="serviceCollection"/>.
        /// After add, you can use <see cref="ITaskAdapter{T}"/> to get singleton task adapter.
        /// E.g: <see cref="IServiceProvider"/>.GetTaskAdapter(typeof(<see cref="Task{T}"/>))
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceCollection"/> is null.</exception>
        public static void UseAwaiterAdapter([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            
            // ensure use method invoker.
            serviceCollection.UseMethodInvoker();

            serviceCollection.AddSingleton(typeof(VoidAwaitableAdapter<,>));
            serviceCollection.AddSingleton(typeof(GenericAwaitableAdapter<,,>));
            serviceCollection.AddSingleton<IAwaitableAdapterFactory, AwaitableAdapterFactory>();
            serviceCollection.AddSingleton(typeof(ITaskAdapter<>), typeof(TaskAdapter<>));
        }
    }
}
