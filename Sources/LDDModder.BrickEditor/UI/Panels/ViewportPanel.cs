using LDDModder.BrickEditor.Rendering.Shaders;
using ObjectTK.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using QuickFont;
using QuickFont.Configuration;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ViewportPanel : DockContent
    {
        private bool ViewInitialized;
        private QFontDrawing TextRenderer;
        private QFont RenderFont;
        private Matrix4 UIProjectionMatrix;
        private Matrix4 WorldProjectionMatrix;
        private bool IsClosing;
        private bool RenderLoopEnabled;
        private GLControl glControl1;
        private GridShaderProgram GridShader;

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
            GridShader.MajorGridColor.Set(Color4.White);
            GridShader.MinorGridColor.Set(Color4.Silver);
            GridShader.MajorLineSpacing = 0.8f;
            GridShader.MajorLineOffcenter = true;
            GridShader.MajorLineThickness = 1f;
            GridShader.MinorLineSpacing = 0.4f;
            GridShader.MinorLineOffcenter = false;
            GridShader.MinorLineThickness = 0.5f;
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
            var viewMatrix = Matrix4.LookAt(new Vector3(5), Vector3.Zero, Vector3.UnitY);
            GL.LoadMatrix(ref viewMatrix);

            var mvp = viewMatrix * WorldProjectionMatrix;

            var vertices = new Vector3[]{
                new Vector3(-1, 1, 1),
                new Vector3(-1, -1, 1),
                new Vector3(1, -1, 1),
                new Vector3(1, 1, 1),

                new Vector3(1, 1, -1),
                new Vector3(1, -1, -1),
                new Vector3(1, -1, 1),
                new Vector3(1, 1, 1),

                new Vector3(-1, 1, -1),
                new Vector3(-1, -1, -1),
                new Vector3(-1, -1, 1),
                new Vector3(-1, 1, 1),

                new Vector3(-1, 1, -1),
                new Vector3(-1, 1, 1),
                new Vector3(1, 1, 1),
                new Vector3(1, 1, -1),
            };

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices[i].X * 0.4f, (1f + vertices[i].Y) * 0.48f, vertices[i].Z * 0.4f);
            }
            /*
            GL.Color4(Color.FromArgb(80, 80, 220));
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < vertices.Length; i++)
                GL.Vertex3(vertices[i]);
            GL.End();

            GL.Color4(Color.Black);
            GL.LineWidth(2f);
            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                GL.Vertex3(vertices[i]);
                GL.Vertex3(vertices[i + 1]);
            }
            GL.End();
            */
            GridShader.Use();
            GridShader.MVPMatrix.Set(mvp);

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(-10, 0, -10);
            GL.Vertex3(-10, 0, 10);
            GL.Vertex3(10, 0, 10);
            GL.Vertex3(10, 0, -10);
            GL.End();
            GL.UseProgram(0);
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            IsClosing = true;
            DisposeGLResources();
        }

        private void DisposeGLResources()
        {
            GridShader.Dispose();
        }
    }
}
