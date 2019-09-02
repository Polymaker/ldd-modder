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
using System.Diagnostics;

namespace LDDModder.BrickEditor
{
    class GLTestWindow : GameWindow
    {
        private GLMesh Mesh;
        private List<GLMesh> MeshList;
        private BasicShaderProgram ShaderProgram;

        private Matrix4 ViewMatrix;
        private Matrix4 Projection;

        private float rotation = 0;
        private Vector3 LightPosition;
        private Stopwatch LogTimer;

        public GLTestWindow() : base(800,600, new GraphicsMode(GraphicsMode.Default.ColorFormat, 24,8,4))
        {
            VSync = VSyncMode.Off;
            LightPosition = new Vector3(-5, 10, 5);
            MeshList = new List<GLMesh>();
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
            var cameraPos = new Vector3(8, 8, 8) * cameraRot;
            ViewMatrix = Matrix4.LookAt(cameraPos, Vector3.Zero, Vector3.UnitY);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            rotation += (float)(Math.PI / 4d * e.Time);
            rotation %= ((float)Math.PI * 2f);
            UpdateCamera();

            if (LogTimer != null && LogTimer.ElapsedMilliseconds >= 1000)
            {
                Title = $"GLTest FPS: {UpdateFrequency:0}"; 
                LogTimer.Restart();
            }
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
                var test = Vector3.TransformNormal(Vector3.UnitY, new Matrix4(rot));
                if (Vector3.Distance(norm, test) > 0.5f)
                {
                    rot = Matrix3.CreateFromAxisAngle(cross, -angle);
                }
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

            

            ProgramFactory.BasePath = "Rendering";
            ShaderProgram = ProgramFactory.Create<BasicShaderProgram>();
            ShaderProgram.Use();
            
            ShaderProgram.MaterialColor.Set(new Vector4(0.7f, 0.7f, 0.7f, 1));
            ShaderProgram.DisplayWireframe.Set(true);
            ShaderProgram.LightPosition.Set(LightPosition);

            //Mesh = CreateCubeMesh();
            //Mesh.Transform = Matrix4.CreateRotationX(OpenTK.MathHelper.PiOver4);
            //Mesh.UpdateBuffers();
            var LddDbDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\");
            var brick = LDD.Data.PartMesh.Read(LddDbDirectory, 3020);
            //Mesh = GLMesh.FromGeometry(brick.MainModel.Geometry);
            
            //float curHue = 0;
            foreach (var cull in brick.MainModel.Cullings)
            {
                
                var m = GLMesh.FromGeometry(brick.MainModel.GetCullingGeometry(cull));
                //var color = Color4.FromHsl(new Vector4(curHue, 1, 0.6f, 0.9f));
                //m.Color = new Vector4(color.R, color.G, color.B, color.A);
                m.BindToShader(ShaderProgram);
                MeshList.Add(m);

                //curHue = (curHue + 0.06f) % 1f;
            }

            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.DepthTest);

            SetupPerspective();

            LogTimer = Stopwatch.StartNew();
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

            //if (Mesh != null)
            //{
            //    //ShaderProgram.Use();
            //    ShaderProgram.ModelMatrix.Set(Mesh.Transform);
            //    ShaderProgram.ViewMatrix.Set(ViewMatrix);
            //    ShaderProgram.ModelViewProjectionMatrix.Set(Mesh.Transform * ViewMatrix * Projection);
            //    Mesh.Draw();
            //}

            if (MeshList != null && MeshList.Any())
            {
                foreach(var m in MeshList)
                {
                    //ShaderProgram.MaterialColor.Set(m.Color);
                    ShaderProgram.ModelMatrix.Set(m.Transform);
                    ShaderProgram.ViewMatrix.Set(ViewMatrix);
                    ShaderProgram.ModelViewProjectionMatrix.Set(m.Transform * ViewMatrix * Projection);
                    m.Draw();
                }
            }
            /*
            GL.UseProgram(0);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref Projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref ViewMatrix);

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(10, 0, 0);
            GL.Color3(Color.Blue);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 10, 0);
            GL.Color3(Color.Yellow);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 10);
            GL.Color3(Color.White);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(LightPosition);
            GL.End();*/

            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            foreach (var m in MeshList)
            {
                m.Dispose();
            }
            MeshList.Clear();
            //Mesh.Dispose();
            ShaderProgram.Dispose();
            Mesh = null;
        }

    }
}
