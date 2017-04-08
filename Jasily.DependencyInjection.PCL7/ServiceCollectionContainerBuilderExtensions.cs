using Jasily.DependencyInjection.Internal;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jasily.DependencyInjection
{
    public static class ServiceCollectionContainerBuilderExtensions
    {
        public static ServiceProvider BuildServiceProvider([NotNull] this IServiceCollection services,
            ServiceProviderSettings settings = default(ServiceProviderSettings))
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var nsd = services.Select(z => NamedServiceDescriptor.TryAssignName(z, null)).ToArray();
            Debug.Assert(services.Count == nsd.Length);
            return new RootServiceProvider(nsd, RootServiceProvider.DefaultResolveMode, settings);
        }
    }
}
