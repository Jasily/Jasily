using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jasily.DependencyInjection.Internal;
using JetBrains.Annotations;
// ReSharper disable MemberCanBePrivate.Global

namespace Jasily.DependencyInjection
{
    public static class ServiceCollectionServiceExtensions
    {
        #region common

        public static IList<IServiceDescriptor> AddType<TService, TImplementation>([NotNull] this IList<IServiceDescriptor> collection,
            ServiceLifetime lifetime, [CanBeNull] string serviceName)
            where TImplementation : TService
            => AddType(collection, lifetime, typeof(TService), serviceName, typeof(TImplementation));

        public static IList<IServiceDescriptor> AddType([NotNull] this IList<IServiceDescriptor> collection,
            ServiceLifetime lifetime, [CanBeNull] string serviceName, [NotNull] Type serviceType)
            => AddType(collection, lifetime, serviceType, serviceName, serviceType);

        public static IList<IServiceDescriptor> AddType<TService>([NotNull] this IList<IServiceDescriptor> collection,
            ServiceLifetime lifetime, [CanBeNull] string serviceName)
            => AddType(collection, lifetime, typeof(TService), serviceName, typeof(TService));

        public static IList<IServiceDescriptor> AddType([NotNull] IList<IServiceDescriptor> collection,
            ServiceLifetime lifetime, [NotNull] Type serviceType, [CanBeNull] string serviceName, [NotNull] Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));

            var descriptor = new TypedServiceDescriptor(serviceType, serviceName, lifetime, new TypeDescriptor(implementationType));
            collection.Add(descriptor);
            return collection;
        }

        public static IList<IServiceDescriptor> AddTypeForImplementedInterfaces([NotNull] IList<IServiceDescriptor> collection,
            ServiceLifetime lifetime,
            [CanBeNull] string serviceName, [NotNull] Type implementationType)
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

        public static IList<IServiceDescriptor> AddFactory([NotNull] this IList<IServiceDescriptor> collection,
            ServiceLifetime lifetime, [NotNull] Type serviceType, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));

            var descriptor = new FactoryServiceDescriptor(serviceType, serviceName, lifetime, implementationFactory);
            collection.Add(descriptor);
            return collection;
        }

        public static IList<IServiceDescriptor> AddFactory<TService>([NotNull] this IList<IServiceDescriptor> collection,
            ServiceLifetime lifetime, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TService> implementationFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return collection.AddFactory(lifetime, typeof(TService), serviceName, p => implementationFactory(p));
        }

        public static IList<IServiceDescriptor> AddFactory<TService, TImplementation>([NotNull] this IList<IServiceDescriptor> collection,
            ServiceLifetime lifetime, [CanBeNull] string serviceName,
            [NotNull] Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : TService
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            return collection.AddFactory(lifetime, typeof(TService), serviceName, p => implementationFactory(p));
        }

        #endregion

        #region Singleton

        public static IList<IServiceDescriptor> AddSingletonInstance<TService>(
            [NotNull] this IList<IServiceDescriptor> collection, [CanBeNull] string serviceName,
            [CanBeNull] TService implementationInstance)
            => AddSingletonInstance(collection, typeof(TService), serviceName, implementationInstance);

        public static IList<IServiceDescriptor> AddSingletonInstance([NotNull] this IList<IServiceDescriptor> collection,
            [NotNull] Type serviceType, [CanBeNull] string serviceName, [CanBeNull] object implementationInstance)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));

            var serviceDescriptor = new InstanceServiceDescriptor(serviceType, serviceName, implementationInstance);
            collection.Add(serviceDescriptor);
            return collection;
        }

        #endregion
    }
}