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
    public class ModelMesh : PartComponent
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

        public ModelMesh()
        {
            RefID = Utilities.StringUtils.GenerateUID(8);
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
    }
}
