using System.Linq.Expressions;
using JetBrains.Annotations;

namespace System.Reflection
{
    public static class PropertyExtensions
    {
        public static Func<TObject, TProperty> CompileGetter<TObject, TProperty>([NotNull] this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (typeof(TObject) != property.DeclaringType) throw new InvalidOperationException();
            if (typeof(TProperty) != property.PropertyType) throw new InvalidOperationException();

            var self = Expression.Parameter(typeof(TObject));
            var accessor = Expression.Property(self, property);
            return Expression.Lambda<Func<TObject, TProperty>>(accessor, self).Compile();
        }

        public static Func<object, object> CompileGetter([NotNull] this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var self = Expression.Parameter(typeof(object));
            var accessor = Expression.MakeMemberAccess(Expression.Convert(self, property.DeclaringType), property);
            var selector = Expression.Convert(accessor, typeof(object));
            return Expression.Lambda<Func<object, object>>(selector, self).Compile();
        }

        public static Action<TObject, TProperty> CompileSetter<TObject, TProperty>([NotNull] this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (typeof(TObject) != property.DeclaringType) throw new InvalidOperationException();
            if (typeof(TProperty) != property.PropertyType) throw new InvalidOperationException();

            var self = Expression.Parameter(typeof(TObject));
            var value = Expression.Parameter(typeof(TProperty));
            var accessor = Expression.Property(self, property);
            var assign = Expression.Assign(accessor, value);
            return Expression.Lambda<Action<TObject, TProperty>>(assign, self, value).Compile();
        }

        public static Action<object, object> CompileSetter([NotNull] this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var self = Expression.Parameter(typeof(object));
            var value = Expression.Parameter(typeof(object));
            var accessor = Expression.Property(Expression.Convert(self, property.DeclaringType), property);
            var assign = Expression.Assign(accessor, Expression.Convert(value, property.PropertyType));
            return Expression.Lambda<Action<object, object>>(assign, self, value).Compile();
        }
    }
}