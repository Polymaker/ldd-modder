using LDDModder.Display.Models;
using LDDModder.LDD;
using LDDModder.LDD.Primitives;
using Poly3D.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LDDModder.Views
{
    public partial class ModelViewer : Form
    {
        private Scene MyScene;
        private List<SceneObject> MyObjects;

        public ModelViewer()
        {
            InitializeComponent();
            MyObjects = new List<SceneObject>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MyScene = Scene.CreateDefault();
            polyEngineView.LoadScene(MyScene);
            MyScene.Start();
        }

        public void LoadBrick(int brickId)
        {
            ClearScene();

            var primitiveInfoPath = Path.Combine(LDDManager.GetDirectory(LDDManager.DbDirectories.Primitives), brickId + ".xml");
            var primitiveInfo = Primitive.LoadFrom<Primitive>(primitiveInfoPath);
            
            var brickMeshesPath = Directory.GetFiles(LDDManager.GetDirectory(LDDManager.DbDirectories.LOD0), brickId + ".g*");

            foreach (var modelPath in brickMeshesPath)
            {
                CreateMeshObjectForBrick(modelPath);
            }

            foreach (var collision in primitiveInfo.Collisions)
            {
                var collObj = PrimitiveMeshBuilder.CreateCollisionObject(MyScene, collision);
                if (collObj != null)
                {
                    MyObjects.Add(collObj);
                    var renderer = collObj.GetComponentDownward<MeshRenderer>();
                    renderer.Materials[0].DiffuseColor = Color.FromArgb(120,255,0,0);
                    //renderer.Mode = RenderMode.Transparent;
                    //collObj.RenderLayer = 1;
                }
            }

            foreach (var connection in primitiveInfo.Connections)
            {
                var connObj = PrimitiveMeshBuilder.CreateConnectivityObject(MyScene, connection);
                if (connObj != null)
                {
                    MyObjects.Add(connObj);
                    var renderer = connObj.GetComponentDownward<MeshRenderer>();
                    renderer.Materials[0].DiffuseColor = Color.FromArgb(80, 0, 0, 255);
                    //renderer.Mode = RenderMode.Transparent;
                    //connObj.RenderLayer = 1;
                }
            }
        }

        private void CreateMeshObjectForBrick(string brickModelPath)
        {
            var brickMesh = LddMeshLoader.LoadLddMesh(brickModelPath);
            var brickSceneObj = MyScene.AddObject<SceneObject>();
            var brickRenderer = brickSceneObj.AddComponent<MeshRenderer>();
            brickRenderer.Mesh = brickMesh;
            brickRenderer.Materials[0].DiffuseColor = Color.FromArgb(160, Color.DarkGray);
            brickRenderer.Mode = RenderMode.Transparent;
            brickRenderer.Outlined = true;
            MyObjects.Add(brickSceneObj);
        }

        private void ClearScene()
        {
            if (MyObjects.Count > 0)
            {
                MyObjects.ForEach(so => so.Destroy());
                MyObjects.Clear();
            }
        }
        //private void CreateMeshObjectForBrick(string brickModelPath)
        //{
        //    var brickMesh = LddMeshLoader.LoadLddMesh(brickModelPath);
        //    var brickSceneObj = MyScene.AddObject<ObjectMesh>();
        //    brickSceneObj.Mesh = brickMesh;
        //    brickSceneObj.Material.Color = Color.MediumSlateBlue;
        //    MyObjects.Add(brickSceneObj);
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
            {
                MyScene.Pause();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadBrick(10089);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearScene();
        }
    }
}
