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

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ViewportPanel : ProjectDocumentPanel
    {
        private bool ViewInitialized;
        private QFontDrawing TextRenderer;
        private QFont RenderFont;
        private Matrix4 UIProjectionMatrix;
        private Matrix4 WorldProjectionMatrix;
        private Matrix4 CameraMatrix;
        private Vector3 CameraPosition;
        private Vector3 SceneCenter;
        private bool IsClosing;
        private bool RenderLoopEnabled;
        private GLControl glControl1;
        private GridShaderProgram GridShader;
        private BasicShaderProgram BrickShader;

        private List<GLMeshBase> PartMeshes;

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
            PartMeshes = new List<GLMeshBase>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeBase();
            InitializeView();
            StartRenderLoop();
        }

        private void InitializeBase()
        {
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
                Color = new Color4(0.6f, 0.6f, 0.6f, 0.8f),
                Spacing = 0.4f,
                Thickness = 0.75f,
                OffCenter = false
            });

            BrickShader = ProgramFactory.Create<BasicShaderProgram>();
            BrickShader.Use();
            BrickShader.LightPosition.Set(new Vector3(-8, 12, 4));

            CameraPosition = new Vector3(5);
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

            glControl1.MakeCurrent();
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.ClearColor(glControl1.BackColor);

            UIProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, glControl1.Width, 0, glControl1.Height, -1.0f, 1.0f);
   
            var aspectRatio = glControl1.Width / (float)glControl1.Height;
            WorldProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.PiOver4, aspectRatio, 0.1f, 1000);
            ViewInitialized = true;

            //CameraMatrix = Matrix4.LookAt(new Vector3(0,5,0), Vector3.Zero, Vector3.UnitZ * -1);//top-down
            CameraMatrix = Matrix4.LookAt(CameraPosition, SceneCenter, Vector3.UnitY);
             
            OnRenderFrame();
        }

        private void RenderWorld()
        {
            GL.UseProgram(0);
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref WorldProjectionMatrix);
            
            GL.MatrixMode(MatrixMode.Modelview);
            
            GL.LoadMatrix(ref CameraMatrix);

            foreach (var mesh in PartMeshes)
            {
                BrickShader.Use();
                mesh.AssignShaderValues(CameraMatrix, WorldProjectionMatrix);
                mesh.Draw();
            }

            DrawGrid(CameraMatrix);

            GL.UseProgram(0);
        }

        private void DrawGrid(Matrix4 viewMatrix)
        {
            GridShader.Use();
            GridShader.MVMatrix.Set(viewMatrix);
            GridShader.PMatrix.Set(WorldProjectionMatrix);

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(-20, 0, -20);
            GL.Vertex3(-20, 0, 20);
            GL.Vertex3(20, 0, 20);
            GL.Vertex3(20, 0, -20);
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
            GL.Color4(Color.FromArgb(120, 80,80,80));
            
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

        private void ViewportPanel_SizeChanged(object sender, EventArgs e)
        {
            if (ViewInitialized && !Disposing)
                InitializeView();
        }

        public void LoadPartProject(PartProject project)
        {
            PartMeshes.ForEach(x => x.Dispose());
            PartMeshes.Clear();

            var partMeshes = project.Surfaces.SelectMany(x => x.GetAllMeshes()).ToList();
            BrickShader.Use();
            foreach (var partMesh in partMeshes)
            {
                var glMesh = GLMeshBase.CreateFromGeometry(partMesh.Geometry, true);
                glMesh.MaterialColor = new Color4(0.6f, 0.6f, 0.6f, 1f);
                glMesh.BindToProgram(BrickShader);
                PartMeshes.Add(glMesh);
            }

            SceneCenter = project.Bounding.Center.ToGL();
            SceneCenter.Y = 0;
            var brickSize = project.Bounding.Size;
            float maxSize = Math.Max(brickSize.X, Math.Max(brickSize.Y, brickSize.Z));
            CameraPosition = new Vector3(maxSize + 2);
            CameraPosition.Y = project.Bounding.MaxY + 2;
            CameraMatrix = Matrix4.LookAt(CameraPosition, SceneCenter, Vector3.UnitY);

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            IsClosing = true;
            DisposeGLResources();
        }

        private void DisposeGLResources()
        {
            PartMeshes.ForEach(x => x.Dispose());
            PartMeshes.Clear();
            GridShader.Dispose();
            
            BrickShader.Dispose();
        }
    }
}