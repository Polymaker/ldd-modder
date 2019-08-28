using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectTK.Buffers;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using LDDModder.BrickEditor.Rendering;
using ObjectTK.Shaders;
using System.ComponentModel;
using OpenTK.Graphics;

namespace LDDModder.BrickEditor
{
    class GLTestWindow : GameWindow
    {
        private GLMesh Mesh;
        private BasicShaderProgram ShaderProgram;
        private Matrix4 ModelView;
        private Matrix4 Projection;
        private float rotation = 0;

        public GLTestWindow() : base(800,600, new GraphicsMode(GraphicsMode.Default.ColorFormat, 24,8,4))
        {
            VSync = VSyncMode.Adaptive;
        }

        private void SetupPerspective()
        {
            // setup perspective projection
            var aspectRatio = Width / (float)Height;
            Projection = Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.PiOver4, aspectRatio, 0.1f, 1000);
            
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            var cameraRot = Matrix3.CreateRotationY(rotation);
            var cameraPos = new Vector3(5, 5, 5) * cameraRot;
            ModelView = Matrix4.LookAt(cameraPos, Vector3.Zero, Vector3.UnitY);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            rotation += (float)(Math.PI / 2d * e.Time);
            rotation %= ((float)Math.PI * 2f);
            UpdateCamera();
        }

        private static GLMesh CreateCubeMesh()
        {
            var cubeMesh = new GLMesh();
            var faceNormals = new Vector3[] {
                Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ,
                Vector3.UnitX * -1, Vector3.UnitY * -1, Vector3.UnitZ * -1
            };

            for (int i = 0; i < 6; i++)
            {
                var norm = faceNormals[i];
                var cross = Vector3.Cross(norm, Vector3.UnitY).Normalized();
                var angle = Vector3.CalculateAngle(norm, Vector3.UnitY);
                if (float.IsNaN(angle) || float.IsInfinity(angle))
                    angle = 0;
                if (float.IsNaN(cross.X))
                    cross = Vector3.UnitX;
                var rot = Matrix3.CreateFromAxisAngle(cross, angle);


                var positions = new Vector3[]
                {
                    new Vector3(-1, 1, 1),
                    new Vector3(1, 1, 1),
                    new Vector3(1, 1, -1),
                    new Vector3(-1, 1, -1)
                };

                int curIdx = cubeMesh.Vertices.Count;

                for (int j = 0; j < 4; j++)
                {
                    var vert = new VertVN
                    {
                        Position = positions[j] * rot,//Vector3.TransformPosition(positions[j], trans),
                        Normal = norm
                    };
                    cubeMesh.Vertices.Add(vert);
                }

                cubeMesh.Indices.Add(curIdx);
                cubeMesh.Indices.Add(curIdx + 1);
                cubeMesh.Indices.Add(curIdx + 2);
                cubeMesh.Indices.Add(curIdx);
                cubeMesh.Indices.Add(curIdx + 2);
                cubeMesh.Indices.Add(curIdx + 3);
            }

            return cubeMesh;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.SkyBlue);

            Mesh = CreateCubeMesh();
            Mesh.UpdateBuffers();

            ProgramFactory.BasePath = "Rendering";
            ShaderProgram = ProgramFactory.Create<BasicShaderProgram>();
            ShaderProgram.Use();
            Mesh.BindToShader(ShaderProgram);

            ShaderProgram.MaterialColor.Set(new Vector4(0.7f, 0.7f, 0.7f, 1));
            ShaderProgram.DisplayWireframe.Set(true);

            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            SetupPerspective();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (Mesh != null)
            {
                //ShaderProgram.ModelViewMatrix.Set(ModelView);
                ShaderProgram.ModelViewProjectionMatrix.Set(ModelView * Projection);
                Mesh.Draw();
            }
            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Mesh.Dispose();
            ShaderProgram.Dispose();
            Mesh = null;
        }

        protected override void OnDisposed(EventArgs e)
        {
            
            base.OnDisposed(e);
        }
    }
}
