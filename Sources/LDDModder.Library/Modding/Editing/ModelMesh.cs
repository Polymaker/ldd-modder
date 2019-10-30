using LDDModder.LDD.Meshes;
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
    public class ModelMesh : PartElement
    {
        public const string NODE_NAME = "Mesh";

        [XmlIgnore]
        public MeshGeometry Geometry { get; set; }

        [XmlAttribute]
        public bool IsTextured { get; set; }

        [XmlAttribute]
        public bool IsFlexible { get; set; }

        [XmlAttribute]
        public string FileName { get; set; }

        public PartSurface Surface => (Parent as SurfaceComponent)?.Parent as PartSurface;

        public bool IsModelLoaded => Geometry != null;

        public ModelMesh()
        {
            ID = Utilities.StringUtils.GenerateUID(8);
        }

        public ModelMesh(MeshGeometry geometry)
        {
            Geometry = geometry;
            IsTextured = geometry.IsTextured;
            IsFlexible = geometry.IsFlexible;
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(XmlHelper.ToXml(() => IsTextured));
            elem.Add(XmlHelper.ToXml(() => IsFlexible));

            if (!string.IsNullOrEmpty(FileName))
                elem.Add(XmlHelper.ToXml(() => FileName));

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            IsTextured = element.ReadAttribute("IsTextured", false);
            IsFlexible = element.ReadAttribute("IsFlexible", false);
            FileName = element.ReadAttribute("FileName", string.Empty);
        }

        public bool LoadModel()
        {
            if (Geometry == null && Project != null && !string.IsNullOrEmpty(FileName))
                Geometry = Project.ReadModelMesh(FileName);
            return Geometry != null;
        }
    }
}
