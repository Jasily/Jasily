using System.Collections.Generic;
using System.Reflection;

namespace Jasily.DependencyInjection.MemberInjection.Internal
{
    internal interface IInjectFieldsSelector<T>
    {
        IEnumerable<(FieldInfo, InjectAttribute)> SelectMembers();
    }
}