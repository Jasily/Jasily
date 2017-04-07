namespace Jasily.Core
{
    public sealed class ValueContainer<T>
    {
        public ValueContainer(T value)
        {
            this.Value = value;
        }

        public T Value { get; }
    }
}