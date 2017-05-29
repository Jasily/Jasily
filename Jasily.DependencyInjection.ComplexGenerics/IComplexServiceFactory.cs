using System;
using System.Collections.Generic;

namespace Jasily.DependencyInjection.ComplexGenerics
{
    public interface IComplexServiceFactory
    {
        Type GetClosedServiceTypeOrNull(Type type);

        IEnumerable<Type> GetClosedServiceTypes(Type type);
    }
}