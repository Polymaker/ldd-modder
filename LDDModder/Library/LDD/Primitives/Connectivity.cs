using System;
using System.Xml;
using System.Xml.Serialization;


namespace LDDModder.LDD.Primitives
{
    [Serializable]
    public class Connectivity
    {
        [XmlAttribute("type")]
        public int Type { get; set; }
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

        /*
        Connectivity sub-types:

        Custom2DField (stud connection)
            Attributes = type, width, height, angle, ax, ay, az, tx, ty, tz
        Hinge
            Attributes = type, oriented, angle, ax, ay, az, tx, ty, tz, LimMin, LimMax, FlipLimMin, FlipLimMax, tag
        Axel
            Attributes = type, length, requireGrabbing, startCapped, endCapped, angle, ax, ay, az, tx, ty, tz, grabbing
        Fixed
            Attributes = type, axes, angle, ax, ay, az, tx, ty, tz, tag
        Gear
            Attributes = type, toothCount, radius, angle, ax, ay, az, tx, ty, tz
        Slider
            Attributes = type, length, startCapped, endCapped, angle, ax, ay, az, tx, ty, tz, cylindrical, spring, tag
        Ball
            Attributes = type, angle, ax, ay, az, tx, ty, tz
        Rail
            Attributes = type, angle, ax, ay, az, tx, ty, tz, length

        */
    }
}
