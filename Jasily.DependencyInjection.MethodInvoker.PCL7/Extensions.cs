using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public static class ServiceCollectionServiceExtensions
    {
        public static void UseMethodInvoker([NotNull] this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddSingleton(typeof(IMethodInvoker<>), typeof(ClassMethodInvoker<>));
            serviceCollection.AddSingleton(typeof(ISingletonArguments<>), typeof(SingletonArguments<>));
            serviceCollection.AddScoped(typeof(IScopedArguments<>), typeof(ScopedArguments<>));
        }
    }
}
