using JetBrains.Annotations;

namespace Jasily
{
    internal interface IReThrowContainer<out TValue>
    {
        [CanBeNull]
        TValue GetOrThrow();
    }
}