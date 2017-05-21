using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Features
{
    /// <summary>
    /// Provide the readonly interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyFeaturesContainer<out T>
    {
        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        T Source { get; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull, ItemNotNull]
        IReadOnlyList<object> Features { get; }
    }
}