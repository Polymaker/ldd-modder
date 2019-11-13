using LDDModder.BrickEditor.Rendering.Shaders;
using LDDModder.LDD.Meshes;
using LDDModder.Modding.Editing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LDDModder.BrickEditor.Rendering
{
    public class GLSurfaceModel : IDisposable
    {
        public PartSurface Surface { get; set; }

        public IndexedVertexBuffer<VertVNT> VertexBuffer { get; private set; }

        public List<SurfaceModelMesh> MeshModels { get; private set; }

        public bool IsTransparent => Material.Diffuse.W < 1f;

        public MaterialInfo Material { get; set; }

        public GLSurfaceModel()
        {
            VertexBuffer = new IndexedVertexBuffer<VertVNT>();
            MeshModels = new List<SurfaceModelMesh>();
        }

        public GLSurfaceModel(PartSurface surface)
        {
            Surface = surface;
            VertexBuffer = new IndexedVertexBuffer<VertVNT>();
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

                    if (!distinctMeshes.Contains(meshRef.ModelMesh))
                        distinctMeshes.Add(meshRef.ModelMesh);
                }

                if (surfComp is FemaleStudModel femaleStud)
                {
                    foreach (var meshRef in femaleStud.ReplacementMeshes)
                    {
                        var addedModel = AddMeshGeometry(meshRef, indexList, vertexList);
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

            var model = new SurfaceModelMesh(this, modelMesh, indexOffset, geometry.IndexCount, vertexOffset);
            model.BoundingBox = BBox.FromMinMax(minPos, maxPos);

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

            modelShader.Material.Set(Material);
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
            VertexBuffer.BindAttribute(wireframeShader.Normal, 12);
            wireframeShader.ModelMatrix.Set(Matrix4.Identity);
            //wireframeShader.ModelMatrix.Set(Transform);
        }

        public void UnbindShader(WireframeShaderProgram wireframeShader)
        {
            VertexBuffer.Bind();
            VertexBuffer.UnbindAttribute(wireframeShader.Position);
            VertexBuffer.UnbindAttribute(wireframeShader.Normal);
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
            wireframeShader.Color.Set(model.IsSelected ? new Vector4(1f, 1f, 1f, 1f) : new Vector4(0f, 0f, 0f, 1f));
            wireframeShader.ModelMatrix.Set(model.Transform);
            DrawMesh(model);
        }

        public void DrawModelMesh(SurfaceModelMesh model, ModelShaderProgram modelShader)
        {
            modelShader.IsSelected.Set(model.IsSelected);
            //modelShader.Material.Set(model.IsSelected ? SelectedMaterial : Material);
            modelShader.ModelMatrix.Set(model.Transform);
            //modelShader.Color.Set(model.IsSelected ? new Vector4(1f) : new Vector4(0f, 0f, 0f, 1f));
            DrawMesh(model);
        }

        public void Draw(WireframeShaderProgram wireframeShader, ModelShaderProgram modelShader)
        {
            var visibleMeshes = MeshModels.Where(x => x.Visible)
                .OrderByDescending(x=>x.IsSelected).ToList();

            BindToShader(modelShader);
            modelShader.UseTexture.Set(Surface.SurfaceID > 0);

            //if (visibleMeshes.Any(x => x.IsSelected))
            //{
            //    GL.Enable(EnableCap.StencilTest);
            //    GL.StencilFunc(StencilFunction.Always, 1, 0xFFFF);
            //    GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            //}

            foreach (var mesh in visibleMeshes)
            {
                //if (mesh.IsSelected)
                //{
                //    GL.ClearStencil(0);
                //    GL.Clear(ClearBufferMask.StencilBufferBit);
                //}
                DrawModelMesh(mesh, modelShader);
            }
            //UnbindShader(modelShader);

            BindToShader(wireframeShader);
            foreach (var mesh in visibleMeshes)
                DrawModelMesh(mesh, wireframeShader);
            //UnbindShader(wireframeShader);
        }

        public bool RayIntersects(Ray ray, SurfaceModelMesh model, out float distance)
        {
            distance = float.NaN;
            var vertices = VertexBuffer.VertexBuffer.Content;
            var indices = VertexBuffer.IndexBuffer.Content;

            for (int i = 0; i < model.IndexCount; i += 3)
            {
                var idx1 = indices[i + model.StartIndex];
                var idx2 = indices[i + 1 + model.StartIndex];
                var idx3 = indices[i + 2 + model.StartIndex];

                var v1 = vertices[idx1 + model.StartVertex];
                var v2 = vertices[idx2 + model.StartVertex];
                var v3 = vertices[idx3 + model.StartVertex];

                if (Ray.IntersectsTriangle(ray, v1.Position, v2.Position, v3.Position, out float hitDist))
                    distance = float.IsNaN(distance) ? hitDist : Math.Min(hitDist, distance);
            }

            return !float.IsNaN(distance);
        }

        public void Dispose()
        {
            if (VertexBuffer != null)
                VertexBuffer.Dispose();
        }
    }
}
