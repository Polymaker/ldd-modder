using LDDModder.BrickEditor.Rendering.Models;
using LDDModder.BrickEditor.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    static class ModelManager
    {
        public static IndexedVertexBuffer<VertVN> GeneralMeshBuffer;

        public static PartialModel CubeModel { get; private set; }

        public static PartialModel SphereModel { get; private set; }

        public static PartialModel CrossAxleMaleModel { get; private set; }

        public static PartialModel CrossAxleFemaleModel { get; private set; }

        public static PartialModel CylinderModel { get; private set; }

        public static void InitializeResources()
        {
            GeneralMeshBuffer = new IndexedVertexBuffer<VertVN>();

            var loadedMesh = ResourceHelper.GetResourceModel("Models.Cube.obj", "obj").Meshes[0];
            CubeModel = AppendPartialMesh(loadedMesh);

            loadedMesh = ResourceHelper.GetResourceModel("Models.Sphere.obj", "obj").Meshes[0];
            SphereModel = AppendPartialMesh(loadedMesh);

            loadedMesh = ResourceHelper.GetResourceModel("Models.Cylinder.obj", "obj").Meshes[0];
            CylinderModel = AppendPartialMesh(loadedMesh);

            loadedMesh = ResourceHelper.GetResourceModel("Models.CrossAxleMale.obj", "obj").Meshes[0];
            CrossAxleMaleModel = AppendPartialMesh(loadedMesh);

            loadedMesh = ResourceHelper.GetResourceModel("Models.CrossAxleFemale.obj", "obj").Meshes[0];
            CrossAxleFemaleModel = AppendPartialMesh(loadedMesh);
        }

        private static PartialModel AppendPartialMesh(Assimp.Mesh mesh)
        {
            var primitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
            if (mesh.Faces[0].IndexCount == 4)
                primitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Quads;

            int curIdx = GeneralMeshBuffer.IndexCount;
            int curVert = GeneralMeshBuffer.VertexCount;
            GeneralMeshBuffer.LoadModelVertices(mesh, true);
            int idxCount = GeneralMeshBuffer.IndexCount - curIdx;
            var vertices = GeneralMeshBuffer.VertexBuffer.Content.Skip(curVert);

            var bounding = BBox.FromVertices(vertices.Select(x => x.Position));

            var model = new PartialModel(GeneralMeshBuffer, curIdx, curVert, idxCount, primitiveType);

            model.LoadVertices();
            model.CalculateBoundingBox();
            return model;
        }

        public static void ReleaseResources()
        {
            if (GeneralMeshBuffer != null)
            {
                GeneralMeshBuffer.Dispose();
                GeneralMeshBuffer = null;
            }

            CubeModel = null;
            SphereModel = null;
            CrossAxleMaleModel = null;
            CrossAxleFemaleModel = null;
            CylinderModel = null;
        }
    }
}
