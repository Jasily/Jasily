using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Internal
{
    internal abstract class ServiceEntry
    {
        protected static readonly StringComparer StringComparer = StringComparer.OrdinalIgnoreCase;
        protected readonly object SyncRoot = new object();

        public abstract void Add(Service service);

        public abstract Service Resolve([NotNull] ResolveRequest resolveRequest, ResolveLevel level);
    }
}