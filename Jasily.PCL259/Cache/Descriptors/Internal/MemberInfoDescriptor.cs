using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Cache.Descriptors.Internal
{
    /// <summary>
    /// NOTE: Attribute is alway return new instance, we should never cache it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MemberInfoDescriptor<T> : Descriptor<T> where T : MemberInfo
    {
        internal MemberInfoDescriptor([NotNull] T obj) : base(obj)
        {

        }
    }
}