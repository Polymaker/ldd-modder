using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Models
{
    [XmlRoot("PartMap")]
    public class PartMap
    {
        [XmlAttribute("part")]
        public int Part { get; set; }

        [XmlAttribute("offset")]
        public string Offset { get; set; }

        [XmlAttribute("relativeTo")]
        public int RelativeTo { get; set; }

        [XmlAttribute("relativeToOffset")]
        public string RelativeToOffset { get; set; }

        [XmlAttribute("atIndex")]
        public int AtIndex { get; set; }
    }
}
