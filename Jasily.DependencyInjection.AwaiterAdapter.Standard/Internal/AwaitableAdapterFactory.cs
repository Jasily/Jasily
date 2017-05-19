using System;
using System.Collections.Concurrent;
using Jasily.Extensions.System.Collections.Concurrent;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal class AwaitableAdapterFactory : IAwaitableAdapterFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, IAwaitableAdapter> _adapters = new ConcurrentDictionary<Type, IAwaitableAdapter>();
        private readonly Func<IAwaitableAdapter, bool> _isLockerFunc = o => o is Locker;
        private readonly Func<IAwaitableAdapter> _lockerFactory = () => new Locker();
        private readonly Func<Type, IAwaitableAdapter> _valueFactory;

        public AwaitableAdapterFactory([NotNull] IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this._valueFactory = k => AwaitableAdapter.GetAwaitableAdapter(serviceProvider, k);
        }

        public IAwaitableAdapter GetAwaitableAdapter([NotNull] Type instanceType)
        {
            if (instanceType == null) throw new ArgumentNullException(nameof(instanceType));

            return this._adapters.FastGetOrAdd(instanceType,
                this._isLockerFunc,
                this._lockerFactory,
                this._valueFactory);
        }

        private class Locker : IAwaitableAdapter
        {
            public bool IsAwaitable { get; }

            public bool IsCompleted(object instance)
            {
                throw new NotImplementedException();
            }

            public object GetResult(object instance)
            {
                throw new NotImplementedException();
            }

            public void OnCompleted(object instance, Action continuation)
            {
                throw new NotImplementedException();
            }
        }
    }
}