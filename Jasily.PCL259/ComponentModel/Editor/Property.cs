using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;

namespace Jasily.ComponentModel.Editor
{
    internal class Property
    {
        [NotNull]
        public static readonly PropertyChangedEventArgs ValuePropertyChangedEventArgs
            = new PropertyChangedEventArgs(nameof(Property<int>.Value));
    }

    public class Property<T> : IPropertyContainer, INotifyPropertyChanged
    {
        private T value;
        public event PropertyChangedEventHandler PropertyChanged;

        public Property(T value = default(T))
        {
            this.value = value;
        }

        public T Value
        {
            get { return this.value; }
            set
            {
                var converter = this.SetterConverter;
                if (converter != null) value = converter(value);

                if (EqualityComparer<T>.Default.Equals(value)) return;
                this.value = value;
                this.OnPropertyChanged();
            }
        }

        public Func<T, T> SetterConverter { get; set; }

        object IPropertyContainer.Value
        {
            get { return this.Value; }
            set { this.Value = (T)value; }
        }

        /// <summary>
        /// set value without call notify
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(T value) => this.value = value;

        private void OnPropertyChanged() => this.PropertyChanged?.Invoke(this, Property.ValuePropertyChangedEventArgs);

        public static implicit operator T(Property<T> p) => p.value;
    }
}