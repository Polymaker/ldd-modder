using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
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
            : base()
        {
            MaterialID = 0;
            SubMaterials = new List<SubMaterial>();
            Decorations = new List<Decoration>();
        }

        public Brick(int designID, string elementID, int materialID, int quantity)
            : base(designID, quantity, elementID)
        {
            MaterialID = materialID;
            SubMaterials = new List<SubMaterial>();
            Decorations = new List<Decoration>();
        }

        public Brick(int designID, string elementID, int materialID, int quantity, IEnumerable<SubMaterial> subMaterials, IEnumerable<Decoration> decorations)
            : base(designID, quantity, elementID)
        {
            MaterialID = materialID;
            SubMaterials = new List<SubMaterial>(subMaterials);
            Decorations = new List<Decoration>(decorations);
        }

    }
}
