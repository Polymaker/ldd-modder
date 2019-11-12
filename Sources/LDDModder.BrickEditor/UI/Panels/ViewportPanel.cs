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
        private List<CollisionModel> CollisionModels;

        private Thread UpdateThread;
        private bool ExitUpdateLoop;

        private InputManager InputManager;
        private CameraManipulator CameraManipulator;
        private Camera Camera => CameraManipulator.Camera;

        private GLModel BoxCollisionModel;
        private GLModel SphereCollisionModel;

        public bool ShowCollisions { get; set; }
        public bool ShowMeshes { get; set; }

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
            glControl1 = new GLControl(new GraphicsMode(32, 24, 0, 8));
            glControl1.BackColor = Color.FromArgb(204, 204, 204);
            Controls.Add(glControl1);
            glControl1.Dock = DockStyle.Fill;
            glControl1.BringToFront();
            glControl1.MouseEnter += GlControl1_MouseEnter;
            glControl1.MouseLeave += GlControl1_MouseLeave;
            glControl1.MouseClick += GlControl1_MouseClick;
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

            InitializeTextures();

            InitializeShaders();

            InitializeModels();

            //InitializeFonts();
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
            BoxCollisionModel = GLModel.CreatFromAssimp(modelScene.Meshes[0]);
            BoxCollisionModel.Material = new MaterialInfo
            {
                Diffuse = new Vector4(1f, 0.05f, 0.05f, 1f),
                Specular = new Vector3(1f),
                Shininess = 2f
            };

            modelScene = ResourceHelper.GetResourceModel("Models.Sphere.obj", "obj");
            SphereCollisionModel = GLModel.CreatFromAssimp(modelScene.Meshes[0]);
            SphereCollisionModel.Material = new MaterialInfo
            {
                Diffuse = new Vector4(1f, 0.05f, 0.05f, 1f),
                Specular = new Vector3(1f),
                Shininess = 2f
            };
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
            WireframeShader.Color.Set(new Vector4(0, 0, 0, 1));
            WireframeShader.Thickness.Set(1f);
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

        private void StartRenderLoop()
        {
            if (!RenderLoopEnabled)
            {
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
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            OnRenderFrame();
        }

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

            if (ShowCollisions)
                DrawCollisions();

            DrawConnections();

            if (ShowMeshes)
                DrawPartModels();

            DrawGrid();

            GL.UseProgram(0);

            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref projection);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref viewMatrix);
        }

        private void DrawConnections()
        {

        }

        private void DrawCollisions()
        {
            if (CollisionModels.Any())
            {
                ModelShader.Texture.Set(TextureUnit.Texture0);
                ModelShader.UseTexture.Set(false);
                var selectedMat = BoxCollisionModel.Material;
                selectedMat.Diffuse = new Vector4(1f, 0.4f, 0.4f, 1f);
                foreach (var colModel in CollisionModels.Where(x=>x.Visible))
                {
                    colModel.BaseModel.BindToShader(ModelShader);
                    ModelShader.Material.Set(colModel.IsSelected ? selectedMat : BoxCollisionModel.Material);
                    ModelShader.ModelMatrix.Set(colModel.MeshTransform);
                    colModel.Draw();
                    colModel.BaseModel.UnbindShader(ModelShader);
                }
            }
        }

        private void DrawPartModels()
        {
            CheckboardTexture.Bind(TextureUnit.Texture4);
            ModelShader.Texture.BindTexture(TextureUnit.Texture4, CheckboardTexture);

            var transparentSurfaces = SurfaceModels.Where(x => x.IsTransparent);

            if (transparentSurfaces.Any())
            {
                GL.Enable(EnableCap.CullFace);

                foreach (var surfaceModel in transparentSurfaces)
                {
                    GL.CullFace(CullFaceMode.Front);
                    surfaceModel.Draw(WireframeShader, ModelShader);
                }
                foreach (var surfaceModel in transparentSurfaces)
                {
                    GL.CullFace(CullFaceMode.Back);
                    surfaceModel.Draw(WireframeShader, ModelShader);
                }
                GL.Disable(EnableCap.CullFace);

                foreach (var surfaceModel in SurfaceModels.Where(x => !x.IsTransparent))
                    surfaceModel.Draw(WireframeShader, ModelShader);
            }
            else
            {
                foreach (var surfaceModel in SurfaceModels)
                    surfaceModel.Draw(WireframeShader, ModelShader);
            }
            

            CheckboardTexture.Bind(TextureUnit.Texture0);
            
        }

        private void DrawGrid()
        {
            GridShader.Use();
            GridShader.MVMatrix.Set(Camera.GetViewMatrix());
            GridShader.PMatrix.Set(Camera.GetProjectionMatrix());

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
            InputManager.UpdateInputStates();
            CameraManipulator.HandleCamera(InputManager);
        }

        private void GlControl1_MouseEnter(object sender, EventArgs e)
        {
            InputManager.ContainsMouse = true;
        }

        private void GlControl1_MouseLeave(object sender, EventArgs e)
        {
            InputManager.ContainsMouse = false;
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

            if (BoxCollisionModel != null)
                BoxCollisionModel.Dispose();
            if (SphereCollisionModel != null)
                SphereCollisionModel.Dispose();
            GridShader.Dispose();
            ModelShader.Dispose();
            WireframeShader.Dispose();
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
                Diffuse = new Vector4(0.6f, 0.6f, 0.6f, 0.7f),
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


            surfModel.SelectedMaterial = new MaterialInfo
            {
                Diffuse = new Vector4(surfModel.Material.Diffuse.Xyz * 1.2f, surfModel.Material.Diffuse.W),
                Specular = surfModel.Material.Specular,
                Shininess = surfModel.Material.Shininess
            };

            surfModel.RebuildPartModels();
            SurfaceModels.Add(surfModel);
        }

        protected override void OnProjectClosed()
        {
            base.OnProjectClosed();
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

        protected override void OnSelectedElementChanged(PartElement selectedElement)
        {
            base.OnSelectedElementChanged(selectedElement);

            CollisionModels.ForEach(x => x.IsSelected = (x.Element == selectedElement));

            foreach (var model in SurfaceModels.SelectMany(x => x.MeshModels))
            {
                model.IsSelected = model.Element == selectedElement ||
                    model.Component == selectedElement ||
                    model.Surface == selectedElement ||
                    model.Mesh == selectedElement;
            }
        }



        public IEnumerable<PartElementModel> GetVisibleModels()
        {
            if (ShowMeshes)
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
                minPos = Vector3.ComponentMin(minPos, model.BoundingBox.Min);
                maxPos = Vector3.ComponentMax(maxPos, model.BoundingBox.Max);
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

        private void GlControl1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var ray = Camera.RaycastFromScreen(new Vector2(e.X, e.Y));
                var visibleModels = GetVisibleModels();

                if (ray != null && visibleModels.Any())
                {

                    var intersectingModels = new List<Tuple<PartElementModel, float>>();

                    foreach (var model in visibleModels)
                    {
                        if (model.RayIntersectsBoundingBox(ray, out float boxDist))
                        {
                            if (model.RayIntersects(ray, out float triangleDist))
                                intersectingModels.Add(new Tuple<PartElementModel, float>(model, triangleDist));
                            //if (model.SurfaceModel.RayIntersects(ray, model, out float triangleDist))
                            //    intersectingModels.Add(new Tuple<SurfaceModelMesh, float>(model, triangleDist));
                        }
                    }

                    var closest = intersectingModels.OrderBy(x => x.Item2).FirstOrDefault();
                    ProjectManager.SelectedElement = closest?.Item1.Element;
                }
            }
        }

        #endregion


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
        }

        private void DisplayMenu_Meshes_CheckedChanged(object sender, EventArgs e)
        {
            ShowMeshes = DisplayMenu_Meshes.Checked;
        }
    }
}
