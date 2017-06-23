using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    public class RefreshPropertiesMapper
    {
        private readonly Type _type;
        private readonly PropertyChangedEventArgs[] _properties;

        private RefreshPropertiesMapper([NotNull] Type type)
        {
            this._type = type;
            this._properties = MapNotifyPropertyChangedAttribute(type);
        }

        internal PropertyChangedEventArgs[] GetProperties(NotifyPropertyChangedObject obj)
        {
            if (obj.GetType() != this._type) throw new InvalidOperationException();

            return this._properties;
        }

        internal static PropertyChangedEventArgs[] MapNotifyPropertyChangedAttribute(Type type)
        {
            return (
                from property in type.GetRuntimeProperties()
                let attr = property.GetCustomAttribute<NotifyPropertyChangedAttribute>()
                where attr != null
                orderby attr.Order
                select new PropertyChangedEventArgs(property.Name)
                ).ToArray();
        }

        public static RefreshPropertiesMapper OfType<T>() => InstanceStore<T>.Instance;

        private static class InstanceStore<T>
        {
            public static RefreshPropertiesMapper Instance { get; } = new RefreshPropertiesMapper(typeof(T));
        }
    }
}