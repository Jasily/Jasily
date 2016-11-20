using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal class TypedServiceDescriptor : ServiceDescriptor, IServiceCallSiteProvider
    {
        private readonly Type implementationType;

        public TypedServiceDescriptor([NotNull] Type serviceType, [CanBeNull] string serviceName, ServiceLifetime lifetime,
            [NotNull] Type implementationType)
            : base(serviceType, serviceName, lifetime)
        {
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            this.implementationType = implementationType;
        }

        public IServiceCallSite CreateServiceCallSite(ServiceProvider provider, ISet<IServiceDescriptor> serviceChain)
        {
            var constructors = this.implementationType.GetTypeInfo()
                .DeclaredConstructors
                .Where(constructor => constructor.IsPublic)
                .ToArray();

            if (constructors.Length == 0) throw new InvalidOperationException();

            IServiceCallSite[] parameterCallSites = null;

            if (constructors.Length == 1)
            {
                var constructor = constructors[0];
                var parameters = constructor.GetParameters();
                if (parameters.Length == 0)
                {
                    return new CreateInstanceCallSite(this.implementationType);
                }

                parameterCallSites = this.TryResolveParametersCallSites(
                    provider,
                    serviceChain,
                    parameters);

                if (parameterCallSites == null) throw new InvalidOperationException();

                return new ConstructorCallSite(constructor, parameterCallSites);
            }

            Array.Sort(constructors,
                (a, b) => b.GetParameters().Length.CompareTo(a.GetParameters().Length));

            ConstructorInfo bestConstructor = null;
            HashSet<Type> bestConstructorParameterTypes = null;
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                var currentParameterCallSites = this.TryResolveParametersCallSites(
                    provider,
                    serviceChain,
                    parameters);

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
                throw new InvalidOperationException();
            }
            else
            {
                Debug.Assert(parameterCallSites != null);
                return parameterCallSites.Length == 0
                    ? (IServiceCallSite) new CreateInstanceCallSite(this.implementationType)
                    : new ConstructorCallSite(bestConstructor, parameterCallSites);
            }
        }

        private IServiceCallSite[] TryResolveParametersCallSites(ServiceProvider provider, ISet<IServiceDescriptor> serviceChain,
            ParameterInfo[] parameters)
        {
            var parameterCallSites = new IServiceCallSite[parameters.Length];
            for (var index = 0; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                var callSite = provider.ResolveServiceCallSite(parameter.ParameterType, parameter.Name, serviceChain);
                if (callSite == null && parameter.HasDefaultValue)
                {
                    callSite = new ConstantCallSite(parameter.DefaultValue);
                }
                if (callSite == null) return null;
                parameterCallSites[index] = callSite;
            }
            return parameterCallSites;
        }
    }
}