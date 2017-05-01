using System;
using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal interface IInternalMethodInvokerFactory
    {
        IServiceProvider ServiceProvider { get; }

        bool IsValueType { get; }

        BaseInvoker GetMethodInvoker(MethodInfo method);
    }
}
