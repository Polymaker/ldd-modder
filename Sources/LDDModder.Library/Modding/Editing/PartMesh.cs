using LDDModder.LDD.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class PartMesh : PartComponent
    {
        public MeshGeometry Geometry { get; set; }

        public bool IsTextured { get; set; }

        public bool IsFlexible { get; set; }

        public string FileName { get; set; }

        public PartMesh()
        {
            RefID = Utilities.StringUtils.GenerateUID(8);
        }

        public PartMesh(MeshGeometry geometry)
        {
            Geometry = geometry;
            IsTextured = geometry.IsTextured;
            IsFlexible = geometry.IsFlexible;
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase("Mesh");
            elem.Add(new XAttribute("IsTextured", IsTextured));
            elem.Add(new XAttribute("IsFlexible", IsFlexible));
            if (!string.IsNullOrEmpty(FileName))
                elem.Add(new XAttribute("FileName", FileName));

            return elem;
        }
    }
}
