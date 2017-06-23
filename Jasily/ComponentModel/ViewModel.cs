using System;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    public class ViewModel : NotifyPropertyChangedObject
    {
    }

    public class ViewModel<TSource> : ViewModel
    {
        private TSource _source;
        /// <summary>
        /// raise after invoke <see cref="OnSourceUpdated"/>;
        /// </summary>
        public event TypedEventHandler<ViewModel<TSource>, TSource> SourceUpdated;

        public ViewModel([CanBeNull] TSource source = default(TSource))
        {
            this._source = source;
        }

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

        public static implicit operator TSource([NotNull] ViewModel<TSource> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.Source;
        }

        protected virtual void OnSourceUpdated(TSource source) { }
    }
}