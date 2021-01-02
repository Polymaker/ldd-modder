using Assimp;
using LDDModder.BrickEditor.Resources;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Parts;
using LDDModder.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Meshes
{
    public static class MeshConverter
    {
        

        public static MeshGeometry AssimpToLdd(Scene scene, Mesh mesh)
        {
            bool hasUVs = mesh.HasTextureCoords(0);

            var builder = new GeometryBuilder();
            var meshNode = Assimp.AssimpHelper.GetMeshNode(scene, mesh);
            var meshTransform = Assimp.AssimpHelper.GetFinalTransform(meshNode).ToLDD();

            
            for (int i = 0; i < mesh.VertexCount; i++)
            {
                builder.AddVertex(new Vertex()
                {
                    Position = meshTransform.TransformPosition(mesh.Vertices[i].ToLDD()),
                    Normal = meshTransform.TransformNormal(mesh.Normals[i].ToLDD()),
                    TexCoord = hasUVs ? mesh.TextureCoordinateChannels[0][i].ToLDD().Xy : Simple3D.Vector2.Empty
                }, false);
            }

            for (int i = 0; i < mesh.FaceCount; i++)
            {
                if (mesh.Faces[i].IndexCount != 3)
                    continue;

                builder.AddTriangle(mesh.Faces[i].Indices[0], mesh.Faces[i].Indices[1], mesh.Faces[i].Indices[2]);
            }

            if (mesh.HasBones)
            {
                var boneNames = scene.Meshes.SelectMany(x => x.Bones).Select(x => x.Name).Distinct().ToList();
                var boneNodes = Assimp.AssimpHelper.GetNodeHierarchy(scene.RootNode)
                    .Where(x => boneNames.Contains(x.Name))/*.OrderBy(x => x.GetLevel())*/.ToList();

                boneNames = boneNodes.Select(x => x.Name).ToList();//names in order

                for (int i = 0; i < mesh.BoneCount; i++)
                {
                    int boneIndex = boneNames.IndexOf(mesh.Bones[i].Name);
                    foreach (var vw in mesh.Bones[i].VertexWeights)
                        builder.Vertices[vw.VertexID].BoneWeights.Add(new BoneWeight(boneIndex, vw.Weight));
                }
            }

            var geometry = builder.GetGeometry();
            geometry.SimplifyVertices();
            return geometry;
        }

        #region LDD -> Assimp

        public static Scene LddPartToAssimp(PartWrapper part, MeshExportOptions exportOptions = null)
        {
            if (exportOptions == null)
                exportOptions = new MeshExportOptions();

            var scene = new Scene() { RootNode = new Node("Root") };
            scene.Materials.Add(new Material() { Name = "BaseMaterial" });

            var meshNodes = new List<Node>();


            foreach (var surface in part.Surfaces)
            {
                string nodeName = "MainSurface";
                if (surface.SurfaceID > 0)
                    nodeName = $"Decoration{surface.SurfaceID}";

                var createdNodes = CreateSurfaceMeshNodes(surface, scene, nodeName, exportOptions);
                meshNodes.AddRange(createdNodes);
            }

            //meshNodes.AddRange(scene.GetNodeHierarchy().Where(x => x.MeshCount > 0));

            if (exportOptions.IncludeBones && part.IsFlexible)
            {
                var armatureNode = CreateArmatureNodeHierarchy(part, scene);

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

            if (exportOptions.IncludeConnections && part.Primitive.Connectors.Any())
            {
                var connectionsNode = new Node("Connections");
                scene.RootNode.Children.Add(connectionsNode);

                foreach (var connGroup in part.Primitive.Connectors.GroupBy(x => x.Type))
                {
                    int connectionIdx = 0;
                    foreach (var conn in connGroup)
                    {
                        var connNode = new Node($"{connGroup.Key.ToString()}{connectionIdx++}_Type_{conn.SubType}")
                        {
                            Transform = conn.Transform.ToMatrix4().ToAssimp()
                        };
                        //connNode.Metadata.Add("Type", new Assimp.Metadata.Entry(Assimp.MetaDataType.String, conn.Type.ToString()));
                        connectionsNode.Children.Add(connNode);
                    }
                }
            }

            if (exportOptions.IncludeCollisions && part.Primitive.Collisions.Any())
            {
                var collisionsNode = new Node("Collisions");
                scene.RootNode.Children.Add(collisionsNode);

                var sphereMesh = ResourceHelper.GetResourceModel("Models.Sphere.obj", "obj").Meshes[0];
                var boxMesh = ResourceHelper.GetResourceModel("Models.Cube.obj", "obj").Meshes[0];

                foreach (var collGroup in part.Primitive.Collisions.GroupBy(x => x.CollisionType))
                {
                    int collisionIdx = 0;

                    foreach (var collision in collGroup)
                    {
                        var collNode = new Node($"{collGroup.Key.ToString()}{collisionIdx++}");
                        var meshTransform = 
                            Simple3D.Matrix4d.FromScale(collision.GetSize() * 2f) * collision.Transform.ToMatrix4d();

                        if (collision.CollisionType == LDD.Primitives.Collisions.CollisionType.Box)
                        {
                            collNode.MeshIndices.Add(scene.MeshCount);
                            scene.Meshes.Add(boxMesh.Clone());
                        }
                        else
                        {
                            collNode.MeshIndices.Add(scene.MeshCount);
                            scene.Meshes.Add(sphereMesh.Clone());
                        }
                        //connNode.Metadata.Add("Type", new Assimp.Metadata.Entry(Assimp.MetaDataType.String, conn.Type.ToString()));
                        collNode.Transform = ((Simple3D.Matrix4)meshTransform).ToAssimp();
                        collisionsNode.Children.Add(collNode);
                    }
                }
            }

            //if (exportOptions.IncludeRoundEdgeData)
            //{
            //    foreach (var mat in scene.Materials)
            //    {
            //    }
            //}

            return scene;
        }

        private static List<Node> CreateSurfaceMeshNodes(PartSurfaceMesh partSurface, Scene scene, string surfaceName, MeshExportOptions exportOptions)
        {
            var meshNodes = new List<Node>();

            int materialIndex = scene.MaterialCount;
            scene.Materials.Add(new Material() { Name = $"{surfaceName}_Material" });

            if (exportOptions.IndividualComponents)
            {
                int meshIndex = 0;

                foreach (var comp in partSurface.Mesh.Cullings)
                {
                    string nodeName = $"{surfaceName}_Mesh{meshIndex++}";

                    var compModel = partSurface.Mesh.GetCullingGeometry(comp);
                    var mNode = CreateMeshNode(scene, compModel, nodeName, materialIndex, exportOptions);
                    meshNodes.Add(mNode);


                    if (comp.ReplacementMesh != null && exportOptions.IncludeAltMeshes)
                    {
                        mNode = CreateMeshNode(scene, comp.ReplacementMesh, nodeName + "Alt", materialIndex, exportOptions);
                        meshNodes.Add(mNode);
                    }
                }
            }
            else
            {
                var mNode = CreateMeshNode(scene, partSurface.Mesh.Geometry, surfaceName, materialIndex, exportOptions);
                meshNodes.Add(mNode);
            }

            return meshNodes;
        }

        private static Node CreateArmatureNodeHierarchy(PartWrapper part, Scene scene)
        {
            var armatureNode = new Node() { Name = "Armature" };
            armatureNode.Transform = Assimp.Matrix4x4.Identity;
            var allBoneNodes = new List<Node>();
            Matrix4x4 lastMatrix = Assimp.Matrix4x4.Identity;
            Node lastBoneNode = null;
            var boneMatrixes = new Dictionary<string, Matrix4x4>();
            var primitive = part.Primitive;

            for (int i = 0; i < primitive.FlexBones.Count; i++)
            {
                var flexBone = primitive.FlexBones[i];

                var bonePosMat = flexBone.Transform.ToMatrix4().ToAssimp();
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
                    b.OffsetMatrix = boneMatrixes[b.Name];
            }

            return armatureNode;
        }

        public static Mesh LddMeshToAssimp(MeshGeometry geometry)
        {
            var assimpMesh = new Mesh(Assimp.PrimitiveType.Triangle);
            bool isTextured = geometry.IsTextured;

            foreach (var v in geometry.Vertices)
            {
                assimpMesh.Vertices.Add(v.Position.ToAssimp());
                assimpMesh.Normals.Add(v.Normal.ToAssimp());
                if (isTextured)
                    assimpMesh.TextureCoordinateChannels[0].Add(new Vector3D(v.TexCoord.X, v.TexCoord.Y, 0));
            }

            if (geometry.IsFlexible)
            {
                var boneIDs = geometry.Vertices.SelectMany(x => x.BoneWeights).Select(y => y.BoneID).Distinct().ToList();
                var boneDict = new Dictionary<int, Bone>();
                boneIDs.ForEach(x => boneDict.Add(x, new Bone() { Name = GetBoneName(x) }));

                for (int vIdx = 0; vIdx < geometry.VertexCount; vIdx++)
                {
                    foreach (var bw in geometry.Vertices[vIdx].BoneWeights)
                    {
                        boneDict[bw.BoneID].VertexWeights.Add(new VertexWeight(vIdx, bw.Weight));
                    }
                }

                assimpMesh.Bones.AddRange(boneDict.Values);
            }

            int[] indices = geometry.GetTriangleIndices();

            for (int i = 0; i < indices.Length; i += 3)
                assimpMesh.Faces.Add(new Face(new int[] { indices[i], indices[i + 1], indices[i + 2] }));

            return assimpMesh;
        }

        public static Mesh LddMeshToAssimp(LDD.Files.MeshFile meshFile)
        {
            return LddMeshToAssimp(meshFile.Geometry);
        }

        public static Mesh LddRoundEdgeMeshToAssimp(MeshGeometry geometry)
        {
            var assimpMesh = new Mesh(Assimp.PrimitiveType.Triangle);
            int indexCounter = 0;

            foreach (var tri in geometry.Triangles)
            {
                foreach (var idx in tri.Indices)
                {
                    assimpMesh.Vertices.Add(idx.Vertex.Position.ToAssimp());
                    assimpMesh.Normals.Add(idx.Vertex.Normal.ToAssimp());
                    for (int i = 0; i < 6; i++)
                    {
                        var uvCoord = idx.RoundEdgeData.Coords[i];
                        if (RoundEdgeData.EmptyCoord != uvCoord)
                            uvCoord.X = Math.Abs(uvCoord.X) - 100f;
                        //uvCoord /= 10f;
                        assimpMesh.TextureCoordinateChannels[i].Add(new Vector3D(uvCoord.X, uvCoord.Y, 0));
                    }
                }

                assimpMesh.Faces.Add(new Face(new int[] { indexCounter++, indexCounter++, indexCounter++ }));
            }

            return assimpMesh;
        }

        private static Node CreateMeshNode(Scene scene, MeshGeometry geometry, string name, int materialIndex, MeshExportOptions exportOptions)
        {
            var meshNode = new Node() { Name = name };

            var aMesh = exportOptions.IncludeRoundEdgeData ? 
                LddRoundEdgeMeshToAssimp(geometry) : LddMeshToAssimp(geometry);

            aMesh.MaterialIndex = materialIndex;
            meshNode.MeshIndices.Add(scene.MeshCount);
            scene.Meshes.Add(aMesh);

            return meshNode;
        }

        #endregion

        #region Part Project -> Assimp

        public static Scene PartProjectToAssimp(PartProject project, MeshExportOptions exportOptions = null)
        {
            var partWrapper = project.GenerateLddPart();
            return LddPartToAssimp(partWrapper, exportOptions);
        }

        #endregion

        private static string GetBoneName(LDD.Primitives.FlexBone flexBone)
        {
            return GetBoneName(flexBone.ID);
        }

        private static string GetBoneName(int boneID)
        {
            return $"Bone_{boneID.ToString().PadLeft(4, '0')}";
        }
    }
}
