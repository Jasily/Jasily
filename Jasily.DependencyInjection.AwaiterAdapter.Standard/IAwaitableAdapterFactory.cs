using System;
using JetBrains.Annotations;

namespace Jasily.DependencyInjection.AwaiterAdapter
{
    public interface IAwaitableAdapterFactory
    {
        IAwaitableAdapter GetAwaitableAdapter([NotNull] Type instanceType);
    }
}