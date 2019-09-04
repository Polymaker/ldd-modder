using LDDModder.LDD.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.Editing
{
    public class ModelMeshNode : PartNode
    {
        public MeshGeometry Mesh { get; set; }

        public MeshCullingType MeshType { get; set; }

        public ModelMeshNode() : base()
        {

        }

        public ModelMeshNode(MeshGeometry mesh) : base()
        {
            Mesh = mesh;
            GenerateID();
        }

        public override XElement SerializeToXml()
        {
            var elem = new XElement("Mesh", new XAttribute("ID", ID));
            elem.Add(new XAttribute("Type", MeshType.ToString()));
            return elem;
        }
    }
}
