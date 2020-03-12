using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class ForwardedEventArgs : EventArgs
    {
        public object ChildObject { get; }
        public string EventName { get; }
        public EventArgs ForwardedEvent { get; }

        public ForwardedEventArgs(object childObject, string childPropertyName, EventArgs forwardedEvent)
        {
            ChildObject = childObject;
            EventName = childPropertyName;
            ForwardedEvent = forwardedEvent;
        }
    }

    public delegate void ForwardedEventHandler(object sender, ForwardedEventArgs e);
}
