using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Ball")]
    public class ConnectivityBall : Connectivity
    {
        internal static string[] AttributeOrder = new string[] { "type", "flexAttributes", "angle", "ax", "ay", "az", "tx", "ty", "tz" };

        /// <summary>
        /// Only used when the node is a descendant of Flex
        /// </summary>
        [XmlAttribute("flexAttributes")]
        public string FlexAttributes { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeFlexAttributes()
        {
            return !string.IsNullOrEmpty(FlexAttributes);
        }
    }
}
