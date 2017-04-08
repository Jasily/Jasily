using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Jasily.DependencyInjection.Internal.CallSites;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.Internal
{
    internal class Service : IDisposable
    {
        private readonly ValueStore valueStore;
        private Func<ServiceProvider, object> instanceAccessor;
        private TypeInfo serviceTypeInfo;
        private IServiceCallSite callSite;

        public Service(NamedServiceDescriptor descriptor)
        {
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
                this.valueStore = new ValueStore();

            this.Descriptor = descriptor;
        }

        public NamedServiceDescriptor Descriptor { get; }

        [NotNull]
        public string ServiceName => this.Descriptor.ServiceName;

        [NotNull]
        public Type ServiceType => this.Descriptor.ServiceType;

        [NotNull]
        public TypeInfo ServiceTypeInfo => this.serviceTypeInfo ?? (this.serviceTypeInfo = this.ServiceType.GetTypeInfo());

        public object GetValue(ServiceProvider provider)
        {
            if (this.instanceAccessor == null)
            {
                Interlocked.CompareExchange(ref this.instanceAccessor, this.CreateServiceAccessor(provider), null);
            }

            var store = this.Descriptor.Lifetime == ServiceLifetime.Singleton
                    ? this.valueStore
                    : provider as IValueStore;

            return store.GetValue(this, provider, this.instanceAccessor);
        }

        public IServiceCallSite GetCallSite(ServiceProvider provider, ISet<Service> serviceChain)
        {
            if (this.callSite == null)
            {
                Interlocked.CompareExchange(ref this.callSite, this.GetCallSiteInternal(provider, serviceChain), null);                
            }

            return this.callSite;
        } 

        private IServiceCallSite GetCallSiteInternal(ServiceProvider provider, ISet<Service> serviceChain)
        {
            if (this.Descriptor.ImplementationFactory != null)
            {
                throw new NotImplementedException();
            }
            else if (this.Descriptor.ImplementationType != null)
            {
                return new ConstructorCallSiteFactory(this.Descriptor)
                    .CreateServiceCallSite(provider, serviceChain);
            }
            else
            {
                return new InstanceCallSite(this.Descriptor);
            }

            throw new NotImplementedException();
        }

        private Func<ServiceProvider, object> CreateServiceAccessor(ServiceProvider provider)
        {
            var callSite = this.GetCallSite(provider, new HashSet<Service>());
            if (callSite == null) return Cache.Funcs.ObjectFunc.ReturnDefault<ServiceProvider, object>();
            if (callSite is IImmutableCallSite) return callSite.ResolveValue;
            return RealizeServiceAccessor(provider.RootProvider, this, callSite);
        }

        private static Func<ServiceProvider, object> RealizeServiceAccessor(RootServiceProvider serviceProvider,
            Service service, IServiceCallSite callSite)
        {
            Debug.Assert(serviceProvider.Setting.CompileAfterCallCount != null);
            var compileAfter = serviceProvider.Setting.CompileAfterCallCount.Value;

            var callCount = 0;
            return provider =>
            {
                if (Interlocked.Increment(ref callCount) == compileAfter)
                {
                    Task.Run(() =>
                    {
                        serviceProvider.Log($"begin compile {service}");
                        var func = new CallSiteExpressionBuilder().Build(callSite);
                        Interlocked.Exchange(ref service.instanceAccessor, func);
                    });
                }
                return callSite.ResolveValue(provider);
            };
        }

        public override string ToString() => $"{this.ServiceType.Name} {this.ServiceName}";

        public void Dispose() => (this.valueStore)?.Dispose();
    }
}