using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Box")]
    public class CollisionBox : Collision
    {
        [XmlAttribute("sX")]
        public float Sx { get; set; }
        [XmlAttribute("sY")]
        public float Sy { get; set; }
        [XmlAttribute("sZ")]
        public float Sz { get; set; }
    }
}
