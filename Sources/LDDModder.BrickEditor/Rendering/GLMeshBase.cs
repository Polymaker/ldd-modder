using ObjectTK.Buffers;
using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public abstract class GLMeshBase<VT> : IDisposable where VT : struct
    {
        public VertexArray Vao { get; protected set; }

        public Buffer<int> IndexBuffer { get; protected set; }

        public Buffer<VT> VertexBuffer { get; private set; }

        public Matrix4 Transform { get; set; }

        public Color4 MaterialColor { get; set; }

        public bool Disposed { get; private set; }

        public ObjectTK.Shaders.Program BoundProgram { get; protected set; }

        protected List<Tuple<VertexAttrib,int>> BoundAttributes = new List<Tuple<VertexAttrib, int>> ();

        public GLMeshBase()
        {
            Vao = new VertexArray();
        }

        ~GLMeshBase()
        {
            if (!Disposed)
                Dispose();
        }

        protected void DisposeBuffers()
        {
            if (Vao != null)
                Vao.Bind();

            if (IndexBuffer != null)
            {
                if (Vao != null)
                    Vao.UnbindElementBuffer();

                IndexBuffer.Dispose();
                IndexBuffer = null;
            }

            if (VertexBuffer != null)
            {
                if (Vao != null && BoundAttributes != null)
                    BoundAttributes.ForEach(a => Vao.UnbindAttribute(a.Item1));

                VertexBuffer.Dispose();
                VertexBuffer = null;
            }
        }

        public void UpdateVertices(IEnumerable<int> indices, IEnumerable<VT> vertices)
        {
            DisposeBuffers();

            VertexBuffer = new Buffer<VT>();
            VertexBuffer.Init(BufferTarget.ArrayBuffer, vertices.ToArray());

            IndexBuffer = new Buffer<int>();
            IndexBuffer.Init(BufferTarget.ElementArrayBuffer, indices.ToArray());

            if (BoundProgram != null)
            {
                foreach(var vAttr in BoundAttributes)
                    Vao.BindAttribute(vAttr.Item1, VertexBuffer, vAttr.Item2);

                Vao.BindElementBuffer(IndexBuffer);
            }
        }

        protected void UnbindProgram()
        {
            if (Vao != null && BoundAttributes != null)
            {
                Vao.Bind();
                BoundAttributes.ForEach(a => Vao.UnbindAttribute(a.Item1));
            }

            BoundProgram = null;
            BoundAttributes.Clear();
        }

        public void BindVertexAttribute(VertexAttrib attrib, int offset = 0)
        {
            BoundAttributes.Add(new Tuple<VertexAttrib, int>(attrib, offset));
            if (VertexBuffer != null)
                Vao.BindAttribute(attrib, VertexBuffer, offset);
        }

        protected void BindToProgram(ObjectTK.Shaders.Program program)
        {
            if (BoundProgram != null)
                UnbindProgram();

            BoundProgram = program;
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


        public virtual void AssignShaderValues()
        {
            if (BoundProgram is IMeshShaderProgram program)
            {
                program.ModelMatrix.Set(Transform);
                var v = new Vector4(MaterialColor.R, MaterialColor.G, MaterialColor.B, MaterialColor.A);
                program.MaterialColor.Set(v);
            }
        }

        protected virtual void OnDraw()
        {

        }

        public void Draw()
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);
            
            Vao.Bind();

            OnDraw();

            Vao.DrawElements(PrimitiveType.Triangles, IndexBuffer.ElementCount);
        }
    }
}
