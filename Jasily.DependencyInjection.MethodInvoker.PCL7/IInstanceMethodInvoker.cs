using System;

namespace Jasily.DependencyInjection.MethodInvoker
{
    internal interface IInstanceMethodInvoker<in T>
    {
        object Invoke(T instance, IServiceProvider provider, OverrideArguments arguments);
    }
}
