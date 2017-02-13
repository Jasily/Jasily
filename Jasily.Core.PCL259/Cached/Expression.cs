using System.Reflection;
using JetBrains.Annotations;
using E = System.Linq.Expressions.Expression;

namespace Jasily.Core.Cached
{
    public class Expression
    {
        [NotNull]
        public static E Null { get; } = E.Constant(null);

        [NotNull]
        public static E Default<T>() => C<T>.DefaultValue;

        private static class C<T>
        {
            // ReSharper disable once StaticMemberInGenericType
            [NotNull]
            internal static readonly E DefaultValue;

            static C()
            {
                DefaultValue = typeof(T).GetTypeInfo().IsValueType ? E.Default(typeof(T)) : Null;
            }
        }
    }
}