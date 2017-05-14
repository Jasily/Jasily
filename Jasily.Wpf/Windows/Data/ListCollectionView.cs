using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Jasily.ComponentModel;

namespace Jasily.Wpf.Windows.Data
{
    public class ListCollectionView<T> : ViewModel
    {
        private T selected;
        private Predicate<T> filter;
        private Predicate<object> filterPredicate;

        public ListCollectionView()
        {
            this.Collection = new ObservableCollection<T>();
            this.View = new ListCollectionView(this.Collection);
        }

        public ObservableCollection<T> Collection { get; }

        public ListCollectionView View { get; }

        public T Selected
        {
            get { return this.selected; }
            set { this.SetPropertyRef(ref this.selected, value); }
        }

        public Predicate<T> Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                if (value == null)
                {
                    this.View.Filter = null;
                }
                else
                {
                    if (this.filterPredicate == null) this.filterPredicate = this.OnFilter;
                    this.View.Filter = this.filterPredicate;
                }
            }
        }

        private bool OnFilter(object obj) => this.filter?.Invoke((T)obj) ?? true;
    }
}