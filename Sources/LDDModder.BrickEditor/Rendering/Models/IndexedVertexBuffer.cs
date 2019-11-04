using LDDModder.BrickEditor.Rendering.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class IndexedVertexBuffer : IDisposable
    {
        public VertexArray Vao { get; protected set; }

        public Buffer<int> IndexBuffer { get; protected set; }

        public Buffer<VertVNT> VertexBuffer { get; protected set; }

        public bool BufferInitialized { get; protected set; }

        public bool IsDisposed { get; protected set; }

        public void Bind()
        {
            if (BufferInitialized)
                Vao.Bind();
        }

        public void BindAttribute(VertexAttrib attribute, int offset)
        {
            if (BufferInitialized)
                Vao.BindAttribute(attribute, VertexBuffer, offset);
        }

        public void UnbindAttribute(VertexAttrib attribute)
        {
            if (BufferInitialized)
                Vao.UnbindAttribute(attribute);
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

        public void ClearBuffers()
        {
            if (BufferInitialized)
            {
                IndexBuffer.Clear(BufferTarget.ElementArrayBuffer);
                VertexBuffer.Clear(BufferTarget.ArrayBuffer);
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

        ~IndexedVertexBuffer()
        {
            if (!IsDisposed)
                Dispose();
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

            IsDisposed = true;
        }
    }
}
