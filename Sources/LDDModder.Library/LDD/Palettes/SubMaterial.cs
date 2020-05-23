using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    [XmlRoot("SubMaterial")]
    public class SubMaterial
    {
        [XmlAttribute("surfaceID")]
        public int SurfaceID { get; set; }

        [XmlAttribute("materialID")]
        public int MaterialID { get; set; }

        public SubMaterial()
        {
            SurfaceID = 0;
            MaterialID = 0;
        }

        public SubMaterial(int surfaceID, int materialID)
        {
            SurfaceID = surfaceID;
            MaterialID = materialID;
        }

    }
}
