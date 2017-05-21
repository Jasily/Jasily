using System;

namespace Jasily.DependencyInjection.Features
{
    public struct FeatureBuildSource<T>
    {
        internal FeatureBuildSource(T source, IServiceProvider serviceProvider)
        {
            this.Source = source;
            this.ServiceProvider = serviceProvider;
        }

        public T Source { get; }

        public IServiceProvider ServiceProvider { get; }
    }
}