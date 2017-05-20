using System;

namespace Jasily.DependencyInjection.Typed.Internal.Utilitys
{
    internal interface IReThrowContainer<out TValue>
    {
        TValue GetOrThrow();
    }
}