using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;

namespace Jasily.TypeMakers
{
    public class GenericTypeMakerBucket : IGenericTypeMakerBucket
    {
        private readonly ConcurrentDictionary<Type, IGenericTypeMaker> _table;
        private readonly Func<Type, IGenericTypeMaker> _factory;

        public GenericTypeMakerBucket()
        {
            this._table = new ConcurrentDictionary<Type, IGenericTypeMaker>();
            this._factory = type => new GenericTypeMaker(type);
        }

        public IGenericTypeMaker GetGenericTypeMaker(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return this._table.GetOrAdd(type, this._factory);
        }
    }
}