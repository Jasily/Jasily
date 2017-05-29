using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal class ComplexServiceFactory : IComplexServiceFactory
    {
        [NotNull] private readonly ComplexServiceResolverProvider _provider;
        private readonly ConcurrentDictionary<Type, ClosedServiceTypes> _map = new ConcurrentDictionary<Type, ClosedServiceTypes>();

        public ComplexServiceFactory([NotNull] ComplexServiceResolverProvider provider)
        {
            this._provider = provider;
        }

        private ClosedServiceTypes InternalGetClosedServiceTypes(Type type)
        {
            if (this._map.TryGetValue(type, out var r)) return r;
            r = new ClosedServiceTypes(this._provider, type);
            return this._map.GetOrAdd(type, r);
        }

        public Type GetClosedServiceTypeOrNull(Type type) => this.InternalGetClosedServiceTypes(type).ResolvedType;

        public IEnumerable<Type> GetClosedServiceTypes(Type type) => this.InternalGetClosedServiceTypes(type).ResolvedTypes;

        
    }
}