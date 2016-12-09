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
        private Attribute[] notInheritAttributes;

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
                return this.notInheritAttributes ?? (this.notInheritAttributes =
                    this.DescriptedObject.GetCustomAttributes<Attribute>(false).ToArray());
            }
        }

        public bool HasCustomAttribute<TA>() where TA : Attribute => this.CustomAttributes().OfType<TA>().Any();
    }
}