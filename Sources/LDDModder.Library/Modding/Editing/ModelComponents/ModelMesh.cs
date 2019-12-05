using LDDModder.LDD.Meshes;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private string _FileName;
        private string _WorkingFilePath;
        private int FileFlag = 0; 

        #region Geometry Attributes

        public bool IsTextured { get; set; }

        public bool IsFlexible { get; set; }

        public int VertexCount { get; set; }

        public int IndexCount { get; set; }

        public int BoneCount { get; set; }

        #endregion

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_FileName) && Project != null && 
                    !string.IsNullOrEmpty(Name))
                {
                    _FileName = Project.GenerateMeshFileName(Name);
                }

                return _FileName;
            }
        }

        public string WorkingFilePath => _WorkingFilePath;

        public PartSurface Surface => (Parent as SurfaceComponent)?.Parent as PartSurface;

        public bool MeshFileExists
        {
            get
            {
                if (FileFlag == 0)
                    CheckFileExist();

                return FileFlag == 1;
            }
        }

        public bool IsModelLoaded => Geometry != null;

        public bool CanUnloadModel => MeshFileExists;

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

        protected override void OnAfterRename(string oldName, string newName)
        {
            base.OnAfterRename(oldName, newName);
            FileFlag = 0;
            _WorkingFilePath = null;

            if (Project != null)
            {
                string newMeshFileName = Project.GenerateMeshFileName(newName);

                if (!string.IsNullOrEmpty(_FileName) && Project.FileExist(_FileName))
                {
                    try
                    {
                        string newFilePath = Project.GetFileFullPath(newMeshFileName);
                        File.Move(Project.GetFileFullPath(_FileName), newFilePath);
                        _WorkingFilePath = newFilePath;
                        FileFlag = 1;
                    }
                    catch
                    {
                        Debug.WriteLine("Error: Could not rename mesh file!");
                    }
                }

                _FileName = newMeshFileName;
            }
            else
                _FileName = null;
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
            _FileName = element.ReadAttribute("FileName", string.Empty);
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

        public bool LoadModel()
        {
            if (Geometry == null && Project != null)
                Project.LoadModelMesh(this);
            return Geometry != null;
        }

        public void UnloadModel()
        {
            if (CanUnloadModel)
                Geometry = null;
        }

        public void CheckFileExist()
        {
            if (Project != null && !string.IsNullOrEmpty(FileName))
            {
                if (Project.FileExist(FileName))
                {
                    _WorkingFilePath = Project.GetFileFullPath(FileName);
                    FileFlag = 1;
                }
                else
                {
                    _WorkingFilePath = null;
                    FileFlag = 2;
                }
            }
        }
    }
}
