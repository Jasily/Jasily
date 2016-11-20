using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    public interface IServiceDescriptor
    {
        [CanBeNull]
        string ServiceName { get; }

        [NotNull]
        Type ServiceType { get; }

        ServiceLifetime Lifetime { get; }
    }
}