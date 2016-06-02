using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [Serializable]
    public class Orientation
    {
        [XmlAttribute("angle")]
        public float Angle { get; set; }
        [XmlAttribute("ax")]
        public float Ax { get; set; }
        [XmlAttribute("ay")]
        public float Ay { get; set; }
        [XmlAttribute("az")]
        public float Az { get; set; }
        [XmlAttribute("tx")]
        public float Tx { get; set; }
        [XmlAttribute("ty")]
        public float Ty { get; set; }
        [XmlAttribute("tz")]
        public float Tz { get; set; }
    }
}
