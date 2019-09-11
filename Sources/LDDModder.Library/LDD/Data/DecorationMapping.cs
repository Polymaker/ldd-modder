using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDDModder.LDD.Data
{
    [XmlRoot("Mapping")]
    public class DecorationMapping
    {
        [XmlAttribute("decorationID")]
        public string DecorationID { get; set; }

        [XmlAttribute("designID")]
        public int DesignID { get; set; }

        [XmlAttribute("surfaceID")]
        public int SurfaceID { get; set; }
    }
}
