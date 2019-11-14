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

        public Vector4 WireframeColor { get; set; }

        public Vector4 WireframeColorAlt { get; set; }

        public Vector4 OutlineColor { get; set; }

        public GLSurfaceModel()
        {
            VertexBuffer = new IndexedVertexBuffer<VertVNT>();
            MeshModels = new List<SurfaceModelMesh>();
            WireframeColor = new Vector4(0, 0, 0, 1f);
        }

        public GLSurfaceModel(PartSurface surface)
        {
            Surface = surface;
            VertexBuffer = new IndexedVertexBuffer<VertVNT>();
            MeshModels = new List<SurfaceModelMesh>();
            WireframeColor = new Vector4(0, 0, 0, 1f);
            WireframeColorAlt = new Vector4(0.85f, 0.85f, 0.85f, 1f);// new Vector4(0.956f, 0.6f, 0.168f, 1f);
            OutlineColor = new Vector4(1f);

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


        public void Render(RenderOptions renderOptions, bool alphaPass = false)
        {
            var visibleMeshes = MeshModels.Where(x => x.Visible)
                .OrderByDescending(x=>x.IsSelected).ToList();

            if (!visibleMeshes.Any())
                return;

            var currentMaterial = Material;
            if (renderOptions.DrawTransparent)
            {
                var diffColor = currentMaterial.Diffuse;
                diffColor.W = 0.8f;
                currentMaterial.Diffuse = diffColor;
            }

            RenderHelper.ModelShader.Use();
            RenderHelper.ModelShader.UseTexture.Set(renderOptions.DrawTextured && Surface.SurfaceID > 0);

            bool useOutlineStencil = !alphaPass && visibleMeshes.Any(x => x.IsSelected);
            if (useOutlineStencil)
                RenderHelper.EnableStencilTest();


            foreach (var model in visibleMeshes)
            {
                RenderPartialMesh(renderOptions, model, currentMaterial, useOutlineStencil);

                if (useOutlineStencil && model.IsSelected)
                    DrawModelOutline(model);
            }


            if (useOutlineStencil)
                RenderHelper.DisableStencilTest();
        }

        private void RenderPartialMesh(RenderOptions renderOptions, SurfaceModelMesh model, MaterialInfo material, bool useStencil)
        {
            if (renderOptions.DrawShaded || renderOptions.DrawTextured)
            {
                RenderHelper.BeginDrawModel(VertexBuffer, model.Transform, material);
                RenderHelper.ModelShader.IsSelected.Set(model.IsSelected);

                if (model.IsSelected && useStencil)
                    RenderHelper.EnableStencilMask();

                DrawPartialMesh(model);

                RenderHelper.EndDrawModel(VertexBuffer);

                if (model.IsSelected && useStencil)
                    RenderHelper.RemoveStencilMask();
            }

            if (renderOptions.DrawWireframe)
            {
                RenderHelper.BeginDrawWireframe(VertexBuffer, model.Transform, 1f, model.IsSelected ? WireframeColorAlt : WireframeColor);
                DrawPartialMesh(model);
                RenderHelper.EndDrawWireframe();
            }
        }

        private void DrawModelOutline(SurfaceModelMesh model)
        {
            RenderHelper.BeginDrawWireframe(VertexBuffer, model.Transform, 4f, OutlineColor);
            RenderHelper.ApplyStencilMask();

            DrawPartialMesh(model);

            RenderHelper.EndDrawWireframe();
            RenderHelper.RemoveStencilMask();
        }

        private void DrawPartialMesh(SurfaceModelMesh mesh)
        {
            VertexBuffer.DrawElementsBaseVertex(PrimitiveType.Triangles, mesh.StartVertex, mesh.IndexCount, mesh.StartIndex * 4);
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
