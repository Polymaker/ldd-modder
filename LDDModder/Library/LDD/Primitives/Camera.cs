using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [Serializable]
    public class Camera
    {
        [XmlAttribute("lat")]
        public float Latitude { get; set; }

        [XmlAttribute("lon")]
        public float Longitude { get; set; }

    }
}
