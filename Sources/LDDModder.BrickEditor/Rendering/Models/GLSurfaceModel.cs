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
using System.Diagnostics;

namespace LDDModder.BrickEditor.Rendering
{
    public class GLSurfaceModel : IDisposable
    {
        public PartSurface Surface { get; set; }

        public IndexedVertexBuffer<VertVNT> VertexBuffer { get; private set; }

        public List<SurfaceModelMesh> MeshModels { get; private set; }

        public MaterialInfo Material { get; set; }

        private VertVNT[] Vertices;
        private int[] Indices;

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

            var indices = new List<int>();
            var vertices = new List<VertVNT>();

            var distinctMeshes = new List<ModelMesh>();

            foreach (var surfComp in Surface.Components)
            {
                foreach (var meshRef in surfComp.Meshes)
                {
                    if (!meshRef.ModelMesh.CheckFileExist())
                    {
                        Debug.WriteLine($"Error: Could not load model: {meshRef.ModelMesh.FileName}");
                        continue;
                    }

                    var addedModel = AddMeshGeometry(meshRef, indices, vertices);

                    if (!distinctMeshes.Contains(meshRef.ModelMesh))
                        distinctMeshes.Add(meshRef.ModelMesh);
                }

                if (surfComp is FemaleStudModel femaleStud)
                {
                    foreach (var meshRef in femaleStud.ReplacementMeshes)
                    {
                        if (!meshRef.ModelMesh.CheckFileExist())
                        {
                            Debug.WriteLine($"Error: Could not load model: {meshRef.ModelMesh.FileName}");
                            continue;
                        }

                        var addedModel = AddMeshGeometry(meshRef, indices, vertices);
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

        public void Render(Camera camera, MeshRenderMode renderMode)
        {
            var visibleMeshes = MeshModels.Where(x => x.Visible)
                .OrderByDescending(x=>x.IsSelected).ToList();

            if (!visibleMeshes.Any())
                return;

            RenderHelper.ModelShader.Use();
            RenderHelper.ModelShader.UseTexture.Set(renderMode != MeshRenderMode.Wireframe && Surface.SurfaceID > 0);

            bool useOutlineStencil = visibleMeshes.Any(x => x.IsSelected);
            if (useOutlineStencil)
                RenderHelper.EnableStencilTest();

            foreach (var model in visibleMeshes)
            {
                bool drawMeshOutilne = useOutlineStencil && model.IsSelected/* && renderMode != MeshRenderMode.Wireframe*/;
                RenderHelper.RenderWithStencil(drawMeshOutilne,
                    () =>
                    {
                        RenderPartialMesh(renderMode, model, Material, useOutlineStencil);
                    },
                    () =>
                    {
                        DrawModelOutline(model, 4f);
                    });
            }

            if (useOutlineStencil)
                RenderHelper.DisableStencilTest();

            foreach (var model in visibleMeshes)
            {
                if (model.IsSelected)
                {
                    var selectionBox = model.BoundingBox;
                    selectionBox.Size += new Vector3(0.1f);
                    RenderHelper.DrawBoundingBox(model.Transform, selectionBox, 
                        new Vector4(0f, 1f, 1f, 1f), 1.5f);
                }
            }
        }

        public void RenderPartialModel(SurfaceModelMesh surfaceModel)
        {

        }


        private void RenderPartialMesh(MeshRenderMode renderMode, SurfaceModelMesh model, MaterialInfo material, bool useStencil)
        {
            if (renderMode == MeshRenderMode.Wireframe)
            {
                GL.ColorMask(false, false, false, false);
                GL.DepthMask(false);
            }

            RenderHelper.BeginDrawModel(VertexBuffer, model.Transform, material);
            RenderHelper.ModelShader.IsSelected.Set(model.IsSelected);

            DrawModelElements(model);

            RenderHelper.EndDrawModel(VertexBuffer);

            if (useStencil)
                RenderHelper.DisableStencilMask();

            if (renderMode == MeshRenderMode.Wireframe)
            {
                GL.ColorMask(true, true, true, true);
                GL.DepthMask(true);
            }

            if (renderMode == MeshRenderMode.Wireframe || renderMode == MeshRenderMode.SolidWireframe)
            {
                var wireColor = model.IsSelected && renderMode == MeshRenderMode.SolidWireframe ? RenderHelper.WireframeColorAlt : RenderHelper.WireframeColor;

                RenderHelper.BeginDrawWireframe(VertexBuffer, model.Transform, 1f, wireColor);
                DrawModelElements(model);
                RenderHelper.EndDrawWireframe(VertexBuffer);
            }
        }

        private void DrawModelOutline(SurfaceModelMesh model, float thickness = 4f)
        {
            RenderHelper.BeginDrawWireframe(VertexBuffer, model.Transform, thickness, RenderHelper.SelectionOutlineColor);
            DrawModelElements(model);
            RenderHelper.EndDrawWireframe(VertexBuffer);
        }

        private void DrawModelElements(SurfaceModelMesh mesh)
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
