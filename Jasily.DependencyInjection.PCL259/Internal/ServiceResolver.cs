using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal class ServiceResolver : IDisposable
    {
        private readonly List<Service> services
            = new List<Service>();
        private readonly Dictionary<Type, TypedServiceEntry> typedServices
            = new Dictionary<Type, TypedServiceEntry>();
        private readonly Dictionary<string, NamedServiceEntry> namedServices
            = new Dictionary<string, NamedServiceEntry>(StringComparer.OrdinalIgnoreCase);

        public ServiceResolver([NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors)
        {
            Debug.Assert(serviceDescriptors != null);
            this.Init(serviceDescriptors);
        }

        private void Init(IEnumerable<IServiceDescriptor> serviceDescriptors)
        {
            var descriptors = serviceDescriptors.ToArray();
            foreach (var descriptor in descriptors)
            {
                var service = new Service(descriptor);
                this.services.Add(service);
                TypedServiceEntry typed;
                if (!this.typedServices.TryGetValue(service.ServiceType, out typed))
                {
                    typed = new TypedServiceEntry();
                    this.typedServices.Add(service.ServiceType, typed);
                }
                typed.Add(service);
                NamedServiceEntry named;
                if (!this.namedServices.TryGetValue(service.ServiceName, out named))
                {
                    named = new NamedServiceEntry();
                    this.namedServices.Add(service.ServiceName, named);
                }
                named.Add(service);
            }
        }

        public ServiceEntry ResolveService([NotNull] Type serviceType, [CanBeNull] string serviceName, ResolveLevel level)
        {
            switch (level)
            {
                case ResolveLevel.TypeAndName:
                case ResolveLevel.Type:
                    TypedServiceEntry typedServiceEntry;
                    if (this.typedServices.TryGetValue(serviceType, out typedServiceEntry))
                        return typedServiceEntry;
                    break;
                case ResolveLevel.NameAndType:
                    if (serviceName != null)
                    {
                        NamedServiceEntry namedServiceEntry;
                        if (this.namedServices.TryGetValue(serviceName, out namedServiceEntry))
                            return namedServiceEntry;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }

        public void Dispose()
        {
            foreach (var service in this.services)
            {
                service.Dispose();
            }
            this.services.Clear();
            this.typedServices.Clear();
            this.namedServices.Clear();
        }
    }
}