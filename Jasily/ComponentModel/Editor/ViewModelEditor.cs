using System.Diagnostics;

namespace Jasily.ComponentModel.Editor
{
    internal class ViewModelEditor : BaseEditor
    {
        public ViewModelEditor(string name, EditableViewModelAttribute attribute)
            : base(name, attribute)
        {
        }

        public override void WriteToObject(object vm, object obj)
        {
            Debug.Assert(vm != null && obj != null);
            var value = this.ViewModelGetter(vm);
            if (value == null) return;
            Debug.Assert(value is IEditor);
            ((IEditor)value).WriteToObject(obj);
        }

        public override void ReadFromObject(object obj, object vm)
        {
            Debug.Assert(vm != null && obj != null);
            var value = this.ViewModelGetter(vm);
            if (value == null) return;
            Debug.Assert(value is IEditor);
            ((IEditor)value).ReadFromObject(obj);
        }
    }
}