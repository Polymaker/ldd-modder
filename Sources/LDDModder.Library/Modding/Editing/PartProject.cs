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

        [XmlIgnore]
        public string ProjectPath { get; private set; }

        [XmlArray("ModelSurfaces")]
        public ComponentCollection<PartSurface> Surfaces { get; }

        [XmlArray("Connections")]
        public ComponentCollection<PartConnection> Connections { get; }

        [XmlArray("Collisions")]
        public ComponentCollection<PartCollision> Collisions { get; }

        [XmlArray("Bones")]
        public ComponentCollection<PartBone> Bones { get; }
        
        [XmlIgnore]
        public ComponentCollection<ModelMesh> UnassignedMeshes { get; }
        
        [XmlIgnore]
        public bool IsLoading { get; internal set; }

        public event EventHandler<PropertyChangedEventArgs> ComponentPropertyChanged;

        public PartProject()
        {
            Surfaces = new ComponentCollection<PartSurface>(this);
            Connections = new ComponentCollection<PartConnection>(this);
            Collisions = new ComponentCollection<PartCollision>(this);
            Bones = new ComponentCollection<PartBone>(this);
            UnassignedMeshes = new ComponentCollection<ModelMesh>(this);

            PrimitiveFileVersion = new VersionInfo(1, 0);
            Aliases = new List<int>();
            PartVersion = 1;

            Surfaces.CollectionChanged += Surfaces_CollectionChanged;
            Connections.CollectionChanged += Connections_CollectionChanged;
            Collisions.CollectionChanged += Collisions_CollectionChanged;
        }

        public void ValidatePart()
        {
            //TODO

            if (Decorated != Surfaces.Any(x => x.SurfaceID > 0))
            {
                //Marked as having decorations but does not have any decoration surfaces
            }

            foreach (var surf in Surfaces.Where(x => x.SurfaceID > 0))
            {
                var allMeshes = surf.Components.SelectMany(c => c.GetAllMeshes());
                if (!allMeshes.All(m => m.IsTextured))
                {
                    //The decoration surface has meshes that does not have UV mapping
                }
            }
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

            foreach (var collision in lddPart.Primitive.Collisions)
                project.Collisions.Add(PartCollision.FromLDD(collision));

            foreach (var lddConn in lddPart.Primitive.Connectors)
            {
                var partConn = PartConnection.FromLDD(lddConn);
                if (partConn.ConnectorType == LDD.Primitives.Connectors.ConnectorType.Custom2DField)
                {
                    int connIdx = lddPart.Primitive.Connectors.IndexOf(lddConn);
                    partConn.RefID = StringUtils.GenerateUUID($"{partID}_{connIdx}", 8);
                }
                project.Connections.Add(partConn);
            }

            foreach (var flexBone in lddPart.Primitive.FlexBones)
                project.Bones.Add(PartBone.FromLDD(flexBone));

            foreach (var meshSurf in lddPart.Surfaces)
            {
                var partSurf = new PartSurface(
                    meshSurf.SurfaceID,
                    lddPart.Primitive.GetSurfaceMaterialIndex(meshSurf.SurfaceID)
                );

                project.Surfaces.Add(partSurf);

                foreach (var culling in meshSurf.Mesh.Cullings)
                {
                    var cullingComp = SurfaceComponent.FromLDD(meshSurf.Mesh, culling);
                    partSurf.Components.Add(cullingComp);
                }
            }

            project.GenerateMeshIDs(true);

            project.LinkStudReferences();

            //project.GenerateDefaultComments();

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
            Aliases = lddPart.Primitive.Aliases;
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
            project.IsLoading = true;
            project.Surfaces.Add(new PartSurface(0, 0));
            project.IsLoading = false;
            return project;
        }

        #region Read/Write Xml structure

        public XDocument GenerateProjectXml()
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(PartProject));
            //var doc = new XDocument();
            //var ns = new XmlSerializerNamespaces();
            //ns.Add("", "");
            //using (var docWriter = doc.CreateWriter())
            //    serializer.Serialize(docWriter, this, ns);
            //return doc;

            var doc = new XDocument(new XElement("LDDPART"));

            //Part Info
            {
                var propsElem = doc.Root.AddElement("Properties");
                propsElem.Add(new XElement("PartID", PartID));

                if (Aliases.Where(x => x != PartID).Any())
                    propsElem.Add(new XElement("Aliases", string.Join(";", Aliases.Where(x => x != PartID))));

                propsElem.Add(new XElement("Description", PartDescription));

                propsElem.Add(new XElement("PartVersion", PartVersion));

                if (PrimitiveFileVersion != null)
                    propsElem.Add(PrimitiveFileVersion.ToXmlElement("PrimitiveVersion"));

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
                    propsElem.Add(DefaultOrientation.SerializeToXml("DefaultOrientation"));

                if (DefaultCamera != null)
                    propsElem.Add(XmlHelper.DefaultSerialize(DefaultCamera, "DefaultCamera"));

                if (!string.IsNullOrEmpty(Comments))
                    propsElem.Add(new XElement("Comments", Comments));
            }

            var surfacesElem = doc.Root.AddElement("ModelSurfaces");
            foreach (var surf in Surfaces)
                surfacesElem.Add(surf.SerializeToXml());

            var collisionsElem = doc.Root.AddElement("Collisions");
            foreach (var col in Collisions)
                collisionsElem.Add(col.SerializeToXml());

            var connectionsElem = doc.Root.AddElement("Connections");
            foreach (var conn in Connections)
                connectionsElem.Add(conn.SerializeToXml());

            if (Bones.Any())
            {
                var bonesElem = doc.Root.AddElement("Bones");
                foreach (var bone in Bones)
                    bonesElem.Add(bone.SerializeToXml());
            }

            return doc;
        }

        public static PartProject CreateFromXml(XDocument doc)
        {
            var project = new PartProject();
            project.LoadFromXml(doc);
            return project;
        }

        private void LoadFromXml(XDocument doc)
        {
            Surfaces.Clear();
            Connections.Clear();
            Collisions.Clear();
            Bones.Clear();
            Aliases.Clear();

            //Part info
            if (doc.Root.HasElement("Properties", out XElement propsElem))
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
            }

            var surfacesElem = doc.Root.Element("ModelSurfaces");
            if (surfacesElem != null)
            {
                foreach (var surfElem in surfacesElem.Elements(PartSurface.NODE_NAME))
                    Surfaces.Add(PartSurface.FromXml(surfElem));
            }

            var connectionsElem = doc.Root.Element("Connections");
            if (connectionsElem != null)
            {
                foreach (var connElem in connectionsElem.Elements(PartConnection.NODE_NAME))
                    Connections.Add(PartConnection.FromXml(connElem));
            }

            var collisionsElem = doc.Root.Element("Collisions");
            if (collisionsElem != null)
            {
                foreach (var connElem in collisionsElem.Elements(PartCollision.NODE_NAME))
                    Collisions.Add(PartCollision.FromXml(connElem));
            }

            LinkStudReferences();
        }

        #endregion

        #region Read/Write from Directory

        public void SaveExtracted(string directory)
        {
            directory = Path.GetFileName(directory);
            Directory.CreateDirectory(directory);

            LinkStudReferences();
            GenerateMeshIDs(false);
            GenerateMeshesNames();

            var projectXml = GenerateProjectXml();
            projectXml.Save(Path.Combine(directory, "project.xml"));

            string meshDir = Path.Combine(directory, "Meshes");

            if (!Directory.Exists(meshDir))
                Directory.CreateDirectory(meshDir);
            else
            {
                foreach (var filename in Directory.GetFiles(meshDir, "*", SearchOption.AllDirectories))
                    File.Delete(filename);
            }

            var allMeshes = Surfaces.SelectMany(s => s.GetAllMeshes());

            foreach (var mesh in allMeshes)
            {
                mesh.Geometry.Save(Path.Combine(directory, mesh.FileName));
            }
        }

        public static PartProject LoadFromDirectory(string directory)
        {
            var doc = XDocument.Load(Path.Combine(directory, "project.xml"));
            var project = new PartProject
            {
                ProjectPath = directory
            };
            project.LoadFromXml(doc);
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

                
                zipStream.PutNextEntry(new ZipEntry("project.xml"));

                GenerateMeshesNames();
                var projectXml = GenerateProjectXml();

                projectXml.Save(zipStream);

                zipStream.CloseEntry();

                var allMeshes = Surfaces.SelectMany(s => s.GetAllMeshes());

                foreach (var mesh in allMeshes)
                {
                    using(var ms = new MemoryStream())
                    {
                        mesh.Geometry.Save(ms);
                        ms.Position = 0;
                        zipStream.PutNextEntry(new ZipEntry(mesh.FileName));
                        ms.CopyTo(zipStream);
                        zipStream.CloseEntry();
                    }
                }
            }
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
                            .FirstOrDefault(x => x.RefID == comp.ConnectionID);
                    }

                    comp.ConnectionID = linkedConnection?.RefID;
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

        private void GenerateMeshIDs(bool fromLDD)
        {
            //int maxCompCount = Surfaces.Max(s => s.Components.Count);
            var allMeshes = Surfaces.SelectMany(s => s.Components.SelectMany(c => c.GetAllMeshes()));

            foreach (var surface in Surfaces)
            {
                int componentIndex = 0;
                foreach (var component in surface.Components)
                {
                    int meshIndex = 0;

                    foreach (var compMesh in component.GetAllMeshes())
                    {
                        if (!string.IsNullOrEmpty(compMesh.RefID))
                            continue;

                        if (fromLDD)
                        {
                            string uniqueStr = $"{PartID}_{surface.SurfaceID}_{componentIndex}_{meshIndex++}";
                            string refID = StringUtils.GenerateUUID(uniqueStr, 8);

                            while (allMeshes.Any(x => x.RefID == refID))
                            {
                                uniqueStr = $"{PartID}_{surface.SurfaceID}_{componentIndex}_{meshIndex++}";
                                refID = StringUtils.GenerateUUID(uniqueStr, 8);
                            }

                            compMesh.RefID = refID;
                        }
                        else
                            compMesh.RefID = StringUtils.GenerateUID(8);

                        //compMesh.FileName = $"{compMesh.RefID}.geom";
                    }

                    componentIndex++;
                }
            }
        }

        private void GenerateMeshesNames()
        {
            foreach (var surface in Surfaces)
            {
                foreach (var mesh in surface.GetAllMeshes())
                {
                    if (string.IsNullOrEmpty(mesh.FileName) || !mesh.FileName.Contains(mesh.RefID))
                    {
                        //mesh.FileName = $"Meshes\\Surface_{surface.SurfaceID}\\{mesh.RefID}.geom";
                        mesh.FileName = $"Meshes\\{mesh.RefID}.geom";
                    }
                }
            }
        }

        public void GenerateDefaultComments()
        {
            foreach (var surface in Surfaces)
            {
                surface.Comments = surface.SurfaceID == 0 ? "Main surface" : $"Decoration surface {surface.SurfaceID}";

                foreach (var component in surface.Components)
                {

                    switch (component.ComponentType)
                    {
                        case ModelComponentType.Part:
                            component.Comments = "Main surface";
                            break;
                        case ModelComponentType.FemaleStud:
                            component.Comments = "Female studs";
                            break;
                        default:
                            component.Comments = component.ComponentType.ToString();
                            break;
                    }

                    if (component.ComponentType == ModelComponentType.Part)
                        component.Comments += " geometry";

                    if (surface.Components.Count(x => x.ComponentType == component.ComponentType) > 1)
                    {
                        int compIndex = surface.Components.Where(x => x.ComponentType == component.ComponentType).IndexOf(component);
                        component.Comments += $" {compIndex + 1}";
                    }

                    if (component.ComponentType != ModelComponentType.Part)
                        component.Comments += " geometry";
                }
            }
        }

        public MeshGeometry LoadMesh(ModelMesh mesh)
        {
            if (string.IsNullOrEmpty(ProjectPath))
                return null;

            string meshPath = Path.Combine(ProjectPath, mesh.FileName);
            if (File.Exists(meshPath))
                return MeshGeometry.FromFile(meshPath);

            return null;
        }

        #endregion

        #region Change tracking 

        private void Collisions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        private void Connections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        private void Surfaces_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        internal void OnComponentPropertyChanged(PropertyChangedEventArgs pcea)
        {
            ComponentPropertyChanged?.Invoke(this, pcea);
        }

        #endregion


        #region LDD File Generation

        public LDD.Parts.PartWrapper GenerateLddPart()
        {
            return null;
        }

        #endregion
    }
}
