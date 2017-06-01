using System;
using System.Collections.Generic;

namespace Jasily.DependencyInjection.ComplexService
{
    public interface IComplexServiceFactory
    {
        Type GetClosedServiceTypeOrNull(Type type);

        IEnumerable<Type> GetClosedServiceTypes(Type type);
    }
}