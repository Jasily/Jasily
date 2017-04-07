using System.Linq.Expressions;

namespace Jasily.DependencyInjection.Internal.CallSites
{
    internal interface IServiceCallSite
    {
        Expression ResolveExpression(ParameterExpression provider);

        object ResolveValue(ServiceProvider provider);
    }
}