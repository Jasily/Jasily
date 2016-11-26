namespace Jasily.Interfaces
{
    public struct Value<T> : IValueable<T>
    {
        private readonly T obj;

        public Value(T obj)
        {
            this.obj = obj;
        }

        public T GetValue() => this.obj;
    }
}