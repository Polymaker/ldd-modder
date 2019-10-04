using LDDModder.LDD.Files;
using ObjectTK.Buffers;
using ObjectTK.Textures;
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
    public class BrickModel : IDisposable
    {
        //public GLMeshBase Mesh { get; set; }
        public MeshFile Mesh { get; set; }
        public Color4 MaterialColor { get; set; }
        public List<Vector3> Positions { get; set; }
        public List<Vector2> OutlineIndices { get; set; }
        public Texture2D OutlineTexture { get; set; }

        struct TexCoords
        {
            public Vector2 T0;
            public Vector2 T1;
            public Vector2 T2;
            public Vector2 T3;
            public Vector2 T4;
            public Vector2 T5;

            public TexCoords(Vector2 t0, Vector2 t1, Vector2 t2, Vector2 t3, Vector2 t4, Vector2 t5)
            {
                T0 = t0;
                T1 = t1;
                T2 = t2;
                T3 = t3;
                T4 = t4;
                T5 = t5;
            }

            public TexCoords(Vector2[] coords)
            {
                T0 = coords[0];
                T1 = coords[1];
                T2 = coords[2];
                T3 = coords[3];
                T4 = coords[4];
                T5 = coords[5];
            }
        }

        private VertexArray Vao;
        private Buffer<Vector3> PosBuffer;
        private Buffer<TexCoords> TexBuffer;

        public void InitializeBuffers(OutlineShaderProgram shader)
        {
            Vao = new VertexArray();
            PosBuffer = new Buffer<Vector3>();
            TexBuffer = new Buffer<TexCoords>();

            var positions = Mesh.Indices.Select(x => x.Vertex.Position.ToGL());
            var coords = Mesh.Indices.Select(x => new TexCoords(x.RoundEdgeData.Coords.Select(y=>y.ToGL()).ToArray()));
            PosBuffer.Init(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, positions.ToArray());
            TexBuffer.Init(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, coords.ToArray());

            shader.Use();
            Vao.Bind();
            Vao.BindAttribute(shader.Position, PosBuffer, 0);
            Vao.BindAttribute(shader.vRECoord0, TexBuffer, 0);
            Vao.BindAttribute(shader.vRECoord1, TexBuffer, 8);
            Vao.BindAttribute(shader.vRECoord2, TexBuffer, 16);
            Vao.BindAttribute(shader.vRECoord3, TexBuffer, 24);
            Vao.BindAttribute(shader.vRECoord4, TexBuffer, 32);
            Vao.BindAttribute(shader.vRECoord5, TexBuffer, 40);
        }

        public void Dispose()
        {
            //Mesh.Dispose();
            Vao.Dispose();
            PosBuffer.Dispose();
            TexBuffer.Dispose();
            if (OutlineTexture != null)
                OutlineTexture.Dispose();
        }

        public void DrawOutline(OutlineShaderProgram shader)
        {
            
            //shader.Use();
            shader.MaterialColor.Set(MaterialColor);
            Vao.DrawArrays(PrimitiveType.Triangles, 0, PosBuffer.ElementCount);
            //shader.MVPMatrix.Set((Mesh.BoundProgram as IMeshShaderProgram).ModelViewProjectionMatrix.Value);
            //shader.PackedRoundEdgeDataTexture.BindTexture(TextureUnit.Texture0, OutlineTexture);

            //var vao = new VertexArray();
            //var posBuffer = new Buffer<Vector3>();
            //var txBuffer = new Buffer<Vector2>();
            //posBuffer.Init(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, Positions.ToArray());
            //txBuffer.Init(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, OutlineIndices.ToArray());
            //vao.Bind();
            ////Mesh.BindVertexAttribute(vao, shader.Position, 0);
            //vao.BindAttribute(shader.Position, posBuffer, 0);
            //vao.BindAttribute(shader.OutlineDataIndex, txBuffer, 0);
            ////vao.BindElementBuffer(Mesh.IndexBuffer);
            //vao.DrawArrays(PrimitiveType.Triangles, 0, posBuffer.ElementCount);
            ////vao.DrawElements(PrimitiveType.Triangles, Mesh.IndexBuffer.ElementCount);
            //vao.Dispose();
            //txBuffer.Dispose();
            //posBuffer.Dispose();
        }
    }
}
