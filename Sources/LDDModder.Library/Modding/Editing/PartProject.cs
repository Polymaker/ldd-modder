using ICSharpCode.SharpZipLib.Zip;
using LDDModder.LDD.Data;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Serialization;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    [XmlRoot("LDDPart")]
    public class PartProject
    {
        public const string ProjectFileName = "project.xml";

        #region Part Info

        public PartProperties Properties { get; }

        public int PartID
        {
            get => Properties.PartID;
            set => Properties.PartID = value;
        }

        public string PartDescription
        {
            get => Properties.Description;
            set => Properties.Description = value;
        }

        public List<int> Aliases => Properties.Aliases;

        public Platform Platform
        {
            get => Properties.Platform;
            set => Properties.Platform = value;
        }

        public MainGroup MainGroup
        {
            get => Properties.MainGroup;
            set => Properties.MainGroup = value;
        }

        public PhysicsAttributes PhysicsAttributes
        {
            get => Properties.PhysicsAttributes;
            set => Properties.PhysicsAttributes = value;
        }

        public BoundingBox Bounding
        {
            get => Properties.Bounding;
            set => Properties.Bounding = value;
        }

        public BoundingBox GeometryBounding
        {
            get => Properties.GeometryBounding;
            set => Properties.GeometryBounding = value;
        }

        public ItemTransform DefaultOrientation
        {
            get => Properties.DefaultOrientation;
            set => Properties.DefaultOrientation = value;
        }

        public Camera DefaultCamera
        {
            get => Properties.DefaultCamera;
            set => Properties.DefaultCamera = value;
        }

        public VersionInfo PrimitiveFileVersion
        {
            get => Properties.PrimitiveFileVersion;
            set => Properties.PrimitiveFileVersion = value;
        }

        public int PartVersion
        {
            get => Properties.PartVersion;
            set => Properties.PartVersion = value;
        }

        public bool Decorated
        {
            get => Properties.Decorated;
            set => Properties.Decorated = value;
        }

        public bool Flexible
        {
            get => Properties.Flexible;
            set => Properties.Flexible = value;
        }

        #endregion

        #region Element Collections

        [XmlArray("ModelSurfaces")]
        public ElementCollection<PartSurface> Surfaces { get; }

        [XmlArray("Connections")]
        public ElementCollection<PartConnection> Connections { get; }

        [XmlArray("Collisions")]
        public ElementCollection<PartCollision> Collisions { get; }

        [XmlArray("Bones")]
        public ElementCollection<PartBone> Bones { get; }

        [XmlArray("Meshes")]
        public ElementCollection<ModelMesh> Meshes { get; }

        #endregion

        #region Project File Properties

        public string ProjectPath { get; set; }

        public string ProjectWorkingDir { get; set; }

        public bool IsLoadedFromDisk => !string.IsNullOrEmpty(ProjectWorkingDir);

        #endregion

        #region Statistics properties

        public int TotalVertices { get; private set; }

        public int TotalTriangles { get; private set; }

        #endregion

        public Dictionary<string, string> ProjectProperties { get; set; }

        [XmlIgnore]
        public bool IsLoading { get; internal set; }

        #region Events

        public event EventHandler<ElementValueChangedEventArgs> ElementPropertyChanged;

        public event EventHandler<ElementCollectionChangedEventArgs> ElementCollectionChanged;

        #endregion

        public PartProject()
        {
            Surfaces = new ElementCollection<PartSurface>(this);
            Connections = new ElementCollection<PartConnection>(this);
            Collisions = new ElementCollection<PartCollision>(this);
            Bones = new ElementCollection<PartBone>(this);
            Meshes = new ElementCollection<ModelMesh>(this);

            Properties = new PartProperties(this);

            ProjectProperties = new Dictionary<string, string>();
        }

        #region Creation From LDD

        public static PartProject CreateFromLddPart(int partID)
        {
            return CreateFromLddPart(LDD.LDDEnvironment.Current, partID);
        }

        public static PartProject CreateFromLddPart(LDD.LDDEnvironment environment, int partID)
        {
            var lddPart = LDD.Parts.PartWrapper.LoadPart(environment, partID, true);
            return CreateFromLddPart(lddPart);
        }

        public static PartProject CreateFromLddPart(LDD.Parts.PartWrapper lddPart)
        {
            int partID = lddPart.PartID;

            var project = new PartProject()
            {
                IsLoading = true
            };
            project.Properties.Initialize(lddPart);

            int elementIndex = 0;
            foreach (var collision in lddPart.Primitive.Collisions)
            {
                var collisionElem = PartCollision.FromLDD(collision);
                collisionElem.ID = StringUtils.GenerateUUID($"Part{partID}_Collision{elementIndex++}", 8);
                project.Collisions.Add(collisionElem);
            }

            elementIndex = 0;
            foreach (var lddConn in lddPart.Primitive.Connectors)
            {
                var connectionElem = PartConnection.FromLDD(lddConn);
                connectionElem.ID = StringUtils.GenerateUUID($"Part{partID}_Connection{elementIndex++}", 8);
                project.Connections.Add(connectionElem);
            }

            elementIndex = 0;

            foreach (var flexBone in lddPart.Primitive.FlexBones)
            {
                var boneElement = new PartBone(flexBone.ID);
                boneElement.InternalSetID(
                    StringUtils.GenerateUUID($"Part{partID}_Bone{elementIndex++}", 8));

                project.Bones.Add(boneElement);
                boneElement.LoadFromLDD(flexBone);
            }

            foreach (var meshSurf in lddPart.Surfaces)
            {
                int surfaceID = meshSurf.SurfaceID;

                var surfaceElement = new PartSurface(
                    meshSurf.SurfaceID,
                    lddPart.Primitive.GetSurfaceMaterialIndex(meshSurf.SurfaceID)
                );
                surfaceElement.ID = StringUtils.GenerateUUID($"Surface{surfaceID}", 8);
                //surfaceElement.ID = StringUtils.GenerateUUID($"Part{partID}_Surface{surfaceElement.SurfaceID}", 8);
                project.Surfaces.Add(surfaceElement);

                var surfaceMesh = project.AddMeshGeometry(
                    meshSurf.Mesh.Geometry,
                    StringUtils.GenerateUUID($"P{partID}_S{surfaceID}", 8),
                    $"Surface{surfaceElement.SurfaceID}.Mesh"
                );

                elementIndex = 0;

                foreach (var culling in meshSurf.Mesh.Cullings)
                {
                    var cullingComp = SurfaceComponent.CreateFromLDD(culling, surfaceMesh);
                    cullingComp.InternalSetID(
                        StringUtils.GenerateUUID(
                            $"P{partID}_S{surfaceID}_C{elementIndex}", 8));
                    project.GenerateElementName(cullingComp);

                    if (culling.ReplacementMesh != null)
                    {
                        var replacementMesh = project.AddMeshGeometry(
                            culling.ReplacementMesh,
                            StringUtils.GenerateUUID($"P{partID}_S{surfaceID}_C{elementIndex}_Alt", 8),
                            cullingComp.Name + "_Alt"
                        );

                        if (cullingComp is FemaleStudModel femaleStud)
                        {
                            femaleStud.ReplacementMeshes
                                .Add(new ModelMeshReference(replacementMesh));
                        }
                        else// if (cullingComp is BrickTubeModel brickTube)
                        {
                            Debug.WriteLine($"{cullingComp.ComponentType} has a replacement mesh!!!");

                            //cullingComp.Meshes
                            //    .Add(new ModelMeshReference(replacementMesh));
                        }
                    }

                    surfaceElement.Components.Add(cullingComp);
                    elementIndex++;
                }
            }

            project.GenerateElementIDs(true);
            project.GenerateElementsNames();
            project.LinkBonesAndStudReferences();
            project.IsLoading = false;
            return project;
        }

        #endregion

        public static PartProject CreateEmptyProject()
        {
            var project = new PartProject();
            //project.IsLoading = true;
            project.Surfaces.Add(new PartSurface(0, 0));
            //project.IsLoading = false;
            return project;
        }

        public static PartProject CreateFromXml(XDocument doc)
        {
            var project = new PartProject();
            project.LoadFromXml(doc);
            return project;
        }

        #region Read/Write Xml structure

        public XDocument GenerateProjectXml()
        {
            var doc = new XDocument(new XElement("LDDPART"));

            doc.Root.Add(Properties.SerializeToXml());
            
            var surfacesElem = doc.Root.AddElement("ModelSurfaces");
            foreach (var surf in Surfaces)
                surfacesElem.Add(surf.SerializeToXml());

            var collisionsElem = doc.Root.AddElement(nameof(Collisions));
            foreach (var col in Collisions)
                collisionsElem.Add(col.SerializeToXml());

            var connectionsElem = doc.Root.AddElement(nameof(Connections));
            foreach (var conn in Connections)
                connectionsElem.Add(conn.SerializeToXml());

            if (Bones.Any())
            {
                var bonesElem = doc.Root.AddElement(nameof(Bones));
                foreach (var bone in Bones)
                    bonesElem.Add(bone.SerializeToXml());
            }

            var meshesElem = doc.Root.AddElement(nameof(Meshes));

            foreach(var mesh in Meshes)
            {
                if (mesh.VertexCount > 0)
                    meshesElem.Add(new XComment($"Vertex count: {mesh.VertexCount} Triangle count: {mesh.IndexCount / 3}"));
                meshesElem.Add(mesh.SerializeToXml());
            }

            if (ProjectProperties.Any())
            {
                var elem = doc.Root.AddElement("ProjectProperties");
                foreach (var kv in ProjectProperties)
                    elem.Add(new XElement(kv.Key, kv.Value));
            }

            return doc;
        }

        private void LoadFromXml(XDocument doc)
        {
            IsLoading = true;

            Surfaces.Clear();
            Connections.Clear();
            Collisions.Clear();
            Bones.Clear();
            Aliases.Clear();
            
            var rootElem = doc.Root;

            if (rootElem.HasElement("Properties", out XElement propsElem))
                Properties.LoadFromXml(propsElem);

            var surfacesElem = doc.Root.Element("ModelSurfaces");

            if (surfacesElem != null)
            {
                foreach (var surfElem in surfacesElem.Elements(PartSurface.NODE_NAME))
                    Surfaces.Add(PartSurface.FromXml(surfElem));
            }

            if (rootElem.HasElement(nameof(Connections), out XElement connectionsElem))
            {
                foreach (var connElem in connectionsElem.Elements(PartConnection.NODE_NAME))
                    Connections.Add(PartConnection.FromXml(connElem));
            }

            if (rootElem.HasElement(nameof(Collisions), out XElement collisionsElem))
            {
                foreach (var connElem in collisionsElem.Elements(PartCollision.NODE_NAME))
                    Collisions.Add(PartCollision.FromXml(connElem));
            }

            if (rootElem.HasElement(nameof(Bones), out XElement bonesElem))
            {
                foreach (var boneElem in bonesElem.Elements(PartBone.NODE_NAME))
                    Bones.Add(PartBone.FromXml(boneElem));
            }

            Properties.Flexible = Bones.Any() || Properties.Flexible;

            if (rootElem.HasElement(nameof(Meshes), out XElement meshesElem))
            {
                foreach (var meshElem in meshesElem.Elements(ModelMesh.NODE_NAME))
                    Meshes.Add(ModelMesh.FromXml(meshElem));
            }

            if (rootElem.HasElement("ProjectProperties", out XElement pojectProps))
            {
                foreach (var propElem in pojectProps.Elements())
                    ProjectProperties.Add(propElem.Name.LocalName, propElem.Value);
            }

            LinkBonesAndStudReferences();

            IsLoading = false;
        }

        #endregion

        #region Read/Write from Directory

        public void SaveExtracted(string directory, bool setWorkingDir = true)
        {
            directory = Path.GetFullPath(directory);
            Directory.CreateDirectory(directory);
            if (setWorkingDir)
                ProjectWorkingDir = directory;

            var projectXml = GenerateProjectXml();
            projectXml.Save(Path.Combine(directory, ProjectFileName));

            string meshDir = Path.Combine(directory, "Meshes");
            Directory.CreateDirectory(meshDir);

            foreach (var mesh in Meshes)
            {
                string meshPath = Path.Combine(directory, mesh.FileName);
                mesh.Geometry.Save(meshPath);
            }
        }

        public static PartProject LoadFromDirectory(string directory)
        {
            var doc = XDocument.Load(Path.Combine(directory, ProjectFileName));
            var project = new PartProject
            {
                ProjectWorkingDir = directory
            };
            project.LoadFromXml(doc);
            project.CheckFiles();
            return project;
        }

        #endregion

        #region Read/Write from zip

        public void Save(string filename)
        {
            using (var fs = File.Open(filename, FileMode.Create))
            using (var zipStream = new ZipOutputStream(fs))
            {
                zipStream.SetLevel(1);
                
                zipStream.PutNextEntry(new ZipEntry(ProjectFileName));

                var projectXml = GenerateProjectXml();

                projectXml.Save(zipStream);

                zipStream.CloseEntry();

                foreach (var mesh in Meshes)
                {
                    if (mesh.MeshFileExists)
                    {
                        zipStream.PutNextEntry(new ZipEntry(mesh.FileName));
                        using (var meshFs = File.OpenRead(mesh.WorkingFilePath))
                            meshFs.CopyTo(zipStream);
                    }
                    else if (mesh.IsModelLoaded)
                    {
                        using (var ms = new MemoryStream())
                        {
                            //if (!mesh.IsModelLoaded && mesh.fi)
                            mesh.Geometry.Save(ms);
                            ms.Position = 0;
                            zipStream.PutNextEntry(new ZipEntry(mesh.FileName));
                            ms.CopyTo(zipStream);
                            zipStream.CloseEntry();
                        }
                    }
                }
            }
            
            ProjectPath = filename;
        }

        public static PartProject ExtractAndOpen(Stream stream, string targetPath)
        {
            using (var zipFile = new ZipFile(stream))
            {
                if (zipFile.GetEntry(ProjectFileName) == null)
                    return null;

                foreach (ZipEntry entry in zipFile)
                {
                    if (!entry.IsFile)
                        continue;
                    string fullPath = Path.Combine(targetPath, entry.Name);
                    string dirName = Path.GetDirectoryName(fullPath);
                    if (dirName.Length > 0)
                        Directory.CreateDirectory(dirName);

                    var buffer = new byte[4096];

                    using (var zipStream = zipFile.GetInputStream(entry))
                    using (Stream fsOutput = File.Create(fullPath))
                    {
                        zipStream.CopyTo(fsOutput, 4096);
                    }
                }
            }

            string projectFilePath = Path.Combine(targetPath, ProjectFileName);

            var projectXml = XDocument.Load(projectFilePath);
            var project = new PartProject()
            {
                ProjectWorkingDir = targetPath
            };
            project.LoadFromXml(projectXml);
            project.CheckFiles();
            return project;
        }

        #endregion

        #region Elements methods

        public IEnumerable<PartElement> GetAllElements()
        {
            IEnumerable<PartElement> elems = Surfaces;
            elems = elems.Concat(Connections)
                .Concat(Collisions)
                .Concat(Bones)
                .Concat(Meshes);

            foreach (var elem in elems)
            {
                yield return elem;
                foreach (var child in elem.GetChildsHierarchy())
                    yield return child;
            }
        }

        public IEnumerable<T> GetAllElements<T>() where T : PartElement
        {
            return GetAllElements().OfType<T>();
        }

        public IEnumerable<PartElement> GetAllElements(Predicate<PartElement> predicate)
        {
            return GetAllElements().Where(x => predicate(x));
        }

        public IEnumerable<T> GetAllElements<T>(Predicate<T> predicate) where T : PartElement
        {
            return GetAllElements().OfType<T>().Where(x => predicate(x));
        }

        private void CheckFiles()
        {
            foreach (var mesh in Meshes)
                mesh.CheckFileExist();
        }

        public ModelMesh AddMeshGeometry(MeshGeometry geometry, string name = null)
        {
            ModelMesh modelMesh = AddMeshGeometry(geometry, null, name);

            if (IsLoadedFromDisk)
            {
                var targetFilePath = GetFileFullPath(modelMesh.FileName);
                geometry.Save(targetFilePath);
                modelMesh.CheckFileExist();
            }

            return modelMesh;
        }

        private ModelMesh AddMeshGeometry(MeshGeometry geometry, string id, string name = null)
        {
            var modelMesh = new ModelMesh(geometry);
            
            modelMesh.IsInitializing = true;

            if (string.IsNullOrEmpty(id))
                GenerateElementID(modelMesh);
            else
                modelMesh.InternalSetID(id);

            if (string.IsNullOrEmpty(name))
                GenerateElementName(modelMesh);
            else
                RenameElement(modelMesh, name);

            Meshes.Add(modelMesh);
            modelMesh.IsInitializing = false;
            return modelMesh;
        }

        public PartSurface AddSurface()
        {
            var newSurface = new PartSurface(Surfaces.Count, 
                Surfaces.Any() ? Surfaces.Max(x=>x.SubMaterialIndex) + 1 : 0);
            Surfaces.Add(newSurface);
            return newSurface;
        }

        #endregion

        #region Element ID Handling

        private void GenerateElementIDs(bool deterministicIDs)
        {
            var allElements = GetAllElements().ToList();
            int elemCount = 0;

            foreach (var elem in allElements)
            {
                string elementID = elem.ID;
                while (string.IsNullOrEmpty(elementID) ||
                    allElements.Any(x => x.ID == elementID && x != elem))
                {
                    if (deterministicIDs)
                        elementID = StringUtils.GenerateUUID($"{PartID}_{elemCount++}", 8);
                    else
                        elementID = StringUtils.GenerateUID(8);
                }
                elem.ID = elementID;
            }
        }

        private void GenerateElementID(PartElement element)
        {
            var existingIDs = GetAllElements().Where(x => x != element && !string.IsNullOrEmpty(x.ID)).Select(x => x.ID);
            string elementID = element.ID;
            while (string.IsNullOrEmpty(elementID) || existingIDs.Contains(elementID))
                elementID = StringUtils.GenerateUID(8);
            element.ID = elementID;
        }

        private void GenerateElementIDs(IEnumerable<PartElement> elements)
        {
            var existingIDs = GetAllElements()
                .Where(x => !elements.Contains(x) && !string.IsNullOrEmpty(x.ID))
                .Select(x => x.ID)
                .ToList();

            foreach (var elem in elements)
            {
                string elementID = elem.ID;
                while (string.IsNullOrEmpty(elementID) || existingIDs.Contains(elementID))
                    elementID = StringUtils.GenerateUID(8);
                elem.ID = elementID;
                existingIDs.Add(elementID);
            }
        }

        #endregion

        #region Elements Name Handling

        private void GenerateElementsNames()
        {
            GenerateAllElementNames(GetAllElements<PartSurface>());
            GenerateAllElementNames(GetAllElements<ModelMesh>());
            GenerateAllElementNames(GetAllElements<ModelMeshReference>());
            GenerateAllElementNames(GetAllElements<SurfaceComponent>());
            GenerateAllElementNames(GetAllElements<PartConnection>());
            GenerateAllElementNames(GetAllElements<PartCollision>());
        }

        private void GenerateAllElementNames(IEnumerable<PartElement> allElements)
        {
            foreach (var elemByType in allElements.GroupBy(x => x.GetFullElementType()))
            {
                var elemList = elemByType.ToList();
                int nameCount = elemList.Count(x => !string.IsNullOrEmpty(x.Name));

                foreach(var element in elemList)
                {

                    string elementName = element.Name;

                    while (string.IsNullOrEmpty(elementName) ||
                                elemList.Any(x => x.Name == elementName && x != element))
                    {
                        elementName = GenerateElementName(element, nameCount++);
                        if (elementName == null)
                            break;
                    }

                    element.InternalSetName(elementName, true);
                }
            }
        }

        private void GenerateElementsNames(IEnumerable<PartElement> elements)
        {
            foreach (var elemByType in elements.GroupBy(x => x.GetFullElementType()))
            {
                var allElems = GetAllElements().Where(x => x.GetFullElementType() == elemByType.Key);

                var elemList = elemByType.ToList();
                int nameCount = allElems.Count(x => !string.IsNullOrEmpty(x.Name));

                foreach (var element in elemList)
                {

                    string elementName = element.Name;

                    while (string.IsNullOrEmpty(elementName) ||
                                allElems.Any(x => x.Name == elementName && x != element))
                    {
                        elementName = GenerateElementName(element, nameCount++);
                        if (elementName == null)
                            break;
                    }

                    element.InternalSetName(elementName, true);
                }
            }
        }

        public void GenerateElementName(PartElement element)
        {
            var elemList = GetAllElements().Where(x => x.GetFullElementType() == element.GetFullElementType()).ToList();
            int nameCount = elemList.Count(x => !string.IsNullOrEmpty(x.Name));

            string elementName = element.Name;

            if (element is ModelMesh && !string.IsNullOrEmpty(elementName)
                && !Regex.IsMatch(elementName, "^Mesh\\d+$"))
            {
                string meshName = elementName;
                nameCount = 1;

                while (string.IsNullOrEmpty(elementName) ||
                        elemList.Any(x => x.Name == elementName && x != element))
                {
                    elementName = $"{meshName}_{nameCount++}";
                }
            }
            else
            {
                while (string.IsNullOrEmpty(elementName) ||
                        elemList.Any(x => x.Name == elementName && x != element))
                {
                    elementName = GenerateElementName(element, nameCount++);
                    if (elementName == null)
                        break;
                }
            }

            element.InternalSetName(elementName, true);
        }

        private string GenerateElementName(PartElement element, int number)
        {
            if (element is SurfaceComponent component)
            {
                return $"{component.ComponentType}{number}";
            }
            else if (element is ModelMesh)
            {
                
                return $"Mesh{number}";
            }
            else if (element is PartConnection connection)
            {
                return $"{connection.ConnectorType}{number}";
            }
            else if (element is PartCollision collision)
            {
                return $"{collision.CollisionType}{number}";
            }
            else if (element is ModelMeshReference meshReference)
            {
                if (meshReference.ModelMesh != null)
                {
                    if (string.IsNullOrEmpty(meshReference.ModelMesh.Name))
                        GenerateElementName(meshReference.ModelMesh);

                    var allRefs = meshReference.ModelMesh.GetReferences();
                    
                    if (allRefs.Count() == 1)
                    {
                        return $"{meshReference.ModelMesh.Name}";
                    }
                    else
                    {
                        int refCount = meshReference.ModelMesh.GetReferences().Count(x => !string.IsNullOrEmpty(x.Name));
                        return $"{meshReference.ModelMesh.Name}_{refCount}";
                    }
                }

                return $"MeshRef{number}";
            }
            return null;
        }

        public string RenameElement(PartElement element, string suggestedName)
        {
            var elemList = GetAllElements().Where(x => x.GetFullElementType() == element.GetFullElementType()).ToList();
            int nameCount = 1;
            string elementName = suggestedName;

            while (elemList.Any(x => x.Name == elementName && x != element))
                elementName = $"{suggestedName}_{nameCount++}";

            element.InternalSetName(elementName, true);

            if (element is ModelMeshReference meshRef && meshRef.ModelMesh != null)
            {
                if (meshRef.ModelMesh.GetReferences().Count() == 1 && 
                    !Meshes.Any(x => x.Name == elementName))
                {
                    if (meshRef.ModelMesh.CanRename(elementName))
                        meshRef.ModelMesh.InternalSetName(elementName, true);
                }
            }

            return elementName;
        }

        #endregion

        #region Methods

        private void RenumberSurfaces()
        {
            int surfaceID = 0;

            foreach (var surf in Surfaces)
            {
                surf.SurfaceID = surfaceID;
                surf.InternalSetName($"Surface{surfaceID}");
                surfaceID++;
            }
        }

        private void UpdateStudReferencesIndices()
        {
            foreach (var studRef in GetAllElements<StudReference>())
            {
                PartConnection linkedConnection = null;

                if (!string.IsNullOrEmpty(studRef.ConnectionID))
                {
                    linkedConnection = Connections
                        .FirstOrDefault(x => x.ID == studRef.ConnectionID);
                }

                if (linkedConnection == null &&
                    studRef.ConnectionIndex != -1 &&
                    studRef.ConnectionIndex < Connections.Count)
                {
                    linkedConnection = Connections[studRef.ConnectionIndex];
                }

                if (linkedConnection != null && linkedConnection.ConnectorType != ConnectorType.Custom2DField)
                {
                    Debug.WriteLine("Component references non Custom2DField connection!");
                    linkedConnection = null;
                }

                if (linkedConnection == null &&
                        (studRef.ConnectionIndex >= 0 || !string.IsNullOrEmpty(studRef.ConnectionID))
                    )
                {
                    Debug.WriteLine("Could not find connection!");
                }

                studRef.ConnectionID = linkedConnection?.ID;

                studRef.ConnectionIndex = linkedConnection != null ? Connections.IndexOf(linkedConnection) : -1;

                if (studRef.Connector != null)
                {
                    if (studRef.PositionX >= 0 && studRef.PositionY >= 0)
                    {
                        studRef.FieldIndex = studRef.Connector.PositionToIndex(studRef.PositionX, studRef.PositionY);
                    }
                    else if (studRef.FieldIndex >= 0)
                    {
                        var pos = studRef.Connector.IndexToPosition(studRef.FieldIndex);
                        studRef.PositionX = pos.Item1;
                        studRef.PositionY = pos.Item2;
                    }
                }
                else
                    studRef.FieldIndex = -1;
            }
        }

        public void UpdateBoneReferencesIndices()
        {
            foreach (var bone in Bones)
            {
                if (bone.TargetBoneID < 0 /*|| bone.ConnectionIndex < 0*/)
                    continue;

                var prevBone = Bones.FirstOrDefault(x => x.BoneID == bone.TargetBoneID);
                if (prevBone == null)
                    continue;

                PartConnection sourceConn = null, targetConn = null;

                if (!string.IsNullOrEmpty(bone.SourceConnectionID))
                    sourceConn = bone.Connections.FirstOrDefault(x => x.ID == bone.SourceConnectionID);

                if (sourceConn == null && bone.SourceConnectionIndex >= 0
                    && bone.SourceConnectionIndex < bone.Connections.Count)
                    sourceConn = bone.Connections[bone.SourceConnectionIndex];

                if (sourceConn != null)
                {
                    bone.SourceConnectionID = sourceConn.ID;
                    bone.SourceConnectionIndex = bone.Connections.IndexOf(sourceConn);
                }

                if (!string.IsNullOrEmpty(bone.TargetConnectionID))
                    targetConn = prevBone.Connections.FirstOrDefault(x => x.ID == bone.TargetConnectionID);

                if (targetConn == null && bone.TargetConnectionIndex >= 0
                    && bone.TargetConnectionIndex < prevBone.Connections.Count)
                    targetConn = prevBone.Connections[bone.TargetConnectionIndex];

                if (targetConn != null)
                {
                    bone.TargetConnectionID = targetConn.ID;
                    bone.TargetConnectionIndex = prevBone.Connections.IndexOf(targetConn);
                }
            }
        }

        private void LinkBonesAndStudReferences()
        {
            UpdateStudReferencesIndices();

            UpdateBoneReferencesIndices();
        }

        public MeshGeometry LoadModelMesh(ModelMesh modelMesh)
        {
            if (IsLoadedFromDisk && modelMesh.Geometry == null &&  modelMesh.MeshFileExists)
            {
                try
                {
                    modelMesh.Geometry = MeshGeometry.FromFile(modelMesh.WorkingFilePath);
                    modelMesh.UpdateMeshProperties();
                    UpdateModelStatistics();
                }
                catch { }
            }

            return modelMesh.Geometry;
        }

        public BoundingBox CalculateBoundingBox()
        {
            var meshRefs = Surfaces.SelectMany(x => x.GetAllMeshReferences()).ToList();
            //var meshes = meshRefs.Select(x => x.ModelMesh).Distinct().ToList();
            var unloadedMeshes = meshRefs.Select(x => x.ModelMesh).Where(x => !x.IsModelLoaded).Distinct().ToList();

            try
            {
                var vertices = new List<Vertex>();
                foreach(var meshRef in meshRefs)
                {
                    var meshGeom = meshRef.GetGeometry(true);
                    if (meshGeom != null)
                        vertices.AddRange(meshGeom.Vertices);
                }
                return BoundingBox.FromVertices(vertices).Rounded();
            }
            finally
            {
                unloadedMeshes.ForEach(x => x.UnloadModel());
            }
        }

        public Simple3D.Vector3d CalculateCenterOfMass()
        {
            var meshRefs = Surfaces.SelectMany(x => x.GetAllMeshReferences()).ToList();
            var unloadedMeshes = meshRefs.Select(x => x.ModelMesh).Where(x => !x.IsModelLoaded).Distinct().ToList();

            double totalVolume = 0;
            Simple3D.Vector3d center = Simple3D.Vector3d.Zero;

            try
            {
                var vertices = new List<Vertex>();
                foreach (var meshRef in meshRefs)
                {
                    var meshGeom = meshRef.GetGeometry(true);
                    if (meshGeom != null)
                    {
                        foreach (var tri in meshGeom.Triangles)
                        {
                            var currentVolume = (tri.V1.Position.X * tri.V2.Position.Y * tri.V3.Position.Z - tri.V1.Position.X * tri.V3.Position.Y * tri.V2.Position.Z - tri.V2.Position.X * tri.V1.Position.Y * tri.V3.Position.Z + tri.V2.Position.X * tri.V3.Position.Y * tri.V1.Position.Z + tri.V3.Position.X * tri.V1.Position.Y * tri.V2.Position.Z - tri.V3.Position.X * tri.V2.Position.Y * tri.V1.Position.Z) / 6d;
                            totalVolume += currentVolume;
                            center += ((Simple3D.Vector3d)(tri.V1.Position + tri.V2.Position + tri.V3.Position) / 4d) * currentVolume;
                        }
                    }
                }
                center /= totalVolume;
                return center;
            }
            finally
            {
                unloadedMeshes.ForEach(x => x.UnloadModel());
            }
        }

        public void RemoveUnreferencedMeshes()
        {
            var allMeshes = GetAllElements<ModelMesh>().ToList();
            var allRefs = GetAllElements<ModelMeshReference>().ToList();

            foreach (var mesh in allMeshes)
            {
                if (!allRefs.Any(x => x.MeshID == mesh.ID || x.ModelMesh == mesh))
                {
                    mesh.TempDeleteFile();
                    Meshes.Remove(mesh);
                }
            }
        }

        public void UpdateModelStatistics()
        {
            var allMeshes = Surfaces.SelectMany(x => x.Components.SelectMany(c => c.Meshes));
            TotalTriangles = allMeshes.Sum(x => x.TriangleCount);
            TotalVertices = allMeshes.Sum(x => x.VertexCount == 0 ? x.ModelMesh?.VertexCount ?? 0 : x.VertexCount);
        }

        public void SplitMeshSurfaces(ModelMeshReference meshRef)
        {
            bool wasLoaded = meshRef.IsModelLoaded;
            var meshGeom = meshRef.GetGeometry(true);

            var splittedMeshes = GeometryBuilder.SplitSurfaces(meshGeom);

            int ctr = 0;
            var parentCollection = meshRef.GetParentCollection();

            foreach (var createdMesh in splittedMeshes)
            {
                var modelMesh = AddMeshGeometry(createdMesh, $"{meshRef.Name}_{ctr++}");
                parentCollection.Add(new ModelMeshReference(modelMesh));
            }

            parentCollection.Remove(meshRef);
            RemoveUnreferencedMeshes();

            if (!wasLoaded && meshRef.ModelMesh.CanUnloadModel)
                meshRef.ModelMesh.UnloadModel();
        }

        public void CombineMeshes(IEnumerable<ModelMeshReference> meshRefs)
        {
            var meshParent = meshRefs.FirstOrDefault()?.Parent;
            
            if (!meshRefs.All(x => x.Parent == meshParent)) return;

            var parentCollection = meshRefs.FirstOrDefault().GetParentCollection();

            var builder = new GeometryBuilder();

            foreach (var meshRef in meshRefs.ToArray())
            {
                bool wasLoaded = meshRef.IsModelLoaded;
                var meshGeom = meshRef.GetGeometry(true);
                builder.CombineGeometry(meshGeom);
                if (!wasLoaded && meshRef.ModelMesh.CanUnloadModel)
                    meshRef.ModelMesh.UnloadModel();

                parentCollection.Remove(meshRef);
            }

            var finalGeom = builder.GetGeometry();
            var newModel = AddMeshGeometry(finalGeom);

            parentCollection.Add(new ModelMeshReference(newModel));
            RemoveUnreferencedMeshes();
        }



        #endregion

        #region Bones data handling

        public void ClearBoneConnections()
        {
            foreach (var bone in Bones)
            {
                bone.Connections.RemoveAll(x => x.SubType >= 999000);
                bone.TargetConnectionID = string.Empty;
                bone.TargetConnectionIndex = -1;
                bone.SourceConnectionID = string.Empty;
                bone.SourceConnectionIndex = -1;
            }
        }

        public void RebuildBoneConnections(float flexAmount = 0.06f)
        {
            if (Bones.Count == 0)
                return;

            foreach (var bone in Bones)
            {
                bone.Connections.RemoveAll(x => x.SubType >= 999000);
                bone.TargetConnectionID = string.Empty;
                bone.TargetConnectionIndex = -1;
                bone.SourceConnectionID = string.Empty;
                bone.SourceConnectionIndex = -1;
            }

            int lastBoneId = Bones.Max(x => x.BoneID);

            int curConnType = 0;
            int totalConnection = 0;
            string flexAttributes = string.Format(CultureInfo.InvariantCulture, "-{0},{0},20,10,10", flexAmount);

            foreach (var bone in Bones.OrderBy(x => x.BoneID))
            {
                foreach (var linkedBone in Bones.Where(x => x.TargetBoneID == bone.BoneID))
                {
                    PartConnection targetConn = PartConnection.Create(ConnectorType.Ball);

                    if (linkedBone.BoneID == lastBoneId)
                    {
                        targetConn = PartConnection.Create(ConnectorType.Fixed);
                        var fixConn = targetConn.GetConnector<FixedConnector>();
                        fixConn.SubType = 999000 + curConnType;
                    }
                    else
                    {
                        targetConn = PartConnection.Create(ConnectorType.Ball);
                        var ballConn = targetConn.GetConnector<BallConnector>();
                        ballConn.SubType = 999000 + curConnType;
                    }

                    curConnType = (++curConnType) % 4;

                    var posOffset = linkedBone.Transform.Position - bone.Transform.Position;
                    posOffset = bone.Transform.ToMatrixD().Inverted().TransformVector(posOffset);
                    targetConn.Transform.Position = posOffset;
                    

                    bone.Connections.Add(targetConn);
                    RenameElement(targetConn, $"FlexConn{totalConnection++}");

                    PartConnection sourceConn = null;
                    if (linkedBone.BoneID == lastBoneId)
                    {
                        sourceConn = PartConnection.Create(ConnectorType.Fixed);
                        var fixConn = sourceConn.GetConnector<FixedConnector>();
                        fixConn.SubType = 999000 + curConnType;
                    }
                    else
                    {
                        sourceConn = PartConnection.Create(ConnectorType.Ball);
                        var ballConn = sourceConn.GetConnector<BallConnector>();
                        ballConn.SubType = 999000 + curConnType;
                        ballConn.FlexAttributes = flexAttributes;
                    }

                    curConnType = (++curConnType) % 4;
                    linkedBone.Connections.Add(sourceConn);
                    RenameElement(sourceConn, $"FlexConn{totalConnection++}");

                    linkedBone.TargetConnectionID = targetConn.ID;
                    linkedBone.SourceConnectionID = sourceConn.ID;

                }
            }

            UpdateBoneReferencesIndices();
        }

        public void CalculateBoneBoundingBoxes()
        {
            var meshRefs = Surfaces.SelectMany(x => x.GetAllMeshReferences()).ToList();
            //var meshes = meshRefs.Select(x => x.ModelMesh).Distinct().ToList();
            var unloadedMeshes = meshRefs.Select(x => x.ModelMesh).Where(x => !x.IsModelLoaded).Distinct().ToList();

            var allVerts = new List<Vertex>();
            foreach(var mesh in GetAllElements<ModelMeshReference>())
                allVerts.AddRange(mesh.GetGeometry().Vertices);
            allVerts = allVerts.Distinct().ToList();

            foreach (var bone in Bones.OrderBy(x => x.BoneID))
            {
                var boneTrans = bone.Transform.ToMatrix().Inverted();
                var verts = allVerts.Where(x => x.BoneWeights.Any(y => y.BoneID == bone.BoneID && y.Weight > 0.1f));

                if (bone.PhysicsAttributes == null)
                    bone.PhysicsAttributes = new PhysicsAttributes();

                if (verts.Any())
                {
                    var vertPos = verts.Select(x => boneTrans.TransformPosition(x.Position)).ToList();
                    var newBounds = BoundingBox.FromVertices(vertPos);
                    newBounds.Round();
                    bone.Bounding = newBounds;
                    var physAttr = bone.PhysicsAttributes;
                    physAttr.CenterOfMass = newBounds.Center;
                }
                else
                {
                    bone.Bounding = new BoundingBox(new Simple3D.Vector3d(-0.0001d), new Simple3D.Vector3d(0.0001d));
                    var physAttr = bone.PhysicsAttributes;
                    physAttr.CenterOfMass = Simple3D.Vector3d.Zero;
                    physAttr.Mass = 0;
                    physAttr.InertiaTensor = Simple3D.Matrix3d.Zero;
                }
            }

            unloadedMeshes.ForEach(x => x.UnloadModel());
        }


        #endregion

        #region Change tracking 

        internal void OnElementCollectionChanged(ElementCollectionChangedEventArgs ccea)
        {
            if (ccea.Action == System.ComponentModel.CollectionChangeAction.Add)
            {
                if (!IsLoading)
                {
                    var elementHierarchy = ccea.AddedElements
                        .Concat(ccea.AddedElements.SelectMany(x => x.GetChildsHierarchy()));

                    GenerateElementIDs(elementHierarchy);
                    GenerateElementsNames(elementHierarchy);
                }
            }

            if (!IsLoading)
            {
                if (ccea.Collection.ElementType == typeof(PartConnection) &&
                    ccea.ChangedElements.OfType<PartConnection>()
                        .Any(x => x.ConnectorType == ConnectorType.Custom2DField))
                {
                    UpdateStudReferencesIndices();
                }

                if (ccea.Collection.ElementType == typeof(PartSurface))
                {
                    RenumberSurfaces();
                }

                if (ccea.GetElementHierarchy().OfType<ModelMeshReference>().Any())
                    UpdateModelStatistics();
            }

            if (!IsLoading)
                ElementCollectionChanged?.Invoke(this, ccea);
        }

        internal void OnElementPropertyChanged(ElementValueChangedEventArgs pcea)
        {
            if (!IsLoading)
                ElementPropertyChanged?.Invoke(this, pcea);
        }

        #endregion

        #region Project File/Directory Handling

        public string GenerateMeshFileName(string meshName)
        {
            meshName = FileHelper.GetSafeFileName(meshName);
            return $"Meshes\\{meshName}.geom";
        }

        public string GetFileFullPath(string filename)
        {
            if (IsLoadedFromDisk)
                return Path.Combine(ProjectWorkingDir, filename);
            return null;
        }

        public bool FileExist(string filename)
        {
            if (IsLoadedFromDisk)
                return File.Exists(GetFileFullPath(filename));
            return false;
        }

        #endregion

        #region Extra propeties

        public bool TryGetProperty<T>(string propertyName, out T value)
        {
            value = default;
            if (ProjectProperties.TryGetValue(propertyName, out string strValue))
                return StringUtils.TryParse<T>(strValue, out value);
            return false;
        }

        #endregion

        #region LDD File Generation

        public List<ValidationMessage> ValidatePart()
        {
            var validationMessages = new List<ValidationMessage>();

            void AddMessage(string code, ValidationLevel level, params object[] args)
            {
                validationMessages.Add(new ValidationMessage("PROJECT", code, level, args));
            }

            if (!Surfaces.Any())
                AddMessage("PROJECT_NO_SURFACES", ValidationLevel.Error);

            if (!Flexible && !Connections.Any())
                AddMessage("PROJECT_NO_CONNECTIONS", ValidationLevel.Warning);

            if (!Flexible && !Collisions.Any())
                AddMessage("PROJECT_NO_COLLISIONS", ValidationLevel.Warning);

            if (Flexible && !Bones.Any())
                AddMessage("PROJECT_NO_BONES", ValidationLevel.Error);

            validationMessages.AddRange(Properties.ValidateElement());

            foreach (var surface in Surfaces)
                validationMessages.AddRange(surface.ValidateElement());

            foreach (var conn in Connections)
                validationMessages.AddRange(conn.ValidateElement());

            foreach (var coll in Collisions)
                validationMessages.AddRange(coll.ValidateElement());

            if (Bones.Any())
            {
                var duplicates = Bones.GroupBy(x => x.BoneID).Where(g => g.Count() > 1);
                if (duplicates.Any())
                {
                    AddMessage("BONES_DUPLICATE_IDS", ValidationLevel.Error, 
                        string.Join(", ", duplicates.Select(x => x.Key)));
                }

                for (int i = 0; i < Bones.Count; i++)
                {
                    if (!Bones.Any(x => x.BoneID == i))
                    {
                        AddMessage("BONES_ID_GAP", ValidationLevel.Error, i);
                        break;
                    }
                }
            }

            foreach (var bone in Bones)
                validationMessages.AddRange(bone.ValidateElement());

            if (validationMessages.Count == 0)
                AddMessage("PROJECT_NO_ISSUES", ValidationLevel.Info);

            return validationMessages;
        }

        public Primitive GeneratePrimitive()
        {
            LinkBonesAndStudReferences();

            var primitive = new Primitive()
            {
                ID = PartID,
                Name = PartDescription,
                Bounding = Bounding?.Rounded(6),
                GeometryBounding = GeometryBounding?.Rounded(6),
                DefaultCamera = DefaultCamera,
                DefaultOrientation = DefaultOrientation?.ToLDD(),
                MainGroup = MainGroup,
                Platform = Platform,
                PhysicsAttributes = PhysicsAttributes,
                PartVersion = PartVersion,
                FileVersion = PrimitiveFileVersion,
            };

            if (Surfaces.Count > 1)
                primitive.SubMaterials = Surfaces.Select(x => x.SubMaterialIndex).ToArray();

            primitive.Aliases.Add(PartID);

            if (Aliases.Any())
                primitive.Aliases.AddRange(Aliases);

            if (!Flexible)
            {
                foreach (var conn in Connections)
                    primitive.Connectors.Add(conn.GenerateLDD());

                foreach (var coll in Collisions)
                    primitive.Collisions.Add(coll.GenerateLDD());
            }

            foreach (var bone in Bones)
                primitive.FlexBones.Add(bone.GenerateLDD());

            return primitive;
        }

        public LDD.Parts.PartWrapper GenerateLddPart()
        {
            if (Bounding is null)
                Bounding = CalculateBoundingBox();

            if (GeometryBounding is null)
                GeometryBounding = Bounding;

            var part = new LDD.Parts.PartWrapper()
            {
                PartID = PartID,
                Primitive = GeneratePrimitive()
            };

            foreach (var surface in Surfaces)
            {
                var surfaceMesh = surface.GenerateMeshFile();
                var test = surfaceMesh.Geometry.CheckHasRoundEdgeData();
                part.AddSurfaceMesh(surface.SurfaceID, surfaceMesh);
            }

            return part;
        }

        public void ComputeEdgeOutlines(float breakAngle = 35f)
        {
            var meshRefs = Surfaces.SelectMany(x => x.GetAllMeshReferences()).ToList();
            var unloadedMeshes = meshRefs.Select(x => x.ModelMesh).Where(x => !x.IsModelLoaded).Distinct().ToList();

            if (GetAllElements<FemaleStudModel>().Any())
            {

            }

            foreach (var layerGroup in meshRefs.GroupBy(x => x.RoundEdgeLayer))
            {
                var meshesGeoms = layerGroup.Select(x => new Tuple<ModelMeshReference, MeshGeometry>(x, x.GetGeometry(true))).ToList();

                foreach (var mg in meshesGeoms)
                    mg.Item2.ClearRoundEdgeData();

                if (meshesGeoms.Any(x => x.Item1.Parent is FemaleStudModel fsm && fsm.ReplacementMeshes.Any()))
                {
                    var nonFSM = meshesGeoms.Where(x => !(x.Item1.Parent is FemaleStudModel)).ToList();
                    var baseFSM = meshesGeoms
                        .Where(x => x.Item1.Parent is FemaleStudModel fsm && fsm.Meshes.Contains(x.Item1)).ToList();

                    var altFSM = meshesGeoms
                        .Where(x => x.Item1.Parent is FemaleStudModel fsm && fsm.ReplacementMeshes.Contains(x.Item1)).ToList();

                    var triangles = nonFSM.Concat(baseFSM).SelectMany(x => x.Item2.Triangles);
                    OutlinesGenerator.GenerateOutlines(triangles, breakAngle);
                    foreach (var mg in nonFSM.Concat(baseFSM))
                        mg.Item1.UpdateMeshOutlines(mg.Item2);

                    triangles = nonFSM.Concat(altFSM).SelectMany(x => x.Item2.Triangles);
                    OutlinesGenerator.GenerateOutlines(triangles, breakAngle);
                    foreach (var mg in altFSM)
                        mg.Item1.UpdateMeshOutlines(mg.Item2);
                }
                else
                {
                    var triangles = meshesGeoms.SelectMany(x => x.Item2.Triangles);
                    //ShaderDataGenerator.ComputeEdgeOutlines(triangles, breakAngle);
                    OutlinesGenerator.GenerateOutlines(triangles, breakAngle);

                    foreach (var mg in meshesGeoms)
                        mg.Item1.UpdateMeshOutlines(mg.Item2);
                }
                
            }

            var models = meshRefs.Select(x => x.ModelMesh).Distinct().ToList();
            models.ForEach(x => x.SaveFile());
            unloadedMeshes.ForEach(x => x.UnloadModel());
        }

        public void ClearEdgeOutlines()
        {
            var meshRefs = Surfaces.SelectMany(x => x.GetAllMeshReferences()).ToList();
            var meshModels = meshRefs.Select(x => x.ModelMesh).Distinct().ToList();
            var unloadedMeshes = meshModels.Where(x => !x.IsModelLoaded).ToList();

            foreach (var model in meshModels)
            {
                if (!model.IsModelLoaded)
                {
                    if (!model.LoadModel())
                        continue;
                }
                model.Geometry.ClearRoundEdgeData();
                model.SaveFile();
            }

            unloadedMeshes.ForEach(x => x.UnloadModel());
        }


        #endregion
    }
}
