using LDDModder.LDD.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class ModelMeshReference : PartElement
    {
        public const string NODE_NAME = "MeshRef";

        private ModelMesh _ModelMesh;

        public string MeshID { get; set; }

        public bool IsPartialMesh => IndexCount > 0;

        public int StartIndex { get; set; }
        public int IndexCount { get; set; }
        public int StartVertex { get; set; }
        public int VertexCount { get; set; }

        public ModelMesh ModelMesh => GetModelMesh();

        public bool IsTextured => GetModelMesh()?.IsTextured ?? false;

        public bool IsFlexible => GetModelMesh()?.IsFlexible ?? false;

        public ModelMeshReference()
        {

        }

        public ModelMeshReference(string meshID)
        {
            MeshID = meshID;
        }

        public ModelMeshReference(string meshID, int startIndex, int indexCount, int startVertex, int vertexCount)
        {
            MeshID = meshID;
            StartIndex = startIndex;
            IndexCount = indexCount;
            StartVertex = startVertex;
            VertexCount = vertexCount;
        }

        public ModelMeshReference(ModelMesh model, int startIndex, int indexCount, int startVertex, int vertexCount)
        {
            _ModelMesh = model;
            StartIndex = startIndex;
            IndexCount = indexCount;
            StartVertex = startVertex;
            VertexCount = vertexCount;
        }

        public ModelMeshReference(ModelMesh model, MeshCulling culling)
        {
            MeshID = model.ID;
            _ModelMesh = model;
            StartIndex = culling.FromIndex;
            IndexCount = culling.IndexCount;
            StartVertex = culling.FromVertex;
            VertexCount = culling.VertexCount;
        }

        public ModelMesh GetModelMesh()
        {
            if (_ModelMesh != null && string.IsNullOrEmpty(MeshID))
                MeshID = _ModelMesh.ID;

            if (_ModelMesh?.ID != MeshID)
                _ModelMesh = Project?.Meshes.FirstOrDefault(x => x.ID == MeshID);
            return _ModelMesh;
        }

        public MeshGeometry GetGeometry()
        {
            var modelMesh = GetModelMesh();

            if (modelMesh != null)
            {
                if (!modelMesh.IsModelLoaded)
                    modelMesh.LoadModel();

                if (modelMesh.Geometry != null)
                {
                    if (!IsPartialMesh)
                        return modelMesh.Geometry;

                    return modelMesh.Geometry.GetPartialGeometry(StartIndex, IndexCount, StartVertex, VertexCount);
                }
            }

            return null;
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(new XAttribute(nameof(MeshID), MeshID));

            if (IsPartialMesh)
            {
                elem.AddNumberAttribute(nameof(StartIndex ), StartIndex );
                elem.AddNumberAttribute(nameof(IndexCount ), IndexCount );
                elem.AddNumberAttribute(nameof(StartVertex), StartVertex);
                elem.AddNumberAttribute(nameof(VertexCount), VertexCount);
            }
            
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            MeshID = element.ReadAttribute(nameof(MeshID), string.Empty);
            StartIndex  = element.ReadAttribute(nameof(StartIndex ), 0);
            IndexCount  = element.ReadAttribute(nameof(IndexCount ), 0);
            StartVertex = element.ReadAttribute(nameof(StartVertex), 0);
            VertexCount = element.ReadAttribute(nameof(VertexCount), 0);
        }
    }
}
