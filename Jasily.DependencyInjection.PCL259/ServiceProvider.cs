using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Jasily.DependencyInjection.Internal;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    public class ServiceProvider : IServiceProvider, IValueStore
    {
        private readonly Dictionary<Service, object> valueStore
            = new Dictionary<Service, object>(); 

        protected ServiceProvider()
        {
            this.RootProvider = (RootServiceProvider) this;
        }

        // This constructor is called exclusively to create a child scope from the parent
        internal ServiceProvider(ServiceProvider parent)
        {
            Debug.Assert(parent != null);
            this.RootProvider = parent.RootProvider;
            this.ParentProvider = parent;
        }

        [NotNull]
        internal RootServiceProvider RootProvider { get; }

        [CanBeNull]
        internal ServiceProvider ParentProvider { get; }

        /// <summary>
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [CanBeNull]
        public object GetService([NotNull] Type serviceType) => this.GetService(serviceType, null).Value;

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
            var provider = service.Descriptor.Lifetime == ServiceLifetime.Singleton
                ? this.RootProvider
                : this;
            var value = service.GetValue(provider);
            return new ResolveResult(value);
        }

        internal Service ResolveService([NotNull] Type serviceType, [CanBeNull] string serviceName)
        {
            var request = new ResolveRequest(serviceType, serviceName);
            for (var i = 0; i < this.RootProvider.ResolveMode.Length; i++)
            {
                var level = this.RootProvider.ResolveMode[i];
                var serviceEntry = this.RootProvider.ServiceResolver.ResolveServiceEntry(request, level);
                var service = serviceEntry?.Resolve(request, level);
                if (service != null) return service;
            }
            return null;
        }

        internal IServiceCallSite ResolveCallSite([NotNull] Type serviceType, [CanBeNull] string serviceName,
            ISet<Service> serviceChain)
        {
            var service = this.ResolveService(serviceType, serviceName);
            if (service == null) return null;

            try
            {
                if (!serviceChain.Add(service)) throw new InvalidOperationException();
                return service.GetCallSite(this, serviceChain);
            }
            finally
            {
                serviceChain.Remove(service);
            }
        }

        /// <summary>
        /// if cannot resolve, return null.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="serviceChain"></param>
        /// <returns></returns>
        internal IServiceCallSite[] ResolveCallSites(ParameterInfo[] parameters, ISet<Service> serviceChain)
        {
            var parameterCallSites = new IServiceCallSite[parameters.Length];
            for (var index = 0; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                var callSite = this.ResolveCallSite(parameter.ParameterType, parameter.Name, serviceChain);
                if (callSite == null && parameter.HasDefaultValue)
                {
                    callSite = new ConstantCallSite(parameter.DefaultValue);
                }
                if (callSite == null) return null;
                parameterCallSites[index] = callSite;
            }
            return parameterCallSites;
        }

        public virtual void Dispose()
        {
            foreach (var kvp in this.valueStore)
            {
                (kvp.Value as IDisposable)?.Dispose();
            }
            this.valueStore.Clear();
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

        public ServiceProvider CreateScope() => new ServiceProvider(this);

        #region static helper

        public static IList<IServiceDescriptor> CreateServiceCollection() => new List<IServiceDescriptor>();

        public static ServiceProvider Build([NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors) 
            => Build(serviceDescriptors, RootServiceProvider.DefaultResolveMode);

        public static ServiceProvider Build([NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors,
            [NotNull] IEnumerable<ResolveLevel> mode)
            => new RootServiceProvider(serviceDescriptors, mode, new ServiceProviderSettings());

        public static ServiceProvider Build([NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors, ServiceProviderSettings settings)
            => new RootServiceProvider(serviceDescriptors, RootServiceProvider.DefaultResolveMode, settings);

        public static ServiceProvider Build([NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors,
            [NotNull] IEnumerable<ResolveLevel> mode, ServiceProviderSettings settings)
            => new RootServiceProvider(serviceDescriptors, mode, settings);

        #endregion
    }
}
