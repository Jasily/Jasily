using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.Internal
{
    internal sealed class NamedServiceDescriptor : ServiceDescriptor
    {
        [NotNull]
        public string ServiceName { get; }

        [CanBeNull]
        public object Instance { get; }

        public NamedServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime, [CanBeNull] string name)
            : base(serviceType, implementationType, lifetime)
        {
            this.ServiceName = name ?? string.Empty;
        }

        public NamedServiceDescriptor(Type serviceType, object instance, ServiceLifetime lifetime, [CanBeNull] string name) 
            : base(serviceType, instance ?? new object())
        {
            this.ServiceName = name ?? string.Empty;
            this.Instance = instance;
        }

        public NamedServiceDescriptor(Type serviceType, Func<System.IServiceProvider, object> factory, ServiceLifetime lifetime, [CanBeNull] string name)
            : base(serviceType, factory, lifetime)
        {
            this.ServiceName = name ?? string.Empty;
        }

        internal static NamedServiceDescriptor TryAssignName([NotNull] ServiceDescriptor descriptor, [CanBeNull] string name)
        {
            Debug.Assert(descriptor != null);

            if (descriptor is NamedServiceDescriptor d)
            {
                Debug.Assert(name == null);
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
    }
}