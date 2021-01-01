using LDDModder.LDD.Meshes;
using LDDModder.Serialization;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding
{
    public class ModelMesh : PartElement
    {
        public const string NODE_NAME = "Mesh";
        private MeshGeometry _Geometry;

        public MeshGeometry Geometry
        {
            get => _Geometry;
            set => SetGeometry(value);
        }

        public bool GeometrySaved { get; private set; }

        #region Geometry Attributes

        public bool IsTextured { get; set; }

        public bool IsFlexible { get; set; }

        public int VertexCount { get; set; }

        public int IndexCount { get; set; }

        public int BoneCount { get; set; }

        #endregion

        public string LegacyFilename { get; set; }

        public PartSurface Surface => (Parent as SurfaceComponent)?.Parent as PartSurface;

        public bool IsModelLoaded => Geometry != null;

        public bool CanUnloadModel => GeometrySaved;

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

        private void SetGeometry(MeshGeometry geometry)
        {
            if (_Geometry != geometry)
            {
                _Geometry = geometry;
                if (geometry != null)
                    GeometrySaved = false;
                UpdateMeshProperties();
            }
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);

            XElement geomElem = null;

            if (IsModelLoaded)
            {
                geomElem = Geometry.ConvertToXml().Root;
            }
            else if (GeometrySaved)
            {
                geomElem = GetGeometryElementFromProjectFile();
                return geomElem;
            }

            if (geomElem != null)
            {
                elem.Add(geomElem.Nodes().ToArray());
                elem.Add(geomElem.Attributes().ToArray());
                
            }
            else
            {
                Trace.WriteLine("ERROR Geometry not loaded!");
            }
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            IsTextured = element.ReadAttribute("IsTextured", false);
            IsFlexible = element.ReadAttribute("IsFlexible", false);
            LegacyFilename = element.ReadAttribute("FileName", string.Empty);

            if (element.HasElement("Positions"))
            {
                _Geometry = GetGeometryFromElement(element);
                GeometrySaved = _Geometry != null;
            }
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
            if (Geometry != null)
            {
                VertexCount = Geometry.VertexCount;
                IndexCount = Geometry.IndexCount;
                IsFlexible = Geometry.IsFlexible;
                IsTextured = Geometry.IsTextured;
                BoneCount = IsFlexible ? Geometry.Vertices.Max(x => x.BoneWeights.Max(y => y.BoneID)) : 0;
            }
        }

        public bool ReloadModelFromXml()
        {
            var geomElem = GetGeometryElementFromProjectFile();
            if (geomElem != null)
            {
                _Geometry = GetGeometryFromElement(geomElem);
                GeometrySaved = _Geometry != null;
            }

            //if (Project == null)
            //    return false;

            //var projectXml = Project.GetProjectXml();
            //var meshElem = projectXml.Descendants(NODE_NAME)
            //    .FirstOrDefault(e => e.ReadAttribute("ID", string.Empty) == ID);
            //if (meshElem != null)
            //    LoadGeometry(meshElem);

            ////if (Geometry == null && Project != null)
            ////    Project.LoadModelMesh(this);
            return Geometry != null;
        }

        public XElement GetGeometryElementFromProjectFile()
        {
            var projectXml = Project?.GetProjectXml();
            if (projectXml != null)
            {
                return projectXml.Descendants(NODE_NAME)
                    .FirstOrDefault(e => e.ReadAttribute("ID", string.Empty) == ID);
            }

            return null;
        }

        private MeshGeometry GetGeometryFromElement(XElement element)
        {
            var fakeDoc = new XDocument();

            fakeDoc.Add(new XElement("LddGeometry", element.Nodes().ToArray()));
            fakeDoc.Root.Add(element.Attributes().ToArray());
            return MeshGeometry.FromXml(fakeDoc); ;
        }

        public void UnloadModel()
        {
            if (CanUnloadModel && GeometrySaved)
            {
                _Geometry = null;
            }
        }
        internal void MarkSaved(bool value)
        {
            GeometrySaved = value;
        }

        public override List<ValidationMessage> ValidateElement()
        {
            var messages = new List<ValidationMessage>();

            void AddMessage(string code, ValidationLevel level, params object[] args)
            {
                messages.Add(new ValidationMessage(this, code, level)
                {
                    MessageArguments = args
                });
            }

            if (IsFlexible)
            {
                bool modelLoaded = IsModelLoaded;
                if (ReloadModelFromXml())
                {
                    var meshBoneIDs = Geometry.Vertices.SelectMany(x => x.BoneWeights.Select(b => b.BoneID)).Distinct();
                    var existingBones = Project.Bones.Select(x => x.BoneID).Distinct();

                    var missingBones = meshBoneIDs.Except(existingBones).ToList();

                    if (missingBones.Any())
                        AddMessage("MESH_MISSING_BONES", ValidationLevel.Error, missingBones);

                    if (!modelLoaded)
                        UnloadModel();
                }
            }

            return messages;
        }
    }
}
