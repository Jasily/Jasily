using System;
using System.Collections.Concurrent;
using System.Linq;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Extensions.System.Collections.Concurrent;
using Jasily.Extensions.System.Reflection;
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
            this._valueFactory = this.CreateAwaitableAdapter;
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

        [NotNull]
        private IAwaitableAdapter CreateAwaitableAdapter(Type instanceType)
        {
            var info = AwaitableInfo.TryBuild(instanceType);
            if (info == null) return NonTaskAwaiterAdapter.Instance;

            var oa = new OverrideArguments();
            oa.AddArgument("info", info);

            var resultType = info.GetResultMethod.ReturnType;
            var closedType = resultType == typeof(void)
                ? typeof(VoidAwaitableAdapter<,>).FastMakeGenericType(instanceType, info.AwaiterType)
                : typeof(GenericAwaitableAdapter<,,>).FastMakeGenericType(instanceType, info.AwaiterType, resultType);

            var factory = this._serviceProvider.AsMethodInvokerProvider().GetInvokerFactory(closedType);
            var ctor = factory.Constructors.Single();
            return (IAwaitableAdapter)factory.GetConstructorInvoker(ctor).Invoke(this._serviceProvider, oa);
        }
    }
}