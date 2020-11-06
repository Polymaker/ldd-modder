using LDDModder.Modding.Editing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace LDDModder.BrickEditor.Rendering
{
    public class SurfaceModelMesh : PartElementModel
    {
        public int StartIndex { get; set; }
        public int StartVertex { get; set; }
        public int IndexCount { get; set; }

        public SurfaceComponent Component => MeshReference.Parent as SurfaceComponent;

        public PartSurface Surface => Component?.Surface;

        public ModelMeshReference MeshReference { get; private set; }

        public SurfaceMeshBuffer SurfaceModel { get; private set; }

        public bool IsReplacementModel { get; set; }

        public SurfaceModelMesh(SurfaceMeshBuffer baseModel, ModelMeshReference meshRef, int startIndex, int indexCount, int startVertex) : base(meshRef)
        {
            MeshReference = meshRef;
            SurfaceModel = baseModel;
            StartIndex = startIndex;
            IndexCount = indexCount;
            StartVertex = startVertex;

            var baseTransform = meshRef.Transform.ToMatrix().ToGL();
            SetTransform(baseTransform, false);
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            return SurfaceModel.RayIntersects(ray, this, out distance);
        }

        public override void RenderModel(Camera camera, MeshRenderMode mode = MeshRenderMode.Solid)
        {
            RenderHelper.ModelShader.Use();
            RenderHelper.ModelShader.UseTexture.Set(mode != MeshRenderMode.Wireframe && Surface.SurfaceID > 0);

            RenderHelper.RenderWithStencil(IsSelected,
                () =>
                {
                    RenderPartialMesh(mode, this, SurfaceModel.Material);
                },
                () =>
                {
                    DrawWireframeModel(RenderHelper.SelectionOutlineColor, 4f);
                }
            );

            if (IsSelected && !BoundingBox.IsEmpty)
            {
                var selectionBox = BoundingBox;
                selectionBox.Size += new Vector3(0.1f);
                RenderHelper.DrawBoundingBox(Transform, 
                    selectionBox,
                    new Vector4(0f, 1f, 1f, 1f), 1.5f);
            }
        }

        private void RenderPartialMesh(MeshRenderMode renderMode, SurfaceModelMesh model, MaterialInfo material)
        {
            if (renderMode == MeshRenderMode.Wireframe && model.IsSelected)
            {
                //in wireframe mode, disable depth and color mask
                //but still write in stencil mask, this is needed for drawing the selection outline correctly
                GL.ColorMask(false, false, false, false);
                GL.DepthMask(false);
            }

            if (renderMode != MeshRenderMode.Wireframe || model.IsSelected)
                DrawSolidModel(material);

            if (model.IsSelected)
                RenderHelper.DisableStencilMask();

            if (renderMode == MeshRenderMode.Wireframe && model.IsSelected)
            {
                GL.ColorMask(true, true, true, true);
                GL.DepthMask(true);
            }

            if (renderMode == MeshRenderMode.Wireframe || renderMode == MeshRenderMode.SolidWireframe)
            {
                var wireColor = model.IsSelected && renderMode == MeshRenderMode.SolidWireframe ? RenderHelper.WireframeColorAlt : RenderHelper.WireframeColor;
                DrawWireframeModel(wireColor, 1f);
            }
        }

        private void DrawModelElements()
        {
            SurfaceModel.VertexBuffer.DrawElementsBaseVertex(PrimitiveType.Triangles,
                StartVertex,
                IndexCount,
                StartIndex * 4);
        }

        private void DrawWireframeModel(Vector4 color, float thickness)
        {
            RenderHelper.BeginDrawWireframe(SurfaceModel.VertexBuffer, Transform, thickness, color);
            DrawModelElements();
            RenderHelper.EndDrawWireframe(SurfaceModel.VertexBuffer);
        }

        private void DrawSolidModel(MaterialInfo material)
        {
            RenderHelper.BeginDrawModel(SurfaceModel.VertexBuffer, Transform, material);
            RenderHelper.ModelShader.IsSelected.Set(IsSelected);
            DrawModelElements();
            RenderHelper.EndDrawModel(SurfaceModel.VertexBuffer);
        }
    }
}
