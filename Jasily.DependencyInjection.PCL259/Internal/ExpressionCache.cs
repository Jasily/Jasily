using System.Linq.Expressions;

namespace Jasily.DependencyInjection.Internal
{
    internal class ExpressionCache
    {
        public static readonly Expression Null = Expression.Constant(null);
    }
}