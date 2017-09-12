using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.Features.Internal
{
    internal class FactoryProvider : IFactoryProvider
    {
        private readonly Dictionary<Type, object> _options;
        private readonly ConcurrentDictionary<Type, object> _createdFactorys;
        private readonly Func<Type, object> _creator;
        private readonly MethodInfo _createHelper;

        public FactoryProvider(Dictionary<Type, object> featuresOptions)
        {
            this._options = featuresOptions.ToDictionary(z => z.Key, z => z.Value);
            this._createdFactorys = new ConcurrentDictionary<Type, object>();
            this._creator = this.Creator;
            this._createHelper = typeof(FactoryProvider).GetMethod(
                nameof(this.CreateHelper), 
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new NotSupportedException();
        }

        private object Creator(Type type)
        {
            this._options.TryGetValue(type, out var option);
            return this._createHelper.MakeGenericMethod(type).Invoke(this, new[] {option});
        }

        private TypedFeaturesFactory<T> CreateHelper<T>(FeaturesOptions<T> options)
            where T : class
        {
            return new TypedFeaturesFactory<T>(this, options);
        }

        public object GetFactory(Type type)
        {
            return this._createdFactorys.GetOrAdd(type, this._creator);
        }
    }
}