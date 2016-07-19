using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Gear")]
    public class ConnectivityGear : Connectivity
    {
        internal static string[] AttributeOrder = new string[] { "type", "toothCount", "radius", "angle", "ax", "ay", "az", "tx", "ty", "tz" };

        [XmlAttribute("toothCount")]
        public int ToothCount { get; set; }

        [XmlAttribute("radius")]
        public double Radius { get; set; }
    }
}
