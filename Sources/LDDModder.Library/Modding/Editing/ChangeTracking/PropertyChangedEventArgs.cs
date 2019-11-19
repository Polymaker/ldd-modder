using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class PropertyChangedEventArgs : EventArgs
    {
        public PartElement Element { get; }

        public string PropertyName { get; }

        public object OldValue { get; }

        public object NewValue { get; }

        public PropertyChangedEventArgs(PartElement component, string propertyName, object oldValue, object newValue)
        {
            Element = component;
            PropertyName = propertyName;

            //if (oldValue is ItemTransform t1)
            //    oldValue = t1.Clone();
            //if (newValue is ItemTransform t2)
            //    newValue = t2.Clone();

            OldValue = oldValue;
            NewValue = newValue;

            
        }
    }
}
