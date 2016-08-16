using LDDModder.Display.Models;
using LDDModder.LDD;
using Poly3D.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            if (MyObjects.Count > 0)
            {
                MyObjects.ForEach(so => so.Destroy());
                MyObjects.Clear();
            }
            var brickMeshesPath = Directory.GetFiles(LDDManager.GetDirectory(LDDManager.DbDirectories.LOD0), brickId + ".g*");
            foreach (var modelPath in brickMeshesPath)
            {
                CreateMeshObjectForBrick(modelPath);
            }
        }

        private void CreateMeshObjectForBrick(string brickModelPath)
        {
            var brickMesh = LddMeshLoader.LoadLddMesh(brickModelPath);
            var brickSceneObj = MyScene.AddObject<ObjectMesh>();
            brickSceneObj.Mesh = brickMesh;
            MyObjects.Add(brickSceneObj);
        }

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
    }
}
