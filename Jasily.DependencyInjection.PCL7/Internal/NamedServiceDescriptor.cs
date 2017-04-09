using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.Internal
{
    internal sealed class NamedServiceDescriptor : ServiceDescriptor
    {
        private static Func<System.IServiceProvider, object> NotImpl = p => throw new NotImplementedException();

        public ImplementationMode Mode { get; }

        [NotNull]
        public string ServiceName { get; }

        [CanBeNull]
        public object Instance { get; }

        [CanBeNull]
        public Func<IServiceProvider, object> Factory { get; }

        public NamedServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime,
            [CanBeNull] string name)
            : base(serviceType, implementationType, lifetime)
        {
            this.Mode = ImplementationMode.Typed;
            this.ServiceName = name ?? string.Empty;
        }

        public NamedServiceDescriptor(Type serviceType, object instance, ServiceLifetime lifetime,
            [CanBeNull] string name) 
            : base(serviceType, instance ?? new object())
        {
            this.Mode = ImplementationMode.Instance;
            this.ServiceName = name ?? string.Empty;
            this.Instance = instance;
        }

        public NamedServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime,
            [CanBeNull] string name)
            : base(serviceType, NotImpl, lifetime)
        {
            this.Mode = ImplementationMode.Factory;
            this.ServiceName = name ?? string.Empty;
            this.Factory = factory;
        }

        internal static NamedServiceDescriptor TryAssignName([NotNull] ServiceDescriptor descriptor, [CanBeNull] string name)
        {
            Debug.Assert(descriptor != null);

            if (descriptor is NamedServiceDescriptor d)
            {
                return d;
            }

            if (descriptor.ImplementationFactory != null)
            {
                return new NamedServiceDescriptor(descriptor.ServiceType, descriptor.ImplementationFactory, descriptor.Lifetime, name);
            }
            else if (descriptor.ImplementationType != null)
            {
                return new NamedServiceDescriptor(descriptor.ServiceType, descriptor.ImplementationType,
                    descriptor.Lifetime, name);
            }
            else if (descriptor.ImplementationInstance != null)
            {
                return new NamedServiceDescriptor(descriptor.ServiceType, descriptor.ImplementationInstance, ServiceLifetime.Singleton, name);
            }

            throw new NotImplementedException();
        }

        public static NamedServiceDescriptor DescribeValue<TValue>(TValue instance, ServiceLifetime lifetime, [CanBeNull] string name)
        {
            return new NamedServiceDescriptor(typeof(TValue), instance, lifetime, name);
        }

        public static NamedServiceDescriptor DescribeFactory<TValue>(Type serviceType, Func<IServiceProvider, object> factory,
            ServiceLifetime lifetime, [CanBeNull] string name)
        {
            return new NamedServiceDescriptor(serviceType, factory, lifetime, name);
        }

        public enum ImplementationMode
        {
            Typed, Instance, Factory
        }
    }
}