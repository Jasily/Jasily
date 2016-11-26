using System;
using System.Collections.Generic;

namespace Jasily.ComponentModel.Editor
{
    public class ObjectEditorViewModel<T> : ViewModel, IEditor
    {
        private PropertyMapper mapper;

        [NotifyPropertyChanged(Order = -1)]
        public T ReadCached { get; private set; }

        public virtual void WriteToObject(T obj)
        {
            if (ReferenceEquals(obj, null)) return;
            if (this.mapper == null)
                this.mapper = MapperCache.GetMapperCache(this.GetType());
            this.mapper.WriteToObject(this, obj);
        }

        public virtual void ReadFromObject(T obj)
        {
            if (ReferenceEquals(obj, null)) return;
            if (this.mapper == null)
                this.mapper = MapperCache.GetMapperCache(this.GetType());
            this.ReadCached = obj;
            this.mapper.ReadFromObject(obj, this);
        }

        void IEditor.WriteToObject(object obj) => this.WriteToObject((T)obj);

        void IEditor.ReadFromObject(object obj) => this.ReadFromObject((T)obj);

        internal class MapperCache
        {
            // ReSharper disable once StaticMemberInGenericType
            private static readonly Dictionary<Type, PropertyMapper> Cached = new Dictionary<Type, PropertyMapper>();

            public static PropertyMapper GetMapperCache(Type viewModelType)
            {
                lock (Cached)
                {
                    var ret = Cached.GetValueOrDefault(viewModelType);
                    if (ret != null) return ret;
                }

                var @new = new PropertyMapper(viewModelType, typeof(T));

                lock (Cached)
                {
                    return Cached.GetValueOrAdd(viewModelType, @new);
                }
            }
        }
    }
}