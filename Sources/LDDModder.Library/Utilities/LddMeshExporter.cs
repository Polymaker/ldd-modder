﻿using Assimp;
using LDDModder.LDD.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Utilities
{
    public static class LddMeshExporter
    {
        public static void ExportLddPart(PartMesh part, string filename, string formatID)
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
                var allBoneNodes = new List<Node>();
                Matrix4x4 lastMatrix = Matrix4x4.Identity;
                Node lastBoneNode = null;
                var boneMatrixes = new Dictionary<string, Matrix4x4>();
                var primitive = part.PartInfo;
                var currentDir = new Vector3D(0,1,0);

                for (int i = 0; i < primitive.FlexBones.Count; i++)
                {

                    var flexBone = primitive.FlexBones[i];
                    

                    Vector3D boneDir = new Vector3D();
                    Vector3D bonePos = flexBone.Transform.GetPosition().Convert();
                    

                    if (i + 1 < part.PartInfo.FlexBones.Count)
                    {
                        boneDir = (part.PartInfo.FlexBones[i + 1].Transform.GetPosition() - flexBone.Transform.GetPosition()).Convert();
                        boneDir.Normalize();
                    }
                    else
                    {
                        boneDir = (flexBone.Transform.GetPosition() - part.PartInfo.FlexBones[i - 1].Transform.GetPosition()).Convert();
                        boneDir.Normalize();
                    }
                    
                    var bonePosMat = flexBone.Transform.ToMatrix4().Convert();
                    var rotMat = Matrix4x4.FromToMatrix(currentDir, boneDir);
                    
                    rotMat.Inverse();
                    //bonePosMat = rotMat * bonePosMat;
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

     
                    //if (i + 1 < part.PartInfo.FlexBones.Count)
                    //{
                    //    var nextBone = part.PartInfo.FlexBones[i + 1];
                    //    var up = lastMatrix * new Vector3D(0, 1, 0);
                    //    var mat = Simple3D.Matrix4.LookAt(flexBone.Transform.GetPosition(), nextBone.Transform.GetPosition(), new Simple3D.Vector3(0, 1, 0));
                    //    boneMatrixes.Add(boneNode.Name, mat.Convert());
                    //}
                    //else
                    //    boneMatrixes.Add(boneNode.Name, Matrix4x4.Identity);

  
                    allBoneNodes.Add(boneNode);
                    if (lastBoneNode == null)
                        armatureNode.Children.Add(boneNode);
                    else
                        lastBoneNode.Children.Add(boneNode);

                    lastBoneNode = boneNode;
                    lastMatrix = bonePosMat;
                    currentDir = boneDir;
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

        private static Node CreateMeshNode(Scene scene, PartMesh part, LDD.Meshes.Mesh lddMesh, string name)
        {
            var meshNode = new Node() { Name = name };
            var aMesh = LDD.Meshes.MeshConverter.ConvertFromLDD(lddMesh);
            aMesh.MaterialIndex = scene.MeshCount;
            meshNode.MeshIndices.Add(scene.MeshCount);
            scene.Meshes.Add(aMesh);

            if (lddMesh.IsFlexible)
            {
                foreach (var flexBone in part.PartInfo.FlexBones)
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


        private static Matrix4x4 LddTransformToMatrix(LDD.Primitives.Transform transform)
        {
            var rotAxis = new Vector3D(transform.Axis.X, transform.Axis.Y, transform.Axis.Z);
            var rotAngle = transform.Angle * ((float)Math.PI / 180f);
            var rotMat = Matrix4x4.FromAngleAxis(rotAngle, rotAxis);
            var transMat = Matrix4x4.FromTranslation(new Vector3D(transform.Translation.X, transform.Translation.Y, transform.Translation.Z));

            var finalMat = Matrix4x4.Identity;
            finalMat *= rotMat;
            finalMat *= transMat;
            finalMat.Inverse();
            return finalMat;
        }
    }
}
