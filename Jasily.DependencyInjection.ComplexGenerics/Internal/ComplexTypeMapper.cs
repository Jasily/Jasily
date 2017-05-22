using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal class ComplexTypeMapper
    {
        private readonly Dictionary<Type, ComplexTypeSource[]> _types;

        public ComplexTypeMapper(IEnumerable<ComplexTypeSource> types)
        {
            this._types = types
                .GroupBy(z => z.ServiceType)
                .ToDictionary(z => z.Key, z => z.ToArray());
        }

        public IEnumerable<ComplexTypeSource> GetComplexTypes(Type type)
        {
            return this._types.TryGetValue(type, out var r) ? r : Enumerable.Empty<ComplexTypeSource>();
        }
    }
}