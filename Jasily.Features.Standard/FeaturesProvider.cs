using System;
using System.Collections.Generic;
using Jasily.Features.Internal;

namespace Jasily.Features
{
    public class FeaturesProvider
    {
        private readonly FactoryProvider _factoryProvider;

        public FeaturesProvider(Dictionary<Type, object> features)
        {
            this._factoryProvider = new FactoryProvider(features);
        }

        public IFeaturesFactory<T> GetFeaturesFactory<T>() where T : class
        {
            return new FeaturesFactory<T>(null, (TypedFeaturesFactory<T>) this._factoryProvider.GetFactory(typeof(T)));
        }
    }
}