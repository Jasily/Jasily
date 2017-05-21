using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Features
{
    /// <summary>
    /// The features container interface.
    /// </summary>
    public interface IReadOnlyFeaturesContainer
    {
        /// <summary>
        /// 
        /// </summary>
        [NotNull, ItemNotNull]
        IReadOnlyList<object> Features { get; }
    }

    /// <summary>
    /// The features container interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyFeaturesContainer<out T> : IReadOnlyFeaturesContainer
    {
        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        T Source { get; }
    }
}