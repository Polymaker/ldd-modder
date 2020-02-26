using ICSharpCode.SharpZipLib.Zip;
using LDDModder.LDD.Data;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Serialization;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
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

        public string Comments
        {
            get => Properties.Comments;
            set => Properties.Comments = value;
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

        [XmlIgnore]
        public bool IsLoading { get; internal set; }

        public event EventHandler<ElementValueChangedEventArgs> ElementPropertyChanged;

        public event EventHandler<ElementCollectionChangedEventArgs> ElementCollectionChanged;

        public PartProject()
        {
            Surfaces = new ElementCollection<PartSurface>(this);
            Connections = new ElementCollection<PartConnection>(this);
            Collisions = new ElementCollection<PartCollision>(this);
            Bones = new ElementCollection<PartBone>(this);
            Meshes = new ElementCollection<ModelMesh>(this);

            Properties = new PartProperties(this);
        }

        #region Creation From LDD

        public static PartProject CreateFromLddPart(int partID)
        {
            return CreateFromLddPart(LDD.LDDEnvironment.Current, partID);
        }

        public static PartProject CreateFromLddPart(LDD.LDDEnvironment environment, int partID)
        {
            var lddPart = LDD.Parts.PartWrapper.LoadPart(environment, partID, true);

            var project = new PartProject()
            {
                IsLoading = true
            };
            project.SetBaseInfo(lddPart);

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
                surfaceElement.ID = StringUtils.GenerateUUID($"Part{partID}_Surface{surfaceElement.SurfaceID}", 8);
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

                        (cullingComp as FemaleStudModel).ReplacementMeshes
                            .Add(new ModelMeshReference(replacementMesh));
                    }

                    surfaceElement.Components.Add(cullingComp);
                    elementIndex++;
                }
            }

            project.GenerateElementIDs(true);
            project.GenerateElementsNames();
            project.LinkStudReferences();

            project.IsLoading = false;
            return project;
        }

        private void SetBaseInfo(LDD.Parts.PartWrapper lddPart)
        {
            PartID = lddPart.PartID;
            PartDescription = lddPart.Primitive.Name;
            PartVersion = lddPart.Primitive.PartVersion;
            Decorated = lddPart.IsDecorated;
            Flexible = lddPart.IsFlexible;
            Aliases.AddRange(lddPart.Primitive.Aliases);
            if (Aliases.Contains(PartID))
                Aliases.Remove(PartID);
            Platform = lddPart.Primitive.Platform;
            MainGroup = lddPart.Primitive.MainGroup;
            PhysicsAttributes = lddPart.Primitive.PhysicsAttributes;
            GeometryBounding = lddPart.Primitive.GeometryBounding;
            Bounding = lddPart.Primitive.Bounding;
            if (lddPart.Primitive.DefaultOrientation != null)
                DefaultOrientation = ItemTransform.FromLDD(lddPart.Primitive.DefaultOrientation);
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

            return doc;
        }

        private void LoadFromXml(XDocument doc)
        {
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

            LinkStudReferences();
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
            modelMesh.InternalSetNameAndID(id, name);

            if (string.IsNullOrEmpty(id))
                GenerateElementID(modelMesh);

            if (string.IsNullOrEmpty(name))
                GenerateElementName(modelMesh);
            else
                RenameElement(modelMesh, name);

            Meshes.Add(modelMesh);

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

        public void UpdateConnectionIndexes()
        {
            var connectionIDs = new List<string>();
            foreach (var conn in Connections)
                connectionIDs.Add(conn.ID);

            foreach (var model in GetAllElements<PartCullingModel>())
                model.ConnectionIndex = connectionIDs.IndexOf(model.ConnectionID);
        }

        private void LinkStudReferences()
        {
            foreach (var surf in Surfaces)
            {
                foreach (var comp in surf.Components.OfType<PartCullingModel>())
                {
                    PartConnection linkedConnection = null;

                    if (comp.ConnectionIndex != -1 &&
                        comp.ConnectionIndex < Connections.Count &&
                        Connections[comp.ConnectionIndex].ConnectorType == ConnectorType.Custom2DField)
                    {
                        linkedConnection = Connections[comp.ConnectionIndex];
                    }

                    if (linkedConnection == null && !string.IsNullOrEmpty(comp.ConnectionID))
                    {
                        linkedConnection = Connections
                            .FirstOrDefault(x => x.ID == comp.ConnectionID);
                    }

                    comp.ConnectionID = linkedConnection?.ID;
                    comp.ConnectionIndex = linkedConnection != null ? Connections.IndexOf(linkedConnection) : -1;

                }
            }
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
                return BoundingBox.FromVertices(vertices);
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

            foreach(var mesh in allMeshes)
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

            if (!IsLoading && ccea.Collection.ElementType == typeof(PartConnection) && 
                GetAllElements<PartCullingModel>().Any())
            {
                UpdateConnectionIndexes();
            }

            if (ccea.GetElementHierarchy().OfType<ModelMeshReference>().Any())
                UpdateModelStatistics();

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

            foreach (var bone in Bones)
                validationMessages.AddRange(bone.ValidateElement());

            if (validationMessages.Count == 0)
                AddMessage("PROJECT_NO_ISSUES", ValidationLevel.Info);

            return validationMessages;
        }

        public Primitive GeneratePrimitive()
        {
            var primitive = new Primitive()
            {
                ID = PartID,
                Name = PartDescription,
                Bounding = Bounding,
                GeometryBounding = GeometryBounding,
                DefaultCamera = DefaultCamera,
                DefaultOrientation = DefaultOrientation?.ToLDD(),
                MainGroup = MainGroup,
                Platform= Platform,
                PhysicsAttributes = PhysicsAttributes,
                PartVersion = PartVersion,
                FileVersion = PrimitiveFileVersion,
                //SubMaterials = Surfaces.Select(x => x.SubMaterialIndex).ToArray()
            };

            if (Surfaces.Count > 1)
                primitive.SubMaterials = Surfaces.Select(x => x.SubMaterialIndex).ToArray();

            primitive.Aliases.Add(PartID);

            if (Aliases.Any())
                primitive.Aliases.AddRange(Aliases);

            foreach (var conn in Connections)
                primitive.Connectors.Add(conn.GenerateLDD());

            foreach (var coll in Collisions)
                primitive.Collisions.Add(coll.GenerateLDD());
            
            return primitive;
        }

        public LDD.Parts.PartWrapper GenerateLddPart()
        {
            if (Bounding == null)
                Bounding = CalculateBoundingBox();
            if (GeometryBounding == null)
                GeometryBounding = Bounding;

            var part = new LDD.Parts.PartWrapper()
            {
                PartID = PartID,
                Primitive = GeneratePrimitive()
            };

            foreach (var surface in Surfaces)
            {
                var surfaceMesh = surface.GenerateMeshFile();
                part.AddSurfaceMesh(surface.SurfaceID, surfaceMesh);
            }

            return part;
        }

        public void ComputeEdgeOutlines()
        {
            var allMeshes = Surfaces.SelectMany(x => x.GetAllModelMeshes());
            ShaderDataGenerator.ComputeEdgeOutlines(allMeshes.SelectMany(x => x.Geometry.Triangles));
        }

        #endregion
    }
}
