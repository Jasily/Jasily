using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Features
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFeaturesFactory<in T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="source"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        [CanBeNull]
        TFeature TryCreateFeature<TFeature>([NotNull] T source, bool inherit);
    }
}