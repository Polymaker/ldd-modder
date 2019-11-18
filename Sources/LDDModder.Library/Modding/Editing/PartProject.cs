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

        public int PartID { get; private set; }

        public string PartDescription { get; set; }

        public string Comments { get; set; }

        public List<int> Aliases { get; set; }

        public Platform Platform { get; set; }

        public MainGroup MainGroup { get; set; }

        public PhysicsAttributes PhysicsAttributes { get; set; }

        public BoundingBox Bounding { get; set; }

        public BoundingBox GeometryBounding { get; set; }

        public ItemTransform DefaultOrientation { get; set; }

        public Camera DefaultCamera { get; set; }

        public VersionInfo PrimitiveFileVersion { get; set; }

        public int PartVersion { get; set; }
 
        public bool Decorated { get; set; }

        public bool Flexible { get; set; }

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

        [XmlIgnore]
        public bool IsLoading { get; internal set; }

        public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

        public event EventHandler<CollectionChangedEventArgs> ElementCollectionChanged;

        public PartProject()
        {
            Surfaces = new ElementCollection<PartSurface>(this);
            Connections = new ElementCollection<PartConnection>(this);
            Collisions = new ElementCollection<PartCollision>(this);
            Bones = new ElementCollection<PartBone>(this);
            Meshes = new ElementCollection<ModelMesh>(this);

            PrimitiveFileVersion = new VersionInfo(1, 0);
            Aliases = new List<int>();
            PartVersion = 1;
        }

        #region Creation From LDD

        public static PartProject CreateFromLddPart(int partID)
        {
            return CreateFromLddPart(LDD.LDDEnvironment.Current, partID);
        }

        public static PartProject CreateFromLddPart(LDD.LDDEnvironment environment, int partID)
        {
            var lddPart = LDD.Parts.PartWrapper.LoadPart(environment, partID);

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
                var boneElement = new PartBone
                {
                    ID = StringUtils.GenerateUUID($"Part{partID}_Bone{elementIndex++}", 8),
                    Name = $"Bone{flexBone.ID}"
                };
                project.Bones.Add(boneElement);
                boneElement.LoadFromLDD(flexBone);
                
            }

            foreach (var meshSurf in lddPart.Surfaces)
            {
                var surfaceElement = new PartSurface(
                    meshSurf.SurfaceID,
                    lddPart.Primitive.GetSurfaceMaterialIndex(meshSurf.SurfaceID)
                );
                surfaceElement.ID = StringUtils.GenerateUUID($"Part{partID}_Surface{surfaceElement.SurfaceID}", 8);
                project.Surfaces.Add(surfaceElement);

                var surfaceMesh = project.AddMeshGeometry(
                    meshSurf.Mesh.Geometry,
                    StringUtils.GenerateUUID($"Part{partID}_SurfaceMesh{surfaceElement.SurfaceID}", 8),
                    $"Surface{surfaceElement.SurfaceID}_Mesh"
                );

                elementIndex = 0;
                foreach (var culling in meshSurf.Mesh.Cullings)
                {
                    ModelMesh replacementMesh = null;
                    if (culling.ReplacementMesh != null)
                    {
                        replacementMesh = project.AddMeshGeometry(
                            culling.ReplacementMesh,
                            StringUtils.GenerateUUID($"Part{partID}_SurfaceMesh{surfaceElement.SurfaceID}_Culling{elementIndex}", 8)
                        );
                    }

                    var cullingComp = SurfaceComponent.CreateFromLDD(culling, surfaceMesh, replacementMesh);
                    cullingComp.ID = StringUtils.GenerateUUID($"Part{partID}_Surface{surfaceElement.SurfaceID}_Culling{elementIndex}", 8);
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

            //Part Info
            {
                var propsElem = doc.Root.AddElement("Properties");
                propsElem.Add(new XElement("PartID", PartID));

                if (Aliases.Any())
                    propsElem.Add(new XElement("Aliases", string.Join(";", Aliases)));

                propsElem.Add(new XElement("Description", PartDescription));

                propsElem.Add(new XElement("PartVersion", PartVersion));

                if (PrimitiveFileVersion != null)
                    propsElem.Add(PrimitiveFileVersion.ToXmlElement("PrimitiveVersion"));

                propsElem.Add(new XElement("Flexible", Flexible));

                propsElem.Add(new XElement("Decorated", Decorated));

                if (Platform != null)
                    propsElem.AddElement("Platform", new XAttribute("ID", Platform.ID), new XAttribute("Name", Platform.Name));

                if (MainGroup != null)
                    propsElem.AddElement("MainGroup", new XAttribute("ID", MainGroup.ID), new XAttribute("Name", MainGroup.Name));

                if (PhysicsAttributes != null)
                    propsElem.Add(PhysicsAttributes.SerializeToXml());

                if (Bounding != null)
                    propsElem.Add(XmlHelper.DefaultSerialize(Bounding, "Bounding"));

                if (GeometryBounding != null)
                    propsElem.Add(XmlHelper.DefaultSerialize(GeometryBounding, "GeometryBounding"));

                if (DefaultOrientation != null)
                {
                    //propsElem.Add(new XComment(DefaultOrientation.GetLddXml().ToString()));
                    propsElem.Add(DefaultOrientation.SerializeToXml("DefaultOrientation"));
                }

                if (DefaultCamera != null)
                    propsElem.Add(XmlHelper.DefaultSerialize(DefaultCamera, "DefaultCamera"));

                if (!string.IsNullOrEmpty(Comments))
                    propsElem.Add(new XElement("Comments", Comments));
            }

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

            //Part info
            if (rootElem.HasElement("Properties", out XElement propsElem))
            {
                PartID = int.Parse(propsElem.Element("PartID")?.Value);

                if (propsElem.HasElement("Aliases", out XElement aliasElem))
                {
                    foreach(string partAlias in aliasElem.Value.Split(';'))
                    {
                        if (int.TryParse(partAlias, out int aliasID))
                            Aliases.Add(aliasID);
                    }
                }

                PartDescription = propsElem.ReadElement("Description", string.Empty);
                PartVersion = propsElem.ReadElement("PartVersion", 1);
                
                if (propsElem.HasElement("PhysicsAttributes", out XElement pA))
                {
                    PhysicsAttributes = new PhysicsAttributes();
                    PhysicsAttributes.LoadFromXml(pA);
                }
                
                if (propsElem.HasElement("GeometryBounding", out XElement gb))
                    GeometryBounding = XmlHelper.DefaultDeserialize<BoundingBox>(gb);

                if (propsElem.HasElement("Bounding", out XElement bb))
                    Bounding = XmlHelper.DefaultDeserialize<BoundingBox>(bb);

                if (propsElem.HasElement("DefaultOrientation", out XElement defori))
                    DefaultOrientation = ItemTransform.FromXml(defori);

                if (propsElem.HasElement("DefaultCamera", out XElement camElem))
                    DefaultCamera = XmlHelper.DefaultDeserialize<Camera>(camElem);

                if (propsElem.HasElement("Platform", out XElement platformElem))
                {
                    Platform = new Platform(
                        platformElem.ReadAttribute("ID", 0),
                        platformElem.ReadAttribute("Name", string.Empty)
                    );
                }

                if (propsElem.HasElement("MainGroup", out XElement groupElem))
                {
                    MainGroup = new MainGroup(
                        groupElem.ReadAttribute("ID", 0),
                        groupElem.ReadAttribute("Name", string.Empty)
                    );
                }
            }

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
            //LinkStudReferences();
            //GenerateMeshIDs(false);
            GenerateMeshesNames();

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

                GenerateMeshesNames();
                var projectXml = GenerateProjectXml();

                projectXml.Save(zipStream);

                zipStream.CloseEntry();

                foreach (var mesh in Meshes)
                {
                    if (!string.IsNullOrEmpty(mesh.WorkingFilePath) && 
                        File.Exists(mesh.WorkingFilePath))
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
            {
                if (IsLoadedFromDisk && !string.IsNullOrEmpty(mesh.FileName))
                {
                    var meshPath = Path.Combine(ProjectWorkingDir, mesh.FileName);
                    if (File.Exists(meshPath))
                        mesh.WorkingFilePath = meshPath;
                }
            }
        }

        public ModelMesh AddMeshGeometry(MeshGeometry geometry)
        {
            ModelMesh modelMesh = AddMeshGeometry(geometry, null, null);

            if (IsLoadedFromDisk)
            {
                modelMesh.WorkingFilePath = Path.Combine(ProjectWorkingDir, modelMesh.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(modelMesh.WorkingFilePath));
                modelMesh.Geometry.Save(modelMesh.WorkingFilePath);
            }

            return modelMesh;
        }

        private ModelMesh AddMeshGeometry(MeshGeometry geometry, string id, string name = null)
        {
            var modelMesh = new ModelMesh(geometry)
            {
                ID = id,
                Name = name
            };
            Meshes.Add(modelMesh);
            
            if (string.IsNullOrEmpty(id))
                GenerateElementID(modelMesh);
            if (string.IsNullOrEmpty(name))
                GenerateElementName(modelMesh);

            modelMesh.UpdateMeshProperties();
            modelMesh.FileName = $"Meshes\\{modelMesh.Name}.geom";
            

            return modelMesh;
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

                    element.Name = elementName;
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

                    element.Name = elementName;
                }
            }
        }

        public void GenerateElementName(PartElement element)
        {
            var elemList = GetAllElements().Where(x => x.GetFullElementType() == element.GetFullElementType()).ToList();
            int nameCount = elemList.Count(x => !string.IsNullOrEmpty(x.Name));

            string elementName = element.Name;

            while (string.IsNullOrEmpty(elementName) ||
                        elemList.Any(x => x.Name == elementName && x != element))
            {
                elementName = GenerateElementName(element, nameCount++);
                if (elementName == null)
                    break;
            }

            element.Name = elementName;
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

        #endregion

        #region Methods

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

                    //foreach (var stud in comp.GetStudReferences())
                    //{
                        
                    //    if (stud.Connection == null)
                    //    {
                    //        if (stud.ConnectorIndex != -1)
                    //        {
                    //            if (stud.ConnectorIndex < Connections.Count &&
                    //                Connections[stud.ConnectorIndex].ConnectorType == ConnectorType.Custom2DField)
                    //            {
                    //                stud.Connection = (PartConnection/*<Custom2DFieldConnector>*/)Connections[stud.ConnectorIndex];
                    //            }
                    //            else
                    //            {
                    //                string refID = Utilities.StringUtils.GenerateUUID($"{PartID}_{stud.ConnectorIndex}", 8);
                    //                stud.Connection = Connections.OfType<PartConnection/*<Custom2DFieldConnector>*/>()
                    //                    .FirstOrDefault(x => x.RefID == refID);
                    //            }
                    //        }
                    //        else if (!string.IsNullOrEmpty(stud.ConnectionID))
                    //        {
                    //            stud.Connection = Connections.OfType<PartConnection/*<Custom2DFieldConnector>*/>()
                    //                .FirstOrDefault(x => x.RefID == stud.ConnectionID);
                    //        }
                    //    }

                    //    if (stud.Connection != null)
                    //    {
                            
                    //        stud.ConnectionID = stud.Connection.RefID;
                    //        stud.ConnectorIndex = Connections.IndexOf(stud.Connection);
                    //    }
                    //}
                
                
                }
            }
        }

        private void GenerateMeshesNames()
        {
            foreach (var mesh in Meshes)
            {
                string meshName = string.IsNullOrEmpty(mesh.Name) ? mesh.ID : mesh.Name;

                if (string.IsNullOrEmpty(mesh.FileName) || !mesh.FileName.Contains(meshName))
                {
                    mesh.FileName = $"Meshes\\{meshName}.geom";

                    //if (mesh.Surface != null)
                    //    mesh.FileName = $"Meshes\\Surface{mesh.Surface.SurfaceID}_{meshName}.geom";
                }

                if (IsLoadedFromDisk)
                    mesh.WorkingFilePath = Path.Combine(ProjectWorkingDir, mesh.FileName);
            }

        }

        public MeshGeometry LoadModelMesh(ModelMesh modelMesh)
        {
            if (modelMesh.Geometry == null && !string.IsNullOrEmpty(modelMesh.FileName) && IsLoadedFromDisk)
            {
                string meshPath = Path.Combine(ProjectWorkingDir, modelMesh.FileName);
                try
                {
                    if (File.Exists(meshPath))
                    {
                        modelMesh.Geometry = MeshGeometry.FromFile(meshPath);
                        modelMesh.WorkingFilePath = meshPath;
                    }
                    else
                        modelMesh.WorkingFilePath = null;
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
                    var meshGeom = meshRef.GetGeometry();
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

        #endregion

        #region Change tracking 

        internal void OnElementCollectionChanged(CollectionChangedEventArgs ccea)
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
                ElementCollectionChanged?.Invoke(this, ccea);
        }

        internal void OnElementPropertyChanged(PropertyChangedEventArgs pcea)
        {
            if (!IsLoading)
                ElementPropertyChanged?.Invoke(this, pcea);
        }

        #endregion


        #region LDD File Generation


        public void ValidatePart()
        {
            if (Decorated != Surfaces.Any(x => x.SurfaceID > 0))
            {
                //Marked as having decorations but does not have any decoration surfaces
            }

            foreach (var surface in Surfaces)
            {
                var surfaceModels = surface.GetAllModelMeshes();
                if (surface.SurfaceID == 0 && surfaceModels.Any(x => x.IsTextured))
                {

                }
                else if (surface.SurfaceID > 0 && surfaceModels.Any(x => !x.IsTextured))
                {

                }
                else if (!surfaceModels.Any())
                {

                }
            }

            foreach (var mesh in Meshes)
            {
                bool isReferenced = mesh.GetReferences().Any();
                if (isReferenced)
                {
                    //if (mesh.IsFlexible)
                }
            }
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

        #endregion
    }
}
