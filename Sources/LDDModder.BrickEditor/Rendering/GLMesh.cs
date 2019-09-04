using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectTK.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LDDModder.BrickEditor.Rendering
{
    public class GLMesh : IDisposable
    {
        public VertexArray Vao { get; private set; }

        public Matrix4 Transform { get; set; }
        public Vector4 Color { get; set; }

        public Buffer<VertVN> VertexBuffer { get; private set; }
        public Buffer<int> IndexBuffer { get; private set; }

        public List<VertVN> Vertices { get; set; }
        public List<int> Indices { get; set; }

        public ObjectTK.Shaders.Program Program { get; private set; }

        public bool Disposed { get; private set; }

        public GLMesh()
        {
            Vao = new VertexArray();
            Vertices = new List<VertVN>();
            Indices = new List<int>();
            Transform = Matrix4.Identity;
        }

        ~GLMesh()
        {
            if (!Disposed)
                Dispose();
        }

        public void UpdateBuffers()
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            DisposeBuffers();

            VertexBuffer = new Buffer<VertVN>();
            VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices.ToArray());
            IndexBuffer = new Buffer<int>();
            IndexBuffer.Init(BufferTarget.ElementArrayBuffer, Indices.ToArray());
        }

        public void BindToShader(BasicShaderProgram program)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);
            Program = program;
            Vao.Bind();
            Vao.BindAttribute(program.InPosition, VertexBuffer);
            Vao.BindAttribute(program.InNormal, VertexBuffer, 12);
            Vao.BindElementBuffer(IndexBuffer);
            
        }

        //public void BindToShader(TexturedShaderProgram program)
        //{
        //    if (Disposed)
        //        throw new ObjectDisposedException(GetType().Name);
        //    Program = program;
        //    Vao.Bind();
        //    Vao.BindAttribute(program.InPosition, VertexBuffer);
        //    Vao.BindAttribute(program.InNormal, VertexBuffer, 12);
        //    Vao.BindAttribute(program.InTexCoord, VertexBuffer, 24);
        //    Vao.BindElementBuffer(IndexBuffer);
        //}

        private void DisposeBuffers()
        {
            Vao.Bind();

            if (IndexBuffer != null)
            {
                Vao.UnbindElementBuffer();
                IndexBuffer.Dispose();
                IndexBuffer = null;
            }

            if (VertexBuffer != null)
            {
                VertexBuffer.Dispose();
                VertexBuffer = null;
            }

            //if (TexCoordBuffer != null)
            //{
            //    TexCoordBuffer.Dispose();
            //    TexCoordBuffer = null;
            //}
        }

        public void Dispose()
        {
            DisposeBuffers();

            if (Vao != null)
            {
                Vao.Dispose();
                Vao = null;
            }
            Disposed = true;
            GC.SuppressFinalize(this);
        }

        public void Draw()
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            Vao.Bind();
            Vao.DrawElements(PrimitiveType.Triangles, IndexBuffer.ElementCount);
        }

        public static GLMesh FromGeometry(LDD.Meshes.MeshGeometry geometry)
        {
            var mesh = new GLMesh();
            foreach (var v in geometry.Vertices)
            {
                mesh.Vertices.Add(new VertVN()
                {
                    Position = v.Position.ToGL(),
                    Normal = v.Normal.ToGL()
                });
            }
            mesh.Indices.AddRange(geometry.GetTriangleIndices());
            mesh.UpdateBuffers();
            return mesh;
        }
    }
}
