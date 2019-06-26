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
        private PanOrbitCamera Camera;
        private int currentIndex;
        private List<int> _BrickIDs;
        private PrimitiveModel CurrentModel;
        private SceneObject MeshHighligher;

        public ModelViewer()
        {
            InitializeComponent();
            MyObjects = new List<SceneObject>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //var meshTypes = new Dictionary<byte, string>();
            //foreach(var brickFile in Directory.GetFiles(@"C:\Users\james\AppData\Roaming\LEGO Company\LEGO Digital Designer\db\Primitives\LOD0", "*.g*"))
            //{
            //    using(var fs = File.OpenRead(brickFile))
            //    {
            //        fs.Position = 0x0C;
            //        var meshType = (byte)fs.ReadByte();
            //        if (!meshTypes.ContainsKey(meshType))
            //        {
            //            meshTypes.Add(meshType, Path.GetFileName(brickFile));
            //        }
            //    }
            //}

            //foreach(var kv in meshTypes)
            //{
            //    Trace.WriteLine($"{kv.Key:X2} {kv.Value}");
            //}
            //var test = 
            LddMeshLoader.LoadLddMesh(@"C:\Users\james\AppData\Roaming\LEGO Company\LEGO Digital Designer\db\Primitives\LOD0\19729.g", true);

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
                if (meshGroup != null)
                    meshGroup.Childs.Add(meshObject);
                else
                    meshGroup = meshObject;
            }

            MyObjects.Add(meshGroup);
            MyScene.AddObject(meshGroup);

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
            /*
            int colIndex = 0;
            foreach (var collision in primitiveInfo.Collisions)
            {
                var collObj = PrimitiveMeshBuilder.CreateCollisionObject(MyScene, collision);
                if (collObj != null)
                {
                    collObj.Name = $"Collision{colIndex++}";
                    collObj.Tag = collision;
                    //var manip = collObj.AddComponent<TransformManipulator>();
                    //manip.Size = 2f;
                    MyObjects.Add(collObj);
                    var renderer = collObj.GetComponentDownward<MeshRenderer>();
                    renderer.Materials[0].DiffuseColor = Color.FromArgb(120, 255, 0, 0);
                    //renderer.Mode = RenderMode.Transparent;
                    //collObj.RenderLayer = 1;
                }
            }
            
            foreach (var bone in primitiveInfo.Bones)
            {
                if (!bone.Connections.Any())
                    continue;
                var boneObject = meshGroup.AddObject<SceneObject>();
                boneObject.RenderLayer = 1;
                var manip = boneObject.AddComponent<TransformManipulator>();
                manip.Size = 0.8f;
                boneObject.SetTransform(bone.Get3dTransform(), SceneSpace.World);
                //MyObjects.Add(boneObject);

                //foreach (var col in bone.Collisions)
                //{
                //    var collObj = PrimitiveMeshBuilder.CreateCollisionObject(MyScene, col);
                //    if (collObj != null)
                //    {
                //        boneObject.Childs.Add(collObj);

                //        var renderer = collObj.GetComponentDownward<MeshRenderer>();
                //        renderer.Materials[0].DiffuseColor = Color.FromArgb(120, 255, 0, 0);
                //        //renderer.Mode = RenderMode.Transparent;
                //        //collObj.RenderLayer = 1;
                //    }
                //}

                foreach (var connection in bone.Connections)
                {
                    var connObj = PrimitiveMeshBuilder.CreateConnectivityObject(MyScene, connection);
                    if (connObj != null)
                    {
                        boneObject.Childs.Add(connObj);
                        var renderer = connObj.GetComponentDownward<MeshRenderer>();
                        if (renderer != null)
                        {
                            renderer.Materials[0].DiffuseColor = Color.FromArgb(80, 0, 0, 255);
                            //renderer.Mode = RenderMode.Transparent;
                            //connObj.RenderLayer = 1;
                        }
                    }
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
                    

                    MyObjects.Add(connObj);
                    var renderer = connObj.GetComponentDownward<MeshRenderer>();
                    if(renderer != null)
                    {
                        renderer.Materials[0].DiffuseColor = Color.FromArgb(80, 0, 0, 255);
                        //renderer.Mode = RenderMode.Transparent;
                        //connObj.RenderLayer = 1;
                    }
                }
            }
            */
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
            if (MyObjects.Count > 0)
            {
                MyObjects.ForEach(so => so.Destroy());
                MyObjects.Clear();
            }
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

        private List<Poly3D.Engine.Meshes.Face> AllFaces;
        private void button3_Click(object sender, EventArgs e)
        {
            //EditCollision();
            //WriteFlexData();
            if (AllFaces == null || AllFaces.Count == 0)
            {
                AllFaces = CurrentModel.Mesh.Faces.ToList();
                //136
                for (int i = 0; i < 144/3; i++)
                {
                    CurrentModel.Mesh.Faces.RemoveAt(174 / 3);
                }
                //while (CurrentModel.Mesh.Faces.Count > 30)
                //    CurrentModel.Mesh.Faces.RemoveAt(CurrentModel.Mesh.Faces.Count - 1);
            }
            else
            {
                CurrentModel.Mesh.Faces.Clear();
                foreach (var face in AllFaces)
                    CurrentModel.Mesh.Faces.Add(face);
                AllFaces.Clear();
            }
        }

        private void EditCollision()
        {
            var colObj = MyScene.GetObjectByName("Collision0") as SceneObject;
            var col1 = (Collision)colObj.Tag;
            colObj.Transform.Rotate(Poly3D.Maths.RotationComponent.Yaw, Angle.FromDegrees(7.5f));
            col1.Transform = colObj.Transform.GetTransform(SceneSpace.World).ToLddTransform();


            colObj = MyScene.GetObjectByName("Collision1") as SceneObject;
            var col2 = (Collision)colObj.Tag;
            colObj.Transform.Rotate(Poly3D.Maths.RotationComponent.Yaw, Angle.FromDegrees(-7.5f));

            col2.Transform = colObj.Transform.GetTransform(SceneSpace.World).ToLddTransform();

            Trace.WriteLine(Collision.Serialize(col1).ToString());
            Trace.WriteLine(Collision.Serialize(col2).ToString());
        }

        private void WriteFlexData()
        {
            var brickMesh = MyScene.EngineObjects.FirstOrDefault(o => o.HasComponent<MeshRenderer>()).GetComponent<MeshRenderer>();
            var bounding = brickMesh.Mesh.BoundingBox;
            var boneSize = 0.8f;
            var flexNodeCount = (int)(bounding.Size.MaxComponent() / boneSize) + 1;

            var flexDir = Vector3.UnitX;
            if (bounding.Size.Y == bounding.Size.MaxComponent())
                flexDir = Vector3.UnitY;
            else if (bounding.Size.Z == bounding.Size.MaxComponent())
                flexDir = Vector3.UnitZ;

            var flexBones = new List<FlexBone>();
            var flexPos = new Dictionary<int, Vector3>();


            for (int i = 0; i < flexNodeCount; i++)
            {
                var nodePos = bounding.Center;
                if (flexDir == Vector3.UnitX)
                    nodePos.X = bounding.Left;
                else if (flexDir == Vector3.UnitY)
                    nodePos.Y = bounding.Bottom;
                else
                    nodePos.Z = bounding.Back;
                nodePos += (i * boneSize) * flexDir;
                flexPos.Add(i, nodePos);

                var flexbone = new FlexBone()
                {
                    Id = i,
                    Transform = new LDD.Primitives.Transform() { Tx = nodePos.X, Ty = nodePos.Y, Tz = nodePos.Z, Ay = 1 },
                };
                flexBones.Add(flexbone);


                if (i > 0)
                    flexbone.FlexCheckConnection = $"{((i < flexNodeCount - 1) ? 1 : 0)},{i - 1},{(i == 1 ? 1 : 2)}";

                if (i < flexNodeCount - 1)
                {
                    flexbone.Bounding = new LDD.Primitives.BoundingBox()
                    {
                        MinX = bounding.Left,
                        MaxX = bounding.Right,
                        MinZ = bounding.Back,
                        MaxZ = bounding.Front,
                        MinY = bounding.Bottom,
                        MaxY = bounding.Top
                    };
                    if (flexDir == Vector3.UnitX)
                    {
                        flexbone.Bounding.MinX = 0;
                        flexbone.Bounding.MaxX = boneSize;
                    }
                    else if (flexDir == Vector3.UnitY)
                    {
                        flexbone.Bounding.MinY = 0;
                        flexbone.Bounding.MaxY = boneSize;
                    }
                    else
                    {
                        flexbone.Bounding.MinZ = 0;
                        flexbone.Bounding.MaxZ = boneSize;
                    }
                }
                else
                {
                    flexbone.Bounding = new LDD.Primitives.BoundingBox()
                    {
                        MinX = -0.0001,
                        MinZ = -0.0001,
                        MinY = -0.0001,
                        MaxX = 0.0001,
                        MaxY = 0.0001,
                        MaxZ = 0.0001
                    };
                }

            }

            int flexBonePerVertex = 4;
            var verticesFlexData = new List<List<Tuple<int, float>>>();

            foreach (var vertex in CurrentModel.Vertices)
            {
                var closestBones = flexPos.OrderBy(b => (vertex - b.Value).LengthFast).Take(flexBonePerVertex);
                //var maxDist = (closestBones.Last().Value - vertex).Length;
                //var minDix = (closestBones.First().Value - vertex).Length;
                var distances = closestBones.Select(b => (b.Value - vertex).Length);
                var totalDist = distances.Sum();

                var flexData = new List<Tuple<int, float>>();
                for (int i = 0; i < closestBones.Count(); i++)
                {
                    var bone = closestBones.ElementAt(i);
                    flexData.Add(new Tuple<int, float>(bone.Key,
                        totalDist - ((bone.Value - vertex).Length / totalDist)
                        ));
                }
                var dataSum = flexData.Sum(d => d.Item2);
                flexData = flexData.Select(d => new Tuple<int, float>(d.Item1, d.Item2 / dataSum)).ToList();
                dataSum = flexData.Sum(d => d.Item2);
                verticesFlexData.Add(flexData);
            }

            using (var fs = File.Open("flexdata.bin", FileMode.Create))
            using (var bw = new BinaryWriter(fs))
            {
                int currentOffset = 0;
                var vertexOffsets = new List<int>();
                var flexDataByte = new List<Byte>();
                foreach (var flexData in verticesFlexData)
                {
                    var dataBytes = new List<Byte>();
                    dataBytes.AddRange(BitConverter.GetBytes(flexData.Count));

                    foreach (var flexBone in flexData)
                    {
                        dataBytes.AddRange(BitConverter.GetBytes(flexBone.Item1));
                        dataBytes.AddRange(BitConverter.GetBytes(flexBone.Item2));
                    }

                    //Trace.WriteLine(string.Join(" ", dataBytes.Select(b => b.ToString("X2"))));
                    vertexOffsets.Add(currentOffset);
                    currentOffset += dataBytes.Count;
                    flexDataByte.AddRange(dataBytes);
                }
                bw.Write(currentOffset);
                fs.Write(flexDataByte.ToArray(), 0, flexDataByte.Count);
                foreach (var vOffset in vertexOffsets)
                {
                    bw.Write(vOffset);
                }

                //Trace.WriteLine("Flex data size = " + string.Join(" ", BitConverter.GetBytes(currentOffset).Select(b => b.ToString("X2"))));
                //for (int i = 0; i < Math.Floor(vertexOffsets.Count / 8d); i++)
                //{
                //    var offsets = vertexOffsets.Skip(i * 8).Take(8);
                //    var offsetBytes = offsets.SelectMany(o => BitConverter.GetBytes(o));
                //    Trace.WriteLine(string.Join(" ", offsetBytes.Select(b => b.ToString("X2"))));
                //}


                foreach (var flexBone in flexBones)
                {
                    Trace.WriteLine(XSerializationHelper.Serialize(flexBone).ToString());
                }
            }

        }
    }
}
