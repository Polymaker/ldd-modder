using LDDModder.LDD.Meshes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using ObjectTK.Tools.Cameras;

namespace LDDModder.BrickEditor
{
    public partial class DecorationUtil : Form
    {
        private Mesh CurrentMesh;
        private bool GLInitialized;
        private Matrix4 ModelView;
        private Matrix4 Projection;

        public DecorationUtil()
        {
            InitializeComponent();
            Application.Idle += Application_Idle;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            if (GLInitialized)
                Render();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetupView();
            
        }

        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                string decorationFilter = string.Join(";", Enumerable.Range(1, 15).Select(i => $"*.g{i}"));
                string meshDir = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\LOD0\");
                if (Directory.Exists(meshDir))
                    ofd.InitialDirectory = meshDir;
                ofd.Filter = $"Main mesh files (*.g)|*.g|Decoration meshes (*.g1, *.g2)|{decorationFilter}|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                    OpenMeshFile(ofd.FileName);
            }
        }

        private void OpenMeshFile(string filename)
        {
            try
            {
                var mesh = Mesh.Read(filename);
                CurrentMesh = mesh;
                ReloadTriangleList();
                SetupModelView();
            }
            catch
            {
                MessageBox.Show("Error reading mesh file");
            }
        }

        private void ReloadTriangleList()
        {
            TriangleListBox.Items.Clear();
            for (int i = 0; i < CurrentMesh.TriangleCount; i++)
            {
                //var tri = CurrentMesh.Triangles[i];
                TriangleListBox.Items.Add($"Triangle {i + 1}", false);
            }
        }

        private Matrix4 CameraMatrix;
        private int HighlightedTriangle = -1;

        private void SetupView()
        {
            
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);

            float aspect_ratio = glControl1.Width / (float)glControl1.Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);

            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(Color.SkyBlue);
            GLInitialized = true;
        }

        private void SetupModelView()
        {
            var bounding = LDD.Primitives.BoundingBox.FromVertices(CurrentMesh.Vertices);
            var maxSize = (float)Math.Max(bounding.SizeX, Math.Max(bounding.SizeY, bounding.SizeZ)) + 2;
            CameraMatrix = Matrix4.LookAt(new Vector3(maxSize, maxSize, maxSize * -1),
                new Vector3(bounding.Center.X, bounding.Center.Y, bounding.Center.Z),
                Vector3.UnitY);
            
        }

        private void Render()
        {
            glControl1.MakeCurrent();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref CameraMatrix);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (CurrentMesh != null)
            {
                GL.Begin(PrimitiveType.Triangles);

                GL.Color3(Color.Gray);
                
                for (int i = 0; i < CurrentMesh.TriangleCount; i++)
                {
                    var tri = CurrentMesh.Triangles[i];

                    if (TriangleListBox.GetItemChecked(i))
                        GL.Color3(Color.Red);
                    else
                        GL.Color3(Color.Gray);

                    GL.Vertex3(tri.V1.Position.X, tri.V1.Position.Y, tri.V1.Position.Z);
                    GL.Vertex3(tri.V2.Position.X, tri.V2.Position.Y, tri.V2.Position.Z);
                    GL.Vertex3(tri.V3.Position.X, tri.V3.Position.Y, tri.V3.Position.Z);
                }

                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Black);

                foreach(var tri in CurrentMesh.Triangles)
                {
                    GL.Vertex3(tri.V1.Position.X, tri.V1.Position.Y, tri.V1.Position.Z);
                    GL.Vertex3(tri.V2.Position.X, tri.V2.Position.Y, tri.V2.Position.Z);

                    GL.Vertex3(tri.V2.Position.X, tri.V2.Position.Y, tri.V2.Position.Z);
                    GL.Vertex3(tri.V3.Position.X, tri.V3.Position.Y, tri.V3.Position.Z);

                    GL.Vertex3(tri.V3.Position.X, tri.V3.Position.Y, tri.V3.Position.Z);
                    GL.Vertex3(tri.V1.Position.X, tri.V1.Position.Y, tri.V1.Position.Z);
                }
                GL.End();

                if (HighlightedTriangle >= 0)
                {
                    var tri = CurrentMesh.Triangles[HighlightedTriangle];
                    GL.Clear(ClearBufferMask.DepthBufferBit);
                    GL.Begin(PrimitiveType.Triangles);
                    GL.Color3(Color.Orange);
                    GL.Vertex3(tri.V1.Position.X, tri.V1.Position.Y, tri.V1.Position.Z);
                    GL.Vertex3(tri.V2.Position.X, tri.V2.Position.Y, tri.V2.Position.Z);
                    GL.Vertex3(tri.V3.Position.X, tri.V3.Position.Y, tri.V3.Position.Z);
                    GL.End();
                }
            }
            
            glControl1.SwapBuffers();
        }

        private void TriangleListBox_MouseMove(object sender, MouseEventArgs e)
        {
            int triIndex = TriangleListBox.IndexFromPoint(e.Location);
            if (triIndex != HighlightedTriangle)
            {
                HighlightedTriangle = triIndex;
                Render();
            }
        }

        private void CreateMeshButton_Click(object sender, EventArgs e)
        {
            if (CurrentMesh != null && TriangleListBox.CheckedItems.Count > 0)
            {

            }
        }
    }
}
