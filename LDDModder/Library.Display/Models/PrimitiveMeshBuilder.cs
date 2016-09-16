using LDDModder.LDD.Primitives;
using LDDModder.Utilities;
using OpenTK;
using Poly3D.Engine;
using Poly3D.Engine.Data;
using Poly3D.Engine.Meshes;
using System;
using System.Collections.Generic;
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
            meshObject.Transform.Translate(collision.GetTranslation(), RelativeSpace.Parent);
            meshObject.Transform.Rotation = collision.GetRotation();
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

            var meshObject = scene.AddObject<SceneObject>();
            meshObject.AddComponent<MeshRenderer>().Mesh = mesh;
            meshObject.Transform.Translate(connection.GetTranslation(), RelativeSpace.Parent);
            meshObject.Transform.Rotation = connection.GetRotation();

            if (connection.Type == ConnectivityType.Axel)
            {
                var axel = (ConnectivityAxel)connection;
                if ((axel.SubType == 2 || axel.SubType == 4) && axel.Length <= 1.6)
                {
                    if (axel.StartCapped)
                        meshObject.Transform.Translate(new Vector3(0, 0.8f, 0), RelativeSpace.Self);
                }
            }
            
            return meshObject;
        }

        public static Mesh CreateConnectivityMesh(Connectivity connection)
        {
            if (connection is ConnectivityAxel)
            {
                var axleConn = (ConnectivityAxel)connection;
                if (axleConn.SubType == 2 || axleConn.SubType == 6)//Pin connector
                {
                    if (axleConn.SubType == 2 && axleConn.Length <= 1.6)//SubType 2 = Pin connector (Female)
                        return WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Pin2.obj"));
                    else//SubType 6 = Pin connector (Male)
                    {
                        var pinMesh = WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Pin1.obj"));
                        ScaleMesh(pinMesh, new Vector3(1f, (float)axleConn.Length / 0.8f, 1f));
                        return pinMesh;
                    }
                }
                else if (axleConn.SubType == 4 || axleConn.SubType == 5)//Cross axle connector
                {
                    if (axleConn.SubType == 4 && axleConn.Length <= 1.6)//SubType 4 = Cross axle connector (Female)
                        return WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Cross2.obj"));
                    else//SubType 5 = Cross axle connector (Male)
                    {
                        var axleMesh = WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Cross1.obj"));
                        ScaleMesh(axleMesh, new Vector3(1f, (float)axleConn.Length / 0.8f, 1f));
                        return axleMesh;
                    }
                }
                else if (axleConn.SubType == 19)//SubType 19 = Pole (Male)
                {
                    var poleMesh = WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Pole1.obj"));
                    var adjustedLength = (float)axleConn.Length + (!axleConn.StartCapped ? 0.4f : 0) + (!axleConn.EndCapped ? 0.4f : 0);
                    ScaleMesh(poleMesh, new Vector3(1f, adjustedLength / 0.8f, 1f));

                    if ((!axleConn.StartCapped || !axleConn.EndCapped) && axleConn.StartCapped != axleConn.EndCapped)
                    {
                        TranslateMesh(poleMesh, new Vector3(0, axleConn.StartCapped ? 0.2f : -0.2f, 0));
                    }
                    return poleMesh;
                }
            }
            else if (connection is ConnectivityBall)
                return WavefrontMeshLoader.LoadWavefrontObj(ResourcesHelper.GetResource("Models.Ball.obj"));

            return null;
        }

        #endregion

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
