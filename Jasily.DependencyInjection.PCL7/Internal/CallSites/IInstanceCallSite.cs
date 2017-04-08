using System.Linq.Expressions;

namespace Jasily.DependencyInjection.Internal.CallSites
{
    internal interface IInstanceCallSite
    {
        Expression ResolveExpression(ParameterExpression instance, ParameterExpression provider);

        object ResolveValue(object instance, ServiceProvider provider);
    }
}