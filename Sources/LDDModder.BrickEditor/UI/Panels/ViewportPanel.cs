using LDDModder.BrickEditor.Rendering;
using LDDModder.BrickEditor.Rendering.Shaders;
using LDDModder.Modding.Editing;
using ObjectTK.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
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
using LDDModder.BrickEditor.Resources;
using LDDModder.BrickEditor.Rendering.Gizmos;
using LDDModder.BrickEditor.Rendering.UI;
using LDDModder.BrickEditor.ProjectHandling;

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
        private List<ConnectionModel> ConnectionModels;

        private List<UIElement> UIElements;

        private InputManager InputManager;
        private CameraManipulator CameraManipulator;
        private Camera Camera => CameraManipulator.Camera;

        private TransformGizmo SelectionGizmo;

        private LoopController LoopController;

        public bool ShowCollisions { get; set; }

        public bool ShowConnections { get; set; }

        public bool ShowMeshes { get; set; }

        public RenderOptions ModelRenderingOptions { get; private set; }

        public ViewportPanel()
        {
            InitializeComponent();
            SurfaceModels = new List<GLSurfaceModel>();
            CollisionModels = new List<CollisionModel>();
            ConnectionModels = new List<ConnectionModel>();
            UIElements = new List<UIElement>();
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
            ConnectionModels = new List<ConnectionModel>();
            UIElements = new List<UIElement>();

            CreateGLControl();
            SelectionInfoPanel.BringToFront();
            SelectionInfoPanel.Visible = false;
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
            glControl1.GotFocus += GlControl1_GotFocus;
            glControl1.LostFocus += GlControl1_LostFocus;

            

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

            UpdateGLViewport();

            InitializeMenus();

            LoopController.Start();

            var mainForm = DockPanel.FindForm();
            mainForm.Activated += MainForm_Activated;
            mainForm.Deactivate += MainForm_Deactivate;

            UpdateTitle();
            ProjectManager.ProjectModified += ProjectManager_ProjectModified;
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

            InitializeSelectionGizmo();
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
            int buttonSpacing = 2;

            int buttonSize = 40;

            SelectGizmoButton = new UIButton()
            {
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
                Texture = SelectionIcons,
                NormalSprite =      new SpriteBounds(0.25f, 0, 0.25f, 0.25f),
                OverSprite =        new SpriteBounds(0.25f, 0.25f, 0.25f, 0.25f),
                SelectedSprite =    new SpriteBounds(0.25f, 0.5f, 0.25f, 0.25f),
            };
            MoveGizmoButton.Clicked += MoveGizmoButton_Clicked;
            UIElements.Add(MoveGizmoButton);

            RotateGizmoButton = new UIButton()
            {
                Texture = SelectionIcons,
                NormalSprite =      new SpriteBounds(0.5f, 0, 0.25f, 0.25f),
                OverSprite =        new SpriteBounds(0.5f, 0.25f, 0.25f, 0.25f),
                SelectedSprite =    new SpriteBounds(0.5f, 0.5f, 0.25f, 0.25f),
            };
            UIElements.Add(RotateGizmoButton);
            RotateGizmoButton.Clicked += RotateGizmoButton_Clicked;

            var gizmoButtons = new UIButton[] { SelectGizmoButton, MoveGizmoButton, RotateGizmoButton };
            for (int i = 0; i < gizmoButtons.Length; i++)
            {
                gizmoButtons[i].Bounds = new Vector4(buttonSpacing, 64 + (buttonSize + buttonSpacing) * i, buttonSize, buttonSize);
            }

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

        private void InitializeSelectionGizmo()
        {
            SelectionGizmo = new TransformGizmo();
            SelectionGizmo.InitializeVBO();
            SelectionGizmo.DisplayStyle = GizmoStyle.Plain;
            SelectionGizmo.PivotPointMode = PivotPointMode.MedianCenter;
            SelectionGizmo.OrientationMode = OrientationMode.Global;

            SelectionGizmo.TransformFinishing += SelectionGizmo_TransformFinishing;
            SelectionGizmo.TransformFinished += SelectionGizmo_TransformFinished;
            SelectionGizmo.DisplayStyleChanged += SelectionGizmo_DisplayStyleChanged;
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
            ModelManager.InitializeResources();

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

            if (ShowConnections)
                DrawConnections();

            if (!ModelRenderingOptions.Hidden)
                DrawPartModels();

            DrawGrid();

            GL.UseProgram(0);

            if (SelectionGizmo.Visible)
            {
                GL.Clear(ClearBufferMask.DepthBufferBit);
                SelectionGizmo.Render();
            }

            GL.UseProgram(0);
        }

        private void DrawConnections()
        {
            GL.Disable(EnableCap.Texture2D);
            if (ConnectionModels.Any())
            {
                RenderHelper.UnbindModelTexture();
                foreach (var connModel in ConnectionModels.Where(x => x.Visible))
                    connModel.RenderModel(Camera);
            }
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
        private double AvgRenderFPS = 0;

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


            if (AvgRenderFPS == 0)
                AvgRenderFPS = LoopController.RenderFrequency;
            else
                AvgRenderFPS = (AvgRenderFPS + LoopController.RenderFrequency) / 2d;

            UIRenderHelper.DrawText($"Render FPS: {AvgRenderFPS:0}", UIRenderHelper.NormalFont, Color.White, new Vector2(10, 10));

            if (SelectionGizmo.IsEditing)
            {
                //UIRenderHelper.DrawText("Hold Ctrl to snap", Color.Black, new Vector2(10, UIRenderHelper.ViewSize.Y - 20));

                string amountStr = "";
                if (SelectionGizmo.DisplayStyle == GizmoStyle.Rotation)
                {
                    float degrees = (SelectionGizmo.TransformedAmount / (float)Math.PI) * 180f;
                    amountStr = $"Rotation: {degrees:0.##}°";
                }
                else
                    amountStr = $"Translation: {SelectionGizmo.TransformedAmount:0.##}";

                UIRenderHelper.DrawText(amountStr, UIRenderHelper.NormalFont, Color.Black, new Vector2(10, UIRenderHelper.ViewSize.Y - 20));
            }

            if (CurrentProject != null)
            {
                UIRenderHelper.DrawText($"{CurrentProject.TotalTriangles} triangles", UIRenderHelper.SmallFont, Color.Black, 
                    new Vector4(UIRenderHelper.ViewSize.X - 104, UIRenderHelper.ViewSize.Y - 20, 100, 16), 
                    StringAlignment.Far, StringAlignment.Far);
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

            if (InputManager.ContainsFocus)
            {
                if (InputManager.IsKeyPressed(Key.S))
                    SelectionGizmo.DisplayStyle = GizmoStyle.Plain;
                else if (InputManager.IsKeyPressed(Key.R))
                    SelectionGizmo.DisplayStyle = GizmoStyle.Rotation;
                else if (InputManager.IsKeyPressed(Key.T))
                    SelectionGizmo.DisplayStyle = GizmoStyle.Translation;
            }

            if (SelectionGizmo.Visible)
            {
                if (Camera.IsDirty)
                    SelectionGizmo.UpdateBounds(Camera);
                SelectionGizmo.ProcessInput(Camera, InputManager);
            }
            
            bool mouseClickHandled = SelectionGizmo.Selected || SelectionGizmo.IsEditing;

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

                if (!InputManager.MouseClickHandled)
                {
                    if (overElement != null)
                    {
                        if (InputManager.IsButtonClicked(MouseButton.Left))
                            overElement.PerformClick(InputManager.LocalMousePos, MouseButton.Left);
                        else if (InputManager.IsButtonClicked(MouseButton.Right))
                            overElement.PerformClick(InputManager.LocalMousePos, MouseButton.Right);
                        InputManager.MouseClickHandled = true;
                    }
                }

                if (!InputManager.MouseClickHandled && InputManager.IsButtonClicked(MouseButton.Left))
                {
                    var ray = Camera.RaycastFromScreen(InputManager.LocalMousePos);
                    PerformRaySelection(ray);
                }
            }
        }

        #endregion

        #region Selection Gizmo Handling

        private void SelectionGizmo_TransformFinishing(object sender, EventArgs e)
        {
            ProjectManager.StartBatchChanges();
        }

        private void SelectionGizmo_TransformFinished(object sender, EventArgs e)
        {
            ProjectManager.EndBatchChanges();
            UpdateSelectionInfoPanel();
        }

        private void SelectionGizmo_DisplayStyleChanged(object sender, EventArgs e)
        {
            UpdateGizmoButtonStates();
        }

        private void MoveGizmoButton_Clicked(object sender, EventArgs e)
        {
            if (!MoveGizmoButton.Selected && !SelectionGizmo.IsEditing)
                SelectionGizmo.DisplayStyle = GizmoStyle.Translation;
        }

        private void SelectGizmoButton_Clicked(object sender, EventArgs e)
        {
            if (!SelectGizmoButton.Selected && !SelectionGizmo.IsEditing)
            {
                SelectionGizmo.DisplayStyle = GizmoStyle.Plain;
            }
        }

        private void RotateGizmoButton_Clicked(object sender, EventArgs e)
        {
            if (!RotateGizmoButton.Selected && !SelectionGizmo.IsEditing)
            {
                SelectionGizmo.DisplayStyle = GizmoStyle.Rotation;
            }
        }

        private void UpdateGizmoButtonStates()
        {
            MoveGizmoButton.Selected = SelectionGizmo.DisplayStyle == GizmoStyle.Translation;
            SelectGizmoButton.Selected = SelectionGizmo.DisplayStyle == GizmoStyle.Plain;
            RotateGizmoButton.Selected = SelectionGizmo.DisplayStyle == GizmoStyle.Rotation;
        }

        #endregion

        #region Form handling

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x0005 || m.Msg == 0x0214 || m.Msg == 0x0046 || m.Msg == 0x0047)
            {
                if (ViewInitialized && !Disposing && !IsClosing)
                {
                    UpdateGLViewport();
                    LoopController.ForceRender();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            IsClosing = true;
            LoopController.Stop();
            DisposeGLResources();
        }

        private void UpdateTitle()
        {
            Text = ProjectManager.GetProjectDisplayName();
            if (ProjectManager.IsModified)
                Text += "*";
        }

        #endregion

        private void DisposeGLResources()
        {
            UnloadModels();

            UIRenderHelper.ReleaseResources();
            RenderHelper.ReleaseResources();
            ModelManager.ReleaseResources();

            SelectionGizmo.Dispose();

            GridShader.Dispose();
            CheckboardTexture.Dispose();
        }

        #region Project Handling

        protected override void OnProjectChanged()
        {
            base.OnProjectChanged();
            UpdateTitle();
        }

        protected override void OnProjectLoaded(PartProject project)
        {
            base.OnProjectLoaded(project);

            RebuildSurfaceModels();
            RebuildCollisionModels();
            RebuildConnectionModels();
            SetupDefaultCamera();
        }

        private void ProjectManager_ProjectModified(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(UpdateTitle));
            else
                UpdateTitle();
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
            SelectionGizmo.Deactivate();
            UnloadModels();
        }

        private bool SurfaceMeshesChanged;
        private bool CollisionsChanged;
        private bool ConnectionsChanged;

        protected override void OnElementCollectionChanged(ElementCollectionChangedEventArgs e)
        {
            base.OnElementCollectionChanged(e);
            
            if (e.ElementType == typeof(PartSurface) || 
                e.ElementType == typeof(SurfaceComponent) || 
                e.ElementType == typeof(ModelMeshReference))
            {
                SurfaceMeshesChanged = true;
            }
            else if (e.ElementType == typeof(PartCollision))
            {
                CollisionsChanged = true;
            }
            else if (e.ElementType == typeof(PartConnection))
            {
                ConnectionsChanged = true;
            }
        }

        protected override void OnProjectChangeApplied()
        {
            base.OnProjectChangeApplied();

            if (SurfaceMeshesChanged)
                RebuildSurfaceModels();

            if (CollisionsChanged)
                RebuildCollisionModels();

            if (ConnectionsChanged)
                RebuildConnectionModels();

            SurfaceMeshesChanged = false;
            CollisionsChanged = false;
            ConnectionsChanged = false;
        }

        #endregion

        #region Model/Mesh Handling

        private void RebuildSurfaceModels()
        {
            if (ProjectManager.IsProjectOpen)
            {
                foreach (var surfaceModel in SurfaceModels.ToArray())
                {
                    if (!CurrentProject.Surfaces.Contains(surfaceModel.Surface))
                    {
                        surfaceModel.Dispose();
                        SurfaceModels.Remove(surfaceModel);
                    }
                }

                foreach (var surface in CurrentProject.Surfaces)
                {
                    if (!SurfaceModels.Any(x=>x.Surface == surface))
                    {
                        AddPartSurfaceModel(surface);
                    }
                    else
                    {
                        var model = SurfaceModels.FirstOrDefault(x => x.Surface == surface);
                        model.RebuildPartModels();
                    }
                }
            }
            else if (SurfaceModels.Any())
            {
                SurfaceModels.ForEach(x => x.Dispose());
                SurfaceModels.Clear();
            }
        }

        private void RebuildCollisionModels()
        {
            if (ProjectManager.IsProjectOpen)
            {
                var allCollisions = CurrentProject.GetAllElements<PartCollision>().ToList();

                foreach (var colModel in CollisionModels.ToArray())
                {
                    if (!allCollisions.Contains(colModel.PartCollision))
                    {
                        colModel.Dispose();
                        CollisionModels.Remove(colModel);
                    }
                }

                foreach (var collision in allCollisions)
                {
                    if (!CollisionModels.Any(x => x.PartCollision == collision))
                        CollisionModels.Add(new CollisionModel(collision));
                }
            }
            else if (CollisionModels.Any())
            {
                CollisionModels.ForEach(x => x.Dispose());
                CollisionModels.Clear();
            }
        }

        private void RebuildConnectionModels()
        {
            ConnectionModels.Clear();

            if (ProjectManager.IsProjectOpen)
            {
                foreach (var conn in CurrentProject.Connections)
                    ConnectionModels.Add(new ConnectionModel(conn));
            }
            else if (ConnectionModels.Any())
            {
                ConnectionModels.ForEach(x => x.Dispose());
                ConnectionModels.Clear();
            }
        }

        private void UnloadModels()
        {
            SurfaceModels.ForEach(x => x.Dispose());
            SurfaceModels.Clear();
            CollisionModels.Clear();
            ConnectionModels.Clear();
            GC.Collect();
        }

        public IEnumerable<PartElementModel> GetAllElementModels()
        {
            foreach (var model in SurfaceModels.SelectMany(x => x.MeshModels))
                yield return model;

            foreach (var model in CollisionModels)
                yield return model;

            foreach (var model in ConnectionModels)
                yield return model;
        }

        public IEnumerable<PartElementModel> GetVisibleModels()
        {
            if (!ModelRenderingOptions.Hidden)
            {
                foreach (var model in SurfaceModels.SelectMany(x => x.MeshModels).Where(x => x.Visible))
                    yield return model;
            }

            if (ShowCollisions)
            {
                foreach (var model in CollisionModels.Where(x => x.Visible))
                    yield return model;
            }

            if (ShowConnections)
            {
                foreach (var model in ConnectionModels)
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
                SelectionGizmo.SetActiveElements(selectedModels);

                SelectionGizmo.UpdateBounds(Camera);
            }
            else
            {
                SelectionGizmo.Deactivate();
            }

            UpdateSelectionInfoPanel();
        }

        private bool UpdatingInfo;

        private void UpdateSelectionInfoPanel()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(UpdateSelectionInfoPanel));
                return;
            }
            
            int activeObjectCount = SelectionGizmo.ActiveElements.Count();

            if (activeObjectCount == 1)
            {
                var activeObject = SelectionGizmo.ActiveElements.FirstOrDefault();

                UpdatingInfo = true;
                if (activeObject is PartElementModel elementModel && 
                    elementModel.Element is IPhysicalElement physicalElement)
                {
                    var transPos = physicalElement.Transform.Position.Rounded();
                    var transRot = physicalElement.Transform.Rotation.Rounded();
                    PosXNumBox.Value = transPos.X;
                    PosYNumBox.Value = transPos.Y;
                    PosZNumBox.Value = transPos.Z;
                    RotXNumBox.Value = transRot.X;
                    RotYNumBox.Value = transRot.Y;
                    RotZNumBox.Value = transRot.Z;
                }
                else
                {
                    var translation = activeObject.Transform.ExtractTranslation();

                    PosXNumBox.Value = translation.X;
                    PosYNumBox.Value = translation.Y;
                    PosZNumBox.Value = translation.Z;
                }
                UpdatingInfo = false;
            }

            SelectionInfoPanel.Visible = (activeObjectCount == 1);

        }

        private void PositionNumBoxes_ValueChanged(object sender, EventArgs e)
        {
            if (UpdatingInfo)
                return;

            var activeObject = SelectionGizmo.ActiveElements.FirstOrDefault();
            if (activeObject != null)
            {
                activeObject.BeginEditTransform();
                var trans = Matrix4.CreateTranslation((float)PosXNumBox.Value, (float)PosYNumBox.Value, (float)PosZNumBox.Value);
                var rot = activeObject.Transform.ExtractRotation();
                activeObject.Transform = Matrix4.CreateFromQuaternion(rot) * trans;
                activeObject.EndEditTransform(false);
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

        #region Camera & Viewport Handling

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
                if (!(model is SurfaceModelMesh))
                    continue;

                var worldBounding = model.GetWorldBoundingBox();
                
                minPos = Vector3.ComponentMin(minPos, worldBounding.Min);
                maxPos = Vector3.ComponentMax(maxPos, worldBounding.Max);
            }

            return BBox.FromMinMax(minPos, maxPos);
        }

        public void ResetCameraAlignment(CameraAlignment alignment)
        {
            var visibleModels = GetVisibleModels().OfType<SurfaceModelMesh>();
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

        private void UpdateGLViewport()
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

            if (SelectionGizmo.Visible)
                SelectionGizmo.UpdateBounds(Camera);
        }

        #endregion

        #region Control Events

        private void GlControl_MouseEnter(object sender, EventArgs e)
        {
            InputManager.SetContainsMouse(true);
        }

        private void GlControl_MouseLeave(object sender, EventArgs e)
        {
            InputManager.SetContainsMouse(false);
            foreach (var elem in UIElements)
                elem.SetIsOver(false);
        }

        private void GlControl1_GotFocus(object sender, EventArgs e)
        {
            InputManager.ContainsFocus = true;
        }

        private void GlControl1_LostFocus(object sender, EventArgs e)
        {
            InputManager.ContainsFocus = false;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (ViewInitialized && !IsClosing)
                LoopController.Start();
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            if (ViewInitialized)
                LoopController.Stop();
        }

        private void GlControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            InputManager.ProcessMouseMove(e);
        }

        #endregion

        #region Toolbar Menu

        private void CameraMenu_ResetCamera_Click(object sender, EventArgs e)
        {
            SetupDefaultCamera();
        }

        private void CameraMenu_LookAt_Click(object sender, EventArgs e)
        {
            if (SelectionGizmo.Visible)
            {
                CameraManipulator.Gimbal = SelectionGizmo.Position;
            }
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

        private void DisplayMenu_Collisions_CheckedChanged(object sender, EventArgs e)
        {
            ShowCollisions = DisplayMenu_Collisions.Checked;
            UpdateGizmoFromSelection();
        }

        private void DisplayMenu_Connections_CheckedChanged(object sender, EventArgs e)
        {
            ShowConnections = DisplayMenu_Connections.Checked;
            UpdateGizmoFromSelection();
        }

        private void DisplayMenu_Meshes_CheckedChanged(object sender, EventArgs e)
        {
            ShowMeshes = DisplayMenu_Meshes.Checked;
            ModelRenderingOptions.Hidden = !DisplayMenu_Meshes.Checked;
            UpdateGizmoFromSelection();
        }

        private void xRayToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            ModelRenderingOptions.DrawTransparent = xRayToolStripMenuItem.Checked;
        }

        #region Tranform Gizmo Settings

        private void GizmoOrientationMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Enum.TryParse(e.ClickedItem.Tag as string, out OrientationMode mode))
            {
                SelectionGizmo.OrientationMode = mode;
                SelectCurrentGizmoOptions();
                UpdateGizmoFromSelection();
            }
        }

        private void GizmoPivotModeMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Enum.TryParse(e.ClickedItem.Tag as string, out PivotPointMode mode))
            {
                SelectionGizmo.PivotPointMode = mode;
                SelectCurrentGizmoOptions();
                UpdateGizmoFromSelection();
            }
        }

        private void SelectCurrentGizmoOptions()
        {
            foreach (ToolStripMenuItem item in GizmoOrientationMenu.DropDownItems)
            {
                if (Enum.TryParse(item.Tag as string, out OrientationMode mode))
                    item.Checked = mode == SelectionGizmo.OrientationMode;
            }

            foreach (ToolStripMenuItem item in GizmoPivotModeMenu.DropDownItems)
            {
                if (Enum.TryParse(item.Tag as string, out PivotPointMode mode))
                    item.Checked = mode == SelectionGizmo.PivotPointMode;
            }
        }

        #endregion

        #endregion
       
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!SelectionInfoPanel.ContainsFocus)
            {
                if (keyData == Keys.NumPad0)
                {
                    CameraMenu_ResetCamera.PerformClick();
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
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
