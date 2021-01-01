using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding
{
    public class ElementValueChangedEventArgs : EventArgs
    {
        public PartElement Element { get; }

        public string PropertyName { get; }

        public object OldValue { get; }

        public object NewValue { get; }

        public object ChildProperty { get; }

        public int[] Index { get; set; }

        public ElementValueChangedEventArgs(PartElement component, string propertyName, object oldValue, object newValue)
        {
            Element = component;
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public ElementValueChangedEventArgs(PartElement component, object childProperty, string propertyName, object oldValue, object newValue)
        {
            Element = component;
            ChildProperty = childProperty;
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
