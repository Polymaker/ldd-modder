using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
namespace LDDModder.LDD.Palettes
{
    [Serializable]
    public class Part
    {
        [XmlAttribute("designID")]
        public int DesignID { get; set; }

        [XmlAttribute("materialID")]
        public int MaterialID { get; set; }

        [XmlElement("SubMaterial")]
        public List<SubMaterial> SubMaterials { get; set; }

        public Part()
        {
            DesignID = 0;
            MaterialID = 0;
            SubMaterials = new List<SubMaterial>();
        }

        protected Part(SerializationInfo info, StreamingContext context)
        {
            DesignID = info.GetInt32("DesignID");
            MaterialID = info.GetInt32("MaterialID");
            SubMaterials = (List<SubMaterial>)info.GetValue("SubMaterials", typeof(List<SubMaterial>));
        }
    }
}
