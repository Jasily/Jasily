using System;
using System.Linq;
using Jasily.DependencyInjection.ComplexGenerics.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.ComplexGenerics
{
    public class ComplexGenericsBuilder
    {
        [NotNull] private readonly IServiceCollection _collection;
        [NotNull] private readonly ComplexServiceResolverProvider _resolverProvider;

        internal ComplexGenericsBuilder(IServiceCollection collection)
        {
            this._collection = collection ?? throw new ArgumentNullException(nameof(collection));
            var resolverProvider = collection
                .Select(z => z.ImplementationInstance)
                .OfType<ComplexServiceResolverProvider>()
                .SingleOrDefault();
            if (collection.Select(z => z.ServiceType).SingleOrDefault(z => z == typeof(TypeInfoAccessorFactory)) == null)
            {
                // init.
                resolverProvider = new ComplexServiceResolverProvider();
                collection.AddSingleton(resolverProvider);
                collection.AddSingleton(typeof(TypeInfoAccessorFactory));
                collection.AddSingleton(typeof(ComplexTypeMapper));
                collection.AddSingleton(typeof(ComplexServiceProvider<>));
            }
            this._resolverProvider = resolverProvider;
        }

        private void CommonAdd([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            if (this._collection == null || this._resolverProvider == null) throw new InvalidOperationException();

            var resolver = new GenericsTypeResolver(null);
            this._resolverProvider.AddResolver(resolver);

            var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
            this._collection.AddSingleton(genericTypeDefinition, sp =>
            {
                var provider = sp.GetRequiredService<ComplexServiceResolverProvider>();
                throw new NotImplementedException();
                var closedImplType = provider.GetClosedImplTypeOrNull(null /* not null ! */);
                return closedImplType == null ? null : sp.GetService(closedImplType);
            });
        }

        public ComplexGenericsBuilder AddSingleton([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            this.CommonAdd(serviceType, implementationType);
            this._collection.AddSingleton(implementationType);
            return this;
        }

        public ComplexGenericsBuilder AddScoped([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            this.CommonAdd(serviceType, implementationType);
            this._collection.AddScoped(implementationType);
            return this;
        }

        public ComplexGenericsBuilder AddTransient([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            this.CommonAdd(serviceType, implementationType);
            this._collection.AddTransient(implementationType);
            return this;
        }
    }
}