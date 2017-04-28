using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using Jasily.DependencyInjection.MethodInvoker.Internal;

namespace Jasily.DependencyInjection.MethodInvoker
{
    /// <summary>
    /// extension method for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionServiceExtensions
    {
        /// <summary>
        /// add <see cref="MethodInvoker"/> services to <paramref name="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="serviceCollection"/> is null.</exception>
        public static void UseMethodInvoker([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddSingleton(typeof(IMethodInvokerFactory<>), typeof(MethodInvokerFactory<>));
            serviceCollection.AddSingleton(typeof(ISingletonArguments<>), typeof(ConcurrentArguments<>));
            serviceCollection.AddScoped(typeof(IScopedArguments<>), typeof(ConcurrentArguments<>));
        }
    }
}
