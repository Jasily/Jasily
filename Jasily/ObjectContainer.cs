namespace Jasily
{
    public sealed class ObjectContainer<T>
    {
        private T value;

        public ObjectContainer()
        {

        }

        public ObjectContainer(T value)
        {
            this.SetValue(value);
        }

        public bool IsSet { get; private set; }

        public T Value
        {
            get { return this.value; }
            set { this.SetValue(value); }
        }

        public void Reset()
        {
            this.IsSet = false;
            this.value = default(T);
        }

        public void SetValue(T value)
        {
            this.value = value;
            this.IsSet = true;
        }
    }
}