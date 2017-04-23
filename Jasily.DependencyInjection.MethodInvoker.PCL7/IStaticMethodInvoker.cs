using System;

namespace Jasily.DependencyInjection.MethodInvoker
{
    public interface IStaticMethodInvoker
    {
        object Invoke(OverrideArguments arguments);
    }
}
