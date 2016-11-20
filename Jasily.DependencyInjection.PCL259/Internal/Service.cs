using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal class Service : IValueStore
    {
        private IValueStore valueStore;
        private bool isValueCreated;
        private object value;
        private Func<ServiceProvider, object> instanceAccessor;

        public Service(IServiceDescriptor descriptor)
        {
            this.Descriptor = descriptor;
        }

        public IServiceDescriptor Descriptor { get; }

        [NotNull]
        public string ServiceName => this.Descriptor.ServiceName ?? string.Empty;

        [NotNull]
        public Type ServiceType => this.Descriptor.ServiceType;

        object IValueStore.GetValue(Service service, ServiceProvider provider, Func<ServiceProvider, object> creator)
        {
            if (this.isValueCreated) return this.value;

            lock (this)
            {
                this.value = creator(provider);
                this.isValueCreated = true;
            }

            return this.value;
        }

        public object GetValue(ServiceProvider provider)
        {
            if (this.instanceAccessor == null)
            {
                Interlocked.CompareExchange(ref this.instanceAccessor, this.CreateServiceAccessor(provider), null);
            }

            if (this.Descriptor.Lifetime != ServiceLifetime.Transient)
            {
                if (this.valueStore == null)
                {
                    var store = this.Descriptor.Lifetime == ServiceLifetime.Singleton
                        ? this
                        : (IValueStore) provider;
                    Interlocked.CompareExchange(ref this.valueStore, store, null);
                }

                return this.valueStore.GetValue(this, provider, this.instanceAccessor);
            }
            else
            {
                return this.instanceAccessor(provider);
            }
        }

        public IServiceCallSite GetCallSite(ServiceProvider provider, ISet<IServiceDescriptor> serviceChain)
        {
            var callSite = (this.Descriptor as IServiceCallSite) ??
                           (this.Descriptor as IServiceCallSiteProvider)?.CreateServiceCallSite(provider, serviceChain);
            if (callSite != null) return callSite;
            

            throw new NotImplementedException();
        }

        private Func<ServiceProvider, object> CreateServiceAccessor(ServiceProvider serviceProvider)
        {
            var callSite = this.GetCallSite(serviceProvider, new HashSet<IServiceDescriptor>());
            return callSite != null
                ? RealizeServiceAccessor(serviceProvider, this, callSite)
                : (_ => null);
        }

        private static Func<ServiceProvider, object> RealizeServiceAccessor(ServiceProvider serviceProvider, Service service, IServiceCallSite callSite)
        {
            var callCount = 0;
            return provider =>
            {
                if (Interlocked.Increment(ref callCount) == 2)
                {
                    Task.Run(() =>
                    {
                        var func = new CallSiteExpressionBuilder(new CallSiteRuntimeResolver()).Build(callSite);
                        Interlocked.Exchange(ref service.instanceAccessor, func);
                    });
                }
                return callSite.ResolveValue(provider);
            };
        }
    }
}