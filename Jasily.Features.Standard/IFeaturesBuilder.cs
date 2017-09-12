using System;
using JetBrains.Annotations;

namespace Jasily.Features
{
    public interface IFeaturesBuilder
    {
        /// <summary>
        /// Register a feature to the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="factory"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        IFeaturesBuilder RegisterFeature<T, TFeature>([NotNull] Func<FeatureBuildSource<T>, TFeature> factory,
            bool overwrite = true)
            where T : class;
    }
}