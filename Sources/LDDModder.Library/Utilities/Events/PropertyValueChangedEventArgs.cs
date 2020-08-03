using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class PropertyValueChangedEventArgs : PropertyChangedEventArgs
    {
        public int? Index { get; set; }

        public object OldValue { get; }

        public object NewValue { get; }

        //public string BatchID { get; set; }

        public PropertyValueChangedEventArgs(string propertyName, object oldValue, object newValue) 
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public PropertyValueChangedEventArgs(string propertyName, int index, object oldValue, object newValue)
            : base(propertyName)
        {
            Index = index;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);
}
