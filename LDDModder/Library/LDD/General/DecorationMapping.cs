using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.General
{
    [Serializable]
    public class DecorationMapping
    {
        [XmlAttribute("decorationID")]
        public int DecorationID { get; set; }

        [XmlAttribute("designID")]
        public int DesignID { get; set; }

        [XmlAttribute("surfaceID")]
        public int SurfaceID { get; set; }
    }
}
