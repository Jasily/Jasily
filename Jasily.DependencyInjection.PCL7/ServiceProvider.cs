using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Jasily.DependencyInjection.Internal;
using Jasily.DependencyInjection.Internal.CallSites;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Threading;

namespace Jasily.DependencyInjection
{
    public class ServiceProvider : IServiceProvider, IValueStore
    {
        internal ServiceProvider([NotNull] IEnumerable<NamedServiceDescriptor> serviceDescriptors,
            ServiceProviderSettings setting)
        {
            if (serviceDescriptors == null) throw new ArgumentNullException(nameof(serviceDescriptors));

            this.RootProvider = (RootServiceProvider)this;
            this.Lifetime = ServiceLifetime.Singleton;
            this.serviceResolver = new ServiceResolver(this, setting, serviceDescriptors);
        }

        // This constructor is called exclusively to create a child scope from the parent
        internal ServiceProvider([NotNull] ServiceProvider parent, ServiceLifetime lifetime)
        {
            Debug.Assert(parent != null);
            Debug.Assert(lifetime == ServiceLifetime.Scoped || lifetime == ServiceLifetime.Transient);

            this.RootProvider = parent.RootProvider;
            this.ParentProvider = parent;
            this.Lifetime = lifetime;
        }

        [NotNull]
        internal RootServiceProvider RootProvider { get; }

        [CanBeNull]
        internal ServiceProvider ParentProvider { get; }

        [CanBeNull]
        private IServiceResolver serviceResolver;

        [NotNull]
        internal IServiceResolver ServiceResolver
        {
            get
            {
                if (this.serviceResolver == null)
                    Interlocked.CompareExchange(ref this.serviceResolver,
                        new ServiceResolver(this, this.RootProvider.Settings), null);
                return this.serviceResolver;
            }
        }

        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [CanBeNull]
        public object GetService([NotNull] Type serviceType) => this.GetService(serviceType, null).Value;

        public object GetRequiredService([NotNull] Type serviceType)
        {
            var r = this.GetService(serviceType, null);
            if (r.HasValue) throw new InvalidOperationException();
            return r.Value;
        }

        /// <summary>
        /// return a <see cref="ResolveResult"/> object.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ResolveResult GetService([NotNull] Type serviceType, [CanBeNull] string serviceName)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));

            return this.GetServiceInternal(new ResolveRequest(serviceType, serviceName));
        }

        private ResolveResult GetServiceInternal(ResolveRequest request)
        {
            var result = this.ServiceResolver.ResolveValue(this, request);
            if (!result.HasValue && this.ParentProvider != null)
            {
                return this.ParentProvider.GetServiceInternal(request);
            }
            return result;
        }

        private Service ResolveService(ResolveRequest request)
        {
            var mode = this.RootProvider.Settings.ResolveMode;
            for (var i = 0; i < mode.Count; i++)
            {
                var level = mode[i];
                var service = this.ServiceResolver.ResolveService(request, level);
                if (service != null) return service;
            }
            return null;
        }

        private IServiceCallSite ResolveCallSite(ResolveRequest request, ISet<Service> serviceChain)
        {
            var service = this.ResolveService(request);
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
        internal IServiceCallSite[] ResolveParametersCallSites([NotNull] ParameterInfo[] parameters, [NotNull] ISet<Service> serviceChain)
        {
            Debug.Assert(parameters != null);
            Debug.Assert(serviceChain != null);

            var callSites = new IServiceCallSite[parameters.Length];
            for (var index = 0; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                var callSite = this.ResolveCallSite(new ResolveRequest(parameter.ParameterType, parameter.Name), serviceChain);
                if (callSite == null && parameter.HasDefaultValue)
                {
                    callSite = new ConstantCallSite(parameter.DefaultValue);
                }
                if (callSite == null) return null;
                callSites[index] = callSite;
            }
            return callSites;
        }

        #region IValueStore for Transient or Scoped

        private Dictionary<ServiceBuilder, object> store1;
        private ConcurrentDictionary<ServiceBuilder, object> store2;

        object IValueStore.GetValue(ServiceBuilder builder, ServiceProvider provider, Func<ServiceProvider, object> creator)
        {
            if (this.Lifetime == ServiceLifetime.Transient)
            {
                // don't need thread-saftly
                if (this.store1 == null) this.store1 = new Dictionary<ServiceBuilder, object>();

                if (!this.store1.TryGetValue(builder, out var value))
                {
                    value = creator(provider);
                    this.store1.Add(builder, value);
                }
                return value;
            }
            else
            {
                Debug.Assert(this.Lifetime == ServiceLifetime.Scoped);

                if (this.store2 == null) Interlocked.CompareExchange(ref this.store2,
                    new ConcurrentDictionary<ServiceBuilder, object>(), null);
                if (this.store2.TryGetValue(builder, out var r)) return r;
                return this.store2.GetOrAdd(builder, s => creator(this));
            }
        }

        #endregion

        public ServiceProvider CreateScope() => new ServiceProvider(this, ServiceLifetime.Scoped);

        public ServiceProvider CreateTransient() => new ServiceProvider(this, ServiceLifetime.Transient);

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                this.ServiceResolver.Dispose();
                foreach (var kvp in this.store1)
                {
                    (kvp.Value as IDisposable)?.Dispose();
                }
                this.store1.Clear();

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                this.disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ServiceProvider() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}