using LDDModder.LDD.Meshes;
using LDDModder.LDD.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Meshes
{
    public static class MeshConverter
    {
        public static MeshGeometry AssimpToLdd(Assimp.Scene scene, Assimp.Mesh mesh)
        {
            bool hasUVs = mesh.HasTextureCoords(0);

            var builder = new GeometryBuilder();
            var meshNode = Assimp.AssimpHelper.GetMeshNode(scene, mesh);
            var meshTransform = Assimp.AssimpHelper.GetFinalTransform(meshNode).ToLDD();
            
            for (int i = 0; i < mesh.VertexCount; i++)
            {
                builder.AddVertex(new Vertex()
                {
                    Position = meshTransform.TransformPosition(mesh.Vertices[i].ToLDD()),
                    Normal = meshTransform.TransformNormal(mesh.Normals[i].ToLDD()),
                    TexCoord = hasUVs ? mesh.TextureCoordinateChannels[0][i].ToLDD().Xy : Simple3D.Vector2.Empty
                }, false);
            }

            for (int i = 0; i < mesh.FaceCount; i++)
            {
                if (mesh.Faces[i].IndexCount != 3)
                    continue;

                builder.AddTriangle(mesh.Faces[i].Indices[0], mesh.Faces[i].Indices[1], mesh.Faces[i].Indices[2]);
            }

            var geometry = builder.GetGeometry();
            geometry.SimplifyVertices();
            return geometry;
        }

        public static Assimp.Scene LddPartToAssimp(PartWrapper part)
        {
            var scene = new Assimp.Scene() { RootNode = new Assimp.Node("Root") };
            scene.Materials.Add(new Assimp.Material() { Name = "BaseMaterial" });

            var meshNodes = new List<Assimp.Node>();


            foreach (var surface in part.Surfaces)
            {
                string nodeName = "BaseModel";
                if (surface.SurfaceID > 0)
                    nodeName = $"Decoration{surface.SurfaceID}";

                scene.Materials.Add(new Assimp.Material() { Name = $"{nodeName}_Material" });
                var meshNode = CreateMeshNode(scene, part, surface.Mesh, nodeName);

                meshNodes.Add(meshNode);
            }

            if (meshNodes.Count > 1)
            {
                var groupNode = new Assimp.Node() { Name = "Part" };
                foreach (var node in meshNodes)
                    groupNode.Children.Add(node);
                scene.RootNode.Children.Add(groupNode);
            }
            else
                scene.RootNode.Children.Add(meshNodes[0]);

            
            foreach (var connGroup in part.Primitive.Connectors.GroupBy(x => x.Type))
            {
                int connectionIdx = 0;
                foreach (var conn in connGroup)
                {
                    var connNode = new Assimp.Node($"{connGroup.Key.ToString()}{connectionIdx++}_Type_{conn.SubType}");

                    //connNode.Metadata.Add("Type", new Assimp.Metadata.Entry(Assimp.MetaDataType.String, conn.Type.ToString()));
                    connNode.Transform = conn.Transform.ToMatrix4().ToAssimp();
                    scene.RootNode.Children.Add(connNode);
                }
            }
            

            return scene;
        }

        public static Assimp.Mesh LddMeshToAssimp(MeshGeometry mesh)
        {
            var oMesh = new Assimp.Mesh(Assimp.PrimitiveType.Triangle);

            foreach (var v in mesh.Vertices)
            {
                oMesh.Vertices.Add(v.Position.ToAssimp());
                oMesh.Normals.Add(v.Normal.ToAssimp());
                if (mesh.IsTextured)
                    oMesh.TextureCoordinateChannels[0].Add(new Assimp.Vector3D(v.TexCoord.X, v.TexCoord.Y, 0));
            }

            int[] indices = mesh.GetTriangleIndices();

            for (int i = 0; i < indices.Length; i += 3)
                oMesh.Faces.Add(new Assimp.Face(new int[] { indices[i], indices[i + 1], indices[i + 2] }));

            return oMesh;
        }

        public static Assimp.Mesh LddMeshToAssimp(LDD.Files.MeshFile meshFile)
        {
            return LddMeshToAssimp(meshFile.Geometry);
        }

        private static Assimp.Node CreateMeshNode(Assimp.Scene scene, PartWrapper part, LDD.Files.MeshFile lddMesh, string name)
        {
            var meshNode = new Assimp.Node() { Name = name };
            var aMesh = LddMeshToAssimp(lddMesh);
            aMesh.MaterialIndex = scene.MeshCount;
            meshNode.MeshIndices.Add(scene.MeshCount);
            scene.Meshes.Add(aMesh);

            return meshNode;
        }

    }
}
