using System;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IStaticMethodInvoker
    {
        object Invoke(OverrideArguments arguments);
    }

    public interface IStaticMethodInvoker<out TResult>
    {
        TResult Invoke(OverrideArguments arguments);
    }
}
