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

        public RootServiceProvider([NotNull] IEnumerable<NamedServiceDescriptor> serviceDescriptors,
            [NotNull] IEnumerable<ResolveLevel> mode,
            ServiceProviderSettings setting)
            : base(serviceDescriptors)
        {
            if (mode == null) throw new ArgumentNullException(nameof(mode));

            if (setting.CompileAfterCallCount == null)
                setting.CompileAfterCallCount = ServiceProviderSettings.DefaultCompileAfterCallCount;

            if (setting.NameComparer == null)
                setting.NameComparer = StringComparer.OrdinalIgnoreCase;

            this.Settings = setting;

            this.ResolveMode = mode.OrderBy(z => (int)z).ToArray();
        }

        internal ResolveLevel[] ResolveMode { get; }

        public ServiceProviderSettings Settings { get; }

        [Conditional("DEBUG")]
        public void Log(string message) => Debug.WriteLineIf(this.Settings.EnableDebug, message);
    }
}