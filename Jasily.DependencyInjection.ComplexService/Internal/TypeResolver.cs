using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexService.Internal
{
    internal class TypeResolver : IImplementationTypeResolver
    {
        [NotNull] private readonly ComplexTypeSource _serviceTypeSource;

        public TypeResolver([NotNull] ComplexTypeSource serviceTypeSource)
        {
            this._serviceTypeSource = serviceTypeSource;
        }

        public Type ServiceType => this._serviceTypeSource.ServiceType;

        [CanBeNull]
        public Type Resolve(Type closedServiceType) => this._serviceTypeSource.ImplementationType;
    }
}