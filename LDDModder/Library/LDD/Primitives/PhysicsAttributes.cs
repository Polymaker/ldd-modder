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
        //TODO: change type for class/struct to represent 3x3 matrix (string value = 9 float separated by commas)
        [XmlAttribute("inertiaTensor")]
        public string InertiaTensor { get; set; }

        //TODO: change type for class/struct to represent 3D point (string value = 3 float separated by commas)
        [XmlAttribute("centerOfMass")]
        public string CenterOfMass { get; set; }

        [XmlAttribute("mass")]
        public float Mass { get; set; }

        [XmlAttribute("frictionType")]
        public int FrictionType { get; set; }
    }
}
