using System.Linq.Expressions;

namespace Jasily.DependencyInjection.Internal
{
    internal class ConstantCallSite : IServiceCallSite
    {
        internal object DefaultValue { get; }

        public ConstantCallSite(object defaultValue)
        {
            this.DefaultValue = defaultValue;
        }

        public Expression ResolveExpression(ParameterExpression provider)
            => Expression.Constant(this.DefaultValue);

        public object ResolveValue(ServiceProvider provider) => this.DefaultValue;
    }
}