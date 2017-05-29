using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal class GenericsTypeResolver : IImplementationTypeResolver
    {
        private readonly ConcurrentDictionary<Type, Type> _map;
        private readonly Lazy<GenericTypeStructureTree> _tree;

        public GenericsTypeResolver([NotNull] ComplexTypeSource serviceTypeSource)
        {
            if (serviceTypeSource == null) throw new ArgumentNullException(nameof(serviceTypeSource));

            this.ServiceType = serviceTypeSource.ServiceType.GetGenericTypeDefinition();
            this._tree = new Lazy<GenericTypeStructureTree>(() =>
                new GenericTypeStructureTree(serviceTypeSource.ServiceType, serviceTypeSource.ImplementationType));
            this._map = new ConcurrentDictionary<Type, Type>();
        }

        public Type ServiceType { get; }

        [CanBeNull]
        public Type Resolve(Type closedServiceType)
        {
            if (this._map.TryGetValue(closedServiceType, out var closedImplType)) return closedImplType;
            var r = this._tree.Value.TryMakeClosedImplementationType(closedServiceType, out closedImplType);
            Debug.Assert(r && closedImplType != null || !r && closedImplType == null);
            return this._map.GetOrAdd(closedServiceType, closedImplType);
        }
    }
}