using Assimp;
using LDDModder.Simple3D;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public static class MeshConverter
    {
        public static Mesh ConvertToLDD(Scene scene)
        {
            foreach (var m in scene.Meshes)
            {
                List<Vector3D> verts = m.Vertices;
                List<Vector3D> norms = (m.HasNormals) ? m.Normals : null;
                List<Vector3D> uvs = m.HasTextureCoords(0) ? m.TextureCoordinateChannels[0] : null;
            }

            return null;
        }

        public static Mesh ConvertToLDD(Assimp.Mesh mesh, bool removeUVs = false)
        {
            List<Vector3D> verts = mesh.Vertices;
            List<Vector3D> norms = (mesh.HasNormals) ? mesh.Normals : null;
            List<Vector3D> uvs = mesh.HasTextureCoords(0) && !removeUVs ? mesh.TextureCoordinateChannels[0] : null;

            List<Vertex> vertices = new List<Vertex>();

            for (int i = 0; i < verts.Count; i++)
            {
                Vector3 pos = (Vector3)verts[i];
                Vector3 norm = (norms != null) ? (Vector3)norms[i] : new Vector3(0, 0, 0);
                Vector2 uv = (uvs != null) ? new Vector2(uvs[i].X, 1 - uvs[i].Y) : Vector2.Empty;
                vertices.Add(new Vertex(pos, norm, uv));
            }
            
            var geom = new MeshGeometry();
            geom.SetVertices(vertices);

            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                Face f = mesh.Faces[i];

                //Ignore non-triangle faces
                if (f.IndexCount != 3)
                    continue;

                geom.AddTriangleFromIndices(f.Indices[0], f.Indices[1], f.Indices[2]);
            }

            if (mesh.HasBones)
            {
                for (int i = 0; i < mesh.BoneCount; i++)
                {
                    var bone = mesh.Bones[i];
                    for (int j = 0; j < bone.VertexWeightCount; j++)
                    {
                        var vw = bone.VertexWeights[j];
                        geom.Vertices[vw.VertexID].BoneWeights.Add(new BoneWeight(i, vw.Weight));
                    }
                }
            }

            geom.SimplifyVertices();
            return new Mesh(geom);
        }


        public static Assimp.Mesh ConvertFromLDD(Mesh lddMesh)
        {
            var oMesh = new Assimp.Mesh(PrimitiveType.Triangle);
            var vertIndexer = new ListIndexer<Vertex>(lddMesh.Geometry.Vertices);

            foreach (var v in lddMesh.Geometry.Vertices)
            {
                oMesh.Vertices.Add(new Vector3D(v.Position.X, v.Position.Y, v.Position.Z));
                oMesh.Normals.Add(new Vector3D(v.Normal.X, v.Normal.Y, v.Normal.Z));
                if (lddMesh.Geometry.IsTextured)
                    oMesh.TextureCoordinateChannels[0].Add(new Vector3D(v.TexCoord.X, v.TexCoord.Y, 0));
            }

            foreach (var t in lddMesh.Geometry.Triangles)
            {
                var vIndices = t.Vertices.Select(v => vertIndexer.IndexOf(v)).ToArray();
                oMesh.Faces.Add(new Face(vIndices));
            }

            return oMesh;
        }
    }
}
