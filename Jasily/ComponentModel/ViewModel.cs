using System;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    public class ViewModel : NotifyPropertyChangedObject
    {
    }

    public class ViewModel<TSource> : ViewModel
    {
        private TSource source;

        public ViewModel(TSource source)
        {
            this.source = source;
        }

        [NotifyPropertyChanged(Order = -1)]
        public TSource Source
        {
            get { return this.source; }
            set { this.SetPropertyRef(ref this.source, value); }
        }

        public static implicit operator TSource([NotNull] ViewModel<TSource> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.Source;
        }
    }
}