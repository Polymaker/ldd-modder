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
        public double Sx { get; set; }
        [XmlAttribute("sY")]
        public double Sy { get; set; }
        [XmlAttribute("sZ")]
        public double Sz { get; set; }
    }
}
