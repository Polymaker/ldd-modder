using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    public abstract class PaletteItem : ICloneable
    {
        internal static string[] AttributeOrder = new string[] { "ItemNos", "designID", "materialID", "quantity", "decorationID", "surfaceID" };

        [XmlAttribute("designID")]
        public int DesignID { get; set; }

        [XmlAttribute("quantity")]
        public int Quantity { get; set; }

        [XmlAttribute("ItemNos")]
        public string ElementID { get; set; }

        public PaletteItem()
        {
            DesignID = 0;
            Quantity = 0;
            ElementID = String.Empty;
        }

        public PaletteItem(int designID, int quantity, string elementID)
        {
            DesignID = designID;
            Quantity = quantity;
            ElementID = elementID;
        }

        public abstract object Clone();

        public PaletteItem Clone(int quantity)
        {
            var item = (PaletteItem)Clone();
            item.Quantity = quantity;
            return item;
        }
    }
}
