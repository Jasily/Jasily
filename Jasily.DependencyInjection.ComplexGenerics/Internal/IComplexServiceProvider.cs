using System;
using System.Collections.Generic;

namespace Jasily.DependencyInjection.ComplexGenerics.Internal
{
    internal interface IComplexServiceProvider
    {
        object Get(IServiceProvider serviceProvider, Type closedServiceType);

        IEnumerable<object> GetAll(IServiceProvider serviceProvider, Type closedServiceType);
    }
}