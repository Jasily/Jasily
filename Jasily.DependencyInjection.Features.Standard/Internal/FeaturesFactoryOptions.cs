using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Features.Internal
{
    internal class FeaturesFactoryOptions<T> where T : class
    {
        private readonly Dictionary<Type, object> _factorys;

        public FeaturesFactoryOptions() : this(new Dictionary<Type, object>())
        {
        }

        private FeaturesFactoryOptions(Dictionary<Type, object> factorys)
        {
            this._factorys = factorys;
        }

        public void Register<TFeature>([NotNull] Func<FeatureBuildSource<T>, TFeature> factory, bool overwrite)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            if (overwrite)
            {
                this._factorys[typeof(TFeature)] = factory;
            }
            else
            {
                this._factorys.Add(typeof(TFeature), factory);
            }
        }

        public Func<FeatureBuildSource<T>, TFeature> TryGetFactory<TFeature>()
        {
            if (this._factorys.TryGetValue(typeof(TFeature), out var factory))
            {
                return (Func<FeatureBuildSource<T>, TFeature>) factory;
            }
            return null;
        }
    }
}