using System;
using System.Linq;
using System.Reflection;
using Jasily.Interfaces;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    public class RefreshPropertiesMapper
    {
        private readonly Type type;
        private readonly string[] properties;

        private RefreshPropertiesMapper([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            this.type = type;
            this.properties = MapNotifyPropertyChangedAttribute(type);
        }

        internal string[] GetProperties(NotifyPropertyChangedObject obj)
        {
            if (obj.GetType() != this.type) throw new InvalidOperationException();

            return this.properties;
        }

        internal static string[] MapNotifyPropertyChangedAttribute(Type type)
        {
            return (
                from property in type.GetRuntimeProperties()
                let attr = property.GetCustomAttribute<NotifyPropertyChangedAttribute>()
                where attr != null
                orderby attr.AsOrderable().GetOrderCode()
                select property.Name
                ).ToArray();
        }

        public static RefreshPropertiesMapper OfType<T>() => InstanceStore<T>.Instance;

        private static class InstanceStore<T>
        {
            public static RefreshPropertiesMapper Instance { get; } = new RefreshPropertiesMapper(typeof(T));
        }
    }
}