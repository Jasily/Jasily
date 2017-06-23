using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Jasily.Core.Annotations;
using Jasily.Extensions.System.ComponentModel;
using Jasily.Extensions.System.Linq;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    public class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        protected readonly object SyncRoot = new object();

        #region api 0

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator, InvokeOn(ThreadKind.UIThread)]
        protected void NotifyPropertyChanged<T>([NotNull] Expression<Func<T, object>> propertySelector)
        {
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));
            var propertyName = KeySelector.SelectProperty(propertySelector);
            this.PropertyChanged?.Invoke(this, propertyName);
        }

        [NotifyPropertyChangedInvocator, InvokeOn(ThreadKind.UIThread)]
        protected void NotifyPropertyChanged([NotNull, CallerMemberName] string propertyName = "")
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            this.PropertyChanged?.Invoke(this, propertyName);
        }

        [NotifyPropertyChangedInvocator, InvokeOn(ThreadKind.UIThread)]
        protected void NotifyPropertyChanged([NotNull] params string[] propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            this.PropertyChanged?.Invoke(this, propertyNames);
        }

        [NotifyPropertyChangedInvocator, InvokeOn(ThreadKind.UIThread)]
        protected void NotifyPropertyChanged([NotNull] IEnumerable<string> propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            this.PropertyChanged?.Invoke(this, propertyNames);
        }

        [NotifyPropertyChangedInvocator, InvokeOn(ThreadKind.UIThread)]
        protected void NotifyPropertyChanged([NotNull] params PropertyChangedEventArgs[] propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            this.PropertyChanged?.Invoke(this, propertyNames);
        }

        [NotifyPropertyChangedInvocator, InvokeOn(ThreadKind.UIThread)]
        protected void NotifyPropertyChanged([NotNull] IEnumerable<PropertyChangedEventArgs> propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            this.PropertyChanged?.Invoke(this, propertyNames);
        }

        protected void ClearPropertyChangedInvocationList()
        {
            this.PropertyChanged = null;
        }

        #endregion

        #region api 1

        [NotifyPropertyChangedInvocator, InvokeOn(ThreadKind.UIThread)]
        protected bool SetPropertyRef<T>(ref T property, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(property, newValue)) return false;
            property = newValue;
            this.NotifyPropertyChanged(propertyName);
            return true;
        }

        [NotifyPropertyChangedInvocator, InvokeOn(ThreadKind.UIThread)]
        protected bool SetPropertyRef<T>(ref T property, T newValue, params string[] propertyNames)
        {
            if (EqualityComparer<T>.Default.Equals(property, newValue)) return false;
            property = newValue;
            this.NotifyPropertyChanged(propertyNames);
            return true;
        }

        #endregion

        #region api 2

        private RefreshPropertiesMapper _refreshPropertiesMapper;
        private PropertyChangedEventArgs[] _refreshProperties;

        /// <summary>
        /// Raise after invoke <see cref="RefreshProperties"/>.
        /// </summary>
        public event EventHandler PropertiesRefreshed;

        /// <summary>
        /// Invoke <see cref="INotifyPropertyChanged.PropertyChanged"/> for
        /// each property which contains <see cref="NotifyParentPropertyAttribute"/>.
        /// </summary>
        [InvokeOn(ThreadKind.UIThread)]
        public virtual void RefreshProperties()
        {
            if (this._refreshProperties == null)
            {
                this._refreshProperties = RefreshPropertiesMapper.MapNotifyPropertyChangedAttribute(this.GetType());
            }
            this.PropertyChanged?.Invoke(this, this._refreshProperties);
            this.PropertiesRefreshed?.Invoke(this, EventArgs.Empty);
        }

        public RefreshPropertiesMapper PropertiesMapper
        {
            get => this._refreshPropertiesMapper;
            set
            {
                this._refreshPropertiesMapper = value;
                if (value != null)
                {
                    this._refreshProperties = value.GetProperties(this);
                }
            }
        }

        #endregion

        #region api 3

        private List<PropertyChangedEventArgs> _endRefreshProperties;

        [InvokeOn(ThreadKind.AnyThread), ThreadSafety]
        public void MarkPropertyForEndRefresh<T>(Expression<Func<T, object>> propertySelector)
        {
            var propertyName = KeySelector.SelectProperty(propertySelector);
            this.MarkPropertyForEndRefresh(propertyName);
        }

        [InvokeOn(ThreadKind.AnyThread), ThreadSafety]
        public void MarkPropertyForEndRefresh([NotNull] string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            this.MarkPropertyForEndRefresh(new PropertyChangedEventArgs(propertyName));
        }

        [InvokeOn(ThreadKind.AnyThread), ThreadSafety]
        public void MarkPropertyForEndRefresh([NotNull] IEnumerable<string> propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            this.MarkPropertyForEndRefresh(propertyNames.Select(z => new PropertyChangedEventArgs(z)));
        }

        [InvokeOn(ThreadKind.AnyThread), ThreadSafety]
        public void MarkPropertyForEndRefresh([NotNull] PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));
            lock (this.SyncRoot)
            {
                if (this._endRefreshProperties == null) this._endRefreshProperties = new List<PropertyChangedEventArgs>();
                this._endRefreshProperties.Add(eventArgs);
            }
        }

        [InvokeOn(ThreadKind.AnyThread), ThreadSafety]
        public void MarkPropertyForEndRefresh([NotNull, ItemNotNull] IEnumerable<PropertyChangedEventArgs> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));
            var ea = eventArgs.ToArray();
            if (ea.AnyIsNull()) throw new ArgumentException("item in cannot be null.", nameof(eventArgs));
            lock (this.SyncRoot)
            {
                if (this._endRefreshProperties == null) this._endRefreshProperties = new List<PropertyChangedEventArgs>();
                this._endRefreshProperties.AddRange(ea);
            }
        }

        /// <summary>
        /// please always call this action on UI thread. the method will call PropertyChanged for each Registered property.
        /// </summary>
        [InvokeOn(ThreadKind.UIThread)]
        public void EndRefresh()
        {
            var properties = this._endRefreshProperties;
            if (properties == null) return;
            lock (this.SyncRoot)
            {
                properties = this._endRefreshProperties;
                this._endRefreshProperties = null;
            }
            if (properties.Count == 0) return;
            this.NotifyPropertyChanged(properties);
        }

        #endregion
    }
}