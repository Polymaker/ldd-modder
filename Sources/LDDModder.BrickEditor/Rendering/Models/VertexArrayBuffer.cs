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


namespace LDDModder.BrickEditor.Rendering.Models
{
    public class VertexArrayBuffer<T> : IDisposable where T : struct
    {
        public VertexArray Vao { get; protected set; }

        public Buffer<T> ElementBuffer { get; protected set; }

        public int ElementCount => (ElementBuffer != null && ElementBuffer.Initialized) ? ElementBuffer.ElementCount : 0;

        public bool BufferInitialized { get; protected set; }

        public bool IsDisposed { get; protected set; }

        public void Bind()
        {
            if (BufferInitialized)
                Vao.Bind();
        }

        public void CreateBuffers()
        {
            if (!BufferInitialized)
            {
                Vao = new VertexArray();
                ElementBuffer = new Buffer<T>();
                BufferInitialized = true;
            }
        }

        public void ClearBuffers()
        {
            if (BufferInitialized)
                ElementBuffer.Clear(BufferTarget.ArrayBuffer);
        }

        public void BindAttribute(VertexAttrib attribute, int offset)
        {
            if (BufferInitialized)
                Vao.BindAttribute(attribute, ElementBuffer, offset);
        }

        public void UnbindAttribute(VertexAttrib attribute)
        {
            if (BufferInitialized)
                Vao.UnbindAttribute(attribute);
        }

        public void SetElements(IEnumerable<T> elements)
        {
            if (!BufferInitialized)
                CreateBuffers();

            ElementBuffer.Init(BufferTarget.ArrayBuffer, elements.ToArray());
        }

        public void DrawArray(PrimitiveType drawMode, int first, int count)
        {
            Vao.Bind();
            Vao.DrawArrays(drawMode, first, count);
        }

        public void Dispose()
        {
            if (ElementBuffer != null)
            {
                ElementBuffer.Dispose();
                ElementBuffer = null;
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
