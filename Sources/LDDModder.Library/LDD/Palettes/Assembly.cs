using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    [XmlRoot("Assembly")]
    public class Assembly : PaletteItem
    {
        [XmlElement("Part")]
        public List<Part> Parts { get; set; }

        [XmlIgnore]
        public override bool HasDecorations => Parts.Any(p => p.HasDecorations);

        [XmlRoot("Part")]
        public class Part
        {
            [XmlAttribute("designID")]
            public int DesignID { get; set; }

            [XmlAttribute("materialID")]
            public int MaterialID { get; set; }

            [XmlElement("SubMaterial")]
            public List<SubMaterial> SubMaterials { get; set; }

            [XmlElement("Decoration")]
            public List<Decoration> Decorations { get; set; }

            [XmlIgnore]
            public bool HasDecorations => Decorations.Any();

            public Part()
            {
                SubMaterials = new List<SubMaterial>();
                Decorations = new List<Decoration>();
            }

            public Part(int designID, int materialID)
            {
                DesignID = designID;
                MaterialID = materialID;
                SubMaterials = new List<SubMaterial>();
                Decorations = new List<Decoration>();
            }
        }

        public Assembly()
        {
            Parts = new List<Part>();
        }

        public Assembly(int designID, string elementID) : base(designID, elementID)
        {
            Parts = new List<Part>();
        }

        public Assembly(int designID, string elementID, int quantity) : base(designID, elementID, quantity)
        {
            Parts = new List<Part>();
        }
    }
}
