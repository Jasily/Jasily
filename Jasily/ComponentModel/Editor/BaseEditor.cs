using System;
using System.Diagnostics;
using System.Reflection;
using Jasily.Core;

namespace Jasily.ComponentModel.Editor
{
    internal abstract class BaseEditor : IGetKey<string>
    {
        protected static readonly TypeInfo TwoWayConverterTypeInfo = typeof(ITwoWayConverter).GetTypeInfo();

        protected BaseEditor(string name, EditableAttribute attribute)
        {
            Debug.Assert(name != null);
            Debug.Assert(attribute != null);
            this.Name = string.IsNullOrWhiteSpace(attribute.Name) ? name : attribute.Name;
            this.Attribute = attribute;
        }

        protected string Name { get; }

        protected EditableAttribute Attribute { get; }

        public Func<object, object> ViewModelGetter { get; set; }

        public virtual void Verify()
        {
            Debug.Assert(this.ViewModelGetter != null);
        }

        public abstract void WriteToObject(object vm, object obj);

        public abstract void ReadFromObject(object obj, object vm);

        public string GetKey() => this.Name;
    }
}