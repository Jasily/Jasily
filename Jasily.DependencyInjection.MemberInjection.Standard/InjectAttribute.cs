using System;

namespace Jasily.DependencyInjection.MemberInjection
{
    /// <summary>
    /// attribute work for auto inject.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="isRequired"></param>
        public InjectAttribute(bool isRequired = true)
        {
            this.IsRequired = isRequired;
        }

        /// <summary>
        /// whether the member is required.
        /// </summary>
        public bool IsRequired { get; }
    }
}
