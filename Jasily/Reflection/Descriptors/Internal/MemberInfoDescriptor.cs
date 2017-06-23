using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Jasily.Reflection.Descriptors.Internal
{
    /// <summary>
    /// NOTE: Attribute is alway return new instance, we should never return it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MemberInfoDescriptor<T> : Descriptor<T> where T : MemberInfo
    {
        private Attribute[] inheritAttributes;
        private Attribute[] declaredAttributes;

        internal MemberInfoDescriptor([NotNull] T obj) : base(obj)
        {

        }

        private IEnumerable<Attribute> CustomAttributes(bool inherit = true)
        {
            if (inherit)
            {
                return this.inheritAttributes ?? (this.inheritAttributes = 
                    this.DescriptedObject.GetCustomAttributes<Attribute>(true).ToArray());
            }
            else
            {
                return this.declaredAttributes ?? (this.declaredAttributes =
                    this.DescriptedObject.GetCustomAttributes<Attribute>(false).ToArray());
            }
        }

        public bool HasCustomAttribute<TA>(bool inherit = true) where TA : Attribute
            => this.CustomAttributes(inherit).OfType<TA>().Any();
    }
}