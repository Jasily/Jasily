using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Jasily.ComponentModel.Editor.Conditions;
using Jasily.Interfaces;

namespace Jasily.ComponentModel.Editor
{
    internal class MemberEditor : BaseEditor
    {
        private ITwoWayConverter converter;

        public MemberEditor(string name, EditableMemberAttribute attribute)
            : base(name, attribute)
        {
        }

        public Action<object, object> ViewModelSetter { get; set; }

        public Func<object, object> ObjectGetter { get; set; }

        public Action<object, object> ObjectSetter { get; set; }

        public List<WriteToObjectConditionAttribute> WriteConditions { get; }
            = new List<WriteToObjectConditionAttribute>();

        public bool IsPropertyContainer { get; set; }

        public override void Verify()
        {
            base.Verify();

            if (this.ObjectGetter == null || this.ObjectSetter == null)
                throw new InvalidOperationException($"can not find property or field call [{this.Name}] on object.");

            if (this.IsPropertyContainer)
                Debug.Assert(this.ViewModelSetter == null);
            else
                Debug.Assert(this.ViewModelSetter != null);

            var attr =(EditableMemberAttribute) this.Attribute;
            if (attr.Converter != null)
            {
                var typeInfo = attr.Converter.GetTypeInfo();
                if (typeInfo.IsAbstract || typeInfo.IsInterface)
                    throw new InvalidOperationException($"{typeInfo.Name} is interface or abstract class.");

                if (!TwoWayConverterTypeInfo.IsAssignableFrom(typeInfo))
                    throw new InvalidCastException($"can not cast {attr.Converter} to {typeof(ITwoWayConverter)}");

                if (typeInfo.DeclaredConstructors
                    .FirstOrDefault(z => z.GetParameters().Length == 0) == null)
                    throw new InvalidOperationException($"{attr.Converter} has no none args constructor.");
            }
        }

        private ITwoWayConverter GetConverter(EditableMemberAttribute attr)
        {
            Debug.Assert(attr != null);
            Debug.Assert(attr.Converter != null);

            if (this.converter == null) this.converter = Activator.CreateInstance(attr.Converter) as ITwoWayConverter;
            Debug.Assert(this.converter != null);
            return this.converter;
        }

        public override void WriteToObject(object vm, object obj)
        {
            Debug.Assert(vm != null && obj != null);

            var value = this.ViewModelGetter(vm);

            // unwrap IPropertyContainer
            if (this.IsPropertyContainer)
            {
                value = (value as IPropertyContainer)?.Value;
            }

            // convert
            var attr = (EditableMemberAttribute)this.Attribute;
            if (attr.Converter != null)
            {
                var converter = this.GetConverter(attr);
                if (!converter.CanConvertBack(value)) return;
                value = converter.ConvertBack(value);
            }

            // check for write
            if (this.WriteConditions.Count > 0)
            {
                if (this.WriteConditions.Any(z => !z.IsMatch(value))) return;
            }

            // set
            this.ObjectSetter(obj, value);
        }

        public override void ReadFromObject(object obj, object vm)
        {
            Debug.Assert(vm != null && obj != null);

            var value = this.ObjectGetter(obj);

            // convert
            var attr = (EditableMemberAttribute)this.Attribute;
            if (attr.Converter != null)
            {
                var converter = this.GetConverter(attr);
                if (!converter.CanConvert(value)) return;
                value = converter.Convert(value);
            }

            if (this.IsPropertyContainer)
            {
                // wrap IPropertyContainer
                var container = this.ViewModelGetter(vm) as IPropertyContainer;
                if (container != null) container.Value = value;
            }
            else
            {
                this.ViewModelSetter(vm, value);
            }
        }
    }
}