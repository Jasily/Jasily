using System;
using System.Reflection;

namespace Jasily.DependencyInjection.MethodInvoker.Internal
{
    internal interface IInternalMethodInvokerFactory
    {
        IServiceProvider ServiceProvider { get; }

        bool IsValueType { get; }

        MethodInvoker GetMethodInvoker(MethodInfo method);
    }
}
