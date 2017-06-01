using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexService.Internal
{
    internal class ClosedServiceTypes
    {
        private readonly Type _closedServiceType;
        private readonly IImplementationTypeResolver[] _resolvers;
        private ResolveResult _singleResult;
        private Type[] _results;

        public ClosedServiceTypes([NotNull] ComplexServiceResolverProvider provider, Type closedServiceType)
        {
            this._closedServiceType = closedServiceType;
            this._resolvers = provider.GetResolvers(closedServiceType).Reverse().ToArray();
        }

        [CanBeNull]
        public Type ResolvedType
        {
            get
            {
                if (this._singleResult == null)
                {
                    var source = this._results ?? this._resolvers.Select(z => z.Resolve(this._closedServiceType));
                    var r = new ResolveResult(source.FirstOrDefault(z => z != null));
                    if (this._singleResult == null) Interlocked.CompareExchange(ref this._singleResult, r, null);
                }

                return this._singleResult.Type;
            }
        }

        [NotNull, ItemNotNull]
        public IEnumerable<Type> ResolvedTypes
        {
            get
            {
                if (this._results == null)
                {
                    if (this._singleResult != null && this._singleResult.Type == null)
                    {
                        this._results = new Type[0];
                    }

                    var source = this._resolvers.Select(z => z.Resolve(this._closedServiceType))
                        .Where(z => z != null)
                        .ToArray();
                    if (this._results == null) Interlocked.CompareExchange(ref this._results, source, null);
                }

                return this._results;
            }
        }

        private class ResolveResult
        {
            public ResolveResult(Type type)
            {
                this.Type = type;
            }

            public Type Type { get; }
        }
    }
}