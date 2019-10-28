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

        private int _SubMaterialIndex;

        [XmlAttribute]
        public int SubMaterialIndex
        {
            get => _SubMaterialIndex;
            set => SetPropertyValue(ref _SubMaterialIndex, value);
        }

        [XmlArray("Components"), 
            XmlArrayItem(Type = typeof(PartModel), ElementName = "SurfaceModel"),
            XmlArrayItem(Type = typeof(BrickTubeModel), ElementName = "SurfaceTube"),
            XmlArrayItem(Type = typeof(MaleStudModel), ElementName = "SurfaceStud"),
            XmlArrayItem(Type = typeof(FemaleStudModel), ElementName = "SurfaceFemaleStud")]
        public ComponentCollection<SurfaceComponent> Components { get; set; }

        public PartSurface()
        {
            Components = new ComponentCollection<SurfaceComponent>(this);
        }

        public PartSurface(int surfaceID, int subMaterialIndex)
        {
            SurfaceID = surfaceID;
            SubMaterialIndex = subMaterialIndex;
            Components = new ComponentCollection<SurfaceComponent>(this);
        }

        public IEnumerable<ModelMesh> GetAllMeshes()
        {
            return Components.SelectMany(c => c.GetAllMeshes());
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(XmlHelper.ToXml(() => SurfaceID));
            elem.Add(XmlHelper.ToXml(() => SubMaterialIndex));

            if (Project != null)
                elem.Add(new XAttribute("OutputFile", GetTargetFilename()));

            //var componentsElem = elem.AddElement("Components");

            foreach (var comp in Components)
                elem.Add(comp.SerializeToXml());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            if (element.TryGetIntAttribute("SurfaceID", out int surfID))
                SurfaceID = surfID;

            if (element.TryGetIntAttribute("SubMaterialIndex", out int matIDX))
                SubMaterialIndex = matIDX;

            //if (element.Element("Components") != null)
            {
                foreach (var compElem in element/*.Element("Components")*/.Elements(SurfaceComponent.NODE_NAME))
                    Components.Add(SurfaceComponent.FromXml(compElem));
            }
        }

        public static PartSurface FromXml(XElement element)
        {
            var surface = new PartSurface();
            surface.LoadFromXml(element);
            return surface;
        }

        #endregion

        public string GetTargetFilename()
        {
            if (Project != null)
            {
                if (SurfaceID > 0)
                    return $"{Project.PartID}.g{SurfaceID}";
                return $"{Project.PartID}.g";
            }
            return string.Empty;
        }
    }
}
