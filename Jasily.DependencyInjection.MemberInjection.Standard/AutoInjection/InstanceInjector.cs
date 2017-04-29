using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Jasily.DependencyInjection.MemberInjection.Internal;
using System.Threading;

namespace Jasily.DependencyInjection.MemberInjection.AutoInjection
{
    internal class InstanceInjector<T> : IInstanceInjector<T>
    {
        private readonly (FieldInfo, InjectAttribute)[] fields;
        private readonly (PropertyInfo, InjectAttribute)[] properties;
        private readonly IServiceProvider serviceProvider;
        private Action<IServiceProvider, T> injectAction;

        public InstanceInjector(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.fields = (from field in typeof(T).GetRuntimeFields()
                           let attr = field.GetCustomAttribute<InjectAttribute>()
                           where attr != null
                           select (field, attr)).ToArray();

            this.properties = (from property in typeof(T).GetRuntimeProperties()
                               let attr = property.GetCustomAttribute<InjectAttribute>()
                               where attr != null
                               select (property, attr)).ToArray();

            if (this.properties.FirstOrDefault(z => !z.Item1.CanWrite).Item1 is PropertyInfo p)
            {
                throw new InvalidOperationException($"property {p} does not contains setter.");
            }
        }

        public void Inject(IServiceProvider serviceProvider, T instance)
        {
            if (this.injectAction == null)
            {
                Interlocked.CompareExchange(ref this.injectAction, this.ImplInjectAction(), null);
            }

            this.injectAction(serviceProvider, instance);
        }

        private Action<IServiceProvider, T> ImplInjectAction()
        {
            var factory = (IInternalMemberInjectorFactory)this.serviceProvider.GetRequiredService<IMemberInjectorFactory<T>>();
            var ps = this.properties.Select<(PropertyInfo, InjectAttribute), (MemberInfo, InjectAttribute)>(z => z);
            var fs = this.fields.Select<(FieldInfo, InjectAttribute), (MemberInfo, InjectAttribute)>(z => z);
            var actions = ps.Concat(fs)
                .Select(z => new Action<IServiceProvider, T>((provider, instance) =>
                    ((IMemberInjector<T>)factory.InternalGetMemberInjector(z.Item1))
                    .Inject(provider, instance, z.Item2.IsRequired))
                ).ToArray();
            return new Action<IServiceProvider, T>((provider, instance) => 
            {
                foreach (var a in actions) a(provider, instance);
            });
        }
    }
}
