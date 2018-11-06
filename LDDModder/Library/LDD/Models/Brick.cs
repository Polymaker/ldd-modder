using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Models
{
    [XmlRoot("Brick")]
    public class Brick
    {
        [XmlAttribute("refID")]
        public string RefID { get; set; }

        [XmlAttribute("designID")]
        public string DesignID { get; set; }

        [XmlAttribute("uuid")]
        public string Uuid { get; set; }

        public Part Part { get; set; }
    }
}
