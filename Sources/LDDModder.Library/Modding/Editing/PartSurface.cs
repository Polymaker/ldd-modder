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
    public class PartSurface : PartElement
    {
        public const string NODE_NAME = "Surface";

        public int SurfaceID { get; set; }

        private int _SubMaterialIndex;

        public int SubMaterialIndex
        {
            get => _SubMaterialIndex;
            set => SetPropertyValue(ref _SubMaterialIndex, value);
        }

        public ElementCollection<SurfaceComponent> Components { get; set; }

        public PartSurface()
        {
            Components = new ElementCollection<SurfaceComponent>(this);
        }

        public PartSurface(int surfaceID, int subMaterialIndex)
        {
            SurfaceID = surfaceID;
            SubMaterialIndex = subMaterialIndex;
            Components = new ElementCollection<SurfaceComponent>(this);
        }

        public IEnumerable<ModelMesh> GetAllMeshes()
        {
            return Components.SelectMany(c => c.GetAllMeshes());
        }

        protected override IEnumerable<PartElement> GetAllChilds()
        {
            return Components;
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.RemoveAttributes();

            elem.Add(new XAttribute("SurfaceID", SurfaceID));
            elem.Add(new XAttribute("SubMaterialIndex", SubMaterialIndex));

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

            Name = $"Surface{SurfaceID}";

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
