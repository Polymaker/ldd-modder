using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [Serializable]
    public class BoundingBox
    {
        [XmlAttribute("minX")]
        public double MinX { get; set; }
        [XmlAttribute("minY")]
        public double MinY { get; set; }
        [XmlAttribute("minZ")]
        public double MinZ { get; set; }
        [XmlAttribute("maxX")]
        public double MaxX { get; set; }
        [XmlAttribute("maxY")]
        public double MaxY { get; set; }
        [XmlAttribute("maxZ")]
        public double MaxZ { get; set; }

        [XmlIgnore]
        public Tuple<double, double, double> Center
        {
            get
            {
                return new Tuple<double, double, double>((MaxX + MinX) / 2d, (MaxY + MinY) / 2d, (MaxZ + MinZ) / 2d);
            }
        }
    }
}
