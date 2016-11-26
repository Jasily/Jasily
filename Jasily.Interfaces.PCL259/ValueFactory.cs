using System;
using JetBrains.Annotations;

namespace Jasily.Interfaces
{
    public struct ValueFactory<T> : IValueable<T>
    {
        private readonly Func<T> factory;

        public ValueFactory([NotNull] Func<T> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            this.factory = factory;
        }

        public T GetValue() => this.factory();
    }
}