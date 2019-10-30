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

        public void BindToShader(ModelShaderProgram modelShader)
        {
            Vao.Bind();
            Vao.BindAttribute(modelShader.Position, VertexBuffer, 0);
            Vao.BindAttribute(modelShader.Normal, VertexBuffer, 12);
            Vao.BindAttribute(modelShader.TexCoord, VertexBuffer, 24);
        }

        public void UpdateShaderUniforms(ModelShaderProgram modelShader)
        {
            modelShader.Use();
            modelShader.ModelMatrix.Set(Transform);
            modelShader.Material.Set(Material);
        }

        public void Draw()
        {
            Vao.Bind();
            Vao.DrawElements(PrimitiveType.Triangles, IndexBuffer.ElementCount);
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
