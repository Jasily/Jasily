using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Features
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFeaturesFactory<in T> where T : class
    {
        /// <summary>
        /// Create feature or default(<typeparamref name="TFeature"/>). This will not boxing value type.
        /// </summary>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="source"/> is <see langword="null"/>.</exception>
        [CanBeNull]
        TFeature TryCreateFeature<TFeature>([NotNull] T source, bool inherit);

        /// <summary>
        /// Create feature or null.
        /// </summary>
        /// <param name="featureType"></param>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="featureType"/> or <paramref name="source"/> is <see langword="null"/>.</exception>
        [CanBeNull]
        object TryCreateFeature([NotNull] Type featureType, [NotNull] T source, bool inherit);

        /// <summary>
        /// Create all registered features for source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="source"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        IEnumerable<object> CreateAllFeatures([NotNull] T source, bool inherit);
    }
}