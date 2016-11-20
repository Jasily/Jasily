using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    internal sealed class RootServiceProvider : ServiceProvider
    {
        public static readonly ResolveLevel[] DefaultResolveMode = new ResolveLevel[]
        {
            ResolveLevel.TypeAndName,
            ResolveLevel.Type,
            ResolveLevel.NameAndType,
        };

        internal ResolveLevel[] ResolveMode { get; }

        public RootServiceProvider(
            [NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors,
            [NotNull] IEnumerable<ResolveLevel> mode)
            : base(serviceDescriptors)
        {
            if (mode == null) throw new ArgumentNullException(nameof(mode));

            this.ResolveMode = mode.OrderBy(z => (int)z).ToArray();
        }
    }
}