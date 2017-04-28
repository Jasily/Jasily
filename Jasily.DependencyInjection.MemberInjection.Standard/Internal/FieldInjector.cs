using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Jasily.DependencyInjection.MemberInjection.Internal
{
    internal class FieldInjector<T, TField> : MemberInjector<T, TField>
    {
        public FieldInjector(IInternalMemberInjectorFactory factory, FieldInfo field) : base(factory, field)
        {
            this.Field = field;
            this.injectAction = this.InitializeAction();
        }

        public FieldInfo Field { get; }

        protected override void ReflectInjectAction(T instance, TField value)
        {
            this.Field.SetValue(instance, value);
        }

        protected override Action<T, TField> CompileInjectAction()
        {
            var instance = Expression.Parameter(typeof(T));
            var value = Expression.Parameter(typeof(TField));
            var field = Expression.Field(instance, this.Field);
            var body = Expression.Assign(field, value);
            return Expression.Lambda<Action<T, TField>>(body, instance, value).Compile();
        }
    }
}
