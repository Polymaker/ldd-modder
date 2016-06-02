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
        public float MinX { get; set; }
        [XmlAttribute("minY")]
        public float MinY { get; set; }
        [XmlAttribute("minZ")]
        public float MinZ { get; set; }
        [XmlAttribute("maxX")]
        public float MaxX { get; set; }
        [XmlAttribute("maxY")]
        public float MaxY { get; set; }
        [XmlAttribute("maxZ")]
        public float MaxZ { get; set; }

        [XmlIgnore]
        public Tuple<float,float,float> Center
        {
            get
            {
                return new Tuple<float, float, float>((MaxX + MinX) / 2f, (MaxY + MinY) / 2f, (MaxZ + MinZ) / 2f);
            }
        }
    }
}
