using Jasily.DependencyInjection.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jasily.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceDescriptor AssignName([NotNull] this ServiceDescriptor descriptor, [CanBeNull] string name)
        {
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));
            if (descriptor is NamedServiceDescriptor) throw new InvalidOperationException("cannot assign name again.");

            return NamedServiceDescriptor.TryAssignName(descriptor, name);
        }
    }
}
