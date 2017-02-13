using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal abstract class ServiceDescriptor : IServiceDescriptor
    {
        protected ServiceDescriptor([NotNull] Type serviceType, [CanBeNull] string serviceName, ServiceLifetime lifetime)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));

            this.ServiceName = serviceName;
            this.ServiceType = serviceType;
            this.Lifetime = lifetime;
        }

        public string ServiceName { get; }
        
        public Type ServiceType { get; }

        public ServiceLifetime Lifetime { get; }
    }
}