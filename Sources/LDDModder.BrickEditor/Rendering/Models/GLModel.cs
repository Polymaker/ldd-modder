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
    public class GLModel : IDisposable
    {
        public bool Visible { get; set; }

        public MaterialInfo Material { get; set; }

        public Matrix4 Transform { get; set; }

        public VertexArray Vao { get; protected set; }

        public Buffer<int> IndexBuffer { get; protected set; }

        public Buffer<VertVNT> VertexBuffer { get; protected set; }

        public bool BufferInitialized { get; protected set; }

        public BBox BoundingBox { get; set; }

        public GLModel()
        {
            Transform = Matrix4.Identity;
        }

        public void CreateBuffers()
        {
            if (!BufferInitialized)
            {
                Vao = new VertexArray();
                IndexBuffer = new Buffer<int>();
                VertexBuffer = new Buffer<VertVNT>();
                BufferInitialized = true;

                Vao.Bind();
                Vao.BindElementBuffer(IndexBuffer);
            }
        }

        public void SetIndices(IEnumerable<int> indices)
        {
            if (!BufferInitialized)
                CreateBuffers();
            IndexBuffer.Clear(BufferTarget.ElementArrayBuffer);
            IndexBuffer.Init(BufferTarget.ElementArrayBuffer, indices.ToArray());
        }

        public void SetVertices(IEnumerable<VertVNT> vertices)
        {
            if (!BufferInitialized)
                CreateBuffers();
            VertexBuffer.Clear(BufferTarget.ArrayBuffer);
            VertexBuffer.Init(BufferTarget.ArrayBuffer, vertices.ToArray());
            BoundingBox = BBox.Calculate(vertices.Select(x => x.Position));
        }

        public void SetVertices(IEnumerable<VertVN> vertices)
        {
            if (!BufferInitialized)
                CreateBuffers();
            VertexBuffer.Clear(BufferTarget.ArrayBuffer);
            VertexBuffer.Init(BufferTarget.ArrayBuffer, vertices.Select(x => new VertVNT
            {
                Position = x.Position,
                Normal = x.Normal,
                TexCoord = Vector2.Zero
            }).ToArray());
            BoundingBox = BBox.Calculate(vertices.Select(x => x.Position));
        }

        public void LoadFromLDD(LDD.Meshes.MeshGeometry geometry)
        {
            var verts = new List<VertVNT>();
            foreach (var v in geometry.Vertices)
            {
                verts.Add(new VertVNT()
                {
                    Position = v.Position.ToGL(),
                    Normal = v.Normal.ToGL(),
                    TexCoord = geometry.IsTextured ? v.TexCoord.ToGL() : Vector2.Zero
                });
            }
            SetVertices(verts);
            int[] triIndices = geometry.GetTriangleIndices();
            SetIndices(triIndices);
        }

        public static GLModel CreatFromAssimp(Assimp.Mesh mesh)
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

        public void BindToShader(ModelShaderProgram modelShader)
        {
            modelShader.Use();
            Vao.Bind();
            Vao.BindAttribute(modelShader.Position, VertexBuffer, 0);
            Vao.BindAttribute(modelShader.Normal, VertexBuffer, 12);
            Vao.BindAttribute(modelShader.TexCoord, VertexBuffer, 24);

            modelShader.ModelMatrix.Set(Transform);
            modelShader.Material.Set(Material);
        }

        public void UnbindShader(ModelShaderProgram modelShader)
        {
            Vao.Bind();
            Vao.UnbindAttribute(modelShader.Position);
            Vao.UnbindAttribute(modelShader.Normal);
            Vao.UnbindAttribute(modelShader.TexCoord);
        }

        public void BindToShader(WireframeShaderProgram wireframeShader)
        {
            wireframeShader.Use();

            Vao.Bind();
            Vao.BindAttribute(wireframeShader.Position, VertexBuffer, 0);

            wireframeShader.ModelMatrix.Set(Transform);
        }

        public void UnbindShader(WireframeShaderProgram wireframeShader)
        {
            Vao.Bind();
            Vao.UnbindAttribute(wireframeShader.Position);
        }


        public void Draw(PrimitiveType drawMode = PrimitiveType.Triangles)
        {
            Vao.Bind();
            Vao.DrawElements(drawMode, IndexBuffer.ElementCount);
        }

        public void Dispose()
        {
            if (IndexBuffer != null)
            {
                IndexBuffer.Dispose();
                IndexBuffer = null;
            }

            if (VertexBuffer != null)
            {
                VertexBuffer.Dispose();
                VertexBuffer = null;
            }

            if (Vao != null)
            {
                Vao.Dispose();
                Vao = null;
            }
        }
    }
}
