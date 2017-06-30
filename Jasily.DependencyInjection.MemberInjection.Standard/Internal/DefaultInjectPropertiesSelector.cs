using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.DependencyInjection.MemberInjection.Internal
{
    internal class DefaultInjectPropertiesSelector<T> : IInjectPropertiesSelector<T>
    {
        public IEnumerable<(PropertyInfo, InjectAttribute)> SelectMembers()
        {
            return from property in typeof(T).GetRuntimeProperties()
                let attr = CustomAttributeExtensions.GetCustomAttribute<InjectAttribute>((MemberInfo) property)
                where attr != null
                select (property, attr);
        }
    }
}