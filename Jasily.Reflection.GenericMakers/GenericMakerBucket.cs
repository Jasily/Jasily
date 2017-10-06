using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Jasily.Reflection.GenericMakers
{
    public class GenericMakerBucket : IGenericMakerBucket
    {
        private readonly ConcurrentDictionary<Type, IGenericTypeMaker> _typeMakersTable;
        private readonly ConcurrentDictionary<MethodInfo, IGenericMethodMaker> _methodMakersTable;
        private readonly Func<Type, IGenericTypeMaker> _typeMakerFactory;
        private readonly Func<MethodInfo, IGenericMethodMaker> _methodMakerFactory;

        public GenericMakerBucket()
        {
            this._typeMakersTable = new ConcurrentDictionary<Type, IGenericTypeMaker>();
            this._methodMakersTable = new ConcurrentDictionary<MethodInfo, IGenericMethodMaker>();
            this._typeMakerFactory = t => new GenericTypeMaker(t);
            this._methodMakerFactory = m => new GenericMethodMaker(m);
        }

        public IGenericTypeMaker GetGenericTypeMaker(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return this._typeMakersTable.GetOrAdd(type, this._typeMakerFactory);
        }

        public IGenericMethodMaker GetGenericMethodMaker(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            return this._methodMakersTable.GetOrAdd(method, this._methodMakerFactory);
        }
    }
}