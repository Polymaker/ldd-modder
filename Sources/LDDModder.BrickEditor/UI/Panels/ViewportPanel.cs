using LDDModder.BrickEditor.Rendering;
using LDDModder.BrickEditor.Rendering.Shaders;
using LDDModder.Modding.Editing;
using ObjectTK.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using QuickFont;
using QuickFont.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Threading;
using System.Diagnostics;
using OpenTK.Input;
using ObjectTK.Textures;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ViewportPanel : ProjectDocumentPanel
    {
        private bool ViewInitialized;
        private QFontDrawing TextRenderer;
        private QFont RenderFont;
        private Matrix4 UIProjectionMatrix;
        private bool IsClosing;
        private bool RenderLoopEnabled;
        private GLControl glControl1;
        private Texture2D CheckboardTexture;

        private GridShaderProgram GridShader;
        private ModelShaderProgram ModelShader;
        private WireframeShaderProgram WireframeShader;
        
        private List<GLSurfaceModel> SurfaceModels;

        private Thread UpdateThread;

        private Camera Camera;
        private InputManager InputManager;
        private CameraManipulator CameraManipulator;

        public ViewportPanel()
        {
            InitializeComponent();
            glControl1 = new GLControl(new GraphicsMode(32, 24, 0, 8));
            glControl1.BackColor = Color.FromArgb(204, 204, 204);
            Controls.Add(glControl1);
            glControl1.Dock = DockStyle.Fill;
            //CloseButtonVisible = false;
            //CloseButton = false;
            DockAreas = DockAreas.Document;
            InputManager = new InputManager();

            SurfaceModels = new List<GLSurfaceModel>();
            glControl1.MouseEnter += GlControl1_MouseEnter;
            glControl1.MouseLeave += GlControl1_MouseLeave;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeBase();
            InitializeView();
            StartRenderLoop();

            UpdateThread = new Thread(UpdateLoop);
            UpdateThread.Start();
        }

        private void InitializeBase()
        {
            Camera = new Camera
            {
                Position = new Vector3(5),
            };
            Camera.LookAt(Vector3.Zero, Vector3.UnitY);

            CameraManipulator = new CameraManipulator(Camera);
            CameraManipulator.RotationButton = MouseButton.Right;

            RenderFont = new QFont("C:\\Windows\\Fonts\\segoeui.ttf", 10,
                new QFontBuilderConfiguration(true));
            TextRenderer = new QFontDrawing();

            GridShader = ProgramFactory.Create<GridShaderProgram>();
            GridShader.Use();

            GridShader.MajorGridLine.Set(new GridShaderProgram.GridLineInfo()
            {
                Color = new Color4(1, 1, 1, 0.8f),
                Spacing = 0.8f,
                Thickness = 1f,
                OffCenter = true
            });

            GridShader.MinorGridLine.Set(new GridShaderProgram.GridLineInfo()
            {
                Color = new Color4(0.8f, 0.6f, 0.6f, 0.8f),
                Spacing = 0.4f,
                Thickness = 0.75f,
                OffCenter = false
            });

            var checkboardImage = (Bitmap)Bitmap.FromStream(System.Reflection.Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("LDDModder.BrickEditor.Resources.Textures.DefaultTexture.png"));
            BitmapTexture.CreateCompatible(checkboardImage, out CheckboardTexture, 1);
            CheckboardTexture.LoadBitmap(checkboardImage, 0);
            CheckboardTexture.SetFilter(TextureMinFilter.Linear, TextureMagFilter.Nearest);
            CheckboardTexture.SetWrapMode(TextureWrapMode.Repeat);
            //CheckboardTexture.Bind(TextureUnit.Texture4);

            ModelShader = ProgramFactory.Create<ModelShaderProgram>();
            ModelShader.Use();
            
            var lights = new LightInfo[]
            {
                //Key Light
                new LightInfo { 
                    Position = new Vector3(10, 10, 10), 
                    Ambient = new Vector3(0.3f),
                    Diffuse = new Vector3(0.8f),
                    Specular = new Vector3(1f),
                    Constant = 1f,
                    Linear = 0.07f,
                    Quadratic = 0.017f
                },
                //Fill Light
                new LightInfo {
                    Position = new Vector3(-10, 6, 10),
                    Ambient = new Vector3(0.2f),
                    Diffuse = new Vector3(0.6f),
                    Specular = new Vector3(0.8f),
                    Constant = 1f,
                    Linear = 0.07f,
                    Quadratic = 0.017f
                },
                //Back Light
                new LightInfo {
                    Position = new Vector3(3, 10, -10),
                    Ambient = new Vector3(0.4f),
                    Diffuse = new Vector3(0.7f),
                    Specular = new Vector3(0.7f),
                    Constant = 1f,
                    Linear = 0.045f,
                    Quadratic = 0.075f
                },
            };
            ModelShader.Lights.Set(lights);

            ModelShader.LightCount.Set(lights.Length);
            ModelShader.UseTexture.Set(false);
            
            WireframeShader = ProgramFactory.Create<WireframeShaderProgram>();
            WireframeShader.Use();
            WireframeShader.Color.Set(new Vector4(0,0,0,1));
            WireframeShader.Thickness.Set(1f);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            StartRenderLoop();
        }

        private void StartRenderLoop()
        {
            if (!RenderLoopEnabled)
            {
                Application.Idle += Application_Idle;
                RenderLoopEnabled = true;
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            if (RenderLoopEnabled)
            {
                Application.Idle -= Application_Idle;
                RenderLoopEnabled = false;
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            OnRenderFrame();
        }

        private void InitializeView()
        {
            if (IsDisposed || IsClosing)
                return;

            UpdateViewport();

            GL.ClearColor(glControl1.BackColor);
            
            ViewInitialized = true;
            OnRenderFrame();
        }

        private void UpdateViewport()
        {
            glControl1.MakeCurrent();
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            Camera.Viewport = new RectangleF(0, 0, glControl1.Width, glControl1.Height);

            UIProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, glControl1.Width, 0, glControl1.Height, -1.0f, 1.0f);
        }

        #region Rendering

        private void RenderWorld()
        {
            GL.UseProgram(0);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            var viewMatrix = Camera.GetViewMatrix();
            var projection = Camera.GetProjectionMatrix();

            WireframeShader.Use();
            WireframeShader.ViewMatrix.Set(viewMatrix);
            WireframeShader.Projection.Set(projection);
            

            ModelShader.Use();
            ModelShader.ViewMatrix.Set(viewMatrix);
            ModelShader.Projection.Set(projection);
            ModelShader.ViewPosition.Set(Camera.Position);
            

            foreach (var surfaceModel in SurfaceModels)
            {
                

                var visibleMeshes = surfaceModel.MeshModels.Where(x => x.Visible).ToList();
                surfaceModel.BindToShader(WireframeShader);
                foreach (var mesh in visibleMeshes)
                    surfaceModel.DrawMesh(mesh);
                surfaceModel.UnbindShader(WireframeShader);
                
                surfaceModel.BindToShader(ModelShader);
                ModelShader.UseTexture.Set(surfaceModel.Surface.SurfaceID > 0);
                CheckboardTexture.Bind(TextureUnit.Texture4);
                ModelShader.Texture.BindTexture(TextureUnit.Texture4, CheckboardTexture);

                foreach (var mesh in visibleMeshes)
                    surfaceModel.DrawMesh(mesh);
                surfaceModel.UnbindShader(ModelShader);
            }

            DrawGrid(viewMatrix, projection);

            GL.UseProgram(0);
        }

        private void DrawGrid(Matrix4 viewMatrix, Matrix4 projection)
        {
            GridShader.Use();
            GridShader.MVMatrix.Set(viewMatrix);
            GridShader.PMatrix.Set(projection);

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(-40, 0, -40);
            GL.Vertex3(-40, 0, 40);
            GL.Vertex3(40, 0, 40);
            GL.Vertex3(40, 0, -40);
            GL.End();


        }

        private void RenderUI()
        {
            var viewSize = new Vector2(glControl1.Width, glControl1.Height);

            GL.Disable(EnableCap.DepthTest);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref UIProjectionMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            //GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            var winSize = new Vector2(250, 150);
            var winPos = new Vector2(
                (viewSize.X - winSize.X) / 2f,
                viewSize.Y - ((viewSize.Y - winSize.Y) / 2f));
            //GL.Begin(PrimitiveType.Quads);
            //GL.Color4(Color.FromArgb(120, 80, 80, 80));

            //GL.Vertex2(winPos.X, winPos.Y);
            //GL.Vertex2(winPos.X, winPos.Y - winSize.Y);
            //GL.Vertex2(winPos.X + winSize.X, winPos.Y - winSize.Y);
            //GL.Vertex2(winPos.X + winSize.X, winPos.Y);
            //GL.End();

            GL.Enable(EnableCap.Texture2D);
            GL.UseProgram(0);
            TextRenderer.ProjectionMatrix = UIProjectionMatrix;
            TextRenderer.DrawingPrimitives.Clear();
            //var textHeight = RenderFont.Measure("Wasd").Height;
            TextRenderer.AddText("Hello", RenderFont, Color.White, new Vector2(0.5f, 20f), QFontAlignment.Left);
            TextRenderer.AddText("Hello World!", RenderFont, Color.White,
                winPos, QFontAlignment.Left);
            //TextRenderer.AddText("LDD Modder Splash Screen", RenderFont, Color.White,
            //    new Vector4(winPos.X, winPos.Y, winSize.X, winSize.Y), StringAlignment.Center, StringAlignment.Center);
            TextRenderer.RefreshBuffers();
            TextRenderer.Draw();
            TextRenderer.DisableShader();
        }

        private void OnRenderFrame()
        {
            if (IsDisposed || IsClosing)
                return;

            glControl1.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            RenderWorld();

            //GL.Clear(ClearBufferMask.DepthBufferBit);
            //RenderUI();

            glControl1.SwapBuffers();
        }

        #endregion

        private void UpdateLoop()
        {
            var sw = Stopwatch.StartNew();
            double lastTime = 0;
            const double MaxTimer = 1000000;

            while (!IsClosing)
            {
                double curTime = sw.Elapsed.TotalMilliseconds;
                if (curTime > MaxTimer)
                {
                    lastTime -= curTime;
                    sw.Restart();
                    curTime = 0;
                }

                double delta = curTime - lastTime;
                lastTime = curTime;

                OnUpdateFrame(delta);
            }
            sw.Stop();
        }

        private void GlControl1_MouseEnter(object sender, EventArgs e)
        {
            InputManager.ContainsMouse = true;
        }

        private void GlControl1_MouseLeave(object sender, EventArgs e)
        {
            InputManager.ContainsMouse = false;
        }

        private void OnUpdateFrame(double deltaTime)
        {
            InputManager.UpdateInputStates();
            CameraManipulator.HandleCamera(InputManager);
        }

        private void ViewportPanel_SizeChanged(object sender, EventArgs e)
        {
            if (ViewInitialized && !Disposing)
            {
                UpdateViewport();
                OnRenderFrame();
            }
        }

        public void LoadPartProject(PartProject project)
        {
            UnloadModels();

            float curHue = 0;

            foreach (var surface in project.Surfaces)
            {
                var surfModel = new GLSurfaceModel(surface);
                surfModel.Material = new MaterialInfo
                {
                    Diffuse = new Vector4(0.6f, 0.6f, 0.6f, 1f),
                    Specular = new Vector3(1f),
                    Shininess = 6f
                };

                if (surface.SurfaceID > 0)
                {
                    var matColor = Color4.FromHsv(new Vector4((surface.SurfaceID * 0.2f) % 1f, 0.9f, 0.8f, 1f));
                    surfModel.Material = new MaterialInfo
                    {
                        Diffuse = new Vector4(matColor.R, matColor.G, matColor.B, matColor.A),
                        Specular = new Vector3(1f),
                        Shininess = 6f
                    };
                }
                surfModel.RebuildPartModels();
                SurfaceModels.Add(surfModel);
            }

            if (project.Bounding != null)
            {
                var partCenter = project.Bounding.Center.ToGL();
                partCenter.Y = 0;
                var brickSize = project.Bounding.Size;
                float maxSize = Math.Max(brickSize.X, Math.Max(brickSize.Y, brickSize.Z));
                var camPos = new Vector3(maxSize + 2);
                camPos.Y = Math.Max(project.Bounding.MaxY + 2, 6);
                Camera.Position = camPos;
                CameraManipulator.Gimbal = partCenter;
            }
            else
            {
                Camera.Position = new Vector3(5);
                CameraManipulator.Gimbal = Vector3.Zero;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            IsClosing = true;
            DisposeGLResources();
        }

        private void UnloadModels()
        {
            SurfaceModels.ForEach(x => x.Dispose());
            SurfaceModels.Clear();
        }

        private void DisposeGLResources()
        {
            UnloadModels();
            GridShader.Dispose();
            ModelShader.Dispose();
            WireframeShader.Dispose();
            CheckboardTexture.Dispose();
        }
    }
}
