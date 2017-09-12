using JetBrains.Annotations;
using System;

namespace Jasily.Features
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct FeatureBuildSource<T>
    {
        internal FeatureBuildSource(T source, IServiceProvider serviceProvider)
        {
            this.Source = source;
            this.ServiceProvider = serviceProvider;
        }

        public T Source { get; }

        /// <summary>
        /// If feature module does not work on IoC, this will be <see langword="null"/>.
        /// </summary>
        [CanBeNull]
        public IServiceProvider ServiceProvider { get; }
    }
}