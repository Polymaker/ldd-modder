using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    public abstract class PaletteItem
    {
        [XmlAttribute("designID")]
        public int DesignID { get; set; }

        [XmlAttribute("quantity")]
        public int Quantity { get; set; }

        [XmlAttribute("ItemNos")]
        public string ElementID { get; set; }
    }
}
