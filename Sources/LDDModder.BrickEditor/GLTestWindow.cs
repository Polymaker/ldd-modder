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
using ObjectTK.Textures;

namespace LDDModder.BrickEditor
{
    class GLTestWindow : GameWindow
    {
        private List<GLMeshBase> MeshList;
        private BasicShaderProgram BasicShader;
        private TexturedShaderProgram TexturedShader;
        private Texture2D DefaultTexture;

        private Matrix4 ViewMatrix;
        private Matrix4 Projection;

        private float rotation = 0;
        private Vector3 LightPosition;
        private Stopwatch LogTimer;

        public GLTestWindow() : base(800,600, new GraphicsMode(GraphicsMode.Default.ColorFormat, 24,8,4))
        {
            VSync = VSyncMode.Off;
            LightPosition = new Vector3(-5, 10, 5);
            MeshList = new List<GLMeshBase>();
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

            rotation += (float)(Math.PI / 6d * e.Time);
            rotation %= ((float)Math.PI * 2f);
            UpdateCamera();

            if (LogTimer != null && LogTimer.ElapsedMilliseconds >= 1000)
            {
                Title = $"GLTest FPS: {UpdateFrequency:0}"; 
                LogTimer.Restart();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.SkyBlue);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            var texStream = System.Reflection.Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("LDDModder.BrickEditor.Resources.Textures.DefaultTexture.png");
            var texImage = (Bitmap)Image.FromStream(texStream);
            BitmapTexture.CreateCompatible(texImage, out DefaultTexture, 1);
            DefaultTexture.LoadBitmap(texImage, 0);
            DefaultTexture.SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);

            ProgramFactory.BasePath = "Rendering";
            BasicShader = ProgramFactory.Create<BasicShaderProgram>();
            BasicShader.Use();
            BasicShader.DisplayWireframe.Set(true);
            BasicShader.LightPosition.Set(LightPosition);

            TexturedShader = ProgramFactory.Create<TexturedShaderProgram>();
            TexturedShader.Use();
            TexturedShader.DisplayWireframe.Set(true);
            TexturedShader.LightPosition.Set(LightPosition);

            var LddDbDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\");
            var brick = LDD.Data.LDDPartFiles.Read(LddDbDirectory, 99380);
            //Mesh = GLMesh.FromGeometry(brick.MainModel.Geometry);
            
            //float curHue = 0;
            foreach(var model in brick.AllMeshes)
            {
                var shaderToUse = model.IsTextured ? 
                    (ObjectTK.Shaders.Program)TexturedShader : 
                    (ObjectTK.Shaders.Program)BasicShader;
                model.Geometry.SeparateDistinctSurfaces();
                foreach (var cull in model.Cullings)
                {
                    var mesh = GLMeshBase.CreateFromGeometry(model.GetCullingGeometry(cull));

                    mesh.BindToProgram(shaderToUse);
                    mesh.MaterialColor = new Color4(0.7f, 0.7f, 0.7f, 1);

                    if (mesh is GLTexturedMesh texturedMesh)
                    {
                        mesh.MaterialColor = Color4.White;
                        texturedMesh.Texture = DefaultTexture;
                    }
                    //var color = Color4.FromHsl(new Vector4(curHue, 1, 0.6f, 0.9f));
                    //m.Color = new Vector4(color.R, color.G, color.B, color.A);
                    //curHue = (curHue + 0.06f) % 1f;

                    MeshList.Add(mesh);
                }

            }
            

            
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

            if (MeshList != null && MeshList.Any())
            {
                foreach (var model in MeshList)
                {
                    model.BoundProgram.Use();
                    model.AssignShaderValues(ViewMatrix, Projection);
                    model.Draw();
                }
            }

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
            BasicShader.Dispose();
            TexturedShader.Dispose();
            if (DefaultTexture != null)
                DefaultTexture.Dispose();
        }

    }
}
