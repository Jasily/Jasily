using System.Collections.Generic;
using System.Reflection;

namespace Jasily.DependencyInjection.MemberInjection.Internal
{
    internal interface IInjectPropertiesSelector<T>
    {
        IEnumerable<(PropertyInfo, InjectAttribute)> SelectMembers();
    }
}