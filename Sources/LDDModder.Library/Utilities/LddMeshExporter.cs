using Assimp;
using LDDModder.LDD.Data;
using LDDModder.LDD.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Utilities
{
    public static class LddMeshExporter
    {
        public static void ExportLddPart(PartWrapper part, string filename, string formatID, bool includeBones)
        {
            var scene = new Scene() { RootNode = new Node("Root") };
            scene.Materials.Add(new Assimp.Material() { Name = "BaseMaterial" });

            var meshNodes = new List<Node>();

            foreach (var surface in part.Surfaces)
            {
                string nodeName = "BaseModel";
                if (surface.SurfaceID > 0)
                    nodeName = $"Decoration{surface.SurfaceID}";

                scene.Materials.Add(new Assimp.Material() { Name = $"{nodeName}_Material" });
                var meshNode = CreateMeshNode(scene, part, surface.Mesh, nodeName, includeBones);

                meshNodes.Add(meshNode);
            }

            if (part.IsFlexible && includeBones)
            {
                var armatureNode = new Node() { Name = "Armature" };
                armatureNode.Transform = Matrix4x4.Identity;
                //armatureNode.Transform = Matrix4x4.FromToMatrix(new Vector3D(0, 1, 0), new Vector3D(0, 0, 1));
                var allBoneNodes = new List<Node>();
                Matrix4x4 lastMatrix = Matrix4x4.Identity;
                Node lastBoneNode = null;
                var boneMatrixes = new Dictionary<string, Matrix4x4>();
                var primitive = part.Primitive;
                //var currentDir = new Vector3D(0, 0, 1);

                for (int i = 0; i < primitive.FlexBones.Count; i++)
                {
                    var flexBone = primitive.FlexBones[i];

                    var bonePosMat = flexBone.Transform.ToMatrix4().Convert();
                    var localPosMat = bonePosMat;

                    if (!lastMatrix.IsIdentity)
                    {
                        var inv = lastMatrix;
                        inv.Inverse();
                        localPosMat = bonePosMat * inv;
                    }

                    var boneNode = new Node()
                    {
                        Name = GetBoneName(flexBone),
                        Transform = localPosMat
                    };


                    var boneOrientation = bonePosMat;
                    boneOrientation.Inverse();

                    boneMatrixes.Add(boneNode.Name, boneOrientation);

                    allBoneNodes.Add(boneNode);
                    if (lastBoneNode == null)
                        armatureNode.Children.Add(boneNode);
                    else
                        lastBoneNode.Children.Add(boneNode);

                    lastBoneNode = boneNode;
                    lastMatrix = bonePosMat;
                }

                foreach (var mesh in scene.Meshes)
                {
                    foreach (var b in mesh.Bones)
                    {
                        //var bone = allBoneNodes.First(x => x.Name == b.Name);
                        b.OffsetMatrix = boneMatrixes[b.Name];
                    }
                }

                foreach (var node in meshNodes)
                    armatureNode.Children.Add(node);
                scene.RootNode.Children.Add(armatureNode);
            }
            else if (meshNodes.Count > 1)
            {
                var groupNode = new Node() { Name = "Part" };
                foreach (var node in meshNodes)
                    groupNode.Children.Add(node);
                scene.RootNode.Children.Add(groupNode);
            }
            else
                scene.RootNode.Children.Add(meshNodes[0]);


            AssimpContext importer = new AssimpContext();
            importer.ExportFile(scene, filename, formatID, PostProcessSteps.ValidateDataStructure);
        }

        public static void ExportLddPart(LDDPartFiles part, string filename, string formatID)
        {
            var scene = new Scene() { RootNode = new Node("Root") };
            scene.Materials.Add(new Assimp.Material() { Name = "BaseMaterial" });

            var meshNodes = new List<Node>();
            int decID = 1;

            foreach (var mesh in part.AllMeshes)
            {
                string nodeName = "BaseModel";
                if (mesh != part.MainModel)
                    nodeName = $"Decoration{decID++}";

                scene.Materials.Add(new Assimp.Material() { Name = $"{nodeName}_Material" });
                var meshNode = CreateMeshNode(scene, part, mesh, nodeName);
                                
                meshNodes.Add(meshNode);
            }

            if (part.MainModel.IsFlexible)
            {
                var armatureNode = new Node() { Name = "Armature" };
                armatureNode.Transform = Matrix4x4.Identity;
                //armatureNode.Transform = Matrix4x4.FromToMatrix(new Vector3D(0, 1, 0), new Vector3D(0, 0, 1));
                var allBoneNodes = new List<Node>();
                Matrix4x4 lastMatrix = Matrix4x4.Identity;
                Node lastBoneNode = null;
                var boneMatrixes = new Dictionary<string, Matrix4x4>();
                var primitive = part.Info;
                var currentDir = new Vector3D(0,0,1);

                for (int i = 0; i < primitive.FlexBones.Count; i++)
                {
                    var flexBone = primitive.FlexBones[i];
                   
                    var bonePosMat = flexBone.Transform.ToMatrix4().Convert();
                    var localPosMat = bonePosMat;

                    if (!lastMatrix.IsIdentity)
                    {
                        var inv = lastMatrix;
                        inv.Inverse();
                        localPosMat = bonePosMat * inv;
                    }
                    //var testPt = (localPosMat * lastMatrix) * new Vector3D();
                    //Console.WriteLine($"{bonePos}   {testPt}");
                    
                    var boneNode = new Node()
                    {
                        Name = GetBoneName(flexBone),
                        Transform = localPosMat
                    };


                    var boneOrientation = bonePosMat; 
                    boneOrientation.Inverse();

                    boneMatrixes.Add(boneNode.Name, boneOrientation);
  
                    allBoneNodes.Add(boneNode);
                    if (lastBoneNode == null)
                        armatureNode.Children.Add(boneNode);
                    else
                        lastBoneNode.Children.Add(boneNode);

                    lastBoneNode = boneNode;
                    lastMatrix = bonePosMat;
                }

                foreach (var mesh in scene.Meshes)
                {
                    foreach (var b in mesh.Bones)
                    {
                        //var bone = allBoneNodes.First(x => x.Name == b.Name);
                        b.OffsetMatrix = boneMatrixes[b.Name];
                    }
                }

                foreach (var node in meshNodes)
                    armatureNode.Children.Add(node);
                scene.RootNode.Children.Add(armatureNode);
            }
            else if (meshNodes.Count > 1)
            {
                var groupNode = new Node() { Name = "Part" };
                foreach (var node in meshNodes)
                    groupNode.Children.Add(node);
                scene.RootNode.Children.Add(groupNode);
            }
            else
                scene.RootNode.Children.Add(meshNodes[0]);


            AssimpContext importer = new AssimpContext();
            importer.ExportFile(scene, filename, formatID, PostProcessSteps.ValidateDataStructure);
        }

        public static void ExportLddPart(LDD.Files.MeshFile part, string filename, string formatID)
        {
            var scene = new Scene() { RootNode = new Node("Root") };
            scene.Materials.Add(new Assimp.Material() { Name = "BaseMaterial" });
            var meshNode = new Node() { Name = "BaseModel" };
            var aMesh = LDD.Meshes.MeshConverter.ConvertFromLDD(part.Geometry);
            aMesh.MaterialIndex = scene.MeshCount;
            meshNode.MeshIndices.Add(scene.MeshCount);
            scene.Meshes.Add(aMesh);
            scene.RootNode.Children.Add(meshNode);
            AssimpContext importer = new AssimpContext();
            importer.ExportFile(scene, filename, formatID, PostProcessSteps.ValidateDataStructure);
        }

        public static void ExportRoundEdge(LDD.Meshes.MeshGeometry geometry, string filename, string formatID)
        {
            var scene = new Scene() { RootNode = new Node("Root") };
            scene.Materials.Add(new Assimp.Material() { Name = "BaseMaterial" });
            var meshNode = new Node() { Name = "BaseModel" };
            var aMesh = LDD.Meshes.MeshConverter.ConvertFromLDD2(geometry);
            aMesh.MaterialIndex = scene.MeshCount;
            meshNode.MeshIndices.Add(scene.MeshCount);
            scene.Meshes.Add(aMesh);
            scene.RootNode.Children.Add(meshNode);
            AssimpContext importer = new AssimpContext();
            filename = System.IO.Path.GetFullPath(filename);
            importer.ExportFile(scene, filename, formatID, PostProcessSteps.ValidateDataStructure);
        }

        private static Node CreateMeshNode(Scene scene, LDDPartFiles part, LDD.Files.MeshFile lddMesh, string name)
        {
            var meshNode = new Node() { Name = name };
            var aMesh = LDD.Meshes.MeshConverter.ConvertFromLDD(lddMesh);
            aMesh.MaterialIndex = scene.MeshCount;
            meshNode.MeshIndices.Add(scene.MeshCount);
            scene.Meshes.Add(aMesh);

            if (lddMesh.IsFlexible)
            {
                foreach (var flexBone in part.Info.FlexBones)
                {
                    var bone = new Assimp.Bone()
                    {
                        Name = GetBoneName(flexBone),
                        //OffsetMatrix = LddTransformToMatrix(flexBone.Transform)
                    };
                    aMesh.Bones.Add(bone);
                    for (int i = 0; i < lddMesh.VertexCount; i++)
                    {
                        var v = lddMesh.Vertices[i];
                        foreach (var bw in v.BoneWeights)
                        {
                            if (bw.BoneID == flexBone.ID)
                                bone.VertexWeights.Add(new VertexWeight(i, bw.Weight));
                        }
                    }
                }
            }

            return meshNode;
        }

        private static Node CreateMeshNode(Scene scene, PartWrapper part, LDD.Files.MeshFile lddMesh, string name, bool includeBones)
        {
            var meshNode = new Node() { Name = name };
            var aMesh = LDD.Meshes.MeshConverter.ConvertFromLDD(lddMesh);
            aMesh.MaterialIndex = scene.MeshCount;
            meshNode.MeshIndices.Add(scene.MeshCount);
            scene.Meshes.Add(aMesh);

            if (lddMesh.IsFlexible && includeBones)
            {
                foreach (var flexBone in part.Primitive.FlexBones)
                {
                    var bone = new Assimp.Bone()
                    {
                        Name = GetBoneName(flexBone),
                        //OffsetMatrix = LddTransformToMatrix(flexBone.Transform)
                    };
                    aMesh.Bones.Add(bone);
                    for (int i = 0; i < lddMesh.VertexCount; i++)
                    {
                        var v = lddMesh.Vertices[i];
                        foreach (var bw in v.BoneWeights)
                        {
                            if (bw.BoneID == flexBone.ID)
                                bone.VertexWeights.Add(new VertexWeight(i, bw.Weight));
                        }
                    }
                }
            }

            return meshNode;
        }

        private static string GetBoneName(LDD.Primitives.FlexBone flexBone)
        {
            return $"Bone_{flexBone.ID.ToString().PadLeft(4, '0')}";
        }
    }
}
