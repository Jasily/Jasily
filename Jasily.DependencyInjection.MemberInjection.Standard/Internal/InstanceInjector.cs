using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MemberInjection.Internal
{
    internal class InstanceInjector<T> : IInstanceInjector<T>
    {
        private readonly (FieldInfo, InjectAttribute)[] _fields;
        private readonly (PropertyInfo, InjectAttribute)[] _properties;
        private readonly IServiceProvider _serviceProvider;
        private Action<IServiceProvider, T> _injectAction;

        public InstanceInjector(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;

            this._fields = serviceProvider.GetRequiredService<IInjectFieldsSelector<T>>()
                .SelectMembers().ToArray();
            this._properties = serviceProvider.GetRequiredService<IInjectPropertiesSelector<T>>()
                .SelectMembers().ToArray();

            if (this._properties.FirstOrDefault(z => !z.Item1.CanWrite).Item1 is PropertyInfo p)
            {
                throw new InvalidOperationException($"property {p} does not contains setter.");
            }
        }

        public void Inject(IServiceProvider serviceProvider, T instance)
        {
            if (this._injectAction == null)
            {
                Interlocked.CompareExchange(ref this._injectAction, this.ImplInjectAction(), null);
            }

            this._injectAction(serviceProvider, instance);
        }

        private Action<IServiceProvider, T> ImplInjectAction()
        {
            var factory = (IInternalMemberInjectorFactory)this._serviceProvider.GetRequiredService<IMemberInjectorFactory<T>>();
            var ps = this._properties.Select<(PropertyInfo, InjectAttribute), (MemberInfo, InjectAttribute)>(z => z);
            var fs = this._fields.Select<(FieldInfo, InjectAttribute), (MemberInfo, InjectAttribute)>(z => z);
            var actions = ps.Concat(fs)
                .Select(z => new Action<IServiceProvider, T>((provider, instance) =>
                    ((IMemberInjector<T>)factory.InternalGetMemberInjector(z.Item1))
                    .Inject(provider, instance, z.Item2.IsRequired))
                ).ToArray();
            return (provider, instance) => 
            {
                foreach (var a in actions) a(provider, instance);
            };
        }
    }
}
