using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace LDDModder.LDD.Palettes
{
    [Serializable, XmlRoot("Decoration")]
    public class Decoration : ICloneable
    {
        [XmlAttribute("surfaceID")]
        public int SurfaceID { get; set; }

        [XmlAttribute("decorationID")]
        public int DecorationID { get; set; }

        public Decoration()
        {
            SurfaceID = 0;
            DecorationID = 0;
        }

        public Decoration(int surfaceID, int decorationID)
        {
            SurfaceID = surfaceID;
            DecorationID = decorationID;
        }

        public object Clone()
        {
            return MemberwiseClone();
            //return new Decoration(SurfaceID, DecorationID);
        }
    }
}
