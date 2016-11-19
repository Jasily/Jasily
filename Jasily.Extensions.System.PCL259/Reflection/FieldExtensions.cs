using System.Linq.Expressions;
using JetBrains.Annotations;

namespace System.Reflection
{
    public static class FieldExtensions
    {
        public static Func<TObject, TField> CompileGetter<TObject, TField>([NotNull] this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (typeof(TObject) != field.DeclaringType) throw new InvalidOperationException();
            if (typeof(TField) != field.FieldType) throw new InvalidOperationException();

            var self = Expression.Parameter(typeof(TObject));
            var accessor = Expression.Field(self, field);
            return Expression.Lambda<Func<TObject, TField>>(accessor, self).Compile();
        }

        public static Func<object, object> CompileGetter([NotNull] this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            var self = Expression.Parameter(typeof(object));
            var accessor = Expression.MakeMemberAccess(Expression.Convert(self, field.DeclaringType), field);
            return Expression.Lambda<Func<object, object>>(Expression.Convert(accessor, typeof(object)), self).Compile();
        }

        public static Action<TObject, TField> CompileSetter<TObject, TField>([NotNull] this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (typeof(TObject) != field.DeclaringType) throw new InvalidOperationException();
            if (typeof(TField) != field.FieldType) throw new InvalidOperationException();

            var self = Expression.Parameter(typeof(TObject));
            var value = Expression.Parameter(typeof(TField));
            var accessor = Expression.Field(self, field);
            var assign = Expression.Assign(accessor, value);
            return Expression.Lambda<Action<TObject, TField>>(assign, self, value).Compile();
        }

        public static Action<object, object> CompileSetter([NotNull] this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            var self = Expression.Parameter(typeof(object));
            var value = Expression.Parameter(typeof(object));
            var accessor = Expression.Field(Expression.Convert(self, field.DeclaringType), field);
            var assign = Expression.Assign(accessor, Expression.Convert(value, field.FieldType));
            return Expression.Lambda<Action<object, object>>(assign, self, value).Compile();
        }
    }
}