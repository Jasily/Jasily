using System.Linq.Expressions;

namespace Jasily.DependencyInjection.Internal
{
    public interface IServiceCallSite
    {
        Expression ResolveExpression(ParameterExpression provider);

        object ResolveValue(ServiceProvider provider);
    }
}