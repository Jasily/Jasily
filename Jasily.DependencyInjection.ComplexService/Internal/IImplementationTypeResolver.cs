using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexService.Internal
{
    internal interface IImplementationTypeResolver
    {
        Type ServiceType { get; }

        [CanBeNull]
        Type Resolve(Type closedServiceType);
    }
}