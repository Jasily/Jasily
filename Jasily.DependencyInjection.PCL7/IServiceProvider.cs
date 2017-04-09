using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Jasily.DependencyInjection
{
    public interface IServiceProvider : System.IServiceProvider, ISupportRequiredService
    {
        ResolveResult GetService([NotNull] Type serviceType, [CanBeNull] string serviceName);
    }
}