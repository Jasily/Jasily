using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IMethodInvoker<T>
    {
        object InvokeInstanceMethod(MethodInfo method, T instance, OverrideArguments arguments = default(OverrideArguments));

        object InvokeStaticMethod(MethodInfo method, OverrideArguments arguments = default(OverrideArguments));
    }
}
