using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Fixed")]
    public class ConnectivityFixed : Connectivity
    {
        internal static string[] AttributeOrder = new string[] { "type", "axes", "tag", "angle", "ax", "ay", "az", "tx", "ty", "tz" };

        [XmlAttribute("axes")]
        public int Axes { get; set; }

        [XmlAttribute("tag")]
        public string Tag { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeAxes()
        {
            return Axes > 0;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeTag()
        {
            return !string.IsNullOrEmpty(Tag);
        }
    }
}
