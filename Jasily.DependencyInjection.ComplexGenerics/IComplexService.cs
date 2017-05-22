using System.Collections.Generic;

namespace Jasily.DependencyInjection.ComplexGenerics
{
    public interface IComplexService<out T>
    {
        T Value();

        IEnumerable<T> Values();
    }
}