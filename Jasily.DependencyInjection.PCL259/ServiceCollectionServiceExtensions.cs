using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jasily.DependencyInjection.Internal;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    public static class ServiceCollectionServiceExtensions
    {
        #region Transient

        public static IList<IServiceDescriptor> AddTransientForImplementedInterfaces([NotNull] this IList<IServiceDescriptor> collection,
            [CanBeNull] string serviceName, [NotNull] Type implementationType)
            => AddForImplementedInterfaces(collection, serviceName, ServiceLifetime.Transient, implementationType);

        public static IList<IServiceDescriptor> AddTransient([NotNull] this IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName,
            [NotNull] Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            return Add(collection, serviceType, serviceName, ServiceLifetime.Transient, new TypeDescriptor(implementationType));
        }

        public static IList<IServiceDescriptor> AddTransient([NotNull] this IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return Add(collection, serviceType, serviceName, ServiceLifetime.Transient, implementationFactory);
        }

        public static IList<IServiceDescriptor> AddTransient<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName)
            where TImplementation : TService
            => AddTransient(collection, typeof(TService), serviceName, typeof(TImplementation));

        public static IList<IServiceDescriptor> AddTransient([NotNull] this IList<IServiceDescriptor> collection,
            [CanBeNull] string serviceName, [NotNull] Type serviceType)
            => AddTransient(collection, serviceType, serviceName, serviceType);

        public static IList<IServiceDescriptor> AddTransient<TService>([NotNull] this IList<IServiceDescriptor> collection,
            [CanBeNull] string serviceName)
            => AddTransient(collection, typeof(TService), serviceName, typeof(TService));

        public static IList<IServiceDescriptor> AddTransient<TService>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return collection.AddTransient(typeof(TService), serviceName, implementationFactory);
        }

        public static IList<IServiceDescriptor> AddTransient<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : class, TService
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return AddTransient(collection, typeof(TService), serviceName, implementationFactory);
        }

        #endregion

        #region Scoped

        public static IList<IServiceDescriptor> AddScopedForImplementedInterfaces([NotNull] this IList<IServiceDescriptor> collection,
            [CanBeNull] string serviceName, [NotNull] Type implementationType)
            => AddForImplementedInterfaces(collection, serviceName, ServiceLifetime.Scoped, implementationType);

        public static IList<IServiceDescriptor> AddScoped([NotNull] this IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName,
            [NotNull] Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            return Add(collection, serviceType, serviceName, ServiceLifetime.Scoped, new TypeDescriptor(implementationType));
        }

        public static IList<IServiceDescriptor> AddScoped(
            [NotNull] this IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return Add(collection, serviceType, serviceName, ServiceLifetime.Scoped, implementationFactory);
        }

        public static IList<IServiceDescriptor> AddScoped<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName)
            where TImplementation : TService
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            return AddScoped(collection, typeof(TService), serviceName, typeof(TImplementation));
        }

        public static IList<IServiceDescriptor> AddScoped([NotNull] this IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName)
            => AddScoped(collection, serviceType, serviceName, serviceType);

        public static IList<IServiceDescriptor> AddScoped<TService>([NotNull] this IList<IServiceDescriptor> collection,
            [CanBeNull] string serviceName)
            => AddScoped(collection, typeof(TService), serviceName);

        public static IList<IServiceDescriptor> AddScoped<TService>([NotNull] this IList<IServiceDescriptor> collection,
            [CanBeNull] string serviceName, [NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return collection.AddScoped(typeof(TService), serviceName, implementationFactory);
        }

        public static IList<IServiceDescriptor> AddScoped<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : class, TService
            => AddScoped(collection, typeof(TService), serviceName, implementationFactory);

        #endregion

        #region Singleton

        public static IList<IServiceDescriptor> AddSingletonForImplementedInterfaces([NotNull] this IList<IServiceDescriptor> collection,
            [CanBeNull] string serviceName, [NotNull] Type implementationType)
            => AddForImplementedInterfaces(collection, serviceName, ServiceLifetime.Singleton, implementationType);

        public static IList<IServiceDescriptor> AddSingleton([NotNull] this IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName, [NotNull] Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            return Add(collection, serviceType, serviceName, ServiceLifetime.Singleton, new TypeDescriptor(implementationType));
        }

        public static IList<IServiceDescriptor> AddSingleton([NotNull] this IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName, [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return Add(collection, serviceType, serviceName, ServiceLifetime.Singleton, implementationFactory);
        }

        public static IList<IServiceDescriptor> AddSingleton<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName)
            where TImplementation : TService
            => AddSingleton(collection, typeof(TService), serviceName, typeof(TImplementation));

        public static IList<IServiceDescriptor> AddSingleton([NotNull] this IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName)
            => AddSingleton(collection, serviceType, serviceName, serviceType);

        public static IList<IServiceDescriptor> AddSingleton<TService>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName)
            => AddSingleton(collection, serviceName, typeof(TService));

        public static IList<IServiceDescriptor> AddSingleton<TService>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TService> implementationFactory)
            => AddSingleton(collection, typeof(TService), serviceName, implementationFactory);

        public static IList<IServiceDescriptor> AddSingleton<TService, TImplementation>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : TService
            => AddSingleton(collection, typeof(TService), serviceName, implementationFactory);

        public static IList<IServiceDescriptor> AddSingleton<TService>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName,
            [CanBeNull] TService implementationInstance)
            => AddSingleton(collection, typeof(TService), serviceName, implementationInstance);

        public static IList<IServiceDescriptor> AddSingleton([NotNull] this IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName, [CanBeNull] object implementationInstance)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));

            var serviceDescriptor = new InstanceServiceDescriptor(serviceType, serviceName, implementationInstance);
            collection.Add(serviceDescriptor);
            return collection;
        }

        #endregion

        #region base

        private static IList<IServiceDescriptor> AddForImplementedInterfaces(
            [NotNull] IList<IServiceDescriptor> collection,
            [CanBeNull] string serviceName, ServiceLifetime lifetime,
            [NotNull] Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            var serviceTypes = implementationType.GetTypeInfo().ImplementedInterfaces.ToArray();
            if (serviceTypes.Length > 0)
            {
                var implementationTypeDescriptor = new TypeDescriptor(implementationType, serviceTypes.Length);
                foreach (var serviceType in serviceTypes)
                {
                    collection.Add(new TypedServiceDescriptor(serviceType, serviceName, lifetime, implementationTypeDescriptor));
                }
            }
            return collection;
        }

        private static IList<IServiceDescriptor> Add(
            [NotNull] IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName, ServiceLifetime lifetime,
            [NotNull] TypeDescriptor implementationTypeDescriptor)
        {
            var descriptor = new TypedServiceDescriptor(serviceType, serviceName, lifetime, implementationTypeDescriptor);
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