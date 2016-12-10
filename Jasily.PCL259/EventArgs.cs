using System;

namespace Jasily
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            this.Value = value;
        }

        public T Value { get; }

        public static implicit operator EventArgs<T>(T value) => new EventArgs<T>(value);
    }
}