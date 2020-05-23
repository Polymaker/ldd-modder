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
        public string DecorationID { get; set; }

        public Decoration()
        {
            SurfaceID = 0;
            DecorationID = string.Empty;
        }

        public Decoration(int surfaceID, string decorationID)
        {
            SurfaceID = surfaceID;
            DecorationID = decorationID;
        }
    }
}
