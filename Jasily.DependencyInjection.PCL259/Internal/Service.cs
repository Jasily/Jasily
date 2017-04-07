using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Jasily.DependencyInjection.Internal.CallSites;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal class Service : IDisposable
    {
        private readonly ValueStore valueStore = new ValueStore();
        private Func<ServiceProvider, object> instanceAccessor;
        private TypeInfo serviceTypeInfo;

        public Service(IServiceDescriptor descriptor)
        {
            this.Descriptor = descriptor;
        }

        public IServiceDescriptor Descriptor { get; }

        [NotNull]
        public string ServiceName => this.Descriptor.ServiceName ?? string.Empty;

        [NotNull]
        public Type ServiceType => this.Descriptor.ServiceType;

        [NotNull]
        public TypeInfo ServiceTypeInfo
            => this.serviceTypeInfo ?? (this.serviceTypeInfo = this.ServiceType.GetTypeInfo());

        public object GetValue(ServiceProvider provider)
        {
            if (this.instanceAccessor == null)
            {
                Interlocked.CompareExchange(ref this.instanceAccessor, this.CreateServiceAccessor(provider), null);
            }

            if (this.Descriptor.Lifetime == ServiceLifetime.Transient)
            {
                return this.instanceAccessor(provider);
            }

            var store = this.Descriptor.Lifetime == ServiceLifetime.Singleton
                    ? (this.Descriptor as IValueStore ?? this.valueStore)
                    : provider;

            return store.GetValue(this, provider, this.instanceAccessor);
        }

        public IServiceCallSite GetCallSite(ServiceProvider provider, ISet<Service> serviceChain)
        {
            if (this.Descriptor is IServiceCallSite cs)
                return cs;
            if (this.Descriptor is IServiceCallSiteProvider csp)
                return csp.CreateServiceCallSite(provider, serviceChain) ?? throw new NotImplementedException();
            throw new NotImplementedException();
        }

        private Func<ServiceProvider, object> CreateServiceAccessor(ServiceProvider serviceProvider)
        {
            var callSite = this.GetCallSite(serviceProvider, new HashSet<Service>());
            if (callSite == null) return Cache.Funcs.ObjectFunc.ReturnDefault<ServiceProvider, object>();
            if (callSite is IImmutableCallSite) return callSite.ResolveValue;
            return RealizeServiceAccessor(serviceProvider.RootProvider, this, callSite);
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

        public void Dispose() => (this.Descriptor as IDisposable)?.Dispose();
    }
}