using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class PropertyChangedEventArgs : EventArgs
    {
        public PartComponent Component { get; }

        public string PropertyName { get; }

        public object OldValue { get; }

        public object NewValue { get; }

        public PropertyChangedEventArgs(PartComponent component, string propertyName, object oldValue, object newValue)
        {
            Component = component;
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
