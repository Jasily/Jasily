using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.DependencyInjection.MemberInjection.Internal
{
    internal class DefaultInjectFieldsSelector<T> : IInjectFieldsSelector<T>
    {
        public IEnumerable<(FieldInfo, InjectAttribute)> SelectMembers()
        {
            return from field in typeof(T).GetRuntimeFields()
                let attr = CustomAttributeExtensions.GetCustomAttribute<InjectAttribute>((MemberInfo) field)
                where attr != null
                select (field, attr);
        }
    }
}