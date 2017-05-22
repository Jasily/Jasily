using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal class TypeInfoAccessorFactory
    {
        private readonly ConcurrentDictionary<Type, TypeInfoAccessor> _accessors;
        private readonly Func<Type, TypeInfoAccessor> _accessorFactory;

        public TypeInfoAccessorFactory()
        {
            this._accessors = new ConcurrentDictionary<Type, TypeInfoAccessor>();
            this._accessorFactory = t => new TypeInfoAccessor(this, t);
        }

        public TypeInfoAccessor GetAccessor([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return this._accessors.GetOrAdd(type, this._accessorFactory);
        }
    }
}