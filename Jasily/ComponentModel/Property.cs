using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    internal class Property
    {
        [NotNull]
        public static readonly PropertyChangedEventArgs ValuePropertyChangedEventArgs
            = new PropertyChangedEventArgs(nameof(Property<int>.Value));
    }

    public class Property<T> : IPropertyContainer, INotifyPropertyChanged
    {
        private T _value;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        public Property(T value = default(T))
        {
            this._value = value;
        }

        public T Value
        {
            get => this._value;
            [InvokeOn(ThreadKind.UIThread)] set
            {
                if (EqualityComparer<T>.Default.Equals(value)) return;
                this._value = value;
                this.OnPropertyChanged();
            }
        }

        object IPropertyContainer.Value
        {
            get => this.Value;
            set => this.Value = (T)value;
        }

        /// <summary>
        /// Set value without raise <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        /// <param name="value"></param>
        public void DirectSetValue(T value) => this._value = value;

        private void OnPropertyChanged() => this.PropertyChanged?.Invoke(this, Property.ValuePropertyChangedEventArgs);

        public static implicit operator T(Property<T> p) => p == null ? default(T) : p._value;
    }
}