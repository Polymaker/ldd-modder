using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    [Serializable]
    public class Brick
    {
        [XmlAttribute("designID")]
        public int DesignID { get; set; }

        [XmlAttribute("materialID")]
        public int MaterialID { get; set; }

        [XmlAttribute("quantity")]
        public int Quantity { get; set; }

        [XmlAttribute("ItemNos")]
        public int ElementID { get; set; }
    }
}
