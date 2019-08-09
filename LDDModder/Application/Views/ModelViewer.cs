using LDDModder.Display.Models;
using LDDModder.LDD;
using LDDModder.LDD.Primitives;
using LDDModder.Serialization;
using OpenTK;
using Poly3D.Engine;
using Poly3D.Maths;
using Poly3D.Prefabs.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LDDModder.Views
{
    public partial class ModelViewer : Form
    {
        private Scene MyScene;
        private List<SceneObject> BrickMeshes;
        private List<SceneObject> CollisionMeshes;
        private List<SceneObject> ConnectionMeshes;

        private PanOrbitCamera Camera;
        private int currentIndex;
        private List<int> _BrickIDs;
        private PrimitiveModel CurrentModel;
        

        public ModelViewer()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            BrickMeshes = new List<SceneObject>();
            CollisionMeshes = new List<SceneObject>();
            ConnectionMeshes = new List<SceneObject>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            MyScene = Scene.CreateDefault();
            
            polyEngineView.LoadScene(MyScene);

            Camera = MyScene.ActiveCameras.First().GetComponent<PanOrbitCamera>();

            var sceneOrigin = MyScene.AddObject<SceneObject>().AddComponent<TransformManipulator>();
            sceneOrigin.EngineObject.RenderLayer = 1;
            sceneOrigin.AbsoluteScreenSize = true;
            sceneOrigin.Size = 80;

            //currentIndex = 10089;

            _BrickIDs = new List<int>();
            var primitivesXmlFiles = Directory.GetFiles(LDDManager.GetDirectory(LDDManager.DbDirectories.Primitives), "*.xml");

            foreach(var path in primitivesXmlFiles)
            {
                if(int.TryParse(Path.GetFileNameWithoutExtension(path), out int brickID))
                    _BrickIDs.Add(brickID);
            }
            currentIndex = _BrickIDs.IndexOf(3020);
            LoadBrick(_BrickIDs[currentIndex]);
            
        }

        public void LoadBrick(int brickId)
        {
            ClearScene();
            
            var primitiveInfoPath = Path.Combine(LDDManager.GetDirectory(LDDManager.DbDirectories.Primitives), brickId + ".xml");
            if (!File.Exists(primitiveInfoPath))
                return;

            var primitiveInfo = XSerializable.LoadFrom<Primitive>(primitiveInfoPath);

            Text = $"Brick Model Viewer - Brick # {brickId} {primitiveInfo.Name}";

            var brickMeshesPath = Directory.GetFiles(LDDManager.GetDirectory(LDDManager.DbDirectories.LOD0), brickId + ".g*");

            SceneObject meshGroup = null;

            if (brickMeshesPath.Length > 1)
                meshGroup = new SceneObject();

            foreach (var modelPath in brickMeshesPath)
            {
                var meshObject = CreateMeshObjectForBrick(modelPath);
                meshObject.Active = !HideMeshCheckBox.Checked;
                if (meshGroup != null)
                    meshGroup.Childs.Add(meshObject);
                else
                    meshGroup = meshObject;
            }

            BrickMeshes.Add(meshGroup);
            MyScene.AddObject(meshGroup);

            int colIndex = 0;
            foreach (var collision in primitiveInfo.Collisions)
            {
                var collObj = PrimitiveMeshBuilder.CreateCollisionObject(MyScene, collision);
                if (collObj != null)
                {
                    collObj.Name = $"Collision{colIndex++}";
                    collObj.Tag = collision;
                    collObj.Active = ShowCollisionsCheckBox.Checked;
                    //var manip = collObj.AddComponent<TransformManipulator>();
                    //manip.Size = 2f;
                    CollisionMeshes.Add(collObj);
                    MyScene.AddObject(collObj);
                    var renderer = collObj.GetComponentDownward<MeshRenderer>();
                    renderer.Materials[0].DiffuseColor = Color.FromArgb(120, 255, 0, 0);
                    //renderer.Mode = RenderMode.Transparent;
                    //collObj.RenderLayer = 1;
                }
            }

            foreach (var connection in primitiveInfo.Connections)
            {
                //var trans3d = connection.Transform.Get3dTransform();
                //trans3d.Rotation = new Rotation(new Vector3(0,90,90));
                //var t = trans3d.Translation;
                //t.Z -= 0.08f;
                //trans3d.Translation = t;
                //connection.Transform = trans3d.ToLddTransform();
                var connObj = PrimitiveMeshBuilder.CreateConnectivityObject(MyScene, connection);
                if (connObj != null)
                {
                    connObj.Active = ShowConnectionsCheckBox.Checked;
                    ConnectionMeshes.Add(connObj);
                    MyScene.AddObject(connObj);
                    var renderer = connObj.GetComponentDownward<MeshRenderer>();
                    if (renderer != null)
                    {
                        renderer.Materials[0].DiffuseColor = Color.FromArgb(80, 0, 0, 255);
                        //renderer.Mode = RenderMode.Transparent;
                        //connObj.RenderLayer = 1;
                    }
                }
            }

            

            var meshes = meshGroup.GetComponentsDownward<MeshRenderer>().Select(r => r.Mesh);

            Poly3D.Maths.BoundingBox brickBoundingBox;

            if(brickMeshesPath.Length == 1)
            {
                brickBoundingBox = meshes.First().BoundingBox;
            }
            else
            {
                var boundingMins = meshes.Select(m => m.BoundingBox.Min);
                var boundingMaxs = meshes.Select(m => m.BoundingBox.Max);
                brickBoundingBox = Poly3D.Maths.BoundingBox.FromPoints(boundingMins.Concat(boundingMaxs));
            }

            Trace.WriteLine(brickBoundingBox);
            Camera.CameraTarget = brickBoundingBox.Center;
            Camera.SetDistanceFromTarget(brickBoundingBox.Extents.Length + 5);
        }

        private SceneObject CreateMeshObjectForBrick(string brickModelPath)
        {
            var brickModel = PrimitiveModel.LoadPrimitiveMesh(brickModelPath);
            
            if (brickModelPath.EndsWith("g"))
                CurrentModel = brickModel;
            var brickSceneObj = new SceneObject();
            var brickRenderer = brickSceneObj.AddComponent<MeshRenderer>();
            brickRenderer.Mesh = brickModel.Mesh;
            
            brickRenderer.Materials[0].DiffuseColor = Color.FromArgb(220, brickModel.IsTextured ? Color.White : Color.DarkGray);
            brickRenderer.Mode = RenderMode.Transparent;
            brickRenderer.DrawWireframe = true;
            brickRenderer.Outlined = true;
            return brickSceneObj;
        }

        private void ClearScene()
        {
            BrickMeshes.ForEach(so => so.Destroy());
            BrickMeshes.Clear();
            CollisionMeshes.ForEach(so => so.Destroy());
            CollisionMeshes.Clear();
            ConnectionMeshes.ForEach(so => so.Destroy());
            ConnectionMeshes.Clear();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
                MyScene.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(currentIndex>0)
            {
                currentIndex--;
                LoadBrick(_BrickIDs[currentIndex]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(currentIndex < _BrickIDs.Count - 1)
            {
                currentIndex++;
                LoadBrick(_BrickIDs[currentIndex]);
            }
        }

        private void ShowCollisionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CollisionMeshes.ForEach(so => so.Active = ShowCollisionsCheckBox.Checked);
        }

        private void ShowConnectionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ConnectionMeshes.ForEach(so => so.Active = ShowConnectionsCheckBox.Checked);
        }

        private void HideMeshCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            BrickMeshes.ForEach(so => so.Active = !HideMeshCheckBox.Checked);
        }
    }
}
