using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Jasily.DependencyInjection.MemberInjection.Internal
{
    internal class PropertyInjector<T, TProperty> : MemberInjector<T, TProperty>
    {
        public PropertyInjector(IInternalMemberInjectorFactory factory, PropertyInfo property) : base(factory, property)
        {
            this.Property = property;
            this.injectAction = this.InitializeAction();
        }

        public PropertyInfo Property { get; }

        protected override void ReflectInjectAction(T instance, TProperty value)
        {
            try
            {
                this.Property.SetValue(instance, value);
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
        }

        protected override Action<T, TProperty> CompileInjectAction()
        {
            var instance = Expression.Parameter(typeof(T));
            var value = Expression.Parameter(typeof(TProperty));
            var prop = Expression.Property(instance, this.Property);
            var body = Expression.Assign(prop, value);
            return Expression.Lambda<Action<T, TProperty>>(body, instance, value).Compile();
        }
    }
}
