using System;

namespace Jasily.ComponentModel.Editor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public abstract class EditableAttribute : Attribute
    {
        public string Name { get; set; }
    }
}