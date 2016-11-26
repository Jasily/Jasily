using System;

namespace Jasily.ComponentModel.Editor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class EditableViewModelAttribute : EditableAttribute
    {
    }
}