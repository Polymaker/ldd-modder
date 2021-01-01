using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class ForwardedEventArgs : EventArgs
    {
        public string PropertyName { get; }
        public object ChildObject { get; }
        public string EventName { get; }
        public EventArgs ForwardedEvent { get; }

        internal int Depth { get; set; }

        public ForwardedEventArgs(string propertyName, object childObject, string eventName, EventArgs forwardedEvent)
        {
            PropertyName = propertyName;
            ChildObject = childObject;
            EventName = eventName;
            ForwardedEvent = forwardedEvent;
        }

        //public ForwardedEventArgs(object childObject, string childPropertyName, EventArgs forwardedEvent)
        //{
        //    ChildObject = childObject;
        //    EventName = childPropertyName;
        //    ForwardedEvent = forwardedEvent;
        //}


    }

    public delegate void ForwardedEventHandler(object sender, ForwardedEventArgs e);
}
