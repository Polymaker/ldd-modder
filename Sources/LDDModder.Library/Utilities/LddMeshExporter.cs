using Assimp;
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
            var defMat = new Assimp.Material();
            scene.Materials.Add(defMat);

            CreateMeshNode(scene, part, part.MainModel);

            foreach(var decMesh in part.DecorationMeshes)
                CreateMeshNode(scene, part, decMesh);

            AssimpContext importer = new AssimpContext();
            importer.ExportFile(scene, filename, formatID, PostProcessSteps.ValidateDataStructure);
        }

        private static void CreateMeshNode(Scene scene, PartMesh part, LDD.Meshes.Mesh lddMesh)
        {
            var meshNode = new Node();
            var aMesh = LDD.Meshes.MeshConverter.ConvertFromLDD(lddMesh);
            meshNode.MeshIndices.Add(scene.MeshCount);
            scene.Meshes.Add(aMesh);

            if (lddMesh.IsFlexible)
            {
                //var boneWeights = new Dictionary<int, List<LDD.Meshes.BoneWeight>>();
                var armatureNode = new Node();
                foreach (var flexBone in part.PartInfo.FlexBones)
                {
                    var bone = new Assimp.Bone()
                    {
                        Name = "Bone" + flexBone.ID,
                        OffsetMatrix = LddTransformToMatrix(flexBone.Transform)
                    };
                    var boneNode = new Node(bone.Name)
                    {
                        Transform = bone.OffsetMatrix
                    };
                    armatureNode.Children.Add(boneNode);
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
                armatureNode.Children.Add(meshNode);
                scene.RootNode.Children.Add(armatureNode);
            }
            else
                scene.RootNode.Children.Add(meshNode);


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
