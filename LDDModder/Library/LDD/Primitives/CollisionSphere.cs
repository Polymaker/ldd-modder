using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Sphere")]
    public class CollisionSphere : Collision
    {
        [XmlAttribute("radius")]
        public float Radius { get; set; }
    }
}
