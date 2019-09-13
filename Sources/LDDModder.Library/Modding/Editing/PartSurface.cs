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

        public IEnumerable<PartMesh> GetAllMeshes()
        {
            return Components.SelectMany(c => c.GetAllMeshes());
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase("Surface");
            elem.Add(new XAttribute("SurfaceID", SurfaceID));
            elem.Add(new XAttribute("SubMaterialIndex", SubMaterialIndex));
            var componentsElem = elem.AddElement("Components");
            foreach (var comp in Components)
                componentsElem.Add(comp.SerializeToXml());
            return elem;
        }

        public static PartSurface FromXml(XElement element)
        {

            return null;
        }

        #endregion
    }
}
