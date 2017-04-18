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

        private static ServiceProviderSettings CloneSettings(ServiceProviderSettings setting)
        {
            var s = setting;

            if (s.CompileAfterCallCount == null)
                s.CompileAfterCallCount = ServiceProviderSettings.DefaultCompileAfterCallCount;

            if (s.NameComparer == null)
                s.NameComparer = StringComparer.OrdinalIgnoreCase;

            s.ResolveMode = ((IEnumerable<ResolveLevel>) setting.ResolveMode ?? DefaultResolveMode)
                .ToList();

            return s;
        }

        public RootServiceProvider([NotNull] IEnumerable<NamedServiceDescriptor> serviceDescriptors,
            ServiceProviderSettings setting)
            : base(serviceDescriptors, CloneSettings(setting))
        {
            this.Settings = setting;
        }

        public ServiceProviderSettings Settings { get; }

        [Conditional("DEBUG")]
        public void Log(string message) => Debug.WriteLineIf(this.Settings.EnableDebug, message);
    }
}