using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    [Serializable]
    public class Brick : PaletteItem
    {
        [XmlAttribute("materialID")]
        public int MaterialID { get; set; }

        [XmlElement("SubMaterial")]
        public List<SubMaterial> SubMaterials { get; set; }

        [XmlElement("Decoration")]
        public List<Decoration> Decorations { get; set; }

        public Brick()
        {
            MaterialID = 0;
            SubMaterials = new List<SubMaterial>();
            Decorations = new List<Decoration>();
        }
    }
}
