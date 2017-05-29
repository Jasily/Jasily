using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal interface IImplementationTypeResolver
    {
        Type ServiceType { get; }

        [CanBeNull]
        Type Resolve(Type closedServiceType);
    }
}