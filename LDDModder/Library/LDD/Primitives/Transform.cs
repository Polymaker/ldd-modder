using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace LDDModder.LDD.Primitives
{
    [Serializable]
    public class Transform
    {
        [XmlAttribute("angle")]
        public double Angle { get; set; }
        [XmlAttribute("ax")]
        public double Ax { get; set; }
        [XmlAttribute("ay")]
        public double Ay { get; set; }
        [XmlAttribute("az")]
        public double Az { get; set; }
        [XmlAttribute("tx")]
        public double Tx { get; set; }
        [XmlAttribute("ty")]
        public double Ty { get; set; }
        [XmlAttribute("tz")]
        public double Tz { get; set; }

        public Transform()
        {
            Angle = 0d;
            Ax = 0d;
            Ay = 0d;
            Az = 0d;
            Tx = 0d;
            Ty = 0d;
            Tz = 0d;
        }

        public Transform(double angle, double ax, double ay, double az, double tx, double ty, double tz)
        {
            Angle = angle;
            Ax = ax;
            Ay = ay;
            Az = az;
            Tx = tx;
            Ty = ty;
            Tz = tz;
        }
    }
}
