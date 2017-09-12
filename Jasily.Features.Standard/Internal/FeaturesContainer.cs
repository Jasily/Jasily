using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Jasily.Features.Internal
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class FeaturesContainer<T> : IReadOnlyFeaturesContainer<T>
    {
        private readonly IReadOnlyList<object> _readonlyFeatures;

        public FeaturesContainer([NotNull] T source, IEnumerable<object> features)
        {
            Debug.Assert(source != null);
            this.Source = source;
            this.Features = new List<object>(features);
            this._readonlyFeatures = new ReadOnlyCollection<object>(this.Features);
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public T Source { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<object> Features { get; }

        IReadOnlyList<object> IReadOnlyFeaturesContainer.Features => this._readonlyFeatures;
    }
}