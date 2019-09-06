using Assimp;
using LDDModder.LDD.Data;
using LDDModder.LDD.Files;
using LDDModder.LDD.Files.MeshStructures;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LDDModder.BrickEditor
{
    public partial class Form1 : Form
    {
        private string LddDbDirectory;
        private string LddMeshDirectory;
        private string PrimitiveDirectory;
        public Form1()
        {
            InitializeComponent();
            //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            LddDbDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\");
            LddMeshDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\LOD0\");
            PrimitiveDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\");
            
            TestFlexible();
            
        }

        private void TestFlexible()
        {

            var brick = LDDPartFiles.Read(LddDbDirectory, 47996);

            var yFlexPos = brick.Info.Connectors.OfType<LDD.Primitives.Connectors.AxelConnector>()
                .Where(x => x.Length < 1).Select(x => x.Transform.Translation.Y).Distinct();

            int boneID = 0;
            var primitive = brick.Info;
            var newMesh = brick.MainModel.Geometry.Clone();
            var meshBounds = BoundingBox.FromVertices(newMesh.Vertices.Select(x => x.Position));
            foreach(float yPos in yFlexPos)
            {
                var bone = new FlexBone()
                {
                    ID = boneID++
                };

                bone.Transform.Axis = Simple3D.Vector3.UnitY;
                bone.Transform.Angle = 90;
                bone.Transform.Translation = new Simple3D.Vector3(1.2f, yPos, 0);
                brick.Info.FlexBones.Add(bone);

                if (bone.ID > 0)
                    bone.ConnectionCheck = new Tuple<int, int, int>(0, bone.ID - 1, 2);
                
            }
            var bonePositions = new List<Simple3D.Vector3>();

            foreach (var bone in primitive.FlexBones.OrderByDescending(x => x.Transform.Ty))
            {
                bonePositions.Add(bone.Transform.Translation);

                if (bone.ID == boneID - 1)
                    continue;

                var boneY = bone.Transform.Ty;
                var connectors = primitive.Connectors.Where(x => x.Transform.Ty >= boneY);
                
                bone.Connectors.AddRange(connectors);
                primitive.Connectors.RemoveAll(x => x.Transform.Ty >= boneY);

                if (bone.ConnectionCheck != null)
                {
                    bone.ConnectionCheck = new Tuple<int, int, int>(bone.Connectors.Count, bone.ConnectionCheck.Item2, bone.ConnectionCheck.Item3);
                }
            }

            bonePositions.Reverse();

            var meshVerts = newMesh.Vertices.Select(x => x.Position).ToList();
            for (int i = 0; i < bonePositions.Count - 1; i++)
            {
                var p1 = bonePositions[i];
                var p2 = bonePositions[i + 1];
                var verts = meshVerts.Where(x => x.Y >= p1.Y && x.Y <= p2.Y);
                var segBounds = BoundingBox.FromVertices(verts);
                primitive.FlexBones[i].Bounding = segBounds;
            }

            primitive.Save("47996 v2.xml");
        }
    }
}
