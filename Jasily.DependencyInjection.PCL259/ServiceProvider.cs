using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Jasily.DependencyInjection.Internal;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    public class ServiceProvider : IServiceProvider, IDisposable, IValueStore
    {
        private readonly Dictionary<Type, TypedServiceEntry> typedServices
            = new Dictionary<Type, TypedServiceEntry>();
        private readonly Dictionary<string, NamedServiceEntry> namedServices
            = new Dictionary<string, NamedServiceEntry>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Service, object> valueStore
            = new Dictionary<Service, object>(); 

        protected ServiceProvider([NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors)
        {
            if (serviceDescriptors == null) throw new ArgumentNullException(nameof(serviceDescriptors));
            this.RootProvider = (RootServiceProvider) this;
            this.Init(serviceDescriptors);
        }

        // This constructor is called exclusively to create a child scope from the parent
        internal ServiceProvider(ServiceProvider parent)
        {
            Debug.Assert(parent != null);
            this.RootProvider = parent.RootProvider;
            this.ParentProvider = parent;
        }

        private void Init(IEnumerable<IServiceDescriptor> serviceDescriptors)
        {
            var descriptors = serviceDescriptors.ToArray();
            foreach (var descriptor in descriptors)
            {
                var service = new Service(descriptor);
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

        [NotNull]
        internal RootServiceProvider RootProvider { get; }

        [CanBeNull]
        internal ServiceProvider ParentProvider { get; }

        /// <summary>
        /// return a <see cref="ResolveResult"/> object.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public object GetService([NotNull] Type serviceType) => this.GetService(serviceType, null);

        /// <summary>
        /// return a <see cref="ResolveResult"/> object.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public ResolveResult GetService([NotNull] Type serviceType, [CanBeNull] string serviceName)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));

            var service = this.ResolveService(serviceType, serviceName);
            if (service == null) return ResolveResult.None;
            var value = service.GetValue(this);
            return new ResolveResult(value);
        }

        internal Service ResolveService([NotNull] Type serviceType, [CanBeNull] string serviceName)
        {
            for (var i = 0; i < this.RootProvider.ResolveMode.Length; i++)
            {
                var level = this.RootProvider.ResolveMode[i];

                // current
                var serviceEntry = this.ResolveService(serviceType, serviceName, level);
                var service = serviceEntry?.Resolve(serviceType, serviceName, level);
                if (service != null) return service;

                // parent
                serviceEntry = this.ParentProvider?.ResolveService(serviceType, serviceName, level);
                service = serviceEntry?.Resolve(serviceType, serviceName, level);
                if (service != null) return service;

            }
            return null;
        }

        private ServiceEntry ResolveService([NotNull] Type serviceType, [CanBeNull] string serviceName, ResolveLevel level)
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

        internal IServiceCallSite ResolveServiceCallSite([NotNull] Type serviceType, [CanBeNull] string serviceName, ISet<IServiceDescriptor> serviceChain)
        {
            var service = this.ResolveService(serviceType, serviceName);
            if (service == null) return null;

            try
            {
                if (!serviceChain.Add(service.Descriptor)) throw new InvalidOperationException();
                return service.GetCallSite(this, serviceChain);
            }
            finally
            {
                serviceChain.Remove(service.Descriptor);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        object IValueStore.GetValue(Service service, ServiceProvider provider, Func<ServiceProvider, object> creator)
        {
            object value;
            if (!this.valueStore.TryGetValue(service, out value))
            {
                value = creator(provider);
                this.valueStore.Add(service, value);
            }
            return value;
        }

        public static IList<IServiceDescriptor> CreateServiceCollection() => new List<IServiceDescriptor>();

        public static ServiceProvider Build([NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors) 
            => new RootServiceProvider(serviceDescriptors, RootServiceProvider.DefaultResolveMode);

        public static ServiceProvider Build([NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors,
            [NotNull] IEnumerable<ResolveLevel> mode)
            => new RootServiceProvider(serviceDescriptors, mode);
    }
}
