using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Jasily.Collections.ObjectModel
{
    public class GroupedObservableCollection<TKey, TElement> : ObservableCollection<TElement>, IGrouping<TKey, TElement>
    {
        private TKey key;

        public GroupedObservableCollection(TKey key)
        {
            this.key = key;
        }
        public GroupedObservableCollection(TKey key, IEnumerable<TElement> collection)
            : base(collection)
        {
            this.key = key;
        }
        public GroupedObservableCollection(TKey key, List<TElement> list)
            : base(list)
        {
            this.key = key;
        }
        public GroupedObservableCollection()
        {
        }
        public GroupedObservableCollection(IEnumerable<TElement> collection)
            : base(collection)
        {
        }
        public GroupedObservableCollection(List<TElement> list)
            : base(list)
        {
        }

        public TKey Key
        {
            get { return this.key; }
            set
            {
                this.key = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Key)));
            }
        }
    }
}