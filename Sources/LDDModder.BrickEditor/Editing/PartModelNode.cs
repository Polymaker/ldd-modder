using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.Editing
{
    public class PartModelNode : PartNode
    {
        public int SurfaceID { get; set; }

        public int SubMaterialID { get; set; }

        public bool IsMainModel => SurfaceID == 0;

        public override string GetName()
        {
            return IsMainModel ? "Main model" : $"Decoration {SurfaceID}";
        }

        public override XElement SerializeToXml()
        {
            var elem = new XElement("Model", new XAttribute("ID", ID));
            //if (IsMainModel)
            //    elem.Add(new XAttribute("MainModel", 1));

            elem.Add(new XAttribute("SurfaceID", SurfaceID));
            elem.Add(new XAttribute("SubMaterialID", SubMaterialID));

            return elem;
        }

        public MeshFile GenerateLDDFile()
        {
            var verts = new List<Vertex>();
            var triangles = new List<Triangle>();
            var cullings = new List<MeshCulling>();

            foreach (var subMesh in Nodes.OfType<ModelMeshNode>())
            {
                var cullingInfo = new MeshCulling(subMesh.MeshType)
                {
                    FromVertex = verts.Count,
                    FromIndex = triangles.Count * 3,
                    VertexCount = subMesh.Mesh.VertexCount,
                    IndexCount = subMesh.Mesh.IndexCount
                };

                foreach (var studNode in subMesh.Nodes.OfType<StudReference>())
                {
                    cullingInfo.Studs.Add(new Custom2DFieldReference(
                        studNode.ConnectorNode.Index,
                        studNode.StudIndex));

                    if (cullingInfo.Type == MeshCullingType.Tube)
                    {
                        var fieldNode = studNode.Connector[studNode.StudIndex];
                        var node1 = studNode.Connector.GetNode(fieldNode.X - 1, fieldNode.Y);
                        var node2 = studNode.Connector.GetNode(fieldNode.X + 1, fieldNode.Y);
                        var node3 = studNode.Connector.GetNode(fieldNode.X, fieldNode.Y - 1);
                        var node4 = studNode.Connector.GetNode(fieldNode.X, fieldNode.Y + 1);


                    }
                }

                verts.AddRange(subMesh.Mesh.Vertices);
                triangles.AddRange(subMesh.Mesh.Triangles);

                cullings.Add(cullingInfo);
            }

            var geom = new MeshGeometry();
            geom.SetVertices(verts);
            geom.SetTriangles(triangles);

            var meshFile = new MeshFile(geom);
            meshFile.Cullings.AddRange(cullings);

            return meshFile;
        }
    }
}
