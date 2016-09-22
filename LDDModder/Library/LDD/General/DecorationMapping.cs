using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
namespace LDDModder.LDD.General
{
    [Serializable]
    public class DecorationMapping
    {
        [XmlAttribute("decorationID")]
        public int DecorationID { get; set; }

        [XmlAttribute("designID")]
        public int DesignID { get; set; }

        [XmlAttribute("surfaceID")]
        public int SurfaceID { get; set; }


        public DecorationMapping()
        {
            DecorationID = 0;
            DesignID = 0;
            SurfaceID = 0;
        }

        public DecorationMapping(int decorationID, int designID, int surfaceID)
        {
            DecorationID = decorationID;
            DesignID = designID;
            SurfaceID = surfaceID;
        }
    }
}
