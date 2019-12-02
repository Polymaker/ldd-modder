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

        private VertVNT[] Vertices;
        private int[] Indices;

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

            var indices = new List<int>();
            var vertices = new List<VertVNT>();

            var distinctMeshes = new List<ModelMesh>();

            foreach (var surfComp in Surface.Components)
            {
                foreach (var meshRef in surfComp.Meshes)
                {
                    var addedModel = AddMeshGeometry(meshRef, indices, vertices);
                    addedModel.Visible = true;

                    if (!distinctMeshes.Contains(meshRef.ModelMesh))
                        distinctMeshes.Add(meshRef.ModelMesh);
                }

                if (surfComp is FemaleStudModel femaleStud)
                {
                    foreach (var meshRef in femaleStud.ReplacementMeshes)
                    {
                        var addedModel = AddMeshGeometry(meshRef, indices, vertices);
                        addedModel.Visible = false;
                        addedModel.IsReplacementModel = true;

                        if (!distinctMeshes.Contains(meshRef.ModelMesh))
                            distinctMeshes.Add(meshRef.ModelMesh);
                    }
                }
            }

            distinctMeshes.ForEach(x => x.UnloadModel());
            Indices = indices.ToArray();
            Vertices = vertices.ToArray();
            VertexBuffer.SetIndices(indices);
            VertexBuffer.SetVertices(vertices);
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
                diffColor.W = 0.5f;
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

            foreach (var model in visibleMeshes)
            {
                if (model.IsSelected)
                {
                    RenderHelper.DrawBoundingBox(model.Transform, model.BoundingBox, 
                        new Vector4(0f, 1f, 1f, 1f), 1.5f);
                }
            }
        }

        public void RenderPartialModel(SurfaceModelMesh surfaceModel)
        {

        }

        public void RenderWireframe(Vector4 color, float size = 1f)
        {
            var visibleMeshes = MeshModels.Where(x => x.Visible)
                .OrderByDescending(x => x.IsSelected).ToList();

            if (!visibleMeshes.Any())
                return;

            foreach (var model in visibleMeshes)
            {
                RenderHelper.BeginDrawWireframe2(VertexBuffer, model.Transform, size, color);
                DrawPartialMesh(model);
                //RenderHelper.EndDrawWireframe(VertexBuffer);
            }
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
                RenderHelper.BeginDrawWireframe(VertexBuffer, model.Transform, 1f, model.IsSelected ? renderOptions.WireframeColorAlt : renderOptions.WireframeColor);
                DrawPartialMesh(model);
                RenderHelper.EndDrawWireframe(VertexBuffer);
            }
        }

        private void DrawModelOutline(SurfaceModelMesh model)
        {
            RenderHelper.BeginDrawWireframe(VertexBuffer, model.Transform, 4f, OutlineColor);
            RenderHelper.ApplyStencilMask();

            DrawPartialMesh(model);

            RenderHelper.EndDrawWireframe(VertexBuffer);
            RenderHelper.RemoveStencilMask();
        }

        private void DrawPartialMesh(SurfaceModelMesh mesh)
        {
            VertexBuffer.DrawElementsBaseVertex(PrimitiveType.Triangles, mesh.StartVertex, mesh.IndexCount, mesh.StartIndex * 4);
        }

        public bool RayIntersects(Ray ray, SurfaceModelMesh model, out float distance)
        {
            distance = float.NaN;
            var vertices = Vertices;
            var indices = Indices;
            var localRay = Ray.Transform(ray, model.Transform.Inverted());

            for (int i = 0; i < model.IndexCount; i += 3)
            {
                var idx1 = indices[i + model.StartIndex];
                var idx2 = indices[i + 1 + model.StartIndex];
                var idx3 = indices[i + 2 + model.StartIndex];

                var v1 = vertices[idx1 + model.StartVertex];
                var v2 = vertices[idx2 + model.StartVertex];
                var v3 = vertices[idx3 + model.StartVertex];

                if (Ray.IntersectsTriangle(localRay, v1.Position, v2.Position, v3.Position, out float hitDist))
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
