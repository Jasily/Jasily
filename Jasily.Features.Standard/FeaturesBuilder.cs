using System;
using System.Collections.Generic;
using System.Linq;
using Jasily.Features.DependencyInjection;
using Jasily.Features.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Jasily.Features
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FeaturesBuilder : IFeaturesBuilder
    {
        private readonly Dictionary<Type, object> _features = new Dictionary<Type, object>();

        private FeaturesOptions<T> GetOrCreateOptions<T>()
        {
            if (!this._features.TryGetValue(typeof(T), out var value))
            {
                this._features.Add(typeof(T), value = new FeaturesOptions<T>());
            }
            return (FeaturesOptions<T>) value;
        }

        /// <summary>
        /// Register a feature to the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="factory"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public FeaturesBuilder RegisterFeature<T, TFeature>(
            [NotNull] Func<FeatureBuildSource<T>, TFeature> factory, bool overwrite = true)
            where T : class
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            this.GetOrCreateOptions<T>().Register(factory, overwrite);
            return this;
        }

        IFeaturesBuilder IFeaturesBuilder.RegisterFeature<T, TFeature>(
            [NotNull] Func<FeatureBuildSource<T>, TFeature> factory, bool overwrite)
        {
            return this.RegisterFeature(factory, overwrite);
        }

        public FeaturesProvider Build()
        {
            return new FeaturesProvider(this._features);
        }
    }
}