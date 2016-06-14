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

        public override bool HasDecorations
        {
            get { return Decorations.Count > 0; }
        }

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

        public override object Clone()
        {
            return new Brick(DesignID, ElementID, MaterialID, Quantity, SubMaterials.Clone(), Decorations.Clone());
        }

        public Brick Clone(string elementID = null, int? materialID = null, int? quantity = null)
        {
            var newBrick = (Brick)((ICloneable)this).Clone();

            if (!string.IsNullOrEmpty(elementID))
                newBrick.ElementID = elementID;

            if (materialID.HasValue)
                newBrick.MaterialID = materialID.Value;

            if (quantity.HasValue)
                newBrick.Quantity = quantity.Value;

            return newBrick;
        }
    }
}
