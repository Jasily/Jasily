using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal class ComplexTypeSource
    {
        public ComplexTypeSource([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            this.ServiceType = serviceType;
            this.ImplementationType = implementationType;
        }

        public Type ServiceType { get; }

        public Type ImplementationType { get; }
    }
}