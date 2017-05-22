using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using System.Collections.Generic;

#if PCL259
#else
using System.Collections.Concurrent;
#endif

namespace Jasily.Extensions.System.Reflection
{
    /// <summary>
    /// extension methods for <see cref="Type"/>.
    /// </summary>
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

        public static bool IsValueWriteAtomic([NotNull] this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            //
            // this is a copied from:
            // mscorlib/system/Collections/Concurrent/ConcurrentDictionary.cs
            //
            var isAtomic = type.GetTypeInfo().IsClass
                || type == typeof(bool)
                || type == typeof(char)
                || type == typeof(byte)
                || type == typeof(sbyte)
                || type == typeof(short)
                || type == typeof(ushort)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(float);

            if (!isAtomic && IntPtr.Size == 8)
            {
                isAtomic |= type == typeof(double) || type == typeof(long);
            }

            return isAtomic;
        }

#region FastMakeGenericType

#if PCL259
#else

        private static readonly ConcurrentDictionary<TypeArray, Type> Cache
            = new ConcurrentDictionary<TypeArray, Type>(new TypeArrayEqualityComparer());

        /// <summary>
        /// make generic type and cache.
        /// </summary>
        /// <param name="genericType"></param>
        /// <param name="typeArguments"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        public static Type FastMakeGenericType([NotNull] this Type genericType, [NotNull] params Type[] typeArguments)
        {
            if (genericType == null) throw new ArgumentNullException(nameof(genericType));
            if (typeArguments == null) throw new ArgumentNullException(nameof(typeArguments));

            var array = new TypeArray(genericType, typeArguments);
            return Cache.TryGetValue(array, out var result)
                ? result
                : Cache.GetOrAdd(array, genericType.MakeGenericType(typeArguments));
        }

        private class TypeArrayEqualityComparer : IEqualityComparer<TypeArray>
        {
            public bool Equals(TypeArray x, TypeArray y) => x.Equals(y);

            public int GetHashCode(TypeArray obj) => obj.HashCode;
        }

        private struct TypeArray : IEquatable<TypeArray>
        {
            internal readonly int HashCode;
            private readonly Type[] _types;

            public TypeArray([NotNull] Type type, [NotNull] Type[] types)
            {
                this._types = new Type[types.Length + 1];
                this._types[0] = type;
                this.HashCode = type.GetHashCode();
                for (var i = 0; i < types.Length; i++)
                {
                    this.HashCode ^= types[i].GetHashCode();
                    this._types[i + 1] = types[i];
                }
            }

            public bool Equals(TypeArray other)
            {
                if (this._types.Length != other._types.Length) return false;
                for (var i = 0; i < this._types.Length; i++)
                {
                    if (this._types[i] != other._types[i]) return false;
                }
                return true;
            }
        }

#endif
#endregion
    }
}