using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using Jasily.DependencyInjection.MethodInvoker.Internal;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public static class ServiceCollectionServiceExtensions
    {
        public static void UseMethodInvoker([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddSingleton(typeof(IMethodInvokerFactory<>), typeof(MethodInvokerFactory<>));
            serviceCollection.AddSingleton(typeof(ISingletonArguments<>), typeof(ConcurrentArguments<>));
            serviceCollection.AddScoped(typeof(IScopedArguments<>), typeof(ConcurrentArguments<>));
        }
    }
}
