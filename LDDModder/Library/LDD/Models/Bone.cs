using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Models
{
    [XmlRoot("Bone")]
    public class Bone
    {
        [XmlAttribute("refID")]
        public string RefID { get; set; }

        [XmlAttribute("transformation")]
        public string Transformation { get; set; }

        [XmlAttribute("freeze")]
        public string Freeze { get; set; }

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
    }
}
