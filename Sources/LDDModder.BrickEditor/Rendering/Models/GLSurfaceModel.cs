﻿using LDDModder.BrickEditor.Rendering.Shaders;
using LDDModder.LDD.Meshes;
using LDDModder.Modding.Editing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class GLSurfaceModel : IDisposable
    {
        public PartSurface Surface { get; set; }

        public IndexedVertexBuffer VertexBuffer { get; private set; }

        public List<SurfaceModelMesh> MeshModels { get; private set; }

        public MaterialInfo Material { get; set; }

        public MaterialInfo SelectedMaterial { get; set; }

        public GLSurfaceModel()
        {
            VertexBuffer = new IndexedVertexBuffer();
            MeshModels = new List<SurfaceModelMesh>();
        }

        public GLSurfaceModel(PartSurface surface)
        {
            Surface = surface;
            VertexBuffer = new IndexedVertexBuffer();
            MeshModels = new List<SurfaceModelMesh>();
        }

        public void RebuildPartModels()
        {
            MeshModels.Clear();
            VertexBuffer.ClearBuffers();

            var indexList = new List<int>();
            var vertexList = new List<VertVNT>();

            var distinctMeshes = new List<ModelMesh>();
            foreach (var surfComp in Surface.Components)
            {
                foreach (var meshRef in surfComp.Meshes)
                {
                    var addedModel = AddMeshGeometry(meshRef, indexList, vertexList);
                    addedModel.Visible = true;
                    addedModel.Component = surfComp;

                    if (!distinctMeshes.Contains(meshRef.ModelMesh))
                        distinctMeshes.Add(meshRef.ModelMesh);
                }

                if (surfComp is FemaleStudModel femaleStud)
                {
                    foreach (var meshRef in femaleStud.ReplacementMeshes)
                    {
                        var addedModel = AddMeshGeometry(meshRef, indexList, vertexList);
                        addedModel.Component = surfComp;
                        addedModel.Visible = false;

                        if (!distinctMeshes.Contains(meshRef.ModelMesh))
                            distinctMeshes.Add(meshRef.ModelMesh);
                    }
                }
            }

            distinctMeshes.ForEach(x => x.UnloadModel());

            VertexBuffer.SetIndices(indexList);
            VertexBuffer.SetVertices(vertexList);
        }

        private SurfaceModelMesh AddMeshGeometry(ModelMeshReference modelMesh, List<int> indexList, List<VertVNT> vertexList)
        {
            var geometry = modelMesh.GetGeometry();
            int indexOffset = indexList.Count;
            int vertexOffset = vertexList.Count;
            var triangleIndices = geometry.GetTriangleIndices();
            Vector3 minPos = new Vector3(9999f);
            Vector3 maxPos = new Vector3(-9999f);

            indexList.AddRange(triangleIndices);

            foreach (var vertex in geometry.Vertices)
            {
                var glVertex = new VertVNT()
                {
                    Position = vertex.Position.ToGL(),
                    Normal = vertex.Normal.ToGL(),
                    TexCoord = geometry.IsTextured ? vertex.TexCoord.ToGL() : Vector2.Zero
                };

                minPos = Vector3.ComponentMin(minPos, glVertex.Position);
                maxPos = Vector3.ComponentMax(maxPos, glVertex.Position);
                vertexList.Add(glVertex);
            }

            var model = new SurfaceModelMesh(modelMesh, indexOffset, geometry.IndexCount, vertexOffset);
            model.BoundingBox = new BBox(minPos, maxPos);
            MeshModels.Add(model);
            return model;
        }


        #region Shader Binding

        public void BindToShader(ModelShaderProgram modelShader)
        {
            modelShader.Use();
            VertexBuffer.Bind();
            VertexBuffer.BindAttribute(modelShader.Position, 0);
            VertexBuffer.BindAttribute(modelShader.Normal, 12);
            VertexBuffer.BindAttribute(modelShader.TexCoord, 24);
            //modelShader.ModelMatrix.Set(Matrix4.Identity);
            //modelShader.ModelMatrix.Set(Transform);
            //modelShader.Material.Set(Material);
        }

        public void UnbindShader(ModelShaderProgram modelShader)
        {
            VertexBuffer.Bind();
            VertexBuffer.UnbindAttribute(modelShader.Position);
            VertexBuffer.UnbindAttribute(modelShader.Normal);
            VertexBuffer.UnbindAttribute(modelShader.TexCoord);
        }

        public void BindToShader(WireframeShaderProgram wireframeShader)
        {
            wireframeShader.Use();

            VertexBuffer.Bind();
            VertexBuffer.BindAttribute(wireframeShader.Position, 0);
            wireframeShader.ModelMatrix.Set(Matrix4.Identity);
            //wireframeShader.ModelMatrix.Set(Transform);
        }

        public void UnbindShader(WireframeShaderProgram wireframeShader)
        {
            VertexBuffer.Bind();
            VertexBuffer.UnbindAttribute(wireframeShader.Position);
        }

        #endregion

        public void Draw()
        {
            VertexBuffer.Vao.DrawElements(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, VertexBuffer.IndexBuffer.ElementCount);
        }

        public void DrawMesh(SurfaceModelMesh model)
        {
            VertexBuffer.Vao.DrawElementsBaseVertex(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles,
                model.StartVertex, model.IndexCount,
                OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedInt, 
                model.StartIndex * 4);
        }

        public void DrawModelMesh(SurfaceModelMesh model, WireframeShaderProgram wireframeShader)
        {
            wireframeShader.Color.Set(model.IsSelected ? new Vector4(1f, 0.75f, 0.2f, 1f) : new Vector4(0f, 0f, 0f, 1f));
            wireframeShader.ModelMatrix.Set(model.Transform);
            DrawMesh(model);
        }

        public void DrawModelMesh(SurfaceModelMesh model, ModelShaderProgram modelShader)
        {
            modelShader.Material.Set(model.IsSelected ? SelectedMaterial : Material);
            modelShader.ModelMatrix.Set(model.Transform);
            //modelShader.Color.Set(model.IsSelected ? new Vector4(1f) : new Vector4(0f, 0f, 0f, 1f));
            DrawMesh(model);
        }


        public void Dispose()
        {
            if (VertexBuffer != null)
                VertexBuffer.Dispose();
        }
    }
}
