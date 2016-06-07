using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Axel")]
    public class ConnectivityAxel : Connectivity
    {
        internal static string[] AttributeOrder = new string[] { "type", "length", "grabbing", "requireGrabbing", "startCapped", "endCapped", "angle", "ax", "ay", "az", "tx", "ty", "tz" };

        [XmlAttribute("length")]
        public float Length { get; set; }
        [XmlIgnore]
        public bool RequireGrabbing { get; set; }
        [XmlIgnore]
        public bool Grabbing { get; set; }
        [XmlIgnore]
        public bool StartCapped { get; set; }
        [XmlIgnore]
        public bool EndCapped { get; set; }

        #region int (0,1) <-> bool trick properties

        [XmlAttribute("requireGrabbing")]
        [EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int RequireGrabbingInt
        {
            get { return Convert.ToInt32(Grabbing); }
            set { Grabbing = Convert.ToBoolean(value); }
        }

        [XmlAttribute("grabbing")]
        [EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int GrabbingInt
        {
            get { return Convert.ToInt32(Grabbing); }
            set { Grabbing = Convert.ToBoolean(value); }
        }

        [XmlAttribute("startCapped")]
        [EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int StartCappedInt
        {
            get { return Convert.ToInt32(StartCapped); }
            set { StartCapped = Convert.ToBoolean(value); }
        }

        [XmlAttribute("endCapped")]
        [EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int EndCappedInt
        {
            get { return Convert.ToInt32(EndCapped); }
            set { EndCapped = Convert.ToBoolean(value); }
        }

        #endregion

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeRequireGrabbingInt()
        {
            return Grabbing;
        }

    }
}
