using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IMethodInvokerFactory<in T>
    {
        object InvokeInstanceMethod(MethodInfo method, T instance, OverrideArguments arguments = default(OverrideArguments));

        object InvokeStaticMethod(MethodInfo method, OverrideArguments arguments = default(OverrideArguments));

        IInstanceMethodInvoker<T> GetInstanceMethodInvoker(MethodInfo method);

        IStaticMethodInvoker GetStaticMethodInvoker(MethodInfo method);
    }
}
