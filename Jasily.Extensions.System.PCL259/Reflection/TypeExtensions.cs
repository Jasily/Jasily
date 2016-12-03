using System.Linq;
using JetBrains.Annotations;

namespace System.Reflection
{
    public static class TypeExtensions
    {
        /// <summary>
        /// get getter from field or property
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Func<object, object> GetGetter([NotNull] this Type type, string name)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var f = type.GetRuntimeField(name);
            if (f != null) return f.GetValue;
            var p = type.GetRuntimeProperty(name);
            if (p != null) return p.GetValue;
            return null;
        }

        /// <summary>
        /// get setter from field or property
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Action<object, object> GetSetter([NotNull] this Type type, string name)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var f = type.GetRuntimeField(name);
            if (f != null) return f.SetValue;
            var p = type.GetRuntimeProperty(name);
            if (p != null) return p.SetValue;
            return null;
        }

        public static string GetCSharpName([NotNull] this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return type.IsConstructedGenericType
                ? string.Format("{0}<{1}>",
                    type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.Ordinal)),
                    type.GenericTypeArguments.Select(GetCSharpName).JoinAsString(", "))
                : type.Name;
        }

        public static Func<TObject, TMember> CompileGetter<TObject, TMember>([NotNull] this Type type, string memberName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var field = type.GetRuntimeField(memberName);
            if (field != null) return field.CompileGetter<TObject, TMember>();
            var property = type.GetRuntimeProperty(memberName);
            return property?.CompileGetter<TObject, TMember>();
        }

        public static Func<object, object> CompileGetter([NotNull] this Type type, string memberName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var field = type.GetRuntimeField(memberName);
            if (field != null) return field.CompileGetter();
            var property = type.GetRuntimeProperty(memberName);
            return property?.CompileGetter();
        }

        public static Action<TObject, TMember> CompileSetter<TObject, TMember>([NotNull] this Type type, string memberName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var field = type.GetRuntimeField(memberName);
            if (field != null) return field.CompileSetter<TObject, TMember>();
            var property = type.GetRuntimeProperty(memberName);
            return property?.CompileSetter<TObject, TMember>();
        }

        public static Action<object, object> CompileSetter([NotNull] this Type type, string memberName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var field = type.GetRuntimeField(memberName);
            if (field != null) return field.CompileSetter();
            var property = type.GetRuntimeProperty(memberName);
            return property?.CompileSetter();
        }
    }

    public static class TypeInfoExtensions
    {
        public static bool CanInstantiation([NotNull] this TypeInfo type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return !type.IsAbstract && !type.IsInterface && !type.IsGenericTypeDefinition;
        }

        public static bool IsStaticClass([NotNull] this TypeInfo type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return type.IsAbstract && type.IsSealed;
        }
    }
}