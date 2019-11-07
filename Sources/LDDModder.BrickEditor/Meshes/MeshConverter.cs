using LDDModder.LDD.Meshes;
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


    }
}
