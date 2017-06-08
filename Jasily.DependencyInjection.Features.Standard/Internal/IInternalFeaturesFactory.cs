using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Features.Internal
{
    internal interface IInternalFeaturesFactory<in T> where T : class
    {
        /// <summary>
        /// Create feature or default(<typeparamref name="TFeature"/>). This will not boxing value type.
        /// </summary>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        [CanBeNull]
        TFeature TryCreateFeature<TFeature>([NotNull] IServiceProvider serviceProvider, [NotNull] T source, bool inherit);

        /// <summary>
        /// Create feature or null.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="featureType"></param>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        [CanBeNull]
        object TryCreateFeature([NotNull] IServiceProvider serviceProvider, [NotNull] Type featureType, [NotNull] T source, bool inherit);

        /// <summary>
        /// Create all registered features for source.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        [NotNull, ItemNotNull]
        IEnumerable<object> CreateAllFeatures([NotNull] IServiceProvider serviceProvider, [NotNull] T source, bool inherit);
    }
}