using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jasily.DependencyInjection.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Jasily.DependencyInjection.Internal.CallSites;

// ReSharper disable MemberCanBePrivate.Global

namespace Jasily.DependencyInjection
{
    public static class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AssignNameToLast([NotNull] this IServiceCollection services, [NotNull] string name)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (services.Count == 0) throw new InvalidOperationException();
            services[services.Count - 1] = services[services.Count - 1].AssignName(name);
            return services;
        }

        public static IServiceCollection AddTransientValue<TService>([NotNull] this IServiceCollection services,
            TService implementationInstance, [CanBeNull] string name = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var d = NamedServiceDescriptor.DescribeValue(implementationInstance, ServiceLifetime.Transient, name);
            services.Add(d);
            return services;
        }

        public static IServiceCollection AddScopedValue<TService>([NotNull] this IServiceCollection services,
            TService implementationInstance, [CanBeNull] string name = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var d = NamedServiceDescriptor.DescribeValue(implementationInstance, ServiceLifetime.Scoped, name);
            services.Add(d);
            return services;
        }

        public static IServiceCollection AddSingletonValue<TService>([NotNull] this IServiceCollection services,
            TService implementationInstance, [CanBeNull] string name = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var d = NamedServiceDescriptor.DescribeValue(implementationInstance, ServiceLifetime.Singleton, name);
            services.Add(d);
            return services;
        }
    }
}