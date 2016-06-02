using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.General
{
    [Serializable]
    public class Material
    {
        [XmlAttribute("MatID")]
        public string ID { get; set; }

        [XmlAttribute("Red")]
        public int R { get; set; }

        [XmlAttribute("Green")]
        public int G { get; set; }

        [XmlAttribute("Blue")]
        public int B { get; set; }

        [XmlAttribute("Alpha")]
        public int A { get; set; }

        [XmlAttribute]
        public MaterialType MaterialType { get; set; }

        //[XmlIgnore]
        //public Color Color
        //{
        //    get
        //    {
        //        return Color.FromArgb(A, R, G, B);
        //    }
        //    set
        //    {
        //        A = value.A;
        //        R = value.R;
        //        G = value.G;
        //        B = value.B;
        //    }
        //}
    }
}
