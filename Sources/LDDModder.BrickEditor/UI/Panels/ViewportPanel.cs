using LDDModder.BrickEditor.Rendering;
using LDDModder.BrickEditor.Rendering.Shaders;
using LDDModder.Modding;
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
using LDDModder.BrickEditor.UI.Windows;
using LDDModder.BrickEditor.ProjectHandling.ViewInterfaces;
using System.Threading.Tasks;
using OpenTK.Platform;
using LDDModder.BrickEditor.Rendering.Models;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ViewportPanel : ProjectDocumentPanel, IViewportWindow
    {
        private bool ViewInitialized;
        private bool IsClosing;

        private GLControl glControl1;
        private GraphicsContext ResourceContext;

        private List<SurfaceMeshBuffer> SurfaceModels;

        private ThreadSafeList<ModelBase> LoadedModels;

        private List<UIElement> UIElements;

        private InputManager InputManager;
        private CameraManipulator CameraManipulator;
        private Camera Camera => CameraManipulator.Camera;

        private TransformGizmo SelectionGizmo;
        private CursorModel CursorModel;

        private LoopController LoopController;

        private List<LightInfo> SceneLights;

        public ViewportPanel()
        {
            InitializeComponent();
            //InitializeView();
        }

        public ViewportPanel(ProjectManager projectManager) : base(projectManager)
        {
            projectManager.ViewportWindow = this;
            InitializeComponent();
            //InitializeView();
        }

        private void InitializeView()
        {
            DockAreas = DockAreas.Document;
            AllowEndUserDocking = false;
            CloseButton = false;
            CloseButtonVisible = false;
            SurfaceModels = new List<SurfaceMeshBuffer>();
            LoadedModels = new ThreadSafeList<ModelBase>();
            UIElements = new List<UIElement>();
            ShowIcon = false;
            
            SelectionInfoPanel.Visible = false;
            BonesDropDownMenu.Visible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitializeView();

            InitializeMenus();
            UpdateDocumentTitle();

            var mainForm = DockPanel.FindForm();
            mainForm.Activated += MainForm_Activated;
            mainForm.Deactivate += MainForm_Deactivate;
        }


        #region Initialization

        private bool GlContextCreated = false;
        private IWindowInfo GlWindowInfo;

        public override void DefferedInitialization()
        {
            try
            {
                GraphicsContext.ShareContexts = true;
                CreateGLControl();

                GlWindowInfo = OpenTKHelper.GetWindowInfo(Handle, false);
             
                ResourceContext = new GraphicsContext(new GraphicsMode(32, 24, 2, 8), GlWindowInfo);
                ResourceContext.MakeCurrent(null);
                GlContextCreated = true;
            }
            catch (Exception ex)
            {
                MessageBoxEX.ShowDetails(this, "An error occured while initializing GL view.", "Error", ex.ToString());
            }
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
            glControl1.MouseDown += GlControl1_MouseDown;

            LoopController = new LoopController(glControl1);
            LoopController.TargetRenderFrequency = 40;
            LoopController.RenderFrame += LoopController_RenderFrame;
            LoopController.UpdateFrame += LoopController_UpdateFrame;

            SelectionInfoPanel.BringToFront();
        }


        public void InitGlResourcesAsync()
        {
            ResourceContext.MakeCurrent(GlWindowInfo);

            try
            {
                //if (!UIRenderHelper.LoadFreetype6())
                //    throw new Exception("Could not load freetype6.dll");

                TextureManager.InitializeResources();
                UIRenderHelper.InitializeResources();
                RenderHelper.InitializeResources();
                ModelManager.InitializeResources();
            }
            catch(Exception ex)
            {
                BeginInvoke((Action)(() =>
                {
                    MessageBoxEX.ShowException(this, ex.Message, Messages.Caption_UnexpectedError, ex);
                }));
                
                GlContextCreated = false;
                return;
            }
            

            SetupUIElements();

            InputManager = new InputManager();

            CameraManipulator = new CameraManipulator(new Camera());
            CameraManipulator.Initialize(new Vector3(5), Vector3.Zero);

            InitializeSelectionGizmo();
        }

        public override void OnInitializationFinished()
        {
            if (GlContextCreated)
            {
                glControl1.MakeCurrent();

                ModelManager.InitializeBuffers();
                UIRenderHelper.InitializeBuffers();
                RenderHelper.SetupGridShader();

                UpdateGLViewport();

                InitializeInputManager();

                SetupSceneLights();

                LoopController.Start();
            }

            SelectCurrentGizmoOptions();
        }

        private void InitializeInputManager()
        {
            bool isMouseOver = glControl1.ClientRectangle.Contains(PointToClient(Control.MousePosition));

            InputManager.SetContainsMouse(isMouseOver);
            InputManager.ContainsFocus = ContainsFocus;
        }

        private void InitializeMenus()
        {
            visualStudioToolStripExtender1.SetStyle(toolStrip1,
                VisualStudioToolStripExtender.VsVersion.Vs2015,
                DockPanel.Theme);

            globalToolStripMenuItem.Tag = OrientationMode.Global.ToString();
            localToolStripMenuItem.Tag = OrientationMode.Local.ToString();

            boundingBoxCenterToolStripMenuItem.Tag = PivotPointMode.BoundingBox.ToString();
            cursorToolStripMenuItem.Tag = PivotPointMode.Cursor.ToString();
            medianBoundingBoxToolStripMenuItem.Tag = PivotPointMode.MedianCenter.ToString();
            medianOriginsToolStripMenuItem.Tag = PivotPointMode.MedianOrigin.ToString();
            activeElementToolStripMenuItem.Tag = PivotPointMode.ActiveElement.ToString();

            DisplayMenuDropDown.DropDown.Closing += DisplayDropDown_Closing;
            
            UpdateToolbarMenu();
        }

        protected override void InitializeProjectManager(ProjectManager projectManager)
        {
            base.InitializeProjectManager(projectManager);
            ProjectManager.ProjectModified += ProjectManager_ProjectModified;
            ProjectManager.PartModelsVisibilityChanged += ProjectManager_ModelsVisibilityChanged;
            ProjectManager.CollisionsVisibilityChanged += ProjectManager_ModelsVisibilityChanged;
            ProjectManager.ConnectionsVisibilityChanged += ProjectManager_ModelsVisibilityChanged;
            ProjectManager.PartRenderModeChanged += ProjectManager_PartRenderModeChanged;
            ProjectManager.BonesVisibilityChanged += ProjectManager_ModelsVisibilityChanged;
            projectManager.CursorVisibilityChanged += ProjectManager_ModelsVisibilityChanged;
        }

        

        private UIButton SelectGizmoButton;
        private UIButton MoveGizmoButton;
        private UIButton RotateGizmoButton;
        private UIButton ScaleGizmoButton;

        private void SetupUIElements()
        {
            int buttonSpacing = 2;

            int buttonSize = 40;

            SelectGizmoButton = new UIButton()
            {
                Texture = TextureManager.SelectionIcons,
                NormalSprite =      new SpriteBounds(0,0,0.25f,0.25f),
                OverSprite =        new SpriteBounds(0, 0.25f, 0.25f, 0.25f),
                SelectedSprite =    new SpriteBounds(0, 0.5f, 0.25f, 0.25f),
                Selected = true
            };
            SelectGizmoButton.Clicked += SelectGizmoButton_Clicked;
            UIElements.Add(SelectGizmoButton);

            MoveGizmoButton = new UIButton()
            {
                Texture = TextureManager.SelectionIcons,
                NormalSprite =      new SpriteBounds(0.25f, 0, 0.25f, 0.25f),
                OverSprite =        new SpriteBounds(0.25f, 0.25f, 0.25f, 0.25f),
                SelectedSprite =    new SpriteBounds(0.25f, 0.5f, 0.25f, 0.25f),
            };
            MoveGizmoButton.Clicked += MoveGizmoButton_Clicked;
            UIElements.Add(MoveGizmoButton);

            RotateGizmoButton = new UIButton()
            {
                Texture = TextureManager.SelectionIcons,
                NormalSprite =      new SpriteBounds(0.5f, 0, 0.25f, 0.25f),
                OverSprite =        new SpriteBounds(0.5f, 0.25f, 0.25f, 0.25f),
                SelectedSprite =    new SpriteBounds(0.5f, 0.5f, 0.25f, 0.25f),
            };
            UIElements.Add(RotateGizmoButton);
            RotateGizmoButton.Clicked += RotateGizmoButton_Clicked;

            ScaleGizmoButton = new UIButton()
            {
                Texture = TextureManager.SelectionIcons,
                NormalSprite =      new SpriteBounds(0.75f, 0, 0.25f, 0.25f),
                OverSprite =        new SpriteBounds(0.75f, 0.25f, 0.25f, 0.25f),
                SelectedSprite =    new SpriteBounds(0.75f, 0.5f, 0.25f, 0.25f),
                Visible = false
            };
            ScaleGizmoButton.Clicked += ScaleGizmoButton_Clicked;
            UIElements.Add(ScaleGizmoButton);
            //var viewSize = new Vector2(glControl1.Width, glControl1.Height);
            //UIElements.Add(new UIPanel()
            //{
            //    Texture = SelectionIcons,
            //    TextureRect = new SpriteBounds(0, 0.75f, 0.25f, 0.25f),
            //    Bounds = new Vector4(viewSize.X - 30, 0, 30, 30)
            //});

            var gizmoButtons = new UIButton[] { SelectGizmoButton, MoveGizmoButton, RotateGizmoButton, ScaleGizmoButton };

            for (int i = 0; i < gizmoButtons.Length; i++)
            {
                gizmoButtons[i].Bounds = new Vector4(buttonSpacing, 64 + (buttonSize + buttonSpacing) * i, buttonSize, buttonSize);
            }
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

            CursorModel = new CursorModel()
            {
                RenderLayer = 3
            };
            SelectionGizmo.CursorModel = CursorModel;
        }

        private void CalculateLight(ref LightInfo light, Vector3 lightDir, BBox sceneBounds, 
            float lightConeAngle = 50f)
        {
            lightDir = lightDir.Normalized();
            var boxVerts = sceneBounds.GetCorners();
            var ray = new Ray(sceneBounds.Center + lightDir * sceneBounds.Size.Length, lightDir * -1);

            var lightMatrix = Matrix4.LookAt(ray.Origin, sceneBounds.Center, Vector3.UnitY);

            Vector2 minPos = Vector2.Zero;
            Vector2 maxPos = Vector2.Zero;
            float viewDistance = 0;
            float maxDist = float.MinValue;
            float minDist = float.MaxValue;

            foreach (var vert in boxVerts)
            {
                var tp = Vector3.TransformPosition(vert, lightMatrix);
                minPos = Vector2.ComponentMin(tp.Xy, minPos);
                maxPos = Vector2.ComponentMax(tp.Xy, maxPos);
                maxDist = Math.Max(maxDist, tp.Z * -1f);
                minDist = Math.Min(minDist, tp.Z * -1f);
            }

            viewDistance = Math.Abs((maxDist * 0.35f + minDist * 0.75f));

            var maxDiag = Math.Max(maxPos.X - minPos.X, maxPos.Y - minPos.Y);
            var lightDist = (maxDiag * 0.5f) / (float)Math.Tan(MathHelper.ToRadian(lightConeAngle / 2f));
            lightDist = Math.Max(lightDist, 4f);
            light.Position = sceneBounds.Center + (lightDir * lightDist);
            var offset = Vector3.Distance(sceneBounds.Center, light.Position) - Vector3.Distance(sceneBounds.Center, ray.Origin);
            viewDistance += offset;

            var powerOffset = MathHelper.Map(sceneBounds.Size.Length, 0, 25f, 5f, 40f);
            powerOffset = MathHelper.Clamp(powerOffset, 2, 40);
            var lightPower = powerOffset + (viewDistance / 0.10f);

            var lightComponents = RenderHelper.CalculateLightComponents(lightPower);
            light.Linear = lightComponents.X;
            light.Quadratic = lightComponents.Y;
        }

        private void SetupSceneLights()
        {
            

            var sceneBounds = GetSceneBoundingBox();


            bool isFlatShape = sceneBounds.SizeY / 0.5f < ((sceneBounds.SizeX + sceneBounds.SizeZ) / 2f);
            bool isTallShape = sceneBounds.SizeY * 0.6f >= ((sceneBounds.SizeX + sceneBounds.SizeZ) / 2f);
            //bool isTallerThanWidth = sceneBounds.SizeY * 0.6f >= sceneBounds.SizeX;
            //bool isTallerThanDepth = sceneBounds.SizeY * 0.6f >= sceneBounds.SizeZ;

            float heightOffset = isTallShape ? 0.45f : 0.65f;
            float depthOffset = isFlatShape ? 0.65f : 1f;

            var ambiantLight = new LightInfo()
            {
                Ambient = new Vector3(0.5f),
                Diffuse = new Vector3(0.01f),
                Specular = new Vector3(0.01f),
                Constant = 0.8f,
            };
            CalculateLight(ref ambiantLight, new Vector3(1,1,1), sceneBounds, 30);

            var keyLight = new LightInfo()
            {
                Ambient = new Vector3(0.1f),
                Diffuse = new Vector3(0.7f),
                Specular = new Vector3(0.2f),
                Constant = 1f,
            };
            CalculateLight(ref keyLight, new Vector3(0, heightOffset, depthOffset), sceneBounds);

            var fillLight = new LightInfo()
            {
                Ambient = new Vector3(0.1f),
                Diffuse = new Vector3(0.6f),
                Specular = new Vector3(0.2f),
                Constant = 1f,
            };
            CalculateLight(ref fillLight, new Vector3(depthOffset, heightOffset, 0), sceneBounds);

            var backLight = new LightInfo()
            {
                Ambient = new Vector3(0.15f),
                Diffuse = new Vector3(0.5f),
                Specular = new Vector3(0.1f),
                Constant = 1f,
            };
            CalculateLight(ref backLight, new Vector3(depthOffset * -0.6f, heightOffset * 0.75f, depthOffset * -0.4f), sceneBounds);

            SceneLights = new List<LightInfo>()
            {
                ambiantLight,
                keyLight,
                fillLight,
                backLight,
            };
            RenderHelper.ModelShader.Use();
            RenderHelper.ModelShader.Lights.Set(SceneLights.ToArray());
            RenderHelper.ModelShader.LightCount.Set(SceneLights.Count);
            RenderHelper.ModelShader.UseTexture.Set(false);
        }

        #endregion

        public void StopRenderingLoop()
        {
            if (LoopController.IsRunning)
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
 
            if(ProjectManager.ShowGrid)
                DrawGrid(); // Draw grid a first time for transparent meshes/textures

            DrawSceneModels();

            //if (CurrentProject?.Flexible == true && ProjectManager.ShowBones)
            //{
            //    GL.Clear(ClearBufferMask.DepthBufferBit);
            //    DrawBones();
            //}

            GL.UseProgram(0);

            //DebugDrawLights();

            if (SelectionGizmo.Visible)
            {
                GL.Clear(ClearBufferMask.DepthBufferBit);
                SelectionGizmo.Render(Camera);
            }

            GL.UseProgram(0);
            
        }

        private void DrawSceneModels()
        {
            GL.Enable(EnableCap.Texture2D);

            RenderHelper.BindModelTexture(TextureManager.Checkerboard, TextureUnit.Texture4);

            int lastLayer = 0;

            foreach(var renderLayer in LoadedModels
                .GroupBy(x => x.RenderLayer).OrderBy(g => g.Key))
            {
                if (renderLayer.Key != lastLayer)
                {
                    GL.Clear(ClearBufferMask.DepthBufferBit);
                    lastLayer = renderLayer.Key;
                }

                foreach (var model in renderLayer.OrderBy(x => x.ZDepth))
                {
                    if (!model.Visible)
                        continue;

                    if (model is BoneModel && !ProjectManager.ShowBones)
                        continue;

                    if (model is SurfaceModelMesh)
                        model.RenderModel(Camera, ProjectManager.PartRenderMode);
                    else
                        model.RenderModel(Camera);
                }

                Draw3DTexts();

                if (renderLayer.Key == 0 && ProjectManager.ShowGrid)
                    DrawGrid(); //Draw grid as second time 
            }
            

            RenderHelper.UnbindModelTexture();
            TextureManager.Checkerboard.Bind(TextureUnit.Texture0);
            GL.Disable(EnableCap.Texture2D);

            
        }

        private void Draw3DTexts()
        {
            if (UIRenderHelper.Freetype6Loaded && UIRenderHelper.TextRenderer.DrawingPrimitives.Any())
            {
                UIRenderHelper.TextRenderer.RefreshBuffers();
                UIRenderHelper.TextRenderer.Draw();
                UIRenderHelper.TextRenderer.DisableShader();
                UIRenderHelper.TextRenderer.DrawingPrimitives.Clear();
            }
        }

        //private void DrawBones()
        //{
        //    foreach (var model in LoadedModels.OfType<BoneModel>())
        //    {
        //        if (model.Visible)
        //            model.RenderModel(Camera);
        //    }
        //}

        private void DebugDrawLights()
        {
            if (SceneLights != null)
            {
                foreach (var light in SceneLights)
                {
                    RenderHelper.DrawGizmoAxes(Matrix4.CreateTranslation(light.Position), 0.5f, false);
                }
            }
        }

        private void DrawGrid()
        {
            RenderHelper.GridShader.Use();
            RenderHelper.GridShader.MVMatrix.Set(Camera.GetViewMatrix());
            RenderHelper.GridShader.PMatrix.Set(Camera.GetProjectionMatrix());
            RenderHelper.GridShader.FadeDistance.Set(Camera.IsPerspective ? 20f : 0f);
            GL.DepthMask(false);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(-40, 0, -40);
            GL.Vertex3(-40, 0, 40);
            GL.Vertex3(40, 0, 40);
            GL.Vertex3(40, 0, -40);
            GL.End();
            GL.DepthMask(true);
        }
        
        private double AvgRenderFPS = 0;

        private void RenderUI()
        {
            //var viewSize = new Vector2(glControl1.Width, glControl1.Height);

            GL.Disable(EnableCap.DepthTest);

            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref UIProjectionMatrix);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

            GL.Enable(EnableCap.Texture2D);
            UIRenderHelper.IntializeBeforeRender();

            foreach (var elem in UIElements)
                elem.Draw();

            if (CurrentProject != null && CurrentProject.Flexible && ProjectManager.ShowBones)
                DrawBoneNames();


            if (AvgRenderFPS == 0)
                AvgRenderFPS = LoopController.RenderFrequency;
            else
                AvgRenderFPS = (AvgRenderFPS + LoopController.RenderFrequency) / 2d;

            //UIRenderHelper.DrawText($"Render FPS: {AvgRenderFPS:0}", UIRenderHelper.NormalFont, Color.White, new Vector2(10, 10));

            if (SelectionGizmo.IsEditing)
                DrawGizmoStatus();

            if (CurrentProject != null)
            {
                UIRenderHelper.DrawText($"{CurrentProject.TotalTriangles} triangles", UIRenderHelper.SmallFont, Color.Black, 
                    new Vector4(UIRenderHelper.ViewSize.X - 104, UIRenderHelper.ViewSize.Y - 20, 100, 16), 
                    StringAlignment.Far, StringAlignment.Far);
            }
            
            UIRenderHelper.RenderElements();
        }

        private void DrawBoneNames()
        {
            var boneModels = LoadedModels.OfType<BoneModel>().ToList();

            foreach (var bone in boneModels)
            {
                if (!bone.Visible)
                    continue;

                var rootPos = bone.Transform.ExtractTranslation();
                var tipPos = rootPos + Vector3.TransformVector(new Vector3(bone.BoneLength, 0, 0), bone.Transform);
                var boneCenter = (rootPos + tipPos) / 2f;
                var screenPos = Camera.WorldPointToScreen(boneCenter);
                var boneBounds = new Vector4(screenPos.X - 30, screenPos.Y - 20, 60, 40);

                UIRenderHelper.DrawShadowText(bone.Element.Name,
                    UIRenderHelper.NormalFont, Color.White, boneBounds,
                    StringAlignment.Center, StringAlignment.Center);
            }
        }

        private void DrawGizmoStatus()
        {
            string amountStr = "";
            if (SelectionGizmo.DisplayStyle == GizmoStyle.Rotation)
            {
                float degrees = (SelectionGizmo.TransformedAmount / (float)Math.PI) * 180f;
                amountStr = $"Rotation: {degrees:0.##}°";
            }
            else if (SelectionGizmo.DisplayStyle == GizmoStyle.Scaling)
            {
                amountStr = $"Scaling: {SelectionGizmo.TransformedAmount:0.##}";
            }
            else if (SelectionGizmo.DisplayStyle == GizmoStyle.Translation)
                amountStr = $"Translation: {SelectionGizmo.TransformedAmount:0.##}";

            UIRenderHelper.DrawText(amountStr, UIRenderHelper.NormalFont, Color.Black, new Vector2(10, UIRenderHelper.ViewSize.Y - 20));
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
                if (!InputManager.IsShiftDown() && !InputManager.IsControlDown())
                {
                    if (InputManager.IsKeyPressed(Key.S))
                        SelectionGizmo.DisplayStyle = GizmoStyle.Plain;
                    else if (InputManager.IsKeyPressed(Key.R))
                        SelectionGizmo.DisplayStyle = GizmoStyle.Rotation;
                    else if (InputManager.IsKeyPressed(Key.T))
                        SelectionGizmo.DisplayStyle = GizmoStyle.Translation;
                    //else if (InputManager.IsKeyPressed(Key.S) && ScaleGizmoButton.Visible)
                    //    SelectionGizmo.DisplayStyle = GizmoStyle.Scaling;
                }
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

                foreach (var elem in UIElements
                    .OrderByDescending(x => x.ZOrder)
                    .Where(x => x.Visible))
                {
                    if (overElement == null && elem.Contains(InputManager.LocalMousePos))
                    {
                        overElement = elem;
                        elem.SetIsOver(true);
                    }
                    else
                        elem.SetIsOver(false);
                }

                if (InputManager.ContainsFocus)
                {
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

            var cameraProjection = Camera.GetProjectionMatrix();

            foreach (var model in LoadedModels.ToArray())
            {
                var originPt = Vector3.TransformPosition(model.BoundingBox.Center, model.Transform);
                var cameraPt = Camera.WorldPointToViewport(originPt);
                model.ZDepth = cameraPt.Z * -1;
            }

            try
            {
                //TODO: improve access to LoadedModels list to prevent collection changed exceptions
                var boneModels = LoadedModels.ToArray().OfType<BoneModel>().ToList();
                if (boneModels.Any(x => x.IsLengthDirty))
                {
                    foreach (var boneModel in boneModels)
                        boneModel.CalculateBoneLength();
                }
                CursorModel.Visible = ProjectManager.Show3dCursor;
            }
            catch { }
            
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
            if (SelectionTransformEditor.Visible && SelectionTransformEditor.PhysicalElement == null)
            {
                UpdateSelectionInfoPanel();
            }
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

        private void ScaleGizmoButton_Clicked(object sender, EventArgs e)
        {
            if (!ScaleGizmoButton.Selected && !SelectionGizmo.IsEditing)
            {
                SelectionGizmo.DisplayStyle = GizmoStyle.Scaling;
            }
        }

        private void UpdateGizmoButtonStates()
        {
            MoveGizmoButton.Selected = SelectionGizmo.DisplayStyle == GizmoStyle.Translation;
            SelectGizmoButton.Selected = SelectionGizmo.DisplayStyle == GizmoStyle.Plain;
            RotateGizmoButton.Selected = SelectionGizmo.DisplayStyle == GizmoStyle.Rotation;
            ScaleGizmoButton.Selected = SelectionGizmo.DisplayStyle == GizmoStyle.Scaling;
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

        private void UpdateDocumentTitle()
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(UpdateDocumentTitle));
            else
            {
                Text = ProjectManager.GetProjectDisplayName();
                if (ProjectManager.IsModified)
                    Text += "*";
            }
        }

        #endregion

        private void DisposeGLResources()
        {
            UnloadModels();
            if (SelectionGizmo != null)
                SelectionGizmo.Dispose();
            if (CursorModel != null)
                CursorModel.Dispose();

            UIRenderHelper.ReleaseResources();
            RenderHelper.ReleaseResources();
            ModelManager.ReleaseResources();
            TextureManager.ReleaseResources();

            SelectionGizmo = null;
            CursorModel = null;

            if (ResourceContext != null)
            {
                ResourceContext.Dispose();
                ResourceContext = null;
            }
        }

        #region Project Handling

        protected override void OnProjectChanged()
        {
            base.OnProjectChanged();
            UpdateDocumentTitle();
            UpdateToolbarMenu();

            SetupSceneLights();
        }

        protected override void OnProjectLoaded(PartProject project)
        {
            base.OnProjectLoaded(project);

            RebuildSurfaceModels();
            RebuildCollisionModels();
            RebuildConnectionModels();
            RebuildBoneModels();
            SetupDefaultCamera();
            LoadedModels.Add(CursorModel);
        }

        private void ProjectManager_ProjectModified(object sender, EventArgs e)
        {
            UpdateDocumentTitle();
        }

        private void AddPartSurfaceModel(PartSurface surface)
        {
            var surfModel = new SurfaceMeshBuffer(surface);

            var material = new MaterialInfo
            {
                Diffuse = new Vector4(0.7f, 0.7f, 0.7f, 1f),
                Specular = new Vector3(1f),
                Shininess = 8f
            };

            if (surface.SurfaceID > 0)
            {
                material.Shininess = 1f;
                //material.Diffuse = new Vector4(0.4f, 0.4f, 0.4f, 1f);
                material.Specular = new Vector3(0.4f, 0.4f, 0.4f);
            }

            surfModel.Material = material;
            //if (surface.SurfaceID > 0)
            //{
            //    var matColor = Color4.FromHsv(new Vector4((surface.SurfaceID * 0.2f) % 1f, 0.9f, 0.8f, 1f));
            //    surfModel.Material = new MaterialInfo
            //    {
            //        Diffuse = new Vector4(matColor.R, matColor.G, matColor.B, surfModel.Material.Diffuse.W),
            //        Specular = new Vector3(1f),
            //        Shininess = 6f
            //    };
            //}

            surfModel.RebuildPartModels();
            SurfaceModels.Add(surfModel);
        }

        protected override void OnProjectClosed()
        {
            base.OnProjectClosed();
            SelectionGizmo.Deactivate();
            UnloadModels();
            //SetupSceneLights();
        }

        private bool SurfaceMeshesChanged;
        private bool CollisionsChanged;
        private bool ConnectionsChanged;
        private bool BonesChanged;
        private bool PatternsChanged;
        private bool ModelsMoved;

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
            else if (e.ElementType == typeof(PartBone))
            {
                BonesChanged = true;
            }
            else if (e.ElementType == typeof(ClonePattern))
            {
                PatternsChanged = true;
            }
        }

        public void InvalidateBones()
        {
            BonesChanged = true;
            ConnectionsChanged = true;
        }

        protected override void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.Element is PartProperties)
            {
                if (e.PropertyName == nameof(PartProperties.ID) ||
                    e.PropertyName == nameof(PartProperties.Description))
                {
                    UpdateDocumentTitle();
                }
                else if (e.PropertyName == nameof(PartProperties.Flexible))
                {
                    BonesDropDownMenu.Visible = CurrentProject.Flexible;
                }
            }
            if (e.PropertyName == nameof(ITransformableElement.Transform))
            {
                ModelsMoved = true;
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

            if (BonesChanged)
                RebuildBoneModels();

            if (PatternsChanged)
                RebuildPatternModels();
            
            if (ModelsMoved)
                SetupSceneLights();

            SurfaceMeshesChanged = false;
            CollisionsChanged = false;
            ConnectionsChanged = false;
            BonesChanged = false;
            PatternsChanged = false;
            ModelsMoved = false;
        }

        #endregion

        #region Model/Mesh Handling

        private void RebuildSurfaceModels()
        {
            LoadedModels.RemoveAll(x => x is SurfaceModelMesh);

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
                    if (!SurfaceModels.Any(x => x.Surface == surface))
                    {
                        AddPartSurfaceModel(surface);
                    }
                    else
                    {
                        var model = SurfaceModels.FirstOrDefault(x => x.Surface == surface);
                        model.RebuildPartModels();
                    }
                }

                LoadedModels.AddRange(SurfaceModels.SelectMany(x => x.MeshModels));
            }
            else if (SurfaceModels.Any())
            {
                SurfaceModels.ForEach(x => x.Dispose());
                SurfaceModels.Clear();
            }
        }

        private void RebuildCollisionModels()
        {
            var existingModels = LoadedModels.OfType<CollisionModel>().ToList();

            if (ProjectManager.IsProjectOpen)
            {
                var allCollisions = CurrentProject.GetAllElements<PartCollision>().ToList();

                foreach (var colModel in existingModels)
                {
                    if (!allCollisions.Contains(colModel.PartCollision))
                    {
                        colModel.Dispose();
                        LoadedModels.Remove(colModel);
                    }
                }

                foreach (var collision in allCollisions)
                {
                    if (!existingModels.Any(x => x.PartCollision == collision))
                        LoadedModels.Add(new CollisionModel(collision));
                }
            }
            else if (existingModels.Any())
            {
                existingModels.ForEach(x =>
                {
                    x.Dispose();
                    LoadedModels.Remove(x);
                });
            }
        }

        private void RebuildConnectionModels()
        {
            var existingModels = LoadedModels.OfType<ConnectionModel>().ToList();

            if (ProjectManager.IsProjectOpen)
            {
                var allConnections = CurrentProject.GetAllElements<PartConnection>().ToList();

                foreach (var conModel in existingModels)
                {
                    if (!allConnections.Contains(conModel.Connection))
                    {
                        conModel.Dispose();
                        LoadedModels.Remove(conModel);
                    }
                }

                foreach (var connection in allConnections)
                {
                    if (!existingModels.Any(x => x.Connection == connection))
                        LoadedModels.Add(new ConnectionModel(connection));
                }
            }
            else if (existingModels.Any())
            {
                existingModels.ForEach(x =>
                {
                    x.Dispose();
                    LoadedModels.Remove(x);
                });
            }
        }

        private void RebuildBoneModels()
        {
            var existingModels = LoadedModels.OfType<BoneModel>().ToList();

            if (ProjectManager.IsProjectOpen)
            {
                var allBones = CurrentProject.GetAllElements<PartBone>().ToList();

                foreach (var conModel in existingModels)
                {
                    if (!allBones.Contains(conModel.Bone))
                    {
                        conModel.Dispose();
                        LoadedModels.Remove(conModel);
                    }
                }

                foreach (var bone in allBones)
                {
                    if (!existingModels.Any(x => x.Bone == bone))
                    {
                        LoadedModels.Add(new BoneModel(bone)
                        {
                            RenderLayer = 1
                        });
                    }
                }
            }
            else if (existingModels.Any())
            {
                existingModels.ForEach(x =>
                {
                    x.Dispose();
                    LoadedModels.Remove(x);
                });
            }
        }

        private void RebuildPatternModels()
        {
            var existingModels = LoadedModels.OfType<ClonePatternModel>().ToList();
            if (ProjectManager.IsProjectOpen)
            {
                var allPatterns = CurrentProject.GetAllElements<ClonePattern>().ToList();

                foreach (var patternModel in existingModels)
                {
                    if (!allPatterns.Contains(patternModel.ClonePattern))
                    {
                        patternModel.Dispose();
                        LoadedModels.Remove(patternModel);
                    }
                }

                foreach (var pattern in allPatterns)
                {
                    var patternModel = existingModels.FirstOrDefault(x => x.ClonePattern == pattern);
                    if (patternModel == null)
                    {
                        patternModel = new ClonePatternModel(pattern);
                        LoadedModels.Add(patternModel);
                    }
                    var elemModels = LoadedModels.OfType<PartElementModel>().Where(x => pattern.Elements.Contains(x.Element));
                    patternModel.SetElementModels(elemModels);
                }
            }
            else if (existingModels.Any())
            {
                existingModels.ForEach(x =>
                {
                    x.Dispose();
                    LoadedModels.Remove(x);
                });
            }
        }

        public void UnloadModels()
        {
            SurfaceModels.ForEach(x => x.Dispose());

            foreach (var model in LoadedModels)
                model.Dispose();
            LoadedModels.Clear();

            SurfaceModels.Clear();
            //CollisionModels.Clear();
            //ConnectionModels.Clear();
            GC.Collect();
        }

        public IEnumerable<PartElementModel> GetAllElementModels()
        {
            return LoadedModels.OfType<PartElementModel>();
        }

        public IEnumerable<ModelBase> GetVisibleModels()
        {
            return LoadedModels.Where(x => x.Visible);
        }

        public IEnumerable<ModelBase> GetSelectedModels(bool onlyVisible = false)
        {
            var selectedModels = LoadedModels.Where(m => m.IsSelected && (!onlyVisible || m.Visible));

            return selectedModels
                .OrderBy(x => x is PartElementModel pem ? 
                ProjectManager.GetSelectionIndex(pem.Element) : 0);
        }

        public IEnumerable<PartElementModel> GetSelectedElementModels(bool onlyVisible = false)
        {
            var selectedModels = LoadedModels.OfType<PartElementModel>()
                .Where(m => m.IsSelected && (!onlyVisible || m.Visible));

            return selectedModels
                .OrderBy(x => ProjectManager.GetSelectionIndex(x.Element));
        }

        #endregion

        #region Selection Handling

        protected override void OnElementSelectionChanged()
        {
            base.OnElementSelectionChanged();
            if (CurrentProject == null)
                return;

            bool isFlexible = CurrentProject.Bones.Any();

            foreach (var model in GetAllElementModels())
            {
                if (isFlexible && model.Element.Parent is IPhysicalElement)
                {
                    model.IsSelected = ProjectManager.IsSelected(model.Element);
                }
                else
                    model.IsSelected = ProjectManager.IsContainedInSelection(model.Element);
            }

            UpdateGizmoFromSelection();
            UpdateSelectionInfoPanel();
        }

        private void UpdateGizmoFromSelection()
        {
            var selectedModels = GetSelectedModels(true).ToList();

            if (selectedModels.Any() &&
                selectedModels.All(x => x is CollisionModel))
            {
                ScaleGizmoButton.Visible = true;
            }

            else if (ScaleGizmoButton.Visible)
            {
                bool hideButton = selectedModels.Count > 0 || SelectionGizmo.DisplayStyle != GizmoStyle.Scaling;

                if (hideButton)
                {
                    ScaleGizmoButton.Visible = false;
                    if (SelectionGizmo.DisplayStyle == GizmoStyle.Scaling)
                        SelectionGizmo.DisplayStyle = GizmoStyle.Plain;
                }
                
            }

            if (selectedModels.Any())
            {
                SelectionGizmo.SetActiveElements(selectedModels);

                SelectionGizmo.UpdateBounds(Camera);
            }
            else
            {
                SelectionGizmo.Deactivate();
            }
        }

        private void UpdateSelectionInfoPanel()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(UpdateSelectionInfoPanel));
                return;
            }
            using (FlagManager.UseFlag(nameof(UpdateSelectionInfoPanel)))
            {
                int activeObjectCount = SelectionGizmo.ActiveElements.Count();

                if (activeObjectCount == 1)
                {
                    var activeObject = SelectionGizmo.ActiveElements.FirstOrDefault();


                    if (activeObject is PartElementModel elementModel &&
                        elementModel.Element is IPhysicalElement physicalElement)
                    {
                        SelectionTransformEditor.BindPhysicalElement(physicalElement);
                    }
                    else
                    {
                        SelectionTransformEditor.BindPhysicalElement(null);
                        var lddTrans = ItemTransform.FromMatrix(activeObject.Transform.ToLDD());
                        SelectionTransformEditor.Value = lddTrans;
                    }
                }
                else
                    SelectionTransformEditor.BindPhysicalElement(null);

                SelectionInfoPanel.Visible = (activeObjectCount == 1);
            }
        }

        private void SelectionTransformEditor_ValueChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet(nameof(UpdateSelectionInfoPanel)))
                return;

            if (SelectionTransformEditor.PhysicalElement == null)
            {
                var activeObject = SelectionGizmo.ActiveElements.FirstOrDefault();
                if (activeObject != null)
                    activeObject.Transform = SelectionTransformEditor.Value.ToMatrix().ToGL();
            }
        }

        private void PerformRaySelection(Ray ray)
        {
            var visibleModels = GetVisibleModels();
            var selectedModels = LoadedModels.Where(x => x.IsSelected).ToList();
            bool nonElementModelSelected = false;

            foreach(var model in selectedModels)
            {
                if (!(model is PartElementModel))
                {
                    model.IsSelected = false;
                    nonElementModelSelected = true;
                }
            }

            if (ray != null && visibleModels.Any())
            {
                var intersectingModels = new List<Tuple<ModelBase, float>>();

                foreach (var model in visibleModels)
                {
                    if (!model.IsSelectable)
                        continue;

                    if (model.RayIntersectsBoundingBox(ray, out float boxDist))
                    {
                        if (model.RayIntersects(ray, out float triangleDist))
                        {
                            intersectingModels.Add(new Tuple<ModelBase, float>(model, triangleDist));
                        }
                    }
                }

                int maxRenderLayer = intersectingModels.Any() ? 
                    intersectingModels.Max(x => x.Item1.RenderLayer) : 0;

                var closestHit = intersectingModels
                    .Where(x=>x.Item1.RenderLayer == maxRenderLayer)
                    .OrderBy(x => x.Item2).FirstOrDefault();

                //if (intersectingModels.Any(x => x.Item1 is BoneModel))
                //{
                //    closestHit = intersectingModels.Where(x => x.Item1 is BoneModel)
                //        .OrderBy(x => x.Item2).FirstOrDefault();
                //}

                var closestModel = closestHit?.Item1;
                var closestElement = (closestModel as PartElementModel)?.Element;

                if (closestModel != null && closestElement == null)
                {
                    ProjectManager.ClearSelection();
                    closestModel.IsSelected = true;
                    nonElementModelSelected = true;
                }
                else if(closestElement != null)
                {
                    nonElementModelSelected = false;

                    if (InputManager.IsControlDown())
                    {
                        ProjectManager.SetSelected(closestElement, !ProjectManager.IsSelected(closestElement));
                    }
                    else if (InputManager.IsShiftDown())
                    {
                        ProjectManager.SetSelected(closestElement, true);
                    }
                    else if (closestElement != null)
                    {
                        ProjectManager.SelectElement(closestElement);
                    }
                }
                else
                    ProjectManager.ClearSelection();
            }

            if (nonElementModelSelected)
            {
                UpdateGizmoFromSelection();
                UpdateSelectionInfoPanel();
            }
        }

        #endregion

        #region Camera & Viewport Handling

        private void SetupDefaultCamera()
        {
            SetCameraAlignment(CameraAlignment.Isometric);

            //if (CurrentProject != null && CurrentProject.DefaultOrientation != null)
            //{
            //    var matrix = CurrentProject.DefaultOrientation.ToMatrix().ToGL().Inverted();
            //    var newPos = Vector3.TransformPosition(Camera.Position, matrix);
            //    var newUp = Vector3.TransformNormal(Camera.Up, matrix);
            //    CameraManipulator.Initialize(newPos, CameraManipulator.Gimbal, newUp); 
            //}
            
        }

        public BBox GetSceneBoundingBox()
        {
            if (CurrentProject != null && LoadedModels.Any())
            {
                var bbox = CalculateBoundingBox(LoadedModels);
                if (!bbox.IsEmpty)
                    return bbox;
                //var modelMeshes = SurfaceModels.SelectMany(x => x.MeshModels);
                //if (modelMeshes.Any())
                //    return CalculateBoundingBox(modelMeshes);
                
            }

            return BBox.FromCenterSize(Vector3.Zero, new Vector3(2f));
        }

        private BBox CalculateBoundingBox(IEnumerable<ModelBase> modelMeshes)
        {
            Vector3 minPos = new Vector3(float.MaxValue);
            Vector3 maxPos = new Vector3(float.MinValue);

            foreach (var model in modelMeshes)
            {
                //if (!(model is SurfaceModelMesh))
                //    continue;

                var worldBounding = model.GetWorldBoundingBox();
                
                minPos = Vector3.ComponentMin(minPos, worldBounding.Min);
                maxPos = Vector3.ComponentMax(maxPos, worldBounding.Max);
            }

            return BBox.FromMinMax(minPos, maxPos);
        }

        private void CalculateCameraDirection(CameraAlignment alignment, out Vector3 cameraDirection, out Vector3 upVector)
        {
            cameraDirection = Vector3.Zero;
            upVector = Vector3.UnitY;

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
        }

        public void SetCameraAlignment(CameraAlignment alignment)
        {
            CalculateCameraDirection(alignment, out Vector3 cameraDirection, out Vector3 upVector);
            RepositionCamera(cameraDirection, upVector);
        }

        private void RepositionCamera(Vector3 cameraDirection, Vector3 upVector)
        {
            var visibleModels = LoadedModels.OfType<SurfaceModelMesh>();
            var bounding = GetSceneBoundingBox();

            float distanceToTarget = 3f;
            Vector2 targetSize = new Vector2(2f);

            CameraManipulator.Initialize(bounding.Center + cameraDirection *  3, bounding.Center, upVector);
            var viewMatrix = CameraManipulator.Camera.GetViewMatrix();

            if (!bounding.IsEmpty)
            {
                var corners = bounding.GetCorners();
                for (int i = 0; i < corners.Length; i++)
                {
                    corners[i] = Vector3.TransformPosition(corners[i], viewMatrix);
                }
                var minX = corners.Min(v => v.X);
                var maxX = corners.Max(v => v.X);

                var minY = corners.Min(v => v.Y);
                var maxY = corners.Max(v => v.Y);

                targetSize = new Vector2(maxX - minX, maxY - minY);
            }

            if (visibleModels.Any())
            {
                var frontRay = new Ray(bounding.Center, cameraDirection);
                var rightNormal = Vector3.Cross(cameraDirection, upVector);
                var upNormal = Vector3.Cross(cameraDirection, rightNormal);
                var topRay = new Ray(bounding.Center, upNormal);
                var sideRay = new Ray(bounding.Center, rightNormal);

                if (Ray.IntersectsBox(frontRay, bounding, out float distance))
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
            if (ViewInitialized && InputManager != null)
                InputManager.SetContainsMouse(true);
        }

        private void GlControl_MouseLeave(object sender, EventArgs e)
        {
            if (!ViewInitialized || InputManager == null)
                return;

            InputManager.SetContainsMouse(false);
            foreach (var elem in UIElements)
                elem.SetIsOver(false);

        }

        private void GlControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!ViewInitialized || InputManager == null)
                return;

            if (e.Button == MouseButtons.Left)
            {
                if (glControl1.ContainsFocus && !InputManager.ContainsFocus)
                {
                    InputManager.ContainsFocus = true;
                    glControl1.Focus();
                }
            }
        }

        private void GlControl1_GotFocus(object sender, EventArgs e)
        {
            if (ViewInitialized && InputManager != null)
                InputManager.ContainsFocus = true;
        }

        private void GlControl1_LostFocus(object sender, EventArgs e)
        {
            if (ViewInitialized && InputManager != null)
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
            if (ViewInitialized && InputManager != null)
                InputManager.ProcessMouseMove(e);
        }

        #endregion

        #region Toolbar Menu

        private void UpdateToolbarMenu()
        {
            BonesDropDownMenu.Visible = CurrentProject?.Flexible ?? false;
            DisplayMenu_Bones.Visible = CurrentProject?.Flexible ?? false;
            MeshesMenu_CalculateOutlines.Enabled = ProjectManager.IsProjectOpen;
            MeshesMenu_RemoveOutlines.Enabled = ProjectManager.IsProjectOpen;
        }

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
            SetCameraAlignment(alignment);
        }

        private void CameraMenu_Orthographic_CheckedChanged(object sender, EventArgs e)
        {
            Camera.IsPerspective = !CameraMenu_Orthographic.Checked;
        }

        private void ProjectManager_ModelsVisibilityChanged(object sender, EventArgs e)
        {
            if (SelectionGizmo.Visible)
                SelectionGizmo.Deactivate();

            using (FlagManager.UseFlag("ModelsVisibilityChanged"))
            {
                DisplayMenu_Collisions.Checked = ProjectManager.ShowCollisions;
                DisplayMenu_Connections.Checked = ProjectManager.ShowConnections;
                DisplayMenu_Meshes.Checked = ProjectManager.ShowPartModels;
                DisplayMenu_Bones.Checked = ProjectManager.ShowBones;
                DisplayMenu_3dCursor.Checked = ProjectManager.Show3dCursor;
                UpdateGizmoFromSelection();
            }
        }

        private void ProjectManager_PartRenderModeChanged(object sender, EventArgs e)
        {
            using (FlagManager.UseFlag("PartRenderModeChanged"))
            {
                ModelRenderMode1Button.Checked = ProjectManager.PartRenderMode == MeshRenderMode.Wireframe;
                ModelRenderMode2Button.Checked = ProjectManager.PartRenderMode == MeshRenderMode.Solid;
                ModelRenderMode3Button.Checked = ProjectManager.PartRenderMode == MeshRenderMode.SolidWireframe;
            }
        }

        private void DisplayMenu_Collisions_CheckedChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("ModelsVisibilityChanged"))
                ProjectManager.ShowCollisions = DisplayMenu_Collisions.Checked;
        }

        private void DisplayMenu_Connections_CheckedChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("ModelsVisibilityChanged"))
                ProjectManager.ShowConnections = DisplayMenu_Connections.Checked;
        }

        private void DisplayMenu_Meshes_CheckedChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("ModelsVisibilityChanged"))
                ProjectManager.ShowPartModels = DisplayMenu_Meshes.Checked;
        }

        private void DisplayMenu_3dCursor_CheckedChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("ModelsVisibilityChanged"))
                ProjectManager.Show3dCursor = DisplayMenu_3dCursor.Checked;
        }

        private void DisplayMenu_Bones_CheckedChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("ModelsVisibilityChanged"))
                ProjectManager.ShowBones = DisplayMenu_Bones.Checked;
        }

        private void DisplayDropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }

        private void ToggleGridButton_CheckedChanged(object sender, EventArgs e)
        {
            ProjectManager.ShowGrid = ToggleGridButton.Checked;
        }

        private void ModelRenderModeButton_Click(object sender, EventArgs e)
        {
            if (FlagManager.IsSet("ModelsVisibilityChanged"))
                return;

            if (sender == ModelRenderMode1Button)
                ProjectManager.PartRenderMode = MeshRenderMode.Wireframe;
            else if (sender == ModelRenderMode2Button)
                ProjectManager.PartRenderMode = MeshRenderMode.Solid;
            else if (sender == ModelRenderMode3Button)
                ProjectManager.PartRenderMode = MeshRenderMode.SolidWireframe;
        }

        private void MeshesDropDownMenu_DropDownOpening(object sender, EventArgs e)
        {
            bool hasSelectedMeshes = false;
            if (ProjectManager.IsProjectOpen)
            {
                hasSelectedMeshes = ProjectManager.GetSelectionHierarchy().OfType<ModelMeshReference>().Any();
            }

            MeshesMenu_Merge.Enabled = hasSelectedMeshes;
            MeshesMenu_Separate.Enabled = hasSelectedMeshes;
        }

        private void MeshesMenu_Separate_Click(object sender, EventArgs e)
        {
            if (ProjectManager.IsProjectOpen)
            {
                ProjectManager.StartBatchChanges();
                var selectedMeshes = ProjectManager.GetSelectionHierarchy().OfType<ModelMeshReference>().ToList();
                foreach (var meshRef in selectedMeshes)
                    ProjectManager.CurrentProject.SplitMeshSurfaces(meshRef);
                ProjectManager.CurrentProject.SaveMeshesToXml();
                ProjectManager.EndBatchChanges();
            }
        }

        private void MeshesMenu_Merge_Click(object sender, EventArgs e)
        {

            if (ProjectManager.IsProjectOpen)
            {
                ProjectManager.StartBatchChanges();
                var selectedMeshes = ProjectManager.GetSelectionHierarchy().OfType<ModelMeshReference>().ToList();
                ProjectManager.CurrentProject.CombineMeshes(selectedMeshes);
                ProjectManager.CurrentProject.SaveMeshesToXml();
                ProjectManager.EndBatchChanges();
            }
        }

        private void MeshesMenu_CalculateOutlines_Click(object sender, EventArgs e)
        {
            if (ProjectManager.IsProjectOpen)
                CurrentProject.ComputeEdgeOutlines();
        }

        private void MeshesMenu_RemoveOutlines_Click(object sender, EventArgs e)
        {
            if (ProjectManager.IsProjectOpen)
                CurrentProject.ClearEdgeOutlines();
        }

        private void Bones_CalcBounding_Click(object sender, EventArgs e)
        {
            if (CurrentProject != null)
            {
                ProjectManager.StartBatchChanges();
                CurrentProject.CalculateBoneBoundingBoxes();
                ProjectManager.EndBatchChanges();
            }
        }

        private void Bones_LinkBones_Click(object sender, EventArgs e)
        {
            ProjectManager.ShowLinkBonesDialog();
        }

        private void Bones_RebuildConnections_Click(object sender, EventArgs e)
        {
            ProjectManager.RebuildBoneConnections();
        }

        private void Bones_CopyData_Click(object sender, EventArgs e)
        {
            ProjectManager.ShowCopyBoneDataDialog();
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
            if (SelectionGizmo == null)
                return;

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

        #region Keyboard Handling

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //Trace.WriteLine($"ProcessCmdKey( keyData => {keyData} ({(int)keyData}))");
            var normalKey = keyData & ~Keys.Control;
            normalKey &= ~Keys.Shift;
            normalKey &= ~Keys.Alt;

            bool isControlPressed = (keyData & Keys.Control) != 0;
            bool isShiftPressed = (keyData & Keys.Shift) != 0;
            bool isAltPressed = (keyData & Keys.Alt) != 0;
            //Trace.WriteLine($"normalKey=> {normalKey} isControlPressed => {isControlPressed}, Is3DViewFocused => {Is3DViewFocused}");
            
            if (Is3DViewFocused)
            {
                if (normalKey == Keys.NumPad0)
                {
                    SetCameraAlignment(CameraAlignment.Isometric);
                    return true;
                }
                else if (normalKey == Keys.NumPad7)
                {
                    if (isControlPressed)
                        SetCameraAlignment(CameraAlignment.Bottom);
                    else
                        SetCameraAlignment(CameraAlignment.Top);
                    return true;
                }
                else if (normalKey == Keys.NumPad1)
                {
                    if (isControlPressed)
                        SetCameraAlignment(CameraAlignment.Back);
                    else
                        SetCameraAlignment(CameraAlignment.Front);
                    return true;
                }
                else if (normalKey == Keys.NumPad3)
                {
                    if (isControlPressed)
                        SetCameraAlignment(CameraAlignment.Left);
                    else
                        SetCameraAlignment(CameraAlignment.Right);

                    return true;
                }
                else if (normalKey == Keys.Delete && ProjectManager.IsProjectOpen)
                {

                    var selectedModelElements = GetSelectedElementModels(true)
                        .Select(x => x.Element).ToList();

                    if (selectedModelElements.Any())
                    {
                        ProjectManager.DeleteElements(selectedModelElements);
                        return true;
                    }
                    
                }
                else if (normalKey == Keys.H && ProjectManager.IsProjectOpen)
                {
                    if (!isShiftPressed && !isAltPressed)
                    {
                        ProjectManager.HideSelectedElements();
                        return true;
                    }
                    else if (isShiftPressed)
                    {
                        ProjectManager.HideUnselectedElements();
                        return true;
                    }
                    else if (isAltPressed)
                    {
                        ProjectManager.UnhideEverything();
                        return true;
                    }
                }
                else if (normalKey == Keys.C && isControlPressed && ProjectManager.IsProjectOpen)
                {
                    ProjectManager.CopySelectedElementsToClipboard();
                    return true;
                }
                else if (normalKey == Keys.V && isControlPressed && ProjectManager.IsProjectOpen)
                {
                    ProjectManager.HandlePasteFromClipboard();
                    return true;
                }
                else if (normalKey == Keys.A && isControlPressed && ProjectManager.IsProjectOpen)
                {
                    ProjectManager.SelectAllVisible();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
        
        #region IViewportWindow

        public void RebuildModels()
        {
            RebuildCollisionModels();
            RebuildConnectionModels();
            RebuildBoneModels();
            RebuildSurfaceModels();
            RebuildPatternModels();
        }

        public void ForceRender()
        {
            if (!LoopController.IsRunning)
                LoopController.ForceRender();
        }

        public bool Is3DViewFocused => InputManager.ContainsFocus;

        #endregion

        
    }
}
