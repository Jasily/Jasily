using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Jasily.DependencyInjection.MethodInvoker;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.AwaiterAdapter.Internal
{
    internal class AwaitableAdapterFactory : IAwaitableAdapterFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, Lazy<IAwaitableAdapter>> _adapters = new ConcurrentDictionary<Type, Lazy<IAwaitableAdapter>>();
        private readonly Func<Type, Lazy<IAwaitableAdapter>> _valueFactory;

        public AwaitableAdapterFactory([NotNull] IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this._valueFactory = t => new Lazy<IAwaitableAdapter>(() => this.CreateAwaitableAdapter(t), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IAwaitableAdapter GetAwaitableAdapter([NotNull] Type instanceType)
        {
            if (instanceType == null) throw new ArgumentNullException(nameof(instanceType));

            return this._adapters.GetOrAdd(instanceType, this._valueFactory).Value;
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
                ? typeof(VoidAwaitableAdapter<,>).MakeGenericType(instanceType, info.AwaiterType)
                : typeof(GenericAwaitableAdapter<,,>).MakeGenericType(instanceType, info.AwaiterType, resultType);

            var factory = this._serviceProvider.AsMethodInvokerProvider().GetInvokerFactory(closedType);
            var ctor = factory.Constructors.Single();
            return (IAwaitableAdapter)factory.GetConstructorInvoker(ctor).Invoke(this._serviceProvider, oa);
        }
    }
}