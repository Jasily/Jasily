using System;
using System.Collections.Generic;
using System.Text;
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
        /// add <see cref="ITaskAdapter{T}"/> services to <paramref name="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceCollection"/> is null.</exception>
        public static void UseAwaiterAdapter([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            
            // ensure use method invoker.
            serviceCollection.UseMethodInvoker();

            serviceCollection.AddSingleton(typeof(TaskAwaiterAdapter<,>));
            serviceCollection.AddSingleton(typeof(TaskAwaiterAdapter<,,>));
            serviceCollection.AddSingleton(typeof(ITaskAdapter<>), typeof(TaskAdapter<>));
        }
    }
}
