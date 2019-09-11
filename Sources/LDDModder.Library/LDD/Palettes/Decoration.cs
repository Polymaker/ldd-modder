using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    [XmlRoot("Decoration")]
    public class Decoration
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
    }
}
