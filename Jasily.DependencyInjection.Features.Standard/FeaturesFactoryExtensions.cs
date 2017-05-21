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

        /// <summary>
        /// create a required feature. or throw a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="featureType"></param>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        [NotNull]
        public static object CreateRequiredFeature<T>([NotNull] IFeaturesFactory<T> factory,
            [NotNull] Type featureType, [NotNull] T source, bool inherit)
            where T : class
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            return factory.TryCreateFeature(featureType, source, inherit) ?? throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        [NotNull]
        public static FeaturesContainer<T> CreateFeaturesContainer<T>([NotNull] IFeaturesFactory<T> factory, [NotNull] T source, bool inherit)
            where T : class
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var features = factory.CreateAllFeatures(source, inherit);
            return new FeaturesContainer<T>(source, features);
        }
    }
}