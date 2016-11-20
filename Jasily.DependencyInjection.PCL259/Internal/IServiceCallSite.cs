using System.Linq.Expressions;

namespace Jasily.DependencyInjection.Internal
{
    internal interface IServiceCallSite
    {
        Expression ResolveExpression(ParameterExpression provider);

        object ResolveValue(ServiceProvider provider);
    }
}