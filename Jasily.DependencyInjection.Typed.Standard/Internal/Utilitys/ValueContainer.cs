namespace Jasily.DependencyInjection.Typed.Internal.Utilitys
{
    internal sealed class ValueContainer<T> : IReThrowContainer<T>
    {
        private readonly T _value;

        public ValueContainer(T value)
        {
            this._value = value;
        }

        public T GetOrThrow() => this._value;
    }
}