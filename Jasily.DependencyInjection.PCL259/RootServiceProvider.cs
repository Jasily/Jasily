using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Jasily.DependencyInjection.Internal;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection
{
    internal sealed class RootServiceProvider : ServiceProvider
    {
        public static readonly ResolveLevel[] DefaultResolveMode = {
            ResolveLevel.TypeAndName,
            ResolveLevel.Type,
            ResolveLevel.NameAndType,
        };

        public RootServiceProvider([NotNull] IEnumerable<IServiceDescriptor> serviceDescriptors,
            [NotNull] IEnumerable<ResolveLevel> mode,
            ServiceProviderSettings setting)
        {
            if (serviceDescriptors == null) throw new ArgumentNullException(nameof(serviceDescriptors));
            if (mode == null) throw new ArgumentNullException(nameof(mode));

            if (setting.CompileAfterCallCount == null) setting.CompileAfterCallCount = 2;
            this.Setting = setting;

            this.ResolveMode = mode.OrderBy(z => (int)z).ToArray();
            this.ServiceResolver = new ServiceResolver(serviceDescriptors);
        }

        internal ResolveLevel[] ResolveMode { get; }

        public ServiceProviderSettings Setting { get; }

        [Conditional("DEBUG")]
        public void Log(string message) => Debug.WriteLineIf(this.Setting.EnableDebug, message);

        internal ServiceResolver ServiceResolver { get; }

        public override void Dispose()
        {
            base.Dispose();
            this.ServiceResolver.Dispose();
        }
    }


}