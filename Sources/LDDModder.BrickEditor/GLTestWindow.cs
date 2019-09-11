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
using System.IO;
using OpenTK.Input;

namespace LDDModder.BrickEditor
{
    class GLTestWindow : GameWindow
    {
        private int CurrentBrickIndex;
        private List<int> BrickIDList;
        private LDD.Data.LDDPartFiles CurrentBrick;
        private List<GLMeshBase> MeshList;
        private BasicShaderProgram BasicShader;
        private TexturedShaderProgram TexturedShader;
        private Texture2D DefaultTexture;

        private Matrix4 ViewMatrix;
        private Matrix4 Projection;

        private float CameraRotation = 0;
        private Vector3 LightPosition;
        private Vector3 CameraPosition;
        private Vector3 SceneCenter;
        private Stopwatch LogTimer;
        private string LddDbDirectory;

        public GLTestWindow() : base(800,600, new GraphicsMode(GraphicsMode.Default.ColorFormat, 24,8,4))
        {
            VSync = VSyncMode.Off;
            LightPosition = new Vector3(-5, 10, 5);
            MeshList = new List<GLMeshBase>();
            BrickIDList = new List<int>();
            
        }

        private void SetupPerspective()
        {
            // setup perspective projection
            var aspectRatio = Width / (float)Height;
            Projection = Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.PiOver4, aspectRatio, 0.1f, 1000);
            
            UpdateCamera();
        }

        private void SetupCamera()
        {
            CameraPosition = new Vector3(8, 8, 8);
            SceneCenter = Vector3.Zero;
            if (CurrentBrick != null)
            {
                if (CurrentBrick.Info.Bounding != null)
                {
                    SceneCenter = CurrentBrick.Info.Bounding.Center.ToGL();
                    //SceneCenter.Y = 0;

                    var brickSize = CurrentBrick.Info.Bounding.Size;
                    float maxSize = Math.Max(brickSize.X, Math.Max(brickSize.Y, brickSize.Z));
                    CameraPosition = new Vector3(maxSize + 2);
                }
                else if (CurrentBrick.Info.GeometryBounding != null)
                {
                    SceneCenter = CurrentBrick.Info.GeometryBounding.Center.ToGL();
                    //SceneCenter.Y = 0;

                    var brickSize = CurrentBrick.Info.GeometryBounding.Size;
                    float maxSize = Math.Max(brickSize.X, Math.Max(brickSize.Y, brickSize.Z));
                    CameraPosition = new Vector3(maxSize + 2);
                }
            }

            ViewMatrix = Matrix4.LookAt(CameraPosition, SceneCenter, Vector3.UnitY);
            CameraRotation = 0;
        }

        private void UpdateCamera()
        {
            var cameraRot = Matrix3.CreateRotationY(CameraRotation);
            
            var cameraPos = ((CameraPosition - SceneCenter) * cameraRot) + SceneCenter;
            ViewMatrix = Matrix4.LookAt(cameraPos, SceneCenter, Vector3.UnitY);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            CameraRotation += (float)(Math.PI / 6d * e.Time);
            CameraRotation %= ((float)Math.PI * 2f);
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
            LddDbDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\");

            InitializeGL();

            LoadBrickList();

            if (BrickIDList.Any() && CurrentBrickIndex >= 0 && CurrentBrickIndex < BrickIDList.Count)
            {
                LoadBrickModel(BrickIDList[CurrentBrickIndex]);
            }
            
            
            LogTimer = Stopwatch.StartNew();
        }

        private void InitializeGL()
        {
            GL.ClearColor(Color.SkyBlue);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

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

            SetupPerspective();
            SetupCamera();
        }

        private void LoadBrickList()
        {
            var path = Path.Combine(LddDbDirectory, "primitives");
            BrickIDList.Clear();

            foreach (string fname in Directory.EnumerateFiles(path, "*.xml"))
            {
                if (int.TryParse(Path.GetFileNameWithoutExtension(fname), out int brickID))
                    BrickIDList.Add(brickID);
            }
        }

        private void ClearMeshes()
        {
            if (MeshList.Any())
            {
                foreach (var m in MeshList)
                    m.Dispose();
                MeshList.Clear();
            }
        }

        private void LoadBrickModel(int brickID)
        {
            ClearMeshes();

            CurrentBrick = LDD.Data.LDDPartFiles.Read(LddDbDirectory, brickID);

            float curHue = 0;
            float hueStep = 1f / (float)(CurrentBrick.AllMeshes.Sum(x => x.Cullings.Count));

            foreach (var model in CurrentBrick.AllMeshes)
            {
                var shaderToUse = model.IsTextured ?
                    (ObjectTK.Shaders.Program)TexturedShader :
                    (ObjectTK.Shaders.Program)BasicShader;
                
                foreach (var cull in model.Cullings)
                {
                    var mesh = GLMeshBase.CreateFromGeometry(model.GetCullingGeometry(cull));

                    mesh.BindToProgram(shaderToUse);
                    //mesh.MaterialColor = new Color4(0.7f, 0.7f, 0.7f, 1);
                    mesh.MaterialColor = Color4.FromHsl(new Vector4(curHue, 1, 0.6f, 0.7f));
                    curHue = (curHue + hueStep) % 1f;

                    if (mesh is GLTexturedMesh texturedMesh)
                    {
                        mesh.MaterialColor = new Color4(1, 1, 1, 0.7f);
                        texturedMesh.Texture = DefaultTexture;
                    }

                    MeshList.Add(mesh);
                }

            }
            MeshList.Reverse();
            SetupCamera();
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

                    if (model.MaterialColor.A < 1f)
                    {
                        GL.Enable(EnableCap.CullFace);

                        GL.CullFace(CullFaceMode.Front);
                        model.Draw();

                        GL.CullFace(CullFaceMode.Back);
                        model.Draw();
                        GL.Disable(EnableCap.CullFace);
                    }
                    else
                    {
                        model.Draw();
                    }
                }
            }

            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            ClearMeshes();
            BasicShader.Dispose();
            TexturedShader.Dispose();
            if (DefaultTexture != null)
                DefaultTexture.Dispose();
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Key == Key.Left)
            {
                CurrentBrickIndex--;
                if (CurrentBrickIndex < 0)
                    CurrentBrickIndex = BrickIDList.Count - 1;
                if (CurrentBrickIndex >= 0 && CurrentBrickIndex < BrickIDList.Count)
                    LoadBrickModel(BrickIDList[CurrentBrickIndex]);
            }
            else if (e.Key == Key.Right)
            {
                CurrentBrickIndex = (++CurrentBrickIndex) % BrickIDList.Count;
                if (CurrentBrickIndex >= 0 && CurrentBrickIndex < BrickIDList.Count)
                    LoadBrickModel(BrickIDList[CurrentBrickIndex]);
            }
        }
    }
}
