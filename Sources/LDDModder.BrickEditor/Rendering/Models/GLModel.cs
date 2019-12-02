using LDDModder.BrickEditor.Rendering.Shaders;
using ObjectTK.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class GLModel : ModelBase, IDisposable
    {

        public MaterialInfo Material { get; set; }

        public IndexedVertexBuffer<VertVNT> VertexBuffer { get; private set; }

        public GLModel()
        {
            Transform = Matrix4.Identity;
            VertexBuffer = new IndexedVertexBuffer<VertVNT>();
        }

        public void SetIndices(IEnumerable<int> indices)
        {
            VertexBuffer.SetIndices(indices);
        }

        public void SetVertices(IEnumerable<VertVNT> vertices)
        {
            VertexBuffer.SetVertices(vertices);
            BoundingBox = BBox.FromVertices(vertices.Select(x => x.Position));
        }

        public void SetVertices(IEnumerable<VertVN> vertices)
        {
            VertexBuffer.SetVertices(vertices.Select(x => new VertVNT
            {
                Position = x.Position,
                Normal = x.Normal,
                TexCoord = Vector2.Zero
            }).ToArray());
            BoundingBox = BBox.FromVertices(vertices.Select(x => x.Position));
        }

        //public void LoadFromLDD(LDD.Meshes.MeshGeometry geometry)
        //{
        //    var verts = new List<VertVNT>();
        //    foreach (var v in geometry.Vertices)
        //    {
        //        verts.Add(new VertVNT()
        //        {
        //            Position = v.Position.ToGL(),
        //            Normal = v.Normal.ToGL(),
        //            TexCoord = geometry.IsTextured ? v.TexCoord.ToGL() : Vector2.Zero
        //        });
        //    }
        //    SetVertices(verts);
        //    int[] triIndices = geometry.GetTriangleIndices();
        //    SetIndices(triIndices);
        //}

        public static GLModel CreateFromAssimp(Assimp.Mesh mesh)
        {
            var model = new GLModel();
            model.LoadFromAssimp(mesh);
            return model;
        }

        public void LoadFromAssimp(Assimp.Mesh mesh)
        {
            var verts = new List<VertVNT>();
            bool isTextured = mesh.HasTextureCoords(0);

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                verts.Add(new VertVNT()
                {
                    Position = mesh.Vertices[i].ToGL(),
                    Normal = mesh.Normals[i].ToGL(),
                    TexCoord = isTextured ? mesh.TextureCoordinateChannels[0][i].ToGL().Xy : Vector2.Zero
                });
            }

            SetVertices(verts);

            var indices = new List<int>();
            int indexPerFace = mesh.Faces[0].IndexCount;
            foreach (var face in mesh.Faces)
                if (face.IndexCount == indexPerFace)
                    indices.AddRange(face.Indices);
            SetIndices(indices);
        }

        public void Draw(PrimitiveType drawMode = PrimitiveType.Triangles)
        {
            VertexBuffer.DrawElements(drawMode);
        }

        public override void Dispose()
        {
            if (VertexBuffer != null)
                VertexBuffer.Dispose();
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            distance = float.NaN;
            var vertices = VertexBuffer.VertexBuffer.Content;
            var indices = VertexBuffer.IndexBuffer.Content;

            for (int i = 0; i < VertexBuffer.IndexCount; i += 3)
            {
                var idx1 = indices[i];
                var idx2 = indices[i + 1];
                var idx3 = indices[i + 2];

                var v1 = vertices[idx1];
                var v2 = vertices[idx2];
                var v3 = vertices[idx3];

                if (Ray.IntersectsTriangle(ray, v1.Position, v2.Position, v3.Position, out float hitDist))
                    distance = float.IsNaN(distance) ? hitDist : Math.Min(hitDist, distance);
            }

            return !float.IsNaN(distance);
        }
    }
}
