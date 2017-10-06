using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Jasily.Extensions.System.Collections.Generic;
using JetBrains.Annotations;

namespace Jasily.Reflection.Descriptors
{
    /// <summary>
    /// <remarks>
    ///     Since <see cref="CustomAttributeExtensions.GetCustomAttributes(MemberInfo)"/> always return new instance,
    ///     if you need new <see cref="Attribute"/>,
    ///     try create the a new <see cref="Descriptor{T}"/>.
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MemberInfoDescriptor<T> : Descriptor<T> where T : MemberInfo
    {
        private IReadOnlyList<Attribute> _inheritedAttributes;
        private IReadOnlyList<Attribute> _declaredAttributes;

        internal MemberInfoDescriptor([NotNull] T obj) : base(obj)
        {

        }

        private IReadOnlyList<Attribute> GetCustomAttributes(bool inherit)
        {
            if (inherit)
            {
                if (this._inheritedAttributes == null)
                {
                    var attrs = this.DescriptedObject.GetCustomAttributes<Attribute>(true).ToArray().AsReadOnly();
                    Interlocked.CompareExchange(ref this._inheritedAttributes, attrs, null);
                }

                return this._inheritedAttributes;
            }
            else
            {
                if (this._declaredAttributes == null)
                {
                    var attrs = this.DescriptedObject.GetCustomAttributes<Attribute>(false).ToArray().AsReadOnly();
                    Interlocked.CompareExchange(ref this._declaredAttributes, attrs, null);
                }

                return this._declaredAttributes;
            }
        }

        public IReadOnlyList<Attribute> DeclaredAttributes => this.GetCustomAttributes(false);

        public IReadOnlyList<Attribute> InheritedAttributes => this.GetCustomAttributes(true);

        public bool HasCustomAttribute<TAttribute>(bool inherit = true) where TAttribute : Attribute
            => this.GetCustomAttributes(inherit).OfType<TAttribute>().Any();
    }
}