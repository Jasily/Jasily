using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.Features.Internal
{
    internal class FeaturesFactoryOptions<T> where T : class
    {
        private readonly Dictionary<Type, object> _genericFactorys = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Func<FeatureBuildSource<T>, object>> _objectsFactorys
            = new Dictionary<Type, Func<FeatureBuildSource<T>, object>>();

        public void Register<TFeature>([NotNull] Func<FeatureBuildSource<T>, TFeature> factory, bool overwrite)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (overwrite)
            {
                this._genericFactorys[typeof(TFeature)] = factory;
                this._objectsFactorys[typeof(TFeature)] = ToObjectFactory(factory);
            }
            else
            {
                this._genericFactorys.Add(typeof(TFeature), factory);
                this._objectsFactorys.Add(typeof(TFeature), ToObjectFactory(factory));
            }
        }

        private static Func<FeatureBuildSource<T>, object> ToObjectFactory<TFeature>(
            [NotNull] Func<FeatureBuildSource<T>, TFeature> factory)
        {
            return z => factory(z);
        }

        public Func<FeatureBuildSource<T>, TFeature> TryGetFactory<TFeature>()
        {
            if (this._genericFactorys.TryGetValue(typeof(TFeature), out var factory))
            {
                return (Func<FeatureBuildSource<T>, TFeature>) factory;
            }
            return null;
        }

        public Func<FeatureBuildSource<T>, object> TryGetFactory([NotNull] Type type)
        {
            return this._objectsFactorys.TryGetValue(type, out var factory) ? factory : null;
        }

        public IEnumerable<Func<FeatureBuildSource<T>, object>> EnumerateFactorys()
        {
            return this._objectsFactorys.Values;
        }
    }
}