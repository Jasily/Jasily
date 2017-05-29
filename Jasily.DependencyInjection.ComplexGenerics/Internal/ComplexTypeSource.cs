using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal class ComplexTypeSource
    {
        public ComplexTypeSource([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            this.ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            this.ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        public Type ServiceType { get; }

        public Type ImplementationType { get; }
    }
}