using LDDModder.LDD.Primitives;
using LDDModder.Utilities;
using OpenTK;
using Poly3D.Engine;
using Poly3D.Engine.Data;
using Poly3D.Engine.Meshes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LDDModder.Display.Models
{
    public class PrimitiveMeshBuilder
    {

        public static Mesh Create(CollisionBox box)
        {
            var tlf = new Vector3((float)-box.Sx, (float)box.Sy, (float)box.Sz);
            var trf = new Vector3((float)box.Sx, (float)box.Sy, (float)box.Sz);
            var trb = new Vector3((float)box.Sx, (float)box.Sy, (float)-box.Sz);
            var tlb = new Vector3((float)-box.Sx, (float)box.Sy, (float)-box.Sz);

            var blf = new Vector3((float)-box.Sx, (float)-box.Sy, (float)box.Sz);
            var brf = new Vector3((float)box.Sx, (float)-box.Sy, (float)box.Sz);
            var brb = new Vector3((float)box.Sx, (float)-box.Sy, (float)-box.Sz);
            var blb = new Vector3((float)-box.Sx, (float)-box.Sy, (float)-box.Sz);

            var top = Vector3.UnitY;
            var front = Vector3.UnitZ;
            var right = Vector3.UnitX;
            var back = Vector3.UnitZ * -1;
            var left = Vector3.UnitX * -1;
            var bottom = Vector3.UnitY * -1;
            int idx = 0;
            return MeshBuilder.LoadFromVNF(new Vector3[] {
                //top
                tlf,trf,trb,tlf,trb,tlb,
                //front
                blf,brf,trf,blf,trf,tlf,
                //right
                brf,brb,trb,brf,trb,trf,
                //back
                brb,blb,tlb,brb,tlb,trb,
                //left
                blb,blf,tlf,blb,tlf,tlb,
                //bottom
                blb,brb,brf, blb,brf,blf
            },
            new Vector3[] {
                top, top, top, top, top, top,
                front, front, front, front, front, front,
                right, right, right, right, right, right,
                back, back, back, back, back, back,
                left, left, left, left, left, left,
                bottom, bottom, bottom, bottom, bottom, bottom
            },

            new Tuple<int, int, int>[] {
                new Tuple<int, int, int>(idx++,idx++,idx++), new Tuple<int, int, int>(idx++,idx++,idx++),

                new Tuple<int, int, int>(idx++,idx++,idx++), new Tuple<int, int, int>(idx++,idx++,idx++),

                new Tuple<int, int, int>(idx++,idx++,idx++), new Tuple<int, int, int>(idx++,idx++,idx++),

                new Tuple<int, int, int>(idx++,idx++,idx++), new Tuple<int, int, int>(idx++,idx++,idx++),

                new Tuple<int, int, int>(idx++,idx++,idx++), new Tuple<int, int, int>(idx++,idx++,idx++),

                new Tuple<int, int, int>(idx++,idx++,idx++), new Tuple<int, int, int>(idx++,idx++,idx++)
            });
        }

        #region Collisions

        public static SceneObject CreateCollisionObject(Scene scene, Collision collision)
        {
            var mesh = CreateCollisionMesh(collision);
            if (mesh == null)
                return null;

            var meshObject = scene.AddObject<SceneObject>();
            meshObject.AddComponent<MeshRenderer>().Mesh = mesh;
            meshObject.Transform.SetTransform(collision.Get3dTransform(), SceneSpace.World);
            return meshObject;
        }

        public static Mesh CreateCollisionMesh(Collision collision)
        {
            if (collision is CollisionBox)
            {
                var box = (CollisionBox)collision;

                var mesh = WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Cube.obj"));
                var scale = new Vector3((float)box.Sx, (float)box.Sy, (float)box.Sz);
                ScaleMesh(mesh, scale);
                return mesh;
            }
            else if (collision is CollisionSphere)
            {
                var sphere = (CollisionSphere)collision;

                var mesh = WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Sphere.obj"));
                var scale = new Vector3((float)sphere.Radius);
                ScaleMesh(mesh, scale);
                return mesh;
            }
            return null;
        }

        #endregion

        #region Connections

        public static SceneObject CreateConnectivityObject(Scene scene, Connectivity connection)
        {
            var mesh = CreateConnectivityMesh(connection);
            if (mesh == null)
                return null;

            var connectionObject = scene.AddObject<SceneObject>();
            
            connectionObject.Transform.SetTransform(connection.Get3dTransform(), SceneSpace.World);

            //the mesh is in a separate object to keep the ldd transform and the mesh transform separated
            var meshObject = connectionObject.AddObject();
            meshObject.AddComponent<MeshRenderer>().Mesh = mesh;

            if (connection.Type == ConnectivityType.Axel)
            {
                var axel = (ConnectivityAxel)connection;
                if ((axel.SubType == 2 || axel.SubType == 4) && (axel.StartCapped || axel.EndCapped))
                {
                    if (axel.StartCapped)
                        meshObject.Transform.Translate(new Vector3(0, 0.8f, 0), RelativeSpace.Self);
                }
                if ((axel.SubType == 3 || axel.SubType == 19) && axel.Length.IsCloseTo(0.8, 0.05))//male pin (model orientation is important)
                {
                    if (axel.EndCapped)//if the end is capped, flip (180) the model and reposition (the origin is not at center)
                    {
                        meshObject.Transform.Rotate(new Vector3(180, 0, 0), RelativeSpace.Parent);
                        meshObject.Transform.Translate(Vector3.UnitY * (float)-axel.Length, RelativeSpace.Self);
                    }
                }
            }
            
            return connectionObject;
        }

        public static Mesh CreateConnectivityMesh(Connectivity connection)
        {
            if (connection.Type == ConnectivityType.Custom2DField)
                return null;

            if (connection.Type == ConnectivityType.Axel)
            {
                return GetAxelMesh((ConnectivityAxel)connection);
            }
            else if (connection.Type == ConnectivityType.Ball)
            {
                if(connection.SubType == 2 || connection.SubType == 3)//towball (3 = male, 2 = female)
                    return WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.TowBall.obj"));
            }

            //var sphereMesh = WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Sphere.obj"));
            //ScaleMesh(sphereMesh, new Vector3(0.4f));
            //return sphereMesh;
            return null;
        }

        private static Mesh GetAxelMesh(ConnectivityAxel axleConn)
        {
            //***** TECHNIC PINS ******
            if (axleConn.SubType == (int)ConnectivityAxel.SubTypes.PinFemale)
            {
                float adjustedLength = (float)axleConn.Length;

                //if either end is capped, use a male pin model to show connection direction
                if (axleConn.StartCapped || axleConn.EndCapped)
                {
                    if (axleConn.Length.IsCloseTo(0.8, 0.05))
                        return LoadResourceMesh("Models.Pin2.obj");
                    adjustedLength += 0.8f;//add one base unit to protrude
                }

                if (axleConn.Length.IsCloseTo(0.8, 0.05))
                    return LoadResourceMesh("Models.Pin1Female.obj");

                if (!(axleConn.StartCapped || axleConn.EndCapped))
                    Trace.WriteLine("Weird sized Pin connector!");

                var pinMesh = LoadResourceMesh("Models.BarFemale.obj");
                ScaleMesh(pinMesh, new Vector3(1f, adjustedLength / 0.8f, 1f));
                return pinMesh;
            }
            else if (axleConn.SubType == (int)ConnectivityAxel.SubTypes.PinMale || 
                     axleConn.SubType == (int)ConnectivityAxel.SubTypes.PinMale2)
            {
                if (axleConn.Length.IsCloseTo(0.8, 0.05))
                    return LoadResourceMesh("Models.Pin1Male.obj");

                var pinMesh = LoadResourceMesh("Models.BarFemale.obj");
                ScaleMesh(pinMesh, new Vector3(1f, (float)axleConn.Length / 0.8f, 1f));
                return pinMesh;
            }

            //***** TECHNIC CROSS AXLE ******
            if (axleConn.SubType == (int)ConnectivityAxel.SubTypes.CrossAxleFemale)
            {
                float adjustedLength = (float)axleConn.Length;

                //if either end is capped, use a male cross axle model to show connection direction
                if (axleConn.StartCapped || axleConn.EndCapped)
                {
                    if(axleConn.Length.IsCloseTo(0.8, 0.05))
                        return LoadResourceMesh("Models.Cross2.obj");//cross axle 2m with notches
                    adjustedLength += 0.8f;//add one base unit to protrude
                }

                //if (axleConn.Length.IsCloseTo(1.6, 0.05))
                //    return LoadResourceMesh("Models.Cross2.obj");

                var axleMesh = WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Cross1.obj"));
                ScaleMesh(axleMesh, new Vector3(1f, adjustedLength / 0.8f, 1f));
                return axleMesh;
            }
            else if (axleConn.SubType == (int)ConnectivityAxel.SubTypes.CrossAxleMale)
            {
                var axleMesh = WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Cross1.obj"));
                ScaleMesh(axleMesh, new Vector3(1f, (float)axleConn.Length / 0.8f, 1f));
                return axleMesh;
            }

            return null;
        }

        #endregion

        private static Mesh LoadResourceMesh(string resourceName)
        {

            Stream resourceStream = null;
            try
            {
                resourceStream = ResourcesHelper.GetResource(resourceName);
                if (resourceStream == null)
                    return null;
                return WavefrontMeshLoader.LoadWavefrontObj(resourceStream);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (resourceStream != null)
                    resourceStream.Dispose();
            }
        }

        private static void ScaleMesh(Mesh mesh, Vector3 scaleFactor)
        {
            foreach (var vert in mesh.Vertices)
                vert.Position = Vector3.Multiply(vert.Position, scaleFactor);
        }

        private static void TranslateMesh(Mesh mesh, Vector3 translation)
        {
            foreach (var vert in mesh.Vertices)
                vert.Position = vert.Position + translation;
        }
    }
}
