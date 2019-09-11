using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    public partial class Palette
    {
        [XmlRoot("Assembly")]
        public class Assembly : PaletteItem
        {
            [XmlElement("Part")]
            public List<Part> Parts { get; set; }

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
            }
        }
    }
    
}
