using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    public class PartSurface : PartComponent
    {
        public const string NODE_NAME = "Surface";

        [XmlAttribute]
        public int SurfaceID { get; set; }

        [XmlAttribute]
        public int SubMaterialIndex { get; set; }

        [XmlArray("Components")]
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
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(XmlHelper.ToXml(() => SurfaceID));
            elem.Add(XmlHelper.ToXml(() => SubMaterialIndex));
            var componentsElem = elem.AddElement("Components");

            foreach (var comp in Components)
                componentsElem.Add(comp.SerializeToXml());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            if (element.TryGetIntAttribute("SurfaceID", out int surfID))
                SurfaceID = surfID;

            if (element.TryGetIntAttribute("SubMaterialIndex", out int matIDX))
                SubMaterialIndex = matIDX;

        }

        public static PartSurface FromXml(XElement element)
        {
            var surface = new PartSurface();
            surface.LoadFromXml(element);

            if (element.Element("Components") != null)
            {
                foreach (var compElem in element.Element("Components").Elements(SurfaceComponent.NODE_NAME))
                    surface.Components.Add(SurfaceComponent.FromXml(compElem));
            }

            return surface;
        }

        #endregion
    }
}
