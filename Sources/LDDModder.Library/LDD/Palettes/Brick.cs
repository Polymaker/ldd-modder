using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    [XmlRoot("Brick")]
    public class Brick : PaletteItem
    {
        [XmlAttribute("materialID")]
        public int MaterialID { get; set; }

        [XmlElement("SubMaterial")]
        public List<SubMaterial> SubMaterials { get; set; }

        [XmlElement("Decoration")]
        public List<Decoration> Decorations { get; set; }

        [XmlIgnore]
        public override bool HasDecorations => Decorations.Any();

        public Brick()
        {
            Decorations = new List<Decoration>();
            SubMaterials = new List<SubMaterial>();
        }

        public Brick(int designID, string elementID) : base(designID, elementID)
        {
            Decorations = new List<Decoration>();
            SubMaterials = new List<SubMaterial>();
        }

        public Brick(int designID, string elementID, int quantity) : base(designID, elementID, quantity)
        {
            Decorations = new List<Decoration>();
            SubMaterials = new List<SubMaterial>();
        }
    }
}
 