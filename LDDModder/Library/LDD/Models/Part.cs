using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Models
{
    [XmlRoot("Part")]
    public class Part
    {
        [XmlAttribute("refID")]
        public int RefID { get; set; }

        [XmlAttribute("designID")]
        public int DesignID { get; set; }

        [XmlAttribute("materials")]
        public string Materials { get; set; }

        [XmlAttribute("decoration")]
        public string Decorations { get; set; }

        public Bone Bone { get; set; }
    }
}
