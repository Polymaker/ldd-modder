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

        private GridShaderProgram GridShader;
        private ModelShaderProgram ModelShader;
        private WireframeShaderProgram WireframeShader;
        
        private List<GLModel> LoadedModels;

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
            LoadedModels = new List<GLModel>();
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
                Position = new Vector3(5)
            };
            Camera.LookAt(Vector3.Zero, Vector3.UnitY);

            CameraManipulator = new CameraManipulator(Camera);


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

            ModelShader = ProgramFactory.Create<ModelShaderProgram>();
            ModelShader.Use();

            var lights = new LightInfo[]
            {
                new LightInfo { 
                    Position = new Vector3(10, 10, 10), 
                    Ambient = new Vector3(0.3f),
                    Diffuse = new Vector3(0.8f),
                    Specular = new Vector3(1f),
                    Constant = 1f,
                    Linear = 0.09f,
                    Quadratic = 0.032f
                },
                new LightInfo {
                    Position = new Vector3(0, 30, 0),
                    Ambient = new Vector3(0.5f),
                    Diffuse = new Vector3(0.8f),
                    Specular = new Vector3(0.8f),
                    Constant = 1f,
                    Linear = 0.07f,
                    Quadratic = 0.017f
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
            GL.Disable(EnableCap.Texture2D);
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

            foreach (var model in LoadedModels)
            {
                model.BindToShader(WireframeShader);
                model.Draw();
                model.UnbindShader(WireframeShader);

                model.BindToShader(ModelShader);
                model.Draw();
                model.UnbindShader(ModelShader);
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

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            var winSize = new Vector2(250, 150);
            var winPos = new Vector2(
                (viewSize.X - winSize.X) / 2f,
                viewSize.Y - ((viewSize.Y - winSize.Y) / 2f));
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(Color.FromArgb(120, 80, 80, 80));

            GL.Vertex2(winPos.X, winPos.Y);
            GL.Vertex2(winPos.X, winPos.Y - winSize.Y);
            GL.Vertex2(winPos.X + winSize.X, winPos.Y - winSize.Y);
            GL.Vertex2(winPos.X + winSize.X, winPos.Y);
            GL.End();

            GL.Enable(EnableCap.Texture2D);

            TextRenderer.ProjectionMatrix = UIProjectionMatrix;
            TextRenderer.DrawingPrimitives.Clear();
            //var textHeight = RenderFont.Measure("Wasd").Height;

            //TextRenderer.AddText("Hello World!", RenderFont, Color.White,
            //    winPos, QFontAlignment.Left);
            TextRenderer.AddText("LDD Modder Splash Screen", RenderFont, Color.White,
                new Vector4(winPos.X, winPos.Y, winSize.X, winSize.Y), StringAlignment.Center, StringAlignment.Center);
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
            LoadedModels.ForEach(x => x.Dispose());
            LoadedModels.Clear();

            var partMeshes = project.Surfaces.SelectMany(x => x.GetAllMeshes()).ToList();

            float curHue = 0;

            foreach (var partMesh in partMeshes)
            {
                if (!partMesh.IsModelLoaded && !partMesh.LoadModel())
                    continue;

                var glModel = new GLModel();
                glModel.LoadFromLDD(partMesh.Geometry);

                glModel.Material = new MaterialInfo 
                {
                    Diffuse = new Vector4(0.6f, 0.6f, 0.6f, 1f),
                    Specular = new Vector3(1f),
                    Shininess = 8f
                };

                //glModel.BindToShader(ModelShader);
                
                LoadedModels.Add(glModel);
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

        private void DisposeGLResources()
        {
            LoadedModels.ForEach(x => x.Dispose());
            LoadedModels.Clear();
            GridShader.Dispose();
            ModelShader.Dispose();
            WireframeShader.Dispose();
        }
    }
}
