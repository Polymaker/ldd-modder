using LDDModder.BrickEditor.Resources;
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
    public class PartSurface : PartNode
    {
        public int SurfaceID { get; set; }

        public int SubMaterialID { get; set; }

        public bool IsTextured { get; set; }

        public bool IsFlexible { get; set; }

        public PartNodeCollection<SurfaceComponent> Components { get; set; }

        public PartSurface()
        {
            Components = new PartNodeCollection<SurfaceComponent>(this);
        }

        public override string GetDisplayName()
        {
            if (SurfaceID == 0)
                return PartResources.MainSurface;
            return string.Format(PartResources.DecorationSurface, SurfaceID);
        }

        public override XElement SerializeToXml()
        {
            var elem = new XElement("Surface");
            elem.Add(new XAttribute("ID", ID));
            elem.Add(new XAttribute("Surface", SurfaceID));
            elem.Add(new XAttribute("SubMaterial", SubMaterialID));
            elem.Add(new XAttribute("Textured", IsTextured));
            elem.Add(new XAttribute("Flexible", IsFlexible));

            if (!string.IsNullOrEmpty(Description))
                elem.Add(new XAttribute("Description", Description));

            return elem;
        }

        public override XElement SerializeHierarchy()
        {
            var root = SerializeToXml();

            foreach (var comp in Components)
                root.Add(comp.SerializeHierarchy());

            return root;
        }

        public MeshFile GenerateLDDFile()
        {
            var cullings = new List<MeshCulling>();
            var componentMeshes = new List<MeshGeometry>();

            int totalVerts = 0;
            int totalIndices = 0;

            foreach(var component in Components)
            {
                var combinedMesh = MeshGeometry.Combine(component.Meshes.Select(x => x.Geometry).ToArray());
                componentMeshes.Add(combinedMesh);

                var cullingInfo = new MeshCulling(component.ComponentType)
                {
                    FromVertex = totalVerts,
                    FromIndex = totalIndices,
                    VertexCount = combinedMesh.VertexCount,
                    IndexCount = combinedMesh.IndexCount
                };

                totalVerts += combinedMesh.VertexCount;
                totalIndices += combinedMesh.IndexCount;

                if (component.AlternateMeshes.Any())
                {
                    cullingInfo.ReplacementMesh = MeshGeometry.Combine(component.AlternateMeshes.Select(x => x.Geometry).ToArray());
                }

                foreach (var studNode in component.LinkedStuds)
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
                        //TODO
                    }
                }

                cullings.Add(cullingInfo);
            }

            var finalMesh = MeshGeometry.Combine(componentMeshes.ToArray());

            var meshFile = new MeshFile(finalMesh);
            meshFile.Cullings.AddRange(cullings);

            return meshFile;
        }
    }
}
