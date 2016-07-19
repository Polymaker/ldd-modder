using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Rail")]
    public class ConnectivityRail : Connectivity
    {
        internal static string[] AttributeOrder = new string[] { "type", "length", "angle", "ax", "ay", "az", "tx", "ty", "tz" };

        [XmlAttribute("length")]
        public double Length { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeLength()
        {
            return Length > 0;
        }
    }
}
