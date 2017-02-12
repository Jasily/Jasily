using System;
using System.Collections.Generic;
using System.Reflection;
using Jasily.DependencyInjection.Internal;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    public static class ServiceCollectionServiceExtensions
    {
        #region Transient

        public static IList<IServiceDescriptor> AddTransientForImplementedInterfaces([NotNull] this IList<IServiceDescriptor> services,
            [CanBeNull] string serviceName, [NotNull] Type implementationType)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            foreach (var serviceType in implementationType.GetTypeInfo().ImplementedInterfaces)
            {
                Add(services, serviceType, serviceName, ServiceLifetime.Transient, implementationType);
            }
            return services;
        }

        public static IList<IServiceDescriptor> AddTransient([NotNull] this IList<IServiceDescriptor> services,
            [NotNull] Type serviceType, [CanBeNull] string serviceName,
            [NotNull] Type implementationType)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            return Add(services, serviceType, serviceName, ServiceLifetime.Transient, implementationType);
        }

        public static IList<IServiceDescriptor> AddTransient([NotNull] this IList<IServiceDescriptor> services,
            [NotNull] Type serviceType, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return Add(services, serviceType, serviceName, ServiceLifetime.Transient, implementationFactory);
        }

        public static IList<IServiceDescriptor> AddTransient<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> services, [CanBeNull] string serviceName)
            where TImplementation : TService
            => AddTransient(services, typeof(TService), serviceName, typeof(TImplementation));

        public static IList<IServiceDescriptor> AddTransient([NotNull] this IList<IServiceDescriptor> services,
            [CanBeNull] string serviceName, [NotNull] Type serviceType)
            => AddTransient(services, serviceType, serviceName, serviceType);

        public static IList<IServiceDescriptor> AddTransient<TService>([NotNull] this IList<IServiceDescriptor> services,
            [CanBeNull] string serviceName)
            => AddTransient(services, typeof(TService), serviceName, typeof(TService));

        public static IList<IServiceDescriptor> AddTransient<TService>(
            [NotNull] this IList<IServiceDescriptor> services, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return services.AddTransient(typeof(TService), serviceName, implementationFactory);
        }

        public static IList<IServiceDescriptor> AddTransient<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> services, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : class, TService
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return AddTransient(services, typeof(TService), serviceName, implementationFactory);
        }

        #endregion

        #region Scoped

        public static IList<IServiceDescriptor> AddScopedForImplementedInterfaces([NotNull] this IList<IServiceDescriptor> services,
            [CanBeNull] string serviceName, [NotNull] Type implementationType)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            foreach (var serviceType in implementationType.GetTypeInfo().ImplementedInterfaces)
            {
                Add(services, serviceType, serviceName, ServiceLifetime.Scoped, implementationType);
            }
            return services;
        }

        public static IList<IServiceDescriptor> AddScoped([NotNull] this IList<IServiceDescriptor> services,
            [NotNull] Type serviceType, [CanBeNull] string serviceName,
            [NotNull] Type implementationType)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            return Add(services, serviceType, serviceName, ServiceLifetime.Scoped, implementationType);
        }

        public static IList<IServiceDescriptor> AddScoped(
            [NotNull] this IList<IServiceDescriptor> services,
            [NotNull] Type serviceType, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return Add(services, serviceType, serviceName, ServiceLifetime.Scoped, implementationFactory);
        }

        public static IList<IServiceDescriptor> AddScoped<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> services, [CanBeNull] string serviceName)
            where TImplementation : TService
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            return AddScoped(services, typeof(TService), serviceName, typeof(TImplementation));
        }

        public static IList<IServiceDescriptor> AddScoped([NotNull] this IList<IServiceDescriptor> services,
            [NotNull] Type serviceType, [CanBeNull] string serviceName)
            => AddScoped(services, serviceType, serviceName, serviceType);

        public static IList<IServiceDescriptor> AddScoped<TService>([NotNull] this IList<IServiceDescriptor> services,
            [CanBeNull] string serviceName)
            => AddScoped(services, typeof(TService), serviceName);

        public static IList<IServiceDescriptor> AddScoped<TService>([NotNull] this IList<IServiceDescriptor> services,
            [CanBeNull] string serviceName, [NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return services.AddScoped(typeof(TService), serviceName, implementationFactory);
        }

        public static IList<IServiceDescriptor> AddScoped<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> services, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : class, TService
            => AddScoped(services, typeof(TService), serviceName, implementationFactory);

        #endregion

        #region Singleton

        public static IList<IServiceDescriptor> AddSingletonForImplementedInterfaces([NotNull] this IList<IServiceDescriptor> services,
            [CanBeNull] string serviceName, [NotNull] Type implementationType)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            foreach (var serviceType in implementationType.GetTypeInfo().ImplementedInterfaces)
            {
                Add(services, serviceType, serviceName, ServiceLifetime.Singleton, implementationType);
            }
            return services;
        }

        public static IList<IServiceDescriptor> AddSingleton([NotNull] this IList<IServiceDescriptor> services,
            [NotNull] Type serviceType, [CanBeNull] string serviceName, [NotNull] Type implementationType)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            return Add(services, serviceType, serviceName, ServiceLifetime.Singleton, implementationType);
        }

        public static IList<IServiceDescriptor> AddSingleton([NotNull] this IList<IServiceDescriptor> services,
            [NotNull] Type serviceType, [CanBeNull] string serviceName, [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return Add(services, serviceType, serviceName, ServiceLifetime.Singleton, implementationFactory);
        }

        public static IList<IServiceDescriptor> AddSingleton<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> services, [CanBeNull] string serviceName)
            where TImplementation : TService
            => AddSingleton(services, typeof(TService), serviceName, typeof(TImplementation));

        public static IList<IServiceDescriptor> AddSingleton([NotNull] this IList<IServiceDescriptor> services,
            [NotNull] Type serviceType, [CanBeNull] string serviceName)
            => AddSingleton(services, serviceType, serviceName, serviceType);

        public static IList<IServiceDescriptor> AddSingleton<TService>(
            [NotNull] this IList<IServiceDescriptor> services, [CanBeNull] string serviceName)
            => AddSingleton(services, serviceName, typeof(TService));

        public static IList<IServiceDescriptor> AddSingleton<TService>(
            [NotNull] this IList<IServiceDescriptor> services, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TService> implementationFactory)
            => AddSingleton(services, typeof(TService), serviceName, implementationFactory);

        public static IList<IServiceDescriptor> AddSingleton<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> services, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : TService
            => AddSingleton(services, typeof(TService), serviceName, implementationFactory);

        public static IList<IServiceDescriptor> AddSingleton<TService>(
            [NotNull] this IList<IServiceDescriptor> services, [CanBeNull] string serviceName,
            [CanBeNull] TService implementationInstance)
            => AddSingleton(services, typeof(TService), serviceName, implementationInstance);

        public static IList<IServiceDescriptor> AddSingleton([NotNull] this IList<IServiceDescriptor> services,
            [NotNull] Type serviceType, [CanBeNull] string serviceName, [CanBeNull] object implementationInstance)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));

            var serviceDescriptor = new InstanceServiceDescriptor(serviceType, serviceName, implementationInstance);
            services.Add(serviceDescriptor);
            return services;
        }

        #endregion

        #region base

        private static IList<IServiceDescriptor> Add([NotNull]IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName, ServiceLifetime lifetime,
            [NotNull] Type implementationType)
        {
            var descriptor = new TypedServiceDescriptor(serviceType, serviceName, lifetime, implementationType);
            collection.Add(descriptor);
            return collection;
        }

        private static IList<IServiceDescriptor> Add(
            [NotNull] IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName, ServiceLifetime lifetime,
            [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            var descriptor = new FactoryServiceDescriptor(serviceType, serviceName, lifetime, implementationFactory);
            collection.Add(descriptor);
            return collection;
        }

        #endregion
    }
}