using E = System.Linq.Expressions.Expression;

namespace Jasily.Cache
{
    public static class Expression
    {
        public static readonly E Null = E.Constant(null);
    }

    public static class Expression<T>
    {
        public static readonly E Default = E.Default(typeof(T));
    }
}