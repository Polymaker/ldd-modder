using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Custom2DField")]
    public class ConnectivityCustom2DField : Connectivity
    {
        internal static string[] AttributeOrder = new string[] { "type", "width", "height", "angle", "ax", "ay", "az", "tx", "ty", "tz" };

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        //TODO: change type for class to parse custom data
        [XmlText]
        public string ConnectivityData { get; set; }

        //public class ConnectionItem
        //{
        //    public int Type { get; set; }
        //    public int SubType { get; set; }

        //}
    }
}
