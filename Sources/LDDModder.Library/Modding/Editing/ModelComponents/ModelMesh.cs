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

namespace LDDModder.Modding.Editing
{
    public class ModelMesh : PartElement
    {
        public const string NODE_NAME = "Mesh";

        public MeshGeometry Geometry { get; set; }

        //public List<Vector3> Positions { get; set; }
        //public List<Vector3> Normals { get; set; }
        //public List<Vector2> UVs { get; set; }
        //public List<int> Indices { get; set; }
        //public List<Tuple<int,int,float>> BoneWeights { get; set; }
        //public List<Vector2> Outlines { get; set; }

        private string _FileName;
        private int FileFlag = 0;



        /// <summary>
        /// Filename used to "soft" delete the file.
        /// </summary>
        private string TempFileName;

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

        public string WorkingFilePath { get; private set; }

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
            WorkingFilePath = null;

            if (Project != null)
            {
                string newMeshFileName = Project.GenerateMeshFileName(newName);

                if (!string.IsNullOrEmpty(_FileName) && Project.FileExist(_FileName))
                {
                    string oldFilePath = Project.GetFileFullPath(_FileName);
                    try
                    {
                        string newFilePath = Project.GetFileFullPath(newMeshFileName);
                        File.Move(oldFilePath, newFilePath);
                        WorkingFilePath = newFilePath;
                        FileFlag = 1;
                    }
                    catch
                    {
                        Debug.WriteLine("Error: Could not rename mesh file!");

                        //WorkingFilePath = oldFilePath;
                        //FileFlag = 1;
                    }
                }

                _FileName = newMeshFileName;
            }
            else
                _FileName = null;
        }

        public bool CanRename(string newName)
        {
            string newMeshFileName = Project.GenerateMeshFileName(newName);
            return !Project.FileExist(newMeshFileName);
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);

            if (Project.FileVersion > 1)
            {
                if (!IsModelLoaded)
                    LoadModel();
                var xml = Geometry.ConvertToXml();
                elem.Add(xml.Root.Attributes().ToArray());
                elem.Add(xml.Root.Nodes().ToArray());
            }
            else
            {
                elem.Add(new XAttribute(nameof(IsTextured), IsTextured));
                elem.Add(new XAttribute(nameof(IsFlexible), IsFlexible));
                if (!string.IsNullOrEmpty(FileName))
                    elem.Add(new XAttribute(nameof(FileName), FileName));

            }

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

        public bool CheckFileExist()
        {
            if (Project != null && 
                Project.IsLoadedFromDisk && 
                !string.IsNullOrEmpty(FileName))
            {
                if (Project.FileExist(FileName))
                {
                    WorkingFilePath = Project.GetFileFullPath(FileName);
                    FileFlag = 1;
                }
                else if (Project.FileExist(Project.GenerateMeshFileName(ID)))
                {
                    try
                    {
                        var tmpFileName = Project.GetFileFullPath(Project.GenerateMeshFileName(ID));
                        WorkingFilePath = Project.GetFileFullPath(FileName);
                        File.Move(tmpFileName, WorkingFilePath);
                        FileFlag = 1;
                    }
                    catch
                    {
                        WorkingFilePath = null;
                        FileFlag = 3;
                    }
                    
                }
                else
                {
                    WorkingFilePath = null;
                    FileFlag = 2;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(TempFileName))
                    WorkingFilePath = null;
                FileFlag = 0;
            }


            return FileFlag == 1;
        }
    
        //public void SaveFile()
        //{
        //    if (Project?.IsLoadedFromDisk ?? false)
        //    {
        //        //bool wasLoaded = IsModelLoaded;
        //        if (!IsModelLoaded)
        //            LoadModel();

        //        var targetFilePath = Project.GetFileFullPath(FileName);
        //        Geometry.Save(targetFilePath);
        //        var test = Path.ChangeExtension(targetFilePath, ".xml");
        //        Geometry.SaveAsXml(test);
        //        CheckFileExist();
        //    }
        //}

        public void SaveGeometry()
        {
            if (Project?.IsLoadedFromDisk ?? false)
                SaveGeometry(Project.ProjectWorkingDir);
        }

        public void SaveGeometry(string filepath)
        {
            if (!IsModelLoaded)
                LoadModel();

            var targetFilePath = Path.Combine(filepath, FileName);
            Geometry.Save(targetFilePath);
            //var test = Path.ChangeExtension(targetFilePath, ".xml");
            //Geometry.SaveAsXml(test);
            CheckFileExist();
        }

        /// <summary>
        /// Soft delete
        /// </summary>
        internal void TempDeleteFile()
        {
            if (Project != null && CheckFileExist())
            {
                TempFileName = Project.GenerateMeshFileName(ID);
                TempFileName = Project.GetFileFullPath(TempFileName);
                
                try
                {
                    File.Move(WorkingFilePath, TempFileName);
                }
                catch
                {
                    Debug.WriteLine("Error: Could not rename mesh file!");
                }
            }
        }

        protected override void OnProjectAssigned()
        {
            base.OnProjectAssigned();

            if (Project != null && !string.IsNullOrEmpty(TempFileName))
            {
                if (string.IsNullOrEmpty(WorkingFilePath))
                    WorkingFilePath = Project.GetFileFullPath(FileName);

                File.Move(TempFileName, WorkingFilePath);

                TempFileName = null;
            }
            else if (Project == null && MeshFileExists && 
                string.IsNullOrEmpty(TempFileName))
            {
                TempDeleteFile();
            }
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
                if (LoadModel())
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
