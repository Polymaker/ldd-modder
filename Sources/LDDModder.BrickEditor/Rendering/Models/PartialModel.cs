using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Models
{
    public class PartialModel
    {
        public IVertexBuffer VertexBuffer { get; set; }
        public int StartIndex { get; set; }
        public int StartVertex { get; set; }
        public int IndexCount { get; set; }
        public PrimitiveType PrimitiveType { get; set; }
        public BBox BoundingBox { get; set; }

        public List<Vector3> Vertices { get; set; }

        public PartialModel(IVertexBuffer vertexBuffer, int startIndex, int startVertex, int indexCount, PrimitiveType primitiveType)
        {
            VertexBuffer = vertexBuffer;
            StartIndex = startIndex;
            StartVertex = startVertex;
            IndexCount = indexCount;
            PrimitiveType = primitiveType;
        }

        public void CalculateBoundingBox()
        {
            if (Vertices != null)
                BoundingBox = BBox.FromVertices(Vertices);
        }

        public void LoadVertices()
        {
            var indices = VertexBuffer.IndexBuffer.Content;
            Vertices = new List<Vector3>();
            for (int i = 0; i < IndexCount; i++)
            {
                var idx = indices[StartIndex + i];
                var vert = VertexBuffer.GetVertex(StartVertex + idx);
                Vertices.Add(vert);
            }
        }

        public bool RayIntersects(Ray ray, out float distance)
        {
            distance = float.NaN;

            if (Vertices != null)
            {
                for (int i = 0; i < Vertices.Count; i += 3)
                {
                    var v1 = Vertices[i + 0];
                    var v2 = Vertices[i + 1];
                    var v3 = Vertices[i + 2];
                    if (Ray.IntersectsTriangle(ray, v1, v2, v3, out float hitDist))
                        distance = float.IsNaN(distance) ? hitDist : Math.Min(hitDist, distance);
                }
            }

            return !float.IsNaN(distance);
        }

        public bool RayIntersects(Ray ray, Matrix4 transform, out float distance)
        {
            distance = float.NaN;

            if (Vertices != null)
            {
                for (int i = 0; i < Vertices.Count; i += 3)
                {
                    var v1 = Vector3.TransformPosition(Vertices[i + 0], transform);
                    var v2 = Vector3.TransformPosition(Vertices[i + 1], transform);
                    var v3 = Vector3.TransformPosition(Vertices[i + 2], transform);
                    if (Ray.IntersectsTriangle(ray, v1, v2, v3, out float hitDist))
                        distance = float.IsNaN(distance) ? hitDist : Math.Min(hitDist, distance);
                }
            }

            return !float.IsNaN(distance);
        }

        public bool RayIntersectsV2(Ray ray, Matrix4 transform, out float distance)
        {
            var localRay = Ray.Transform(ray, transform.Inverted());

            return RayIntersects(localRay, out distance);
        }

        public void DrawElements()
        {
            VertexBuffer.DrawElementsBaseVertex(PrimitiveType, StartVertex, IndexCount, StartIndex * 4);
        }

        public void DrawModel(Matrix4 transform, MaterialInfo material, bool isSelected)
        {
            RenderHelper.BeginDrawModel(VertexBuffer, transform, material);
            RenderHelper.ModelShader.IsSelected.Set(isSelected);
            DrawElements();
            RenderHelper.EndDrawModel(VertexBuffer);
        }

        public void DrawColored(Matrix4 transform, Vector4 color)
        {
            RenderHelper.BeginDrawColor(VertexBuffer, transform, color);
            DrawElements();
            RenderHelper.EndDrawColor(VertexBuffer);
        }

        public void DrawWireframe(Matrix4 transform, float thickness, Vector4 color)
        {
            RenderHelper.BeginDrawWireframe(VertexBuffer, transform, thickness, color);
            DrawElements();
            RenderHelper.EndDrawWireframe(VertexBuffer);
        }
    }
}
