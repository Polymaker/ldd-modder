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

        public Buffer<Vector3> VertexBuffer { get; private set; }
        //public Buffer<Vector2> TexCoordBuffer { get; protected set; }
        public Buffer<uint> IndexBuffer { get; private set; }

        public List<Vector3> Vertices { get; set; }
        public List<uint> Indices { get; set; }

        public ObjectTK.Shaders.Program Program { get; private set; }

        public bool Disposed { get; private set; }

        public GLMesh()
        {
            Vao = new VertexArray();
            Vertices = new List<Vector3>();
            Indices = new List<uint>();
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

            if (IndexBuffer != null)
                Vao.UnbindElementBuffer();

            DisposeBuffers();

            VertexBuffer = new Buffer<Vector3>();
            VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices.ToArray());

            IndexBuffer = new Buffer<uint>();
            IndexBuffer.Init(BufferTarget.ElementArrayBuffer, Indices.ToArray());

        }

        public void BindToShader(BasicShaderProgram program)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);
            Program = program;
            Vao.Bind();
            Vao.BindAttribute(program.InPosition, VertexBuffer);
            Vao.BindElementBuffer(IndexBuffer);
        }

        private void DisposeBuffers()
        {
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

            Vao.DrawElements(PrimitiveType.Triangles, IndexBuffer.ElementCount);
        }
    }
}
