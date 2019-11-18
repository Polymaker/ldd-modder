﻿using LDDModder.BrickEditor.Rendering;
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
using LDDModder.BrickEditor.EditModels;
using LDDModder.BrickEditor.Resources;
using LDDModder.BrickEditor.Rendering.Gizmos;

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

        private List<GLSurfaceModel> SurfaceModels;
        private List<CollisionModel> CollisionModels;

        private Thread UpdateThread;
        private bool ExitUpdateLoop;

        private InputManager InputManager;
        private CameraManipulator CameraManipulator;
        private Camera Camera => CameraManipulator.Camera;

        private GLModel BoxCollisionModel;
        private GLModel SphereCollisionModel;
        private TransformGizmo TransformGizmo;

        public bool ShowCollisions { get; set; }
        public bool ShowMeshes { get; set; }

        private double UpdateFPS { get; set; }

        private double RenderFPS { get; set; }

        public RenderOptions ModelRenderingOptions { get; private set; }

        public ViewportPanel()
        {
            InitializeComponent();
            SurfaceModels = new List<GLSurfaceModel>();
            CollisionModels = new List<CollisionModel>();
            
        }

        public ViewportPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
            //CloseButtonVisible = false;
            //CloseButton = false;
            DockAreas = DockAreas.Document;

            SurfaceModels = new List<GLSurfaceModel>();
            CollisionModels = new List<CollisionModel>();
            CreateGLControl();
        }

        private void CreateGLControl()
        {
            glControl1 = new GLControl(new GraphicsMode(32, 24, 2, 8));
            glControl1.BackColor = Color.FromArgb(204, 204, 204);
            Controls.Add(glControl1);
            glControl1.Dock = DockStyle.Fill;
            glControl1.BringToFront();
            glControl1.MouseEnter += GlControl_MouseEnter;
            glControl1.MouseLeave += GlControl_MouseLeave;
            glControl1.MouseMove += GlControl_MouseMove;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            visualStudioToolStripExtender1.SetStyle(toolStrip1,
                VisualStudioToolStripExtender.VsVersion.Vs2015,
                DockPanel.Theme);

            InitializeBase();
            UpdateViewport();

            StartRenderLoop();
            StartUpdateLoop();

            InitializeMenus();
            
        }

        #region Initialization

        private void InitializeBase()
        {
            InputManager = new InputManager();

            //Camera = new Camera();
            CameraManipulator = new CameraManipulator(new Camera());
            CameraManipulator.Initialize(new Vector3(5), Vector3.Zero);
            //CameraManipulator.RotationButton = MouseButton.Right;

            ShowMeshes = true;
            ModelRenderingOptions = new RenderOptions()
            {
                DrawShaded = true,
                DrawTextured = true,
                DrawTransparent = false,
                DrawWireframe = true
            };

            InitializeTextures();
            
            InitializeShaders();

            InitializeModels();

            InitializeFonts();
        }

        private void InitializeMenus()
        {
            globalToolStripMenuItem.Tag = OrientationMode.Global.ToString();
            localToolStripMenuItem.Tag = OrientationMode.Local.ToString();

            boundingBoxCenterToolStripMenuItem.Tag = PivotPointMode.BoundingBox.ToString();
            cursorToolStripMenuItem.Tag = PivotPointMode.Cursor.ToString();
            medianBoundingBoxToolStripMenuItem.Tag = PivotPointMode.MedianCenter.ToString();
            medianOriginsToolStripMenuItem.Tag = PivotPointMode.MedianOrigin.ToString();
            activeElementToolStripMenuItem.Tag = PivotPointMode.ActiveElement.ToString();

            SelectCurrentGizmoOptions();
        }

        private void InitializeTextures()
        {
            var checkboardImage = (Bitmap)Bitmap.FromStream(System.Reflection.Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("LDDModder.BrickEditor.Resources.Textures.DefaultTexture.png"));
            BitmapTexture.CreateCompatible(checkboardImage, out CheckboardTexture, 1);
            CheckboardTexture.LoadBitmap(checkboardImage, 0);
            CheckboardTexture.SetFilter(TextureMinFilter.Linear, TextureMagFilter.Linear);
            CheckboardTexture.SetWrapMode(TextureWrapMode.Repeat);
        }

        private void InitializeModels()
        {
            var modelScene = ResourceHelper.GetResourceModel("Models.Cube.obj", "obj");
            BoxCollisionModel = GLModel.CreateFromAssimp(modelScene.Meshes[0]);
            BoxCollisionModel.Material = new MaterialInfo
            {
                Diffuse = new Vector4(1f, 0.05f, 0.05f, 1f),
                Specular = new Vector3(1f),
                Shininess = 2f
            };

            modelScene = ResourceHelper.GetResourceModel("Models.Sphere.obj", "obj");
            SphereCollisionModel = GLModel.CreateFromAssimp(modelScene.Meshes[0]);
            SphereCollisionModel.Material = new MaterialInfo
            {
                Diffuse = new Vector4(1f, 0.05f, 0.05f, 1f),
                Specular = new Vector3(1f),
                Shininess = 2f
            };

            TransformGizmo = new TransformGizmo();
            TransformGizmo.InitializeVBO();
            TransformGizmo.PivotPointMode = PivotPointMode.MedianCenter;
            TransformGizmo.OrientationMode = OrientationMode.Global;
        }

        private void InitializeFonts()
        {
            RenderFont = new QFont("C:\\Windows\\Fonts\\segoeui.ttf", 10,
                new QFontBuilderConfiguration(true));
            TextRenderer = new QFontDrawing();
        }

        private void InitializeShaders()
        {
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

            RenderHelper.InitializeShaders();
            RenderHelper.ModelShader.Use();

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

            RenderHelper.ModelShader.Lights.Set(lights);
            RenderHelper.ModelShader.LightCount.Set(lights.Length);
            RenderHelper.ModelShader.UseTexture.Set(false);
        }

        #endregion

        private void UpdateViewport()
        {
            if (IsDisposed || IsClosing)
                return;

            glControl1.MakeCurrent();
            if (!ViewInitialized)
                GL.ClearColor(glControl1.BackColor);

            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            Camera.Viewport = new RectangleF(0, 0, glControl1.Width, glControl1.Height);

            UIProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, glControl1.Width, 0, glControl1.Height, -1.0f, 1.0f);

            if (!ViewInitialized)
                ViewInitialized = true;

            if (TransformGizmo.Visible)
                TransformGizmo.UpdateBounds(Camera);
        }


        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            StartRenderLoop();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            StopRenderLoop();
        }


        #region Render Loop

        private Stopwatch RenderTimer;
        private double LastRenderTime = 0;

        private void StartRenderLoop()
        {
            if (!RenderLoopEnabled)
            {
                RenderTimer = new Stopwatch();
                RenderTimer.Start();
                LastRenderTime = 0;
                Application.Idle += Application_Idle;
                RenderLoopEnabled = true;
            }
        }

        private void StopRenderLoop()
        {
            if (RenderLoopEnabled)
            {
                Application.Idle -= Application_Idle;
                RenderLoopEnabled = false;
                RenderTimer.Stop();
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            double curTime = RenderTimer.Elapsed.TotalMilliseconds;
            const double MaxTimer = 1000000;
            if (curTime > MaxTimer)
            {
                LastRenderTime -= curTime;
                RenderTimer.Restart();
                curTime = 0;
            }

            double delta = curTime - LastRenderTime;
            RenderFPS = 1000d / delta;
            LastRenderTime = curTime;

            OnRenderFrame();
        }

        private void RenderWorld()
        {
            GL.UseProgram(0);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            var viewMatrix = Camera.GetViewMatrix();
            var projection = Camera.GetProjectionMatrix();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMatrix);

            RenderHelper.InitializeMatrices(Camera);

            if (ShowCollisions)
                DrawCollisions();

            DrawConnections();

            if (!ModelRenderingOptions.Hidden)
                DrawPartModels();

            DrawGrid();

            GL.UseProgram(0);

            if (TransformGizmo.Visible)
            {
                GL.Clear(ClearBufferMask.DepthBufferBit);
                TransformGizmo.Render();
            }

            GL.UseProgram(0);
        }

        private void DrawConnections()
        {

        }

        private void DrawCollisions()
        {
            GL.Disable(EnableCap.Texture2D);
            if (CollisionModels.Any())
            {
                RenderHelper.UnbindModelTexture();
                foreach (var colModel in CollisionModels.Where(x => x.Visible))
                    colModel.RenderModel(Camera);
            }
        }

        private void DrawPartModels()
        {
            GL.Enable(EnableCap.Texture2D);

            RenderHelper.BindModelTexture(CheckboardTexture, TextureUnit.Texture4);

            if (ModelRenderingOptions.DrawTransparent)
            {
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(CullFaceMode.Front);
                GL.Disable(EnableCap.DepthTest);
                var wireColor = new Vector4(0f, 0f, 0f, 1f);
                foreach (var surfaceModel in SurfaceModels)
                    surfaceModel.RenderWireframe(wireColor, 0.5f);

                bool wireframeEnabled = ModelRenderingOptions.DrawWireframe;
                ModelRenderingOptions.DrawWireframe = false;

                
                GL.Disable(EnableCap.CullFace);

                foreach (var surfaceModel in SurfaceModels)
                    surfaceModel.Render(ModelRenderingOptions, true);


                GL.Enable(EnableCap.CullFace);
                GL.CullFace(CullFaceMode.Back);

                foreach (var surfaceModel in SurfaceModels)
                    surfaceModel.RenderWireframe(wireColor, 0.5f);

                GL.Disable(EnableCap.CullFace);
                GL.Enable(EnableCap.DepthTest);

                ModelRenderingOptions.DrawWireframe = wireframeEnabled;
                
                //var wireframeOptions = new RenderOptions
                //{
                //    DrawWireframe = true
                //};

                //foreach (var surfaceModel in SurfaceModels)
                //    surfaceModel.Render(wireframeOptions, true);
            }
            else
            {
                foreach (var surfaceModel in SurfaceModels)
                    surfaceModel.Render(ModelRenderingOptions);
            }

            RenderHelper.UnbindModelTexture();
            CheckboardTexture.Bind(TextureUnit.Texture0);
            GL.Disable(EnableCap.Texture2D);
        }

        private void DrawGrid()
        {
            GridShader.Use();
            GridShader.MVMatrix.Set(Camera.GetViewMatrix());
            GridShader.PMatrix.Set(Camera.GetProjectionMatrix());
            GridShader.FadeDistance.Set(Camera.IsPerspective ? 20f : 0f);

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
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //var winSize = new Vector2(250, 150);
            //var winPos = new Vector2(
            //    (viewSize.X - winSize.X) / 2f,
            //    viewSize.Y - ((viewSize.Y - winSize.Y) / 2f));
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

            var textHeight = RenderFont.Measure("Wasd").Height;

            TextRenderer.AddText($"Render FPS: {RenderFPS:0.00}", RenderFont, 
                Color.White, new Vector2(2f, viewSize.Y - 3), QFontAlignment.Left);
            TextRenderer.AddText($"Update FPS {UpdateFPS:0.00}", RenderFont,
                Color.White, new Vector2(2f, viewSize.Y - textHeight - 9), QFontAlignment.Left);

            if (TransformGizmo.IsEditing)
            {
                TextRenderer.AddText($"Tranform: {TransformGizmo.EditAmount:0.##}", RenderFont,
                    Color.White, new Vector2(2f, viewSize.Y - ((textHeight + 6) * 2) - 3), QFontAlignment.Left);
            }

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

            GL.Clear(ClearBufferMask.DepthBufferBit);
            RenderUI();

            glControl1.SwapBuffers();
        }

        #endregion

        #region Update Loop

        private void StartUpdateLoop()
        {
            if (UpdateThread == null || !UpdateThread.IsAlive)
            {
                UpdateThread = new Thread(UpdateLoop);
                UpdateThread.Start();
            }
        }

        private void StopUpdateLoop()
        {
            if (UpdateThread != null)
            {
                if (UpdateThread.IsAlive)
                {
                    ExitUpdateLoop = true;
                    UpdateThread.Join();
                }
                UpdateThread = null;
                ExitUpdateLoop = false;
            }
        }

        private void UpdateLoop()
        {
            var sw = Stopwatch.StartNew();
            double lastTime = 0;
            const double MaxTimer = 1000000;

            while (!IsClosing)
            {
                if (ExitUpdateLoop)
                    break;

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
                Thread.Sleep(15);
            }
            sw.Stop();
        }

        private void OnUpdateFrame(double deltaTime)
        {
            UpdateFPS = 1000.0 / deltaTime;

            InputManager.UpdateInputStates();
            CameraManipulator.ProcessInput(InputManager);

            if (TransformGizmo.Visible)
            {
                if (Camera.IsDirty)
                    TransformGizmo.UpdateBounds(Camera);
                TransformGizmo.ProcessInput(Camera, InputManager);
            }

            if (!TransformGizmo.Selected && InputManager.ContainsMouse && InputManager.IsButtonClicked(MouseButton.Left))
            {
                var ray = Camera.RaycastFromScreen(InputManager.LocalMousePos);
                PerformRaySelection(ray);
            }
        }

        #endregion

        private void ViewportPanel_SizeChanged(object sender, EventArgs e)
        {
            if (ViewInitialized && !Disposing)
            {
                UpdateViewport();
                OnRenderFrame();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            IsClosing = true;
            StopUpdateLoop();
            StopRenderLoop();
            DisposeGLResources();
        }

        private void UnloadModels()
        {
            SurfaceModels.ForEach(x => x.Dispose());
            SurfaceModels.Clear();
            CollisionModels.Clear();
            GC.Collect();
        }

        private void DisposeGLResources()
        {
            UnloadModels();

            RenderHelper.DisposeShaders();
            BoxCollisionModel.Dispose();
            SphereCollisionModel.Dispose();

            TransformGizmo.Dispose();

            GridShader.Dispose();
            CheckboardTexture.Dispose();
        }

        #region Project Handling

        protected override void OnProjectLoaded(PartProject project)
        {
            base.OnProjectLoaded(project);

            RebuildSurfaceModels();
            RebuildCollisionModels();

            SetupDefaultCamera();
        }

        private void AddPartSurfaceModel(PartSurface surface)
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
                    Diffuse = new Vector4(matColor.R, matColor.G, matColor.B, surfModel.Material.Diffuse.W),
                    Specular = new Vector3(1f),
                    Shininess = 6f
                };
            }

            surfModel.RebuildPartModels();
            SurfaceModels.Add(surfModel);
        }

        protected override void OnProjectClosed()
        {
            base.OnProjectClosed();
            TransformGizmo.Deactivate();
            UnloadModels();
        }

        protected override void OnProjectElementsChanged(CollectionChangedEventArgs e)
        {
            base.OnProjectElementsChanged(e);

            
            if (e.ElementType == typeof(PartSurface))
            {
                if (e.Action == CollectionChangeAction.Add)
                {
                    foreach (PartSurface surface in e.AddedElements)
                        AddPartSurfaceModel(surface);
                }
                else
                {
                    foreach (PartSurface surface in e.RemovedElements)
                    {
                        var model = SurfaceModels.FirstOrDefault(x => x.Surface == surface);
                        if (model != null)
                        {
                            SurfaceModels.Remove(model);
                            model.Dispose();
                        }
                    }
                }
            }
            else if (e.ElementType == typeof(ModelMeshReference))
            {
                var addedMeshes = e.AddedElements.OfType<ModelMeshReference>();
                var changedSurfaces = addedMeshes.Select(x => (x.Parent as SurfaceComponent)?.Surface).Distinct().ToList();

                UpdateSurfacesModels(changedSurfaces);

            }
            else if (e.ElementType == typeof(SurfaceComponent))
            {
                var changedSurfaces = e.AddedElements.OfType<SurfaceComponent>()
                    .Concat(e.RemovedElements.OfType<SurfaceComponent>()).Select(x=>x.Surface).Distinct();

                UpdateSurfacesModels(changedSurfaces);
            }
            else if (e.ElementType == typeof(PartCollision))
            {
                RebuildCollisionModels();
            }
        }

        private void UpdateSurfacesModels(IEnumerable<PartSurface> changedSurfaces)
        {
            foreach (var surface in changedSurfaces)
            {
                bool isRemoved = !CurrentProject.Surfaces.Contains(surface);
                var model = SurfaceModels.FirstOrDefault(x => x.Surface == surface);
                if (isRemoved && model != null)
                {
                    SurfaceModels.Remove(model);
                    model.Dispose();
                }
                else if (!isRemoved)
                {
                    if (model != null)
                        model.RebuildPartModels();
                    else
                        AddPartSurfaceModel(surface);
                }
            }
        }

        private void RebuildSurfaceModels()
        {
            if (SurfaceModels.Any())
            {
                SurfaceModels.ForEach(x => x.Dispose());
                SurfaceModels.Clear();
            }

            if (CurrentProject != null)
            {
                foreach (var surface in CurrentProject.Surfaces)
                    AddPartSurfaceModel(surface);
            }
        }

        private void RebuildCollisionModels()
        {
            CollisionModels.Clear();
            if (CurrentProject != null)
            {
                foreach (var col in CurrentProject.Collisions)
                {
                    if (col is PartBoxCollision boxCollision)
                        CollisionModels.Add(new CollisionModel(col, BoxCollisionModel));
                    else if (col is PartSphereCollision sphereCollision)
                        CollisionModels.Add(new CollisionModel(col, SphereCollisionModel));
                }
            }
        }

        public IEnumerable<PartElementModel> GetAllElementModels()
        {
            foreach (var model in SurfaceModels.SelectMany(x => x.MeshModels))
                yield return model;

            foreach (var model in CollisionModels)
                yield return model;
        }

        public IEnumerable<PartElementModel> GetVisibleModels()
        {
            if (!ModelRenderingOptions.Hidden)
            {
                foreach (var model in SurfaceModels.SelectMany(x => x.MeshModels).Where(x=>x.Visible))
                    yield return model;
            }

            if (ShowCollisions)
            {
                foreach (var model in CollisionModels.Where(x => x.Visible))
                    yield return model;
            }
        }

        public IEnumerable<PartElementModel> GetSelectedModels(bool onlyVisible = false)
        {
            var selectedModels = GetAllElementModels().Where(x => x.IsSelected);
            return selectedModels
                .Where(x => !onlyVisible || (onlyVisible && x.Visible))
                .OrderBy(x => ProjectManager.GetSelectionIndex(x.Element));
        }

        #endregion

        #region Selection Handling

        protected override void OnElementSelectionChanged()
        {
            base.OnElementSelectionChanged();

            foreach (var model in GetAllElementModels())
                model.IsSelected = ProjectManager.IsContainedInSelection(model.Element);

            UpdateGizmoFromSelection();
        }

        private void UpdateGizmoFromSelection()
        {
            var selectedModels = GetSelectedModels(true);

            if (selectedModels.Any())
            {
                TransformGizmo.ActivateForModels(selectedModels, 
                    TransformGizmo.OrientationMode, 
                    TransformGizmo.PivotPointMode);
                TransformGizmo.UpdateBounds(Camera);
            }
            else
            {
                TransformGizmo.Deactivate();
            }
        }

        private void PerformRaySelection(Ray ray)
        {
            var visibleModels = GetVisibleModels();

            if (ray != null && visibleModels.Any())
            {

                var intersectingModels = new List<Tuple<PartElementModel, float>>();
                int ctr = 0;
                foreach (var model in visibleModels)
                {
                    if (model.RayIntersectsBoundingBox(ray, out float boxDist))
                    {
                        if (model.RayIntersects(ray, out float triangleDist))
                        {
                            intersectingModels.Add(new Tuple<PartElementModel, float>(model, triangleDist));
                        }
                    }
                    ctr++;
                }

                var closestHit = intersectingModels.OrderBy(x => x.Item2).FirstOrDefault();
                var closestElement = closestHit?.Item1.Element;

                if (InputManager.IsControlDown())
                {
                    if (closestElement != null)
                        ProjectManager.SetSelected(closestElement, !ProjectManager.IsSelected(closestElement));
                }
                else if (InputManager.IsShiftDown())
                {
                    if (closestElement != null)
                        ProjectManager.SetSelected(closestElement, true);
                }
                else
                {
                    ProjectManager.SelectElement(closestElement);
                }
            }
        }

        #endregion

        #region Viewport Handling

        private void SetupDefaultCamera()
        {
            if (CurrentProject != null && CurrentProject.DefaultOrientation != null)
            {

            }
            ResetCameraAlignment(CameraAlignment.Isometric);
        }

        private BBox CalculateBoundingBox(IEnumerable<ModelBase> modelMeshes)
        {
            Vector3 minPos = new Vector3(99999f);
            Vector3 maxPos = new Vector3(-99999f);

            foreach (var model in modelMeshes)
            {
                var worldBounding = model.GetWorldBoundingBox();
                minPos = Vector3.ComponentMin(minPos, worldBounding.Min);
                maxPos = Vector3.ComponentMax(maxPos, worldBounding.Max);
            }

            return BBox.FromMinMax(minPos, maxPos);
        }

        public void ResetCameraAlignment(CameraAlignment alignment)
        {
            var visibleModels = GetVisibleModels();
            var bounding = CalculateBoundingBox(visibleModels);

            Vector3 cameraDirection = Vector3.Zero;
            Vector3 upVector = Vector3.UnitY;

            switch (alignment)
            {
                case CameraAlignment.Isometric:
                    cameraDirection = new Vector3(1).Normalized();
                    break;
                case CameraAlignment.Front:
                    cameraDirection = new Vector3(0, 0, 1);
                    break;
                case CameraAlignment.Back:
                    cameraDirection = new Vector3(0, 0, -1);
                    break;
                case CameraAlignment.Left:
                    cameraDirection = new Vector3(-1, 0, 0);
                    break;
                case CameraAlignment.Right:
                    cameraDirection = new Vector3(1, 0, 0);
                    break;
                case CameraAlignment.Top:
                    cameraDirection = new Vector3(0, 1, 0);
                    upVector = Vector3.UnitZ * -1f;
                    break;
                case CameraAlignment.Bottom:
                    cameraDirection = new Vector3(0, -1, 0);
                    upVector = Vector3.UnitZ;
                    break;
            }

            float distanceToTarget = 3f;
            Vector2 targetSize = new Vector2(2f);

            if (visibleModels.Any())
            {
                var viewRay = new Ray(bounding.Center, cameraDirection);

                switch (alignment)
                {
                    case CameraAlignment.Isometric:
                        targetSize = new Vector2(bounding.Size.Length);
                        distanceToTarget = bounding.Extents.Length;
                        break;
                    case CameraAlignment.Front:
                    case CameraAlignment.Back:
                        targetSize = new Vector2(bounding.SizeX, bounding.SizeY);
                        distanceToTarget = bounding.SizeZ / 2f;
                        break;
                    case CameraAlignment.Left:
                    case CameraAlignment.Right:
                        targetSize = new Vector2(bounding.SizeZ, bounding.SizeY);
                        distanceToTarget = bounding.SizeX / 2f;
                        break;
                    case CameraAlignment.Top:
                    case CameraAlignment.Bottom:
                        targetSize = new Vector2(bounding.SizeX, bounding.SizeZ);
                        distanceToTarget = bounding.SizeY / 2f;
                        break;
                }

                if (Ray.IntersectsBox(viewRay, bounding, out float distance))
                    distanceToTarget = Math.Abs(distance);

            }

            Camera.FitOrtographicSize(targetSize + new Vector2(0.1f));

            distanceToTarget += Camera.GetDistanceForSize(targetSize);
            var cameraPos = bounding.Center + cameraDirection * distanceToTarget;
            CameraManipulator.Initialize(cameraPos, bounding.Center, upVector);
        }

        #endregion

        #region Control Events

        private void GlControl_MouseEnter(object sender, EventArgs e)
        {
            InputManager.ContainsMouse = true;
        }

        private void GlControl_MouseLeave(object sender, EventArgs e)
        {
            InputManager.ContainsMouse = false;
        }

        private void GlControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            InputManager.ProcessMouseMove(e);
        }

        #endregion

        #region Toolbar Menu

        private void Camera_ResetCameraMenu_Click(object sender, EventArgs e)
        {
            SetupDefaultCamera();
        }

        private void CameraMenu_AlignTo_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var alignment = (CameraAlignment)Enum.Parse(typeof(CameraAlignment), e.ClickedItem.Tag as string);
            ResetCameraAlignment(alignment);
        }

        private void CameraMenu_Orthographic_CheckedChanged(object sender, EventArgs e)
        {
            Camera.IsPerspective = !CameraMenu_Orthographic.Checked;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var components = CurrentProject.Surfaces[0].Components.ToList();
            var meshes = components.SelectMany(x => x.GetAllModelMeshes()).Distinct().ToList();
            meshes.ForEach(x => CurrentProject.Meshes.Remove(x));
            CurrentProject.Surfaces[0].Components.Clear();

            var noRefMeshes = CurrentProject.Meshes.Where(x => !x.GetReferences().Any()).ToList();
            noRefMeshes.ForEach(x => CurrentProject.Meshes.Remove(x));
        }

        private void DisplayMenu_Collisions_CheckedChanged(object sender, EventArgs e)
        {
            ShowCollisions = DisplayMenu_Collisions.Checked;
            UpdateGizmoFromSelection();
        }

        private void DisplayMenu_Meshes_CheckedChanged(object sender, EventArgs e)
        {
            ShowMeshes = DisplayMenu_Meshes.Checked;
            ModelRenderingOptions.Hidden = !DisplayMenu_Meshes.Checked;
            UpdateGizmoFromSelection();
        }

        #region Tranform Gizmo Settings

        private void GizmoOrientationMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Enum.TryParse(e.ClickedItem.Tag as string, out OrientationMode mode))
            {
                TransformGizmo.OrientationMode = mode;
                SelectCurrentGizmoOptions();
                UpdateGizmoFromSelection();
            }
        }

        private void GizmoPivotModeMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Enum.TryParse(e.ClickedItem.Tag as string, out PivotPointMode mode))
            {
                TransformGizmo.PivotPointMode = mode;
                SelectCurrentGizmoOptions();
                UpdateGizmoFromSelection();
            }
        }

        private void SelectCurrentGizmoOptions()
        {
            foreach (ToolStripMenuItem item in GizmoOrientationMenu.DropDownItems)
            {
                if (Enum.TryParse(item.Tag as string, out OrientationMode mode))
                    item.Checked = mode == TransformGizmo.OrientationMode;
            }

            foreach (ToolStripMenuItem item in GizmoPivotModeMenu.DropDownItems)
            {
                if (Enum.TryParse(item.Tag as string, out PivotPointMode mode))
                    item.Checked = mode == TransformGizmo.PivotPointMode;
            }
        }

        #endregion

        #endregion

        private void xRayToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            ModelRenderingOptions.DrawTransparent = xRayToolStripMenuItem.Checked;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.NumPad0)
            {
                Camera_ResetCameraMenu.PerformClick();
                return true;
            }
            else if (keyData == Keys.NumPad7)
            {
                AlignToMenu_Top.PerformClick();
                return true;
            }
            else if (keyData == Keys.NumPad1)
            {
                AlignToMenu_Front.PerformClick();
                return true;
            }
            else if (keyData == Keys.NumPad3)
            {
                AlignToMenu_Right.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        
    }
}
