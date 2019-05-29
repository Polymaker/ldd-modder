using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    public class Camera
    {
        [XmlAttribute("lat")]
        public float Latitude { get; set; }

        [XmlAttribute("lon")]
        public float Longitude { get; set; }
    }
}
