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
    internal class Service
    {
        private TypeInfo serviceTypeInfo;
        private IServiceCallSite callSite;
        

        public Service(IServiceResolver resolver, NamedServiceDescriptor descriptor)
        {
            this.Descriptor = descriptor;
        }

        /// <summary>
        /// The resolver where this service defined.
        /// </summary>
        public IServiceResolver Resolver { get; }

        public NamedServiceDescriptor Descriptor { get; }

        [NotNull]
        public string ServiceName => this.Descriptor.ServiceName;

        [NotNull]
        public Type ServiceType => this.Descriptor.ServiceType;

        [NotNull]
        public TypeInfo ServiceTypeInfo => this.serviceTypeInfo ?? (this.serviceTypeInfo = this.ServiceType.GetTypeInfo());

        #region builder

        private ServiceBuilder builder;

        public ServiceBuilder GetServiceBuilder()
        {
            if (this.builder != null) return this.builder;
            if (this.callSite == null) throw new InvalidOperationException();
            var b = new ServiceBuilder(this.Resolver, this.callSite, this);
            Interlocked.CompareExchange(ref this.builder, b, null);
            return this.builder;
        }

        #endregion

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
            switch (this.Descriptor.Mode)
            {
                case NamedServiceDescriptor.ImplementationMode.Typed:
                    return new ConstructorCallSiteFactory(this.Descriptor)
                        .CreateServiceCallSite(provider, serviceChain);
                case NamedServiceDescriptor.ImplementationMode.Instance:
                    return new InstanceCallSite(this.Descriptor);
                case NamedServiceDescriptor.ImplementationMode.Factory:
                    return new FactoryCallSite(this.Descriptor);
                default:
                    throw new NotImplementedException();
            }
        }

        public override string ToString() => $"{this.ServiceType.Name} {this.ServiceName}";
    }

    internal sealed class ServiceBuilder : IDisposable
    {
        private readonly IValueStore valueStore;
        private Func<ServiceProvider, object> instanceAccessor;

        public ServiceBuilder([NotNull] IServiceResolver resolver, [NotNull] IServiceCallSite callsite, [NotNull] Service service)
        {
            Debug.Assert(resolver != null);
            Debug.Assert(callsite != null);
            Debug.Assert(service != null);

            this.Resolver = resolver;
            this.Lifetime = service.Descriptor.Lifetime;
            this.CallSite = callsite;

            if (this.Lifetime == ServiceLifetime.Singleton)
                this.valueStore = new ValueStore();
        }

        /// <summary>
        /// The ServiceBuilder's owner.
        /// </summary>
        public IServiceResolver Resolver { get; }

        public ServiceLifetime Lifetime { get; }
        
        public IServiceCallSite CallSite { get; }

        public object GetValue(ServiceProvider provider)
        {
            if (this.instanceAccessor == null)
            {
                Interlocked.CompareExchange(ref this.instanceAccessor, this.CreateServiceAccessor(), null);
            }

            IValueStore store;
            switch (this.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    store = this.valueStore;
                    break;

                case ServiceLifetime.Scoped:
                case ServiceLifetime.Transient:
                    store = provider;
                    break;

                default:
                    throw new NotImplementedException();
            }
            return store.GetValue(this, provider, this.instanceAccessor);
        }

        private static Func<ServiceProvider, object> ReturnDefault = _ => null;

        private Func<ServiceProvider, object> CreateServiceAccessor()
        {
            var callSite = this.CallSite;
            if (callSite == null) return ReturnDefault;
            if (callSite is IImmutableCallSite) return callSite.ResolveValue;

            // RealizeServiceAccessor
            var settings = this.Resolver.ServiceProvider.RootProvider.Settings;
            Debug.Assert(settings.CompileAfterCallCount.HasValue);
            var compileAfter = settings.CompileAfterCallCount.Value;
            var db = settings.EnableDebug;

            var callCount = 0;
            return provider =>
            {
                if (Interlocked.Increment(ref callCount) == compileAfter)
                {
                    Task.Run(() =>
                    {
                        Debug.WriteLineIf(db, $"begin compile {this}");
                        var func = new CallSiteExpressionBuilder().Build(callSite);
                        Interlocked.Exchange(ref this.instanceAccessor, func);
                    });
                }
                return callSite.ResolveValue(provider);
            };
        }

        public void Dispose() => (this.valueStore)?.Dispose();
    }
}