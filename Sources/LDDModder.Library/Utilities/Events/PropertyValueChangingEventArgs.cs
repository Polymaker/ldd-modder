using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class PropertyValueChangingEventArgs : PropertyChangingEventArgs
    {
        public int? Index { get; set; }

        public object OldValue { get; }

        public object NewValue { get; }

        public PropertyValueChangingEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public PropertyValueChangingEventArgs(string propertyName, int index, object oldValue, object newValue)
            : base(propertyName)
        {
            Index = index;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public delegate void PropertyValueChangingEventHandler(object sender, PropertyValueChangingEventArgs e);
}
