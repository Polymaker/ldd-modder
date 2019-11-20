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
using LDDModder.BrickEditor.EditModels;
using LDDModder.BrickEditor.Resources;
using LDDModder.BrickEditor.Rendering.Gizmos;
using LDDModder.BrickEditor.Rendering.UI;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ViewportPanel : ProjectDocumentPanel
    {
        private bool ViewInitialized;

        private bool IsClosing;
        private GLControl glControl1;

        private Texture2D CheckboardTexture;
        private Texture2D SelectionIcons;

        private GridShaderProgram GridShader;

        private List<GLSurfaceModel> SurfaceModels;
        private List<CollisionModel> CollisionModels;
        private List<UIElement> UIElements;

        private InputManager InputManager;
        private CameraManipulator CameraManipulator;
        private Camera Camera => CameraManipulator.Camera;

        private GLModel BoxCollisionModel;
        private GLModel SphereCollisionModel;
        private TransformGizmo TransformGizmo;

        private LoopController LoopController;

        public bool ShowCollisions { get; set; }
        public bool ShowMeshes { get; set; }

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
            AllowEndUserDocking = false;
            CloseButton = false;
            CloseButtonVisible = false;
            SurfaceModels = new List<GLSurfaceModel>();
            CollisionModels = new List<CollisionModel>();
            UIElements = new List<UIElement>();
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

            LoopController = new LoopController(glControl1);
            LoopController.TargetRenderFrequency = 40;
            LoopController.RenderFrame += LoopController_RenderFrame;
            LoopController.UpdateFrame += LoopController_UpdateFrame;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            visualStudioToolStripExtender1.SetStyle(toolStrip1,
                VisualStudioToolStripExtender.VsVersion.Vs2015,
                DockPanel.Theme);

            InitializeBase();

            UpdateViewport();

            InitializeMenus();

            LoopController.Start();
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
            InitializeUI();

            InitializeShaders();

            InitializeModels();
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

            var selectionIconsImage = (Bitmap)Bitmap.FromStream(System.Reflection.Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("LDDModder.BrickEditor.Resources.Textures.SelectionIcons.png"));

            BitmapTexture.CreateCompatible(selectionIconsImage, out SelectionIcons, 1);
            SelectionIcons.LoadBitmap(selectionIconsImage, 0);
            SelectionIcons.SetFilter(TextureMinFilter.Linear, TextureMagFilter.Linear);
            SelectionIcons.SetWrapMode(TextureWrapMode.Clamp);
        }

        private UIButton SelectGizmoButton;
        private UIButton MoveGizmoButton;
        private UIButton RotateGizmoButton;

        private void InitializeUI()
        {
            SelectGizmoButton = new UIButton()
            {
                Bounds = new Vector4(8, 64, 32, 32),
                Texture= SelectionIcons,
                NormalSprite = new SpriteBounds(0,0,0.25f,0.25f),
                OverSprite = new SpriteBounds(0, 0.25f, 0.25f, 0.25f),
                SelectedSprite = new SpriteBounds(0, 0.5f, 0.25f, 0.25f),
                Selected = true
            };
            SelectGizmoButton.Clicked += SelectGizmoButton_Clicked;

            UIElements.Add(SelectGizmoButton);
            MoveGizmoButton = new UIButton()
            {
                Bounds = new Vector4(8, 96, 32, 32),
                Texture = SelectionIcons,
                NormalSprite =      new SpriteBounds(0.25f, 0, 0.25f, 0.25f),
                OverSprite =        new SpriteBounds(0.25f, 0.25f, 0.25f, 0.25f),
                SelectedSprite =    new SpriteBounds(0.25f, 0.5f, 0.25f, 0.25f),
            };
            MoveGizmoButton.Clicked += MoveGizmoButton_Clicked;
            UIElements.Add(MoveGizmoButton);
            RotateGizmoButton = new UIButton()
            {
                Bounds = new Vector4(8, 128, 32, 32),
                Texture = SelectionIcons,
                NormalSprite =      new SpriteBounds(0.5f, 0, 0.25f, 0.25f),
                OverSprite =        new SpriteBounds(0.5f, 0.25f, 0.25f, 0.25f),
                SelectedSprite =    new SpriteBounds(0.5f, 0.5f, 0.25f, 0.25f),
            };
            UIElements.Add(RotateGizmoButton);
            RotateGizmoButton.Clicked += RotateGizmoButton_Clicked;

            //var testButton = new UIButton()
            //{
            //    Bounds = new Vector4(8, 160, 32, 32),
            //    Texture = SelectionIcons,
            //    NormalSprite = new SpriteBounds(0.75f, 0, 0.25f, 0.25f),
            //    OverSprite = new SpriteBounds(0.75f, 0.25f, 0.25f, 0.25f),
            //    SelectedSprite = new SpriteBounds(0.75f, 0.5f, 0.25f, 0.25f),
            //    Text = "Test",
            //    TextColor = new Vector4(1f)
            //};
            //UIElements.Add(testButton);
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
            TransformGizmo.DisplayStyle = GizmoStyle.Plain;
            TransformGizmo.PivotPointMode = PivotPointMode.MedianCenter;
            TransformGizmo.OrientationMode = OrientationMode.Global;
            TransformGizmo.TransformFinishing += TransformGizmo_TransformFinishing;
            TransformGizmo.TransformFinished += TransformGizmo_TransformFinished;
            TransformGizmo.DisplayStyleChanged += TransformGizmo_DisplayStyleChanged;
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

            UIRenderHelper.InitializeResources();
            RenderHelper.InitializeResources();
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

            //glControl1.MakeCurrent();
            if (!ViewInitialized)
            {
                GL.ClearColor(glControl1.BackColor);
                ViewInitialized = true;
            }

            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            Camera.Viewport = new RectangleF(0, 0, glControl1.Width, glControl1.Height);

            UIRenderHelper.InitializeMatrices(Camera);

            if (TransformGizmo.Visible)
                TransformGizmo.UpdateBounds(Camera);
        }


        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            LoopController.Start();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            LoopController.Stop();
        }

        

        #region Render Loop

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

            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref UIProjectionMatrix);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

            GL.Enable(EnableCap.Texture2D);
            UIRenderHelper.IntializeBeforeRender();

            foreach (var elem in UIElements)
                elem.Draw();

            //UIRenderHelper.DrawText($"Render FPS: {LoopController.RenderFrequency:0.00}", Color.White, new Vector2(10, 10));

            if (TransformGizmo.IsEditing)
            {
                //UIRenderHelper.DrawText("Hold Ctrl to snap", Color.Black, new Vector2(10, UIRenderHelper.ViewSize.Y - 20));

                string amountStr = "";
                if (TransformGizmo.DisplayStyle == GizmoStyle.Rotation)
                {
                    float degrees = (TransformGizmo.TransformedAmount / (float)Math.PI) * 180f;
                    amountStr = $"Rotation: {degrees:0.##}°";
                }
                else
                    amountStr = $"Translation: {TransformGizmo.TransformedAmount:0.##}";

                UIRenderHelper.DrawText(amountStr, Color.Black, new Vector2(10, UIRenderHelper.ViewSize.Y - 20));
            }

            UIRenderHelper.RenderElements();
        }

        private void LoopController_RenderFrame()
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

        private void LoopController_UpdateFrame(double deltaMs)
        {
            InputManager.UpdateInputStates();
            CameraManipulator.ProcessInput(InputManager);

            if (TransformGizmo.Visible)
            {
                if (Camera.IsDirty)
                    TransformGizmo.UpdateBounds(Camera);
                TransformGizmo.ProcessInput(Camera, InputManager);
            }
            
            bool mouseClickHandled = TransformGizmo.Selected || TransformGizmo.IsEditing;

            if (InputManager.ContainsMouse)
            {
                UIElement overElement = null;

                foreach (var elem in UIElements.OrderByDescending(x => x.ZOrder))
                {
                    if (overElement == null && elem.Contains(InputManager.LocalMousePos))
                    {
                        overElement = elem;
                        elem.SetIsOver(true);
                    }
                    else
                        elem.SetIsOver(false);
                }
                if (!mouseClickHandled)
                {
                    if (overElement != null)
                    {
                        if (InputManager.IsButtonClicked(MouseButton.Left))
                            overElement.PerformClick(InputManager.LocalMousePos, MouseButton.Left);
                        else if (InputManager.IsButtonClicked(MouseButton.Right))
                            overElement.PerformClick(InputManager.LocalMousePos, MouseButton.Right);
                        mouseClickHandled = true;
                    }
                }

                if (!mouseClickHandled && InputManager.IsButtonClicked(MouseButton.Left))
                {
                    var ray = Camera.RaycastFromScreen(InputManager.LocalMousePos);
                    PerformRaySelection(ray);
                }
            }

        }

        #endregion

        #region Editing

        private void TransformGizmo_TransformFinished(object sender, EventArgs e)
        {
            ProjectManager.EndBatchChanges();
        }

        private void TransformGizmo_TransformFinishing(object sender, EventArgs e)
        {

            ProjectManager.StartBatchChanges();
        }

        private void TransformGizmo_DisplayStyleChanged(object sender, EventArgs e)
        {
            UpdateGizmoButtonStates();
        }

        private void MoveGizmoButton_Clicked(object sender, EventArgs e)
        {
            if (!MoveGizmoButton.Selected && !TransformGizmo.IsEditing)
                TransformGizmo.DisplayStyle = GizmoStyle.Translation;
        }

        private void SelectGizmoButton_Clicked(object sender, EventArgs e)
        {
            if (!SelectGizmoButton.Selected && !TransformGizmo.IsEditing)
            {
                TransformGizmo.DisplayStyle = GizmoStyle.Plain;
            }
        }

        private void RotateGizmoButton_Clicked(object sender, EventArgs e)
        {
            if (!RotateGizmoButton.Selected && !TransformGizmo.IsEditing)
            {
                TransformGizmo.DisplayStyle = GizmoStyle.Rotation;
            }
        }


        private void UpdateGizmoButtonStates()
        {
            MoveGizmoButton.Selected = TransformGizmo.DisplayStyle == GizmoStyle.Translation;
            SelectGizmoButton.Selected = TransformGizmo.DisplayStyle == GizmoStyle.Plain;
            RotateGizmoButton.Selected = TransformGizmo.DisplayStyle == GizmoStyle.Rotation;
        }

        #endregion

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x0005 || m.Msg == 0x0214 || m.Msg == 0x0046 || m.Msg == 0x0047)
            {
                if (ViewInitialized && !Disposing && !IsClosing)
                {
                    UpdateViewport();
                    LoopController.ForceRender();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            LoopController.Stop();

            IsClosing = true;
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

            UIRenderHelper.ReleaseResources();
            RenderHelper.ReleaseResources();
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
                if (e.Action == System.ComponentModel.CollectionChangeAction.Add)
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
            IEnumerable<PartElementModel> selectedModels = Enumerable.Empty<PartElementModel>();

            if (onlyVisible)
                selectedModels = GetVisibleModels().Where(x => x.IsSelected);
            else
                selectedModels = GetAllElementModels().Where(x => x.IsSelected);

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
                TransformGizmo.SetActiveElements(selectedModels);

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
            ProjectManager.StartBatchChanges();
            var components = CurrentProject.Surfaces[0].Components.ToList();
            //var meshes = components.SelectMany(x => x.GetAllModelMeshes()).Distinct().ToList();

            CurrentProject.Surfaces[0].Components.Clear();
            //meshes.ForEach(x => CurrentProject.Meshes.Remove(x));
            

            var noRefMeshes = CurrentProject.Meshes.Where(x => !x.GetReferences().Any()).ToList();
            noRefMeshes.ForEach(x => CurrentProject.Meshes.Remove(x));
            ProjectManager.EndBatchChanges();
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
