using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Jasily.ComponentModel.Editor.Conditions;

namespace Jasily.ComponentModel.Editor
{
    internal class PropertyMapper
    {
        /// <summary>
        /// <code>typeof(IEditor).GetTypeInfo()</code>
        /// </summary>
        private static readonly TypeInfo EditorInterfaceTypeInfo;
        /// <summary>
        /// <code>typeof(IPropertyContainer).GetTypeInfo()</code>
        /// </summary>
        private static readonly TypeInfo PropertyContainerInterfaceTypeInfo;

        static PropertyMapper()
        {
            EditorInterfaceTypeInfo = typeof(IEditor).GetTypeInfo();
            PropertyContainerInterfaceTypeInfo = typeof(IPropertyContainer).GetTypeInfo();
        }

        private Dictionary<string, BaseEditor> executors;

        protected Type ViewModelType { get; }

        protected Type ObjectType { get; }

        public PropertyMapper(Type viewModelType, Type objectType)
        {
            this.ViewModelType = viewModelType;
            this.ObjectType = objectType;

            this.MappingType();
        }

        private void MappingType()
        {
            if (this.executors == null)
            {
                var mapped = new Dictionary<string, BaseEditor>();

                // view model
                foreach (var field in this.ViewModelType.GetRuntimeFields())
                {
                    var attr = field.GetCustomAttribute<EditableAttribute>();
                    if (attr == null) continue;
                    var vmAttr = attr as EditableViewModelAttribute;
                    if (vmAttr != null)
                    {
                        if (!EditorInterfaceTypeInfo.IsAssignableFrom(field.FieldType.GetTypeInfo()))
                            throw new InvalidCastException();
                        var executor = new ViewModelEditor(field.Name, vmAttr)
                        {
                            ViewModelGetter = field.CompileGetter()
                        };
                        mapped.Add(executor);
                        continue;
                    }
                    var mAttr = attr as EditableMemberAttribute;
                    if (mAttr != null)
                    {
                        if (PropertyContainerInterfaceTypeInfo.IsAssignableFrom(field.FieldType.GetTypeInfo()))
                        {
                            var executor = new MemberEditor(field.Name, mAttr)
                            {
                                ViewModelGetter = field.CompileGetter()
                            };
                            executor.WriteConditions.AddRange(field.GetCustomAttributes<WriteToObjectConditionAttribute>());
                            executor.IsPropertyContainer = true;
                            mapped.Add(executor);
                        }
                        else
                        {
                            var executor = new MemberEditor(field.Name, mAttr)
                            {
                                ViewModelGetter = field.CompileGetter()
                            };
                            executor.WriteConditions.AddRange(field.GetCustomAttributes<WriteToObjectConditionAttribute>());
                            executor.ViewModelSetter = field.CompileSetter();
                            mapped.Add(executor);
                        }
                    }
                }

                foreach (var property in this.ViewModelType.GetRuntimeProperties())
                {
                    var attr = property.GetCustomAttribute<EditableAttribute>();
                    if (attr == null) continue;
                    var vmAttr = attr as EditableViewModelAttribute;
                    if (vmAttr != null)
                    {
                        if (!EditorInterfaceTypeInfo.IsAssignableFrom(property.PropertyType.GetTypeInfo()))
                            throw new InvalidCastException();
                        var executor = new ViewModelEditor(property.Name, vmAttr)
                        {
                            ViewModelGetter = property.CompileGetter()
                        };
                        mapped.Add(executor);
                        continue;
                    }
                    var mAttr = attr as EditableMemberAttribute;
                    if (mAttr != null)
                    {
                        if (PropertyContainerInterfaceTypeInfo.IsAssignableFrom(property.PropertyType.GetTypeInfo()))
                        {
                            var executor = new MemberEditor(property.Name, mAttr)
                            {
                                ViewModelGetter = property.CompileGetter()
                            };
                            executor.WriteConditions.AddRange(property.GetCustomAttributes<WriteToObjectConditionAttribute>());
                            executor.IsPropertyContainer = true;
                            mapped.Add(executor);
                        }
                        else
                        {
                            var executor = new MemberEditor(property.Name, mAttr)
                            {
                                ViewModelGetter = property.CompileGetter()
                            };
                            executor.WriteConditions.AddRange(property.GetCustomAttributes<WriteToObjectConditionAttribute>());
                            executor.ViewModelSetter = property.CompileSetter();
                            mapped.Add(executor);
                        }
                    }
                }

                // object
                foreach (var field in this.ObjectType.GetRuntimeFields())
                {
                    BaseEditor memberEditor;
                    if (mapped.TryGetValue(field.Name, out memberEditor))
                    {
                        var writer = memberEditor as MemberEditor;
                        if (writer != null)
                        {
                            writer.ObjectGetter = field.CompileGetter();
                            writer.ObjectSetter = field.CompileSetter();
                        }
                    }
                }

                foreach (var property in this.ObjectType.GetRuntimeProperties())
                {
                    BaseEditor memberEditor;
                    if (mapped.TryGetValue(property.Name, out memberEditor))
                    {
                        var writer = memberEditor as MemberEditor;
                        if (writer != null)
                        {
                            writer.ObjectGetter = property.CompileGetter();
                            writer.ObjectSetter = property.CompileSetter();
                        }
                    }
                }

                foreach (var kvp in mapped)
                {
                    kvp.Value.Verify();
                }
                this.executors = mapped;
            }
        }

        public void WriteToObject(object vm, object obj)
        {
            Debug.Assert(vm != null);
            Debug.Assert(obj != null);
            Debug.Assert(vm.GetType() == this.ViewModelType);
            Debug.Assert(obj.GetType() == this.ObjectType);

            foreach (var executor in this.executors.Values)
            {
                executor.WriteToObject(vm, obj);
            }
        }

        public void ReadFromObject(object obj, object vm)
        {
            Debug.Assert(vm != null);
            Debug.Assert(obj != null);
            Debug.Assert(vm.GetType() == this.ViewModelType);
            Debug.Assert(obj.GetType() == this.ObjectType);

            foreach (var executor in this.executors.Values)
            {
                executor.ReadFromObject(obj, vm);
            }
        }
    }
}