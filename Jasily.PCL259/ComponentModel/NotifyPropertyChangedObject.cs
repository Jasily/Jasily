using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Jasily.Extensions.System.ComponentModel;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    public class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        protected readonly object SyncRoot = new object();

        #region api 0

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged<T>([NotNull] Expression<Func<T, object>> propertySelector)
        {
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));
            var propertyName = KeySelector.SelectProperty(propertySelector);
            this.PropertyChanged?.Invoke(this, propertyName);
        }

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged([NotNull] string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            this.PropertyChanged?.Invoke(this, propertyName);
        }

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged([NotNull] params string[] propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            this.PropertyChanged?.Invoke(this, propertyNames);
        }

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged([NotNull] IEnumerable<string> propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            this.PropertyChanged?.Invoke(this, propertyNames);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ClearPropertyChangedInvocationList()
        {
            this.PropertyChanged = null;
        }

        #endregion

        #region api 1

        [NotifyPropertyChangedInvocator]
        protected bool SetPropertyRef<T>(ref T property, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(property, newValue)) return false;
            property = newValue;
            this.NotifyPropertyChanged(propertyName);
            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected bool SetPropertyRef<T>(ref T property, T newValue, params string[] propertyNames)
        {
            if (EqualityComparer<T>.Default.Equals(property, newValue)) return false;
            property = newValue;
            this.NotifyPropertyChanged(propertyNames);
            return true;
        }

        #endregion

        #region api 2

        private RefreshPropertiesMapper refreshPropertiesMapper;
        private PropertyChangedEventArgs[] refreshProperties;

        /// <summary>
        /// the method will call PropertyChanged for each property which has [NotifyPropertyChanged]
        /// </summary>
        public virtual void RefreshProperties()
        {
            var properties = this.refreshProperties;
            if (properties == null)
            {
                this.refreshProperties = properties =
                    RefreshPropertiesMapper.MapNotifyPropertyChangedAttribute(this.GetType());
            }
            this.PropertyChanged?.Invoke(this, properties);
            this.PropertiesRefreshed?.Invoke(this, EventArgs.Empty);
        }

        public RefreshPropertiesMapper PropertiesMapper
        {
            get { return this.refreshPropertiesMapper; }
            set
            {
                if (this.refreshPropertiesMapper == value) return;
                if (value != null)
                {
                    this.refreshProperties = value.GetProperties(this);
                }
                this.refreshPropertiesMapper = value;
            }
        }

        public event EventHandler PropertiesRefreshed;

        #endregion

        #region api 3

        private List<string> endRefreshProperties;

        public void MarkPropertyForEndRefresh<T>(Expression<Func<T, object>> propertySelector)
        {
            var propertyName = KeySelector.SelectProperty(propertySelector);
            this.MarkPropertyForEndRefresh(propertyName);
        }

        public void MarkPropertyForEndRefresh([NotNull] string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            lock (this.SyncRoot)
            {
                if (this.endRefreshProperties == null) this.endRefreshProperties = new List<string>();
                this.endRefreshProperties.Add(propertyName);
            }
        }

        public void MarkPropertyForEndRefresh([NotNull] IEnumerable<string> propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            lock (this.SyncRoot)
            {
                if (this.endRefreshProperties == null) this.endRefreshProperties = new List<string>();
                this.endRefreshProperties.AddRange(propertyNames);
            }
        }

        /// <summary>
        /// please always call this action on UI thread. the method will call PropertyChanged for each Registered property.
        /// </summary>
        public void EndRefresh()
        {
            var properties = this.endRefreshProperties;
            if (properties == null) return;
            lock (this.SyncRoot)
            {
                properties = this.endRefreshProperties;
                this.endRefreshProperties = null;
            }
            if (properties.Count == 0) return;
            this.NotifyPropertyChanged(properties);
        }

        #endregion
    }
}