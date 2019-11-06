using LDDModder.LDD.Meshes;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    public class ModelMesh : PartElement
    {
        public const string NODE_NAME = "Mesh";

        public MeshGeometry Geometry { get; set; }

        #region Geometry Attributes

        public bool IsTextured { get; set; }

        public bool IsFlexible { get; set; }

        public int VertexCount { get; set; }

        public int IndexCount { get; set; }

        public int BoneCount { get; set; }

        #endregion



        public string FileName { get; set; }

        public string WorkingFilePath { get; set; }

        public PartSurface Surface => (Parent as SurfaceComponent)?.Parent as PartSurface;

        public bool IsModelLoaded => Geometry != null;

        public bool CanUnloadModel => !string.IsNullOrEmpty(WorkingFilePath);

        public ModelMesh()
        {
            ID = Utilities.StringUtils.GenerateUID(8);
        }

        public ModelMesh(MeshGeometry geometry)
        {
            Geometry = geometry;
            UpdateMeshProperties();
        }

        public IEnumerable<ModelMeshReference> GetReferences()
        {
            if (Project != null)
                return Project.Surfaces.SelectMany(x => x.GetAllMeshReferences()).Where(y => y.MeshID == ID);
            return Enumerable.Empty<ModelMeshReference>();
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(new XAttribute(nameof(IsTextured), IsTextured));
            elem.Add(new XAttribute(nameof(IsFlexible), IsFlexible));
            if (!string.IsNullOrEmpty(FileName))
                elem.Add(new XAttribute(nameof(FileName), FileName));

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            IsTextured = element.ReadAttribute("IsTextured", false);
            IsFlexible = element.ReadAttribute("IsFlexible", false);
            FileName = element.ReadAttribute("FileName", string.Empty);
        }

        public static ModelMesh FromXml(XElement element)
        {
            var model = new ModelMesh();
            model.LoadFromXml(element);
            return model;
        }

        #endregion


        public void UpdateMeshProperties()
        {
            bool wasLoaded = IsModelLoaded;

            if (LoadModel())
            {
                VertexCount = Geometry.VertexCount;
                IndexCount = Geometry.IndexCount;
                IsFlexible = Geometry.IsFlexible;
                IsTextured = Geometry.IsTextured;
                BoneCount = IsFlexible ? Geometry.Vertices.Max(x => x.BoneWeights.Max(y => y.BoneID)) : 0;
            }

            if (!wasLoaded)
                UnloadModel();
        }

        public bool LoadModel()
        {
            if (Geometry == null && Project != null)
            {
                Project.LoadModelMesh(this);
                UpdateMeshProperties();
            }
            return Geometry != null;
        }

        public void UnloadModel()
        {
            if (CanUnloadModel)
                Geometry = null;
        }
    }
}
