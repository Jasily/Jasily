using System;

namespace Jasily.Features.Internal
{
    internal interface IFactoryProvider
    {
        object GetFactory(Type type);
    }
}