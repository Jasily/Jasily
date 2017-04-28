using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MemberInjection.Internal
{
    internal class MemberInjectorFactory<T> : IMemberInjectorFactory<T>, IInternalMemberInjectorFactory
    {
        private readonly HashSet<MemberInfo> members = new HashSet<MemberInfo>();
        private readonly ConcurrentDictionary<MemberInfo, MemberInjector> injectorMaps
            = new ConcurrentDictionary<MemberInfo, MemberInjector>();

        public IServiceProvider ServiceProvider { get; }

        public bool IsValueType { get; }

        public MemberInjectorFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            var type = typeof(T);
            this.IsValueType = type.GetTypeInfo().IsValueType;

            foreach (var field in type.GetRuntimeFields()) this.members.Add(field);
            foreach (var property in type.GetRuntimeProperties()) this.members.Add(property);
        }

        private Type ResolveType(MemberInfo member)
        {
            switch (member)
            {
                case FieldInfo field:
                    return typeof(FieldInjector<,>).MakeGenericType(typeof(T), field.FieldType);

                case PropertyInfo property:
                    return typeof(PropertyInjector<,>).MakeGenericType(typeof(T), property.PropertyType);

                default:
                    throw new NotImplementedException();
            }
            
        }

        public MemberInjector InternalGetMemberInjector(MemberInfo member)
        {
            if (!this.injectorMaps.TryGetValue(member, out var injector))
            {
                if (member.DeclaringType == typeof(T))
                {
                    injector = (MemberInjector)Activator.CreateInstance(this.ResolveType(member), new object[] { this, member });
                    injector = this.injectorMaps.GetOrAdd(member, injector);
                }
                else
                {
                    var type = typeof(IMemberInjectorFactory<>).MakeGenericType(member.DeclaringType);
                    var container = (IInternalMemberInjectorFactory) this.ServiceProvider.GetService(type);
                    injector = container.InternalGetMemberInjector(member);
                }
                injector = this.injectorMaps.GetOrAdd(member, injector);
            }
            return injector;
        }

        public IMemberInjector<T> GetMemberInjector([NotNull] PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (!this.members.Contains(property)) throw new InvalidOperationException();
            if (!property.CanWrite) throw new InvalidOperationException($"property {property} does not contains setter.");

            return (IMemberInjector<T>) this.InternalGetMemberInjector(property);
        }

        public IMemberInjector<T> GetMemberInjector([NotNull] FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (!this.members.Contains(field)) throw new InvalidOperationException();

            return (IMemberInjector<T>)this.InternalGetMemberInjector(field);
        }
    }
}
