using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    [Serializable]
    public class SubMaterial : ICloneable
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

        public object Clone()
        {
            return MemberwiseClone();
            //return new SubMaterial(SurfaceID, MaterialID);
        }
    }
}
