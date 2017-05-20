using JetBrains.Annotations;

namespace Jasily
{
    public class ValueContainer<T> : IReThrowContainer<T>
    {
        private readonly T _value;

        public ValueContainer([CanBeNull] T value)
        {
            this._value = value;
        }

        [CanBeNull]
        public T GetOrThrow() => this._value;
    }
}