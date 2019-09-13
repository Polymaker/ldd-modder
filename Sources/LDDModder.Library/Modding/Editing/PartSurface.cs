using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class PartSurface : PartComponent
    {
        public int SurfaceID { get; set; }

        public int SubMaterialIndex { get; set; }

        public List<SurfaceComponent> Components { get; set; }

        public PartSurface()
        {
            Components = new List<SurfaceComponent>();
        }

        public PartSurface(int surfaceID, int subMaterialIndex)
        {
            SurfaceID = surfaceID;
            SubMaterialIndex = subMaterialIndex;
            Components = new List<SurfaceComponent>();
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase("Surface");
            elem.Add(new XAttribute("SurfaceID", SurfaceID));
            elem.Add(new XAttribute("SubMaterialIndex", SubMaterialIndex));
            return elem;
        }
    }
}
