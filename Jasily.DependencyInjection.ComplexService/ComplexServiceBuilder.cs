using System;
using System.Linq;
using Jasily.DependencyInjection.ComplexService.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.ComplexService
{
    public class ComplexServiceBuilder
    {
        [NotNull] private readonly IServiceCollection _collection;

        internal ComplexServiceBuilder(IServiceCollection collection)
        {
            this._collection = collection ?? throw new ArgumentNullException(nameof(collection));
            if (collection.SingleOrDefault(z => z.ServiceType == typeof(ComplexServiceResolverProvider)) == null)
            {
                // init.
                collection.AddSingleton<ComplexServiceResolverProvider>();
                collection.AddSingleton<IComplexServiceFactory, ComplexServiceFactory>();
            }
        }

        private void CommonAdd([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));

            this._collection.AddSingleton(new ComplexTypeSource(serviceType, implementationType));
        }

        public ComplexServiceBuilder AddSingleton([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            this.CommonAdd(serviceType, implementationType);
            this._collection.AddSingleton(implementationType);
            return this;
        }

        public ComplexServiceBuilder AddScoped([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            this.CommonAdd(serviceType, implementationType);
            this._collection.AddScoped(implementationType);
            return this;
        }

        public ComplexServiceBuilder AddTransient([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            this.CommonAdd(serviceType, implementationType);
            this._collection.AddTransient(implementationType);
            return this;
        }
    }
}