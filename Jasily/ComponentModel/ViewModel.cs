using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    public class ViewModel : NotifyPropertyChangedObject
    {
        private Dictionary<string, object> _properties;

        /// <summary>
        /// Get property from internal dictionary. 
        /// This method may unboxing <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        [InvokeOn(ThreadKind.UIThread)]
        protected T GetProperty<T>([NotNull, CallerMemberName] string propertyName = "")
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            return this._properties != null && this._properties.TryGetValue(propertyName, out var val) ? (T) val : default(T);
        }

        /// <summary>
        /// Set property to internal dictionary.
        /// This method may boxing <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        [InvokeOn(ThreadKind.UIThread)]
        protected bool SetProperty<T>(T value, [NotNull, CallerMemberName] string propertyName = "")
        {
            if (this._properties == null)
            {
                this._properties = new Dictionary<string, object>();
            }
            else if (this._properties.TryGetValue(propertyName, out var val) && EqualityComparer<T>.Default.Equals(value, (T)val))
            {
                return false;
            }

            this._properties[propertyName] = value;
            return true;
        }
    }

    public class ViewModel<TSource> : ViewModel
    {
        private TSource _source;

        public ViewModel([CanBeNull] TSource source = default(TSource))
        {
            this._source = source;
        }

        [CanBeNull]
        [NotifyPropertyChanged(Order = -1)]
        public TSource Source
        {
            get => this._source;
            set
            {
                if (this.SetPropertyRef(ref this._source, value))
                {
                    this.OnSourceUpdated(value);
                    this.SourceUpdated?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// raise after invoke <see cref="OnSourceUpdated"/>;
        /// </summary>
        public event TypedEventHandler<ViewModel<TSource>, TSource> SourceUpdated;

        public static implicit operator TSource([NotNull] ViewModel<TSource> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.Source;
        }

        protected virtual void OnSourceUpdated(TSource source) { }
    }
}