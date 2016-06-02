using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [Serializable]
    public class PhysicsAttributes
    {
        [XmlAttribute("inertiaTensor")]
        public string InertiaTensor { get; set; }

        [XmlAttribute("centerOfMass")]
        public string CenterOfMass { get; set; }

        [XmlAttribute("mass")]
        public float Mass { get; set; }

        [XmlAttribute("frictionType")]
        public int FrictionType { get; set; }
    }
}
