using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Jasily.DependencyInjection.Attributes;
using Jasily.DependencyInjection.Internal.CallSites;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal class TypedServiceDescriptor : ServiceDescriptor, IServiceCallSiteProvider, IValueStore
    {
        private readonly TypeDescriptor implementationTypeDescriptor;
        private readonly Type implementationType;

        public TypedServiceDescriptor([NotNull] Type serviceType, [CanBeNull] string serviceName, ServiceLifetime lifetime,
            [NotNull] TypeDescriptor implementationTypeDescriptor)
            : base(serviceType, serviceName, lifetime)
        {
            this.implementationTypeDescriptor = implementationTypeDescriptor;
            if (implementationTypeDescriptor == null) throw new ArgumentNullException(nameof(implementationTypeDescriptor));
            this.implementationType = implementationTypeDescriptor.ImplementationType;
        }

        private IServiceCallSite ResolveConstructorCallSite(ServiceProvider provider, ISet<Service> serviceChain,
            ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            if (parameters.Length == 0) return new CreateInstanceCallSite(this.implementationType);

            var parameterCallSites = provider.ResolveParametersCallSites(parameters, serviceChain);
            return parameterCallSites == null ? null : new ConstructorCallSite(constructor, parameterCallSites);
        }

        public IServiceCallSite CreateServiceCallSite(ServiceProvider provider, ISet<Service> serviceChain)
        {
            var constructors = this.implementationType.GetTypeInfo()
                .DeclaredConstructors
                .Where(constructor => constructor.IsPublic)
                .ToArray();

            if (constructors.Length == 0)
                throw new InvalidOperationException($"type [{this.implementationType}] has no constructor.");

            if (constructors.Length == 1)
            {
                return this.ResolveConstructorCallSite(provider, serviceChain, constructors[0])
                    ?? throw new InvalidOperationException(
                        $"cannot resolve parameters for constructor of type [{this.implementationType}]");
            }

            var entryPoints = constructors
                .Where(z => z.GetCustomAttribute<EntryPointAttribute>() != null)
                .ToArray();
            if (entryPoints.Length > 1)
            {
                throw new InvalidOperationException(
                    $"type [{this.implementationType}] has too many entry point.");
            }
            else if (entryPoints.Length == 1)
            {
                return this.ResolveConstructorCallSite(provider, serviceChain, constructors[0])
                    ?? throw new InvalidOperationException(
                        $"cannot resolve parameters for entry point constructor of type [{this.implementationType}]");
            }

            return this.ResolveBestCallSite(provider, serviceChain, constructors);
        }

        private IServiceCallSite ResolveBestCallSite(ServiceProvider provider, ISet<Service> serviceChain,
            ConstructorInfo[] constructors)
        {
            Array.Sort(constructors, (a, b) => b.GetParameters().Length.CompareTo(a.GetParameters().Length));

            ConstructorInfo bestConstructor = null;
            HashSet<Type> bestConstructorParameterTypes = null;
            IServiceCallSite[] parameterCallSites = null;
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                var currentParameterCallSites = provider.ResolveParametersCallSites(parameters, serviceChain);

                if (currentParameterCallSites != null)
                {
                    if (bestConstructor == null)
                    {
                        bestConstructor = constructor;
                        parameterCallSites = currentParameterCallSites;
                    }
                    else
                    {
                        // Since we're visiting constructors in decreasing order of number of parameters,
                        // we'll only see ambiguities or supersets once we've seen a 'bestConstructor'.

                        if (bestConstructorParameterTypes == null)
                        {
                            bestConstructorParameterTypes = new HashSet<Type>(
                                bestConstructor.GetParameters().Select(p => p.ParameterType));
                        }

                        if (!bestConstructorParameterTypes.IsSupersetOf(parameters.Select(p => p.ParameterType)))
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
            }

            if (bestConstructor == null)
            {
                throw new InvalidOperationException($"cannot resolve any constructor from type [{this.implementationType}]");
            }
            else
            {
                Debug.Assert(parameterCallSites != null);
                return parameterCallSites.Length == 0
                    ? (IServiceCallSite)new CreateInstanceCallSite(this.implementationType)
                    : new ConstructorCallSite(bestConstructor, parameterCallSites);
            }
        }

        public void Dispose() => this.implementationTypeDescriptor.Dispose();

        public object GetValue(Service service, ServiceProvider provider, Func<ServiceProvider, object> creator)
            => this.implementationTypeDescriptor.GetValue(service, provider, creator);
    }
}