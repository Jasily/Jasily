using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal interface IMethodInvokerContainer
    {
        MethodInvoker GetInvoker(MethodInfo method);
    }
}
