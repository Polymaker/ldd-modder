using Assimp;
using LDDModder.LDD.Files;
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
        public static MeshFile ConvertToLDD(Scene scene)
        {
            foreach (var m in scene.Meshes)
            {
                List<Vector3D> verts = m.Vertices;
                List<Vector3D> norms = (m.HasNormals) ? m.Normals : null;
                List<Vector3D> uvs = m.HasTextureCoords(0) ? m.TextureCoordinateChannels[0] : null;
            }

            return null;
        }

        public static MeshFile ConvertToLDD(Assimp.Mesh mesh, bool removeUVs = false)
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
            return new MeshFile(geom);
        }

        public static Assimp.Mesh ConvertFromLDD(MeshFile lddMesh)
        {
            return ConvertFromLDD(lddMesh.Geometry);
        }

        public static Assimp.Mesh ConvertFromLDD(MeshGeometry meshGeom)
        {
            var oMesh = new Assimp.Mesh(PrimitiveType.Triangle);
            //var vertIndexer = new ListIndexer<Vertex>(meshGeom.Vertices);

            foreach (var v in meshGeom.Vertices)
            {
                oMesh.Vertices.Add(v.Position.Convert());
                oMesh.Normals.Add(v.Normal.Convert());
                if (meshGeom.IsTextured)
                    oMesh.TextureCoordinateChannels[0].Add(new Vector3D(v.TexCoord.X, v.TexCoord.Y, 0));
            }

            int[] indices = meshGeom.GetTriangleIndices();

            for (int i = 0; i < indices.Length; i += 3)
                oMesh.Faces.Add(new Face(new int[] { indices[i], indices[i + 1], indices[i + 2] }));

            //foreach (var t in meshGeom.Triangles)
            //{
            //    var vIndices = t.Vertices.Select(v => vertIndexer.IndexOf(v)).ToArray();
            //    oMesh.Faces.Add(new Face(vIndices));
            //}

            return oMesh;
        }

        public static Assimp.Mesh ConvertFromLDD2(MeshGeometry meshGeom)
        {
            var oMesh = new Assimp.Mesh(PrimitiveType.Triangle);
            //var vertIndexer = new ListIndexer<Vertex>(meshGeom.Vertices);
            //float edgeWidthRatio = 15.5f / 0.8f;
            for (int i = 0; i < meshGeom.IndexCount; i++)
            {
                oMesh.Vertices.Add(meshGeom.Indices[i].Vertex.Position.Convert());
                oMesh.Normals.Add(meshGeom.Indices[i].Vertex.Normal.Convert());

                for (int j = 0; j < 6; j++)
                {
                    var coord = meshGeom.Indices[i].RoundEdgeData.Coords[j];
                    if (RoundEdgeData.EmptyCoord != coord)
                        coord.X = Math.Abs(coord.X) - 100f;
                    coord /= 10f;
                    oMesh.TextureCoordinateChannels[j].Add(new Vector3D(coord.X, coord.Y, 0));
                }

                if (i % 3 == 0)
                    oMesh.Faces.Add(new Face(new int[] { i, i + 1, i + 2 }));
            }
            return oMesh;
        }
    }
}
