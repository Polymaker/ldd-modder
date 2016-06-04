using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace LDDModder.LDD.Palettes
{
    [Serializable]
    public class Part : ICloneable
    {
        [XmlAttribute("designID")]
        public int DesignID { get; set; }

        [XmlAttribute("materialID")]
        public int MaterialID { get; set; }

        [XmlElement("SubMaterial")]
        public List<SubMaterial> SubMaterials { get; set; }

        [XmlElement("Decoration")]
        public List<Decoration> Decorations { get; set; }

        public Part()
        {
            DesignID = 0;
            MaterialID = 0;
            SubMaterials = new List<SubMaterial>();
            Decorations = new List<Decoration>();
        }

        public Part(int designID, int materialID, IEnumerable<SubMaterial> subMaterials, IEnumerable<Decoration> decorations)
        {
            DesignID = designID;
            MaterialID = materialID;
            SubMaterials = new List<SubMaterial>(subMaterials);
            Decorations = new List<Decoration>(decorations);
        }

        public object Clone()
        {
            return new Part(DesignID, MaterialID, SubMaterials.Clone(), Decorations.Clone());
        }
    }
}
