using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal class ComplexServiceProvider<T> : IComplexServiceProvider
    {
        private readonly IImplementationTypeResolver[] _resolver;

        public ComplexServiceProvider([NotNull] ComplexTypeMapper mapper)
        {
            this._resolver = CreateResolvers(mapper.GetComplexTypes(typeof(T))).Reverse().ToArray();
        }

        public object Get(IServiceProvider serviceProvider, Type closedServiceType)
        {
            return this._resolver.Select(z => z.Resolve(closedServiceType))
                .Where(z => z != null)
                .Select(serviceProvider.GetService)
                .FirstOrDefault();
        }

        public IEnumerable<object> GetAll(IServiceProvider serviceProvider, Type closedServiceType)
        {
            return this._resolver.Select(z => z.Resolve(closedServiceType))
                .Where(z => z != null)
                .Select(serviceProvider.GetService)
                .Reverse()
                .ToList();
        }

        private static IEnumerable<IImplementationTypeResolver> CreateResolvers(IEnumerable<ComplexTypeSource> types)
        {
            foreach (var complexType in types)
            {
                if (complexType.ServiceType.GetTypeInfo().IsGenericType)
                {
                    yield return new TypeResolver(complexType);
                }
                else
                {
                    yield return new GenericsTypeResolver(complexType);
                }
            }
        }
    }
}