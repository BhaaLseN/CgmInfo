using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CgmInfoGui.Converters
{
    // resolves collections into actual expandable items. without this, only public members of the class would be visible
    // (such as List<T>.Count and List<T>.Capacity instead of its items)
    class ExpandableIListConverter<T> : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            if (value is IEnumerable<T> values)
                return GenerateItemsAsProperties(values);

            return base.GetProperties(context, value, attributes);
        }
        private PropertyDescriptorCollection GenerateItemsAsProperties(IEnumerable<T> values)
        {
            return new PropertyDescriptorCollection(values.Select((item, index) => new ListItemPropertyDescriptor(values, index)).ToArray(), readOnly: true);
        }

        private sealed class ListItemPropertyDescriptor : PropertyDescriptor
        {
            private readonly IEnumerable<T> _owner;
            private readonly int _index;

            public ListItemPropertyDescriptor(IEnumerable<T> owner, int index)
                : base($"[{index}]", null)
            {
                _owner = owner;
                _index = index;
            }

            public override Type ComponentType => _owner.GetType();
            public override bool IsReadOnly => true;
            public override Type PropertyType => typeof(T);

            public override bool CanResetValue(object component) => false;
            public override object GetValue(object component) => _owner.ElementAt(_index);
            public override void ResetValue(object component) => throw new NotSupportedException();
            public override void SetValue(object component, object value) => throw new NotSupportedException();
            public override bool ShouldSerializeValue(object component) => false;
        }
    }
}
