using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class ElementValueChangedEventArgs : EventArgs
    {
        public PartElement Element { get; }

        public string PropertyName { get; }

        public object OldValue { get; }

        public object NewValue { get; }

        public ElementValueChangedEventArgs(PartElement component, string propertyName, object oldValue, object newValue)
        {
            Element = component;
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
