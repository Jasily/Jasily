using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Features
{
    /// <summary>
    /// extensions for <see cref="IFeaturesFactory{T}"/>.
    /// </summary>
    public static class FeaturesFactoryExtensions
    {
        /// <summary>
        /// create a required feature. or throw a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="factory"></param>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        [NotNull]
        public static TFeature CreateRequiredFeature<T, TFeature>([NotNull] IFeaturesFactory<T> factory,
            [NotNull] T source, bool inherit)
            where T : class
            where TFeature : class
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            return factory.TryCreateFeature<TFeature>(source, inherit) ?? throw new NotSupportedException();
        }
    }
}