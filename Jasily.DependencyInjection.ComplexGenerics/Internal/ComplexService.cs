using System;
using System.Collections.Generic;
using System.Linq;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal class ComplexService<T> : IComplexService<T>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IComplexServiceProvider _provider;

        public ComplexService(IServiceProvider serviceProvider, TypeInfoAccessorFactory accessorFactory)
        {
            this._serviceProvider = serviceProvider;
            var accessor = accessorFactory.GetAccessor(typeof(T));
            this._provider = (IComplexServiceProvider)serviceProvider.GetService(accessor.ComplexServiceProviderType);
        }

        public T Value() => (T) this._provider.Get(this._serviceProvider, typeof(T));

        public IEnumerable<T> Values() => this._provider.GetAll(this._serviceProvider, typeof(T)).Cast<T>();
    }
}