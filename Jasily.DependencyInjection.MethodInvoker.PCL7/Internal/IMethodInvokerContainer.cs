using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal interface IMethodInvokerContainer
    {
        MethodInvoker GetMethodInvoker(MethodInfo method);
    }
}
