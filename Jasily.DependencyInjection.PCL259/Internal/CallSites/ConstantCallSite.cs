using System.Linq.Expressions;

namespace Jasily.DependencyInjection.Internal.CallSites
{
    internal class ConstantCallSite : IImmutableCallSite
    {
        private readonly object defaultValue;

        public ConstantCallSite(object defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public Expression ResolveExpression(ParameterExpression provider)
            => this.defaultValue == null ? Core.Cached.Expression.Null : Expression.Constant(this.defaultValue);

        public object ResolveValue(ServiceProvider provider) => this.defaultValue;
    }
}