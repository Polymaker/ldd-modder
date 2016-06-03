using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    [Serializable, XmlRoot("Decoration")]
    public class Decoration
    {
        [XmlAttribute("surfaceID")]
        public int SurfaceID { get; set; }

        [XmlAttribute("decorationID")]
        public int DecorationID { get; set; }
    }
}
