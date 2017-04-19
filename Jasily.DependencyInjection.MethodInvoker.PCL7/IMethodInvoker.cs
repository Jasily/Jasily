using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IMethodInvoker<T>
    {
        object Invoke(MethodInfo method, T instance = default(T), OverrideArguments arguments = default(OverrideArguments));
    }
}
