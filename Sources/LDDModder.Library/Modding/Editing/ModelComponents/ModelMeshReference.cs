using LDDModder.LDD.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class ModelMeshReference : PartElement, IPhysicalElement
    {
        public const string NODE_NAME = "MeshRef";

        private ModelMesh _ModelMesh;
        private ItemTransform _Transform;

        public string MeshID { get; set; }

        public bool IsPartialMesh => IndexCount > 0;

        public int StartIndex { get; set; }
        public int IndexCount { get; set; }
        public int StartVertex { get; set; }
        public int VertexCount { get; set; }

        public ItemTransform Transform
        {
            get => _Transform;
            set => SetPropertyValue(ref _Transform, value);
        }

        public ModelMesh ModelMesh => GetModelMesh();

        public bool IsTextured => GetModelMesh()?.IsTextured ?? false;

        public bool IsFlexible => GetModelMesh()?.IsFlexible ?? false;

        public int TriangleCount => (IndexCount > 0) ? IndexCount / 3 : (ModelMesh?.IndexCount ?? 0) / 3;

        public ModelMeshReference()
        {
            _Transform = new ItemTransform();
        }

        public ModelMeshReference(string meshID)
        {
            MeshID = meshID;
            _Transform = new ItemTransform();
        }

        public ModelMeshReference(string meshID, int startIndex, int indexCount, int startVertex, int vertexCount)
        {
            MeshID = meshID;
            StartIndex = startIndex;
            IndexCount = indexCount;
            StartVertex = startVertex;
            VertexCount = vertexCount;
            _Transform = new ItemTransform();
        }

        public ModelMeshReference(ModelMesh model, int startIndex, int indexCount, int startVertex, int vertexCount)
        {
            _ModelMesh = model;
            StartIndex = startIndex;
            IndexCount = indexCount;
            StartVertex = startVertex;
            VertexCount = vertexCount;
            _Transform = new ItemTransform();
        }

        public ModelMeshReference(ModelMesh model, MeshCulling culling)
        {
            MeshID = model.ID;
            _ModelMesh = model;
            StartIndex = culling.FromIndex;
            IndexCount = culling.IndexCount;
            StartVertex = culling.FromVertex;
            VertexCount = culling.VertexCount;
            _Transform = new ItemTransform();
        }

        public ModelMeshReference(ModelMesh model)
        {
            MeshID = model.ID;
            _ModelMesh = model;
            _Transform = new ItemTransform();
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
            MeshGeometry modelGeometry = null;

            if (modelMesh != null)
            {
                if (!modelMesh.IsModelLoaded)
                    modelMesh.LoadModel();

                if (modelMesh.Geometry != null)
                {
                    if (IsPartialMesh)
                        modelGeometry = modelMesh.Geometry.GetPartialGeometry(StartIndex, IndexCount, StartVertex, VertexCount);
                    else if (!Transform.IsEmpty)
                        modelGeometry = modelMesh.Geometry.Clone();
                    else
                        modelGeometry = modelMesh.Geometry;

                    if (!Transform.IsEmpty)
                    {
                        modelGeometry.TransformVertices(Transform.ToMatrix());
                    }
                }
            }

            return modelGeometry;
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(new XAttribute(nameof(MeshID), MeshID));

            if (!Transform.IsEmpty)
                elem.Add(Transform.SerializeToXml(nameof(Transform)));

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

            if (element.HasElement(nameof(Transform), out XElement transElem))
                Transform = ItemTransform.FromXml(transElem);
            else
                Transform = new ItemTransform();
        }
    }
}
