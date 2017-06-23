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

        public ViewModel([CanBeNull] TSource source = default(TSource))
        {
            this._source = source;
        }

        [NotifyPropertyChanged(Order = -1)]
        public TSource Source
        {
            get => this._source;
            set => this.SetPropertyRef(ref this._source, value);
        }

        public static implicit operator TSource([NotNull] ViewModel<TSource> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.Source;
        }
    }
}