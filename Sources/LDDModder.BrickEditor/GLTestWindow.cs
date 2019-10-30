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
        private List<BrickModel> MeshList;
        private BasicShaderProgram BasicShader;
        private TexturedShaderProgram TexturedShader;
        private OutlineShaderProgram OutlineShaderProgram;
        private Texture2D DefaultTexture;
        private Texture2D[] OutlineTexs;

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
            MeshList = new List<BrickModel>();
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

            CurrentBrickIndex = BrickIDList.IndexOf(3023);

            if (BrickIDList.Any() && CurrentBrickIndex >= 0 && CurrentBrickIndex < BrickIDList.Count)
            {
                LoadBrickModel(BrickIDList[CurrentBrickIndex]);
            }
            
            
            LogTimer = Stopwatch.StartNew();
        }

        private Texture2D CreateOutlineTexture(LDD.Files.MeshFile meshFile)
        {
            int h = (int)Math.Ceiling(meshFile.Indices.Count / 21f);
            var texture = new Texture2D(SizedInternalFormat.Rgba32f, 64, h, 1);
            
            var pixels = new List<Vector4>();
            var pixelRows = new List<Vector4[]>();
            foreach(var idx in meshFile.Indices)
            {
                var pix = new Vector4();
                for (int i = 0; i < 3; i++)
                {
                    pix.Xy = idx.RoundEdgeData.Coords[(i * 2)].ToGL();
                    pix.Zw = idx.RoundEdgeData.Coords[(i * 2) + 1].ToGL();
                    pixels.Add(pix);
                }

                if (pixels.Count == 63)
                {
                    pixels.Add(Vector4.Zero);
                    pixelRows.Add(pixels.ToArray());
                    pixels.Clear();
                }
            }

            
            if (pixels.Count > 0)
            {
                while (pixels.Count < 64)
                    pixels.Add(Vector4.Zero);
                pixelRows.Add(pixels.ToArray());
            }
            //pixelRows.Reverse();
            pixels.Clear();
            pixels.AddRange(pixelRows.SelectMany(x => x));

            GL.TexSubImage2D(texture.TextureTarget, 0, 0, 0, 64, h, PixelFormat.Rgba, PixelType.Float, pixels.ToArray());
            texture.SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            return texture;
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
            
            OutlineTexs = new Texture2D[3];
            var imgNames = new string[] { "blue.png", "red.png", "magenta.png" };
            for (int i = 0; i < 3; i++)
            {
                var imgPath = Path.Combine(@"C:\Program Files (x86)\LEGO Company\LEGO Digital Designer\Assets\Shaders", imgNames[i]);
                texImage = (Bitmap)Image.FromFile(imgPath);
                BitmapTexture.CreateCompatible(texImage, out Texture2D outTex, 1);
                OutlineTexs[i] = outTex;
                OutlineTexs[i].LoadBitmap(texImage, 0);
                OutlineTexs[i].SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
                OutlineTexs[i].SetWrapMode(TextureWrapMode.Clamp);
            }

            ProgramFactory.BasePath = "Rendering";
            BasicShader = ProgramFactory.Create<BasicShaderProgram>();
            BasicShader.Use();
            BasicShader.DisplayWireframe.Set(true);
            BasicShader.LightPosition.Set(LightPosition);

            TexturedShader = ProgramFactory.Create<TexturedShaderProgram>();
            TexturedShader.Use();
            TexturedShader.DisplayWireframe.Set(true);
            TexturedShader.LightPosition.Set(LightPosition);



            OutlineShaderProgram = ProgramFactory.Create<OutlineShaderProgram>();
            OutlineShaderProgram.Use();
            //OutlineShaderProgram.PackedRoundEdgeDataTexture.Set(TextureUnit.Texture0);
            //OutlineShaderProgram.Texture0.Set(TextureUnit.Texture1);
            //OutlineShaderProgram.Texture1.Set(TextureUnit.Texture2);
            //OutlineShaderProgram.Texture2.Set(TextureUnit.Texture3);
            

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

                model.Geometry.RebuildIndices();

                //var outlineTexture = CreateOutlineTexture(model);

                //var mesh = GLMeshBase.CreateFromGeometry(model.Geometry);

                //if (mesh is GLTexturedMesh texturedMesh)
                //{
                //    texturedMesh.TextureUnit = TextureUnit.Texture4;
                //}
                //mesh.MaterialColor = new Color4(0.7f, 0.7f, 0.7f, 1);
                //mesh.BindToProgram(shaderToUse);

                var brickModel = new BrickModel()
                {
                    Mesh = model,
                    MaterialColor = new Color4(0.7f, 0.7f, 0.7f, 1),
                    //OutlineTexture = outlineTexture,
                    //OutlineIndices = new List<Vector2>(),
                    //Positions = new List<Vector3>()
                };
                brickModel.InitializeBuffers(OutlineShaderProgram);
                //foreach (var idx in model.Geometry.Indices)
                //{
                //    //int reOffset = idx.IIndex * 3 + (int)Math.Floor(idx.IIndex / 21d);
                //    float x = (idx.IIndex % 21);
                //    float y = (int)Math.Floor(idx.IIndex / 21d);
                //    brickModel.OutlineIndices.Add(new Vector2((x * 3f) / 64f, y / (float)outlineTexture.Height));
                //    brickModel.Positions.Add(idx.Vertex.Position.ToGL());
                //}

                MeshList.Add(brickModel);
                //foreach (var cull in model.Cullings)
                //{
                //    var cullGeom = model.GetCullingGeometry(cull);
                //    var mesh = GLMeshBase.CreateFromGeometry(cullGeom);

                //    var brickModel = new BrickModel()
                //    {
                //        Mesh = mesh,
                //        OutlineTexture = outlineTexture,
                //        OutlineIndices = new List<Vector2>()
                //    };

                //    foreach (var idx in cullGeom.Indices)
                //    {
                //        int x = (idx.IIndex % 21);
                //        int y = (idx.IIndex - x) / 21;
                //        brickModel.OutlineIndices.Add(new Vector2((x * 3f) / 64f, y / (float)outlineTexture.Height));
                //    }

                //    mesh.BindToProgram(shaderToUse);
                //    mesh.MaterialColor = new Color4(0.7f, 0.7f, 0.7f, 1);
                //    //mesh.MaterialColor = Color4.FromHsl(new Vector4(curHue, 1, 0.6f, 0.7f));
                //    curHue = (curHue + hueStep) % 1f;

                //    if (mesh is GLTexturedMesh texturedMesh)
                //    {
                //        mesh.MaterialColor = new Color4(1, 1, 1, 0.7f);
                //        texturedMesh.Texture = DefaultTexture;
                //    }

                //    MeshList.Add(brickModel);
                //}

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
                    //model.Mesh.BoundProgram.Use();
                    //model.Mesh.AssignShaderValues(ViewMatrix, Projection);
                    var mvpMatrix = Matrix4.Identity * ViewMatrix * Projection;
                    OutlineShaderProgram.Use();
                    OutlineShaderProgram.MVPMatrix.Set(mvpMatrix);

                    OutlineShaderProgram.Texture0.BindTexture(TextureUnit.Texture1, OutlineTexs[0]);
                    OutlineShaderProgram.Texture1.BindTexture(TextureUnit.Texture2, OutlineTexs[1]);
                    OutlineShaderProgram.Texture2.BindTexture(TextureUnit.Texture3, OutlineTexs[2]);
                    model.DrawOutline(OutlineShaderProgram);

                    //if (model.Mesh.MaterialColor.A < 1f)
                    //{
                    //    GL.Enable(EnableCap.CullFace);

                    //    GL.CullFace(CullFaceMode.Front);
                    //    model.Mesh.Draw();

                    //    GL.CullFace(CullFaceMode.Back);
                    //    model.Mesh.Draw();
                    //    GL.Disable(EnableCap.CullFace);
                    //}
                    //else
                    //{
                    //    model.Mesh.Draw();
                    //}
                }
            }

            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            DisposeResources();
        }

        private void DisposeResources()
        {
            ClearMeshes();
            BasicShader.Dispose();
            TexturedShader.Dispose();
            OutlineShaderProgram.Dispose();
            for (int i = 0; i < 3; i++)
                OutlineTexs[i].Dispose();
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

            if (e.Key >= Key.Number0 && e.Key <= Key.Number6)
            {
                OutlineShaderProgram.Use();
                int pair = (int)e.Key - (int)Key.Number0;
                OutlineShaderProgram.PairToDisplay.Set(pair);
            }
        }
    }
}
