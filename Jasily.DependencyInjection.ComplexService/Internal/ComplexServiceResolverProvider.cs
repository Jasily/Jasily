using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.DependencyInjection.ComplexService.Internal
{
    internal class ComplexServiceResolverProvider
    {
        private readonly Dictionary<Type, List<IImplementationTypeResolver>> _resolverMap;
        private readonly ConcurrentDictionary<Type, Type> _typeMap = new ConcurrentDictionary<Type, Type>();

        public ComplexServiceResolverProvider(IEnumerable<ComplexTypeSource> sources)
        {
            this._resolverMap = sources.Select(CreateTypeResolver).GroupBy(z => z.ServiceType).ToDictionary(z => z.Key, z => z.ToList());
        }

        private static IImplementationTypeResolver CreateTypeResolver(ComplexTypeSource source)
        {
            if (source.ServiceType.GetTypeInfo().IsGenericType)
            {
                return new GenericsTypeResolver(source);
            }
            else
            {
                return new TypeResolver(source);
            }
        }

        public IEnumerable<IImplementationTypeResolver> GetResolvers(Type type)
        {
            if (!this._typeMap.TryGetValue(type, out var t))
            {
                var ti = type.GetTypeInfo();
                t = ti.IsGenericType ? ti.GetGenericTypeDefinition() : type;
                this._typeMap[type] = t;
            }

            return this._resolverMap.TryGetValue(t, out var r) ? r : Enumerable.Empty<IImplementationTypeResolver>();
        }

    }
}