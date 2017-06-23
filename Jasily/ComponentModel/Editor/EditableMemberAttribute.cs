using System;

namespace Jasily.ComponentModel.Editor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EditableMemberAttribute : EditableAttribute
    {
        /// <summary>
        /// Converter should implemented ITwoWayConverter[Object.Member, ViewModel.Member].
        /// Whatever, IPropertyContainer will auto wrap and unwrap.
        /// </summary>
        public Type Converter { get; set; }
    }
}