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
    public class PartProject
    {

        #region Part Info

        [XmlElement]
        public int PartID { get; private set; }

        [XmlElement]
        public string PartDescription { get; set; }

        [XmlElement]
        public string Comments { get; set; }

        public List<int> Aliases { get; set; }

        public Platform Platform { get; set; }

        public MainGroup MainGroup { get; set; }

        [XmlElement]
        public PhysicsAttributes PhysicsAttributes { get; set; }

        [XmlElement]
        public BoundingBox Bounding { get; set; }

        [XmlElement]
        public BoundingBox GeometryBounding { get; set; }

        [XmlElement]
        public ItemTransform DefaultOrientation { get; set; }

        [XmlElement]
        public Camera DefaultCamera { get; set; }

        public VersionInfo PrimitiveFileVersion { get; set; }

        public int PartVersion { get; set; }

        public bool Decorated { get; set; }

        public bool Flexible { get; set; }

        #endregion

        public string ProjectPath { get; private set; }

        public ComponentCollection<PartSurface> Surfaces { get; }

        public ComponentCollection<PartConnector> Connections { get; }

        public ComponentCollection<PartCollision> Collisions { get; }

        public ComponentCollection<PartBone> Bones { get; }

        public ComponentCollection<PartMesh> UnassignedMeshes { get; }

        public bool IsLoading { get; internal set; }

        public event EventHandler<PropertyChangedEventArgs> ComponentPropertyChanged;

        public PartProject()
        {
            Surfaces = new ComponentCollection<PartSurface>(this);
            Connections = new ComponentCollection<PartConnector>(this);
            Collisions = new ComponentCollection<PartCollision>(this);
            Bones = new ComponentCollection<PartBone>(this);
            UnassignedMeshes = new ComponentCollection<PartMesh>(this);

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
                var partConn = PartConnector.FromLDD(lddConn);
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

            project.GenerateDefaultComments();

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
            var doc = new XDocument(new XElement("LDDPART"));

            //Part Info
            {
                var partElem = doc.Root.AddElement("PartInfo");
                partElem.Add(new XElement("PartID", PartID));

                if (Aliases.Where(x => x != PartID).Any())
                    partElem.Add(new XElement("Aliases", string.Join(";", Aliases.Where(x => x != PartID))));

                partElem.Add(new XElement("Description", PartDescription));

                partElem.Add(new XElement("PartVersion", PartVersion));
                if (PrimitiveFileVersion != null)
                    partElem.Add(PrimitiveFileVersion.ToXmlElement("PrimitiveVersion"));
                
                if (Platform != null)
                    partElem.AddElement("Platform", new XAttribute("ID", Platform.ID), new XAttribute("Name", Platform.Name));

                if (MainGroup != null)
                    partElem.AddElement("MainGroup", new XAttribute("ID", MainGroup.ID), new XAttribute("Name", MainGroup.Name));

                if (!string.IsNullOrEmpty(Comments))
                    partElem.Add(new XElement("Comments", Comments));
            }
            
            if (Helper.AnyNotNull(Bounding, GeometryBounding, DefaultCamera, PhysicsAttributes, DefaultOrientation))
            {
                var propsElem = doc.Root.AddElement("Properties");
                if (PhysicsAttributes != null)
                    propsElem.Add(PhysicsAttributes.SerializeToXml());
                if (Bounding != null)
                    propsElem.Add(XmlHelper.DefaultSerialize(Bounding, "Bounding"));
                if (GeometryBounding != null)
                    propsElem.Add(XmlHelper.DefaultSerialize(GeometryBounding, "GeometryBounding"));
                if (DefaultOrientation != null)
                    propsElem.Add(DefaultOrientation.SerializeToXml("DefaultOrientation"));
            }

            var surfacesElem = doc.Root.AddElement("Surfaces");

            foreach (var surf in Surfaces)
            {
                var surfElem = surf.SerializeToXml();
                surfacesElem.Add(surfElem);
            }

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

            //Part info
            {
                var partElem = doc.Root.Element("PartInfo");
                project.PartID = int.Parse(partElem.Element("PartID")?.Value);

            }

            //Part properties
            var propsElem = doc.Root.Element("Properties");
            if (propsElem != null)
            {

            }

            var surfacesElem = doc.Root.Element("Surfaces");

            if (surfacesElem != null)
            {
                foreach (var surfElem in surfacesElem.Elements(PartSurface.NODE_NAME))
                    project.Surfaces.Add(PartSurface.FromXml(surfElem));
            }

            return null;
        }

        private void LoadFromXml(XDocument doc)
        {
            Surfaces.Clear();
            Connections.Clear();
            Collisions.Clear();

            //Part info
            {
                var partElem = doc.Root.Element("PartInfo");
                PartID = int.Parse(partElem.Element("PartID")?.Value);

                var test = new
                {
                    PartID,
                    PartDescription,
                    Platform
                };

            }

            //Part properties
            var propsElem = doc.Root.Element("Properties");
            if (propsElem != null)
            {

            }

            var surfacesElem = doc.Root.Element("Surfaces");
            
            if (surfacesElem != null)
            {
                foreach (var surfElem in surfacesElem.Elements(PartSurface.NODE_NAME))
                    Surfaces.Add(PartSurface.FromXml(surfElem));
            }

            var connectionsElem = doc.Root.Element("Connections");
            if (connectionsElem != null)
            {
                foreach (var connElem in connectionsElem.Elements(PartConnector.NODE_NAME))
                    Connections.Add(PartConnector.FromXml(connElem));
            }

            LinkStudReferences();
        }

        #endregion

        #region Read/Write from Directory

        public void Save(string directory)
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



        #endregion

        #region Methods

        private void LinkStudReferences()
        {
            foreach (var surf in Surfaces)
            {
                foreach (var comp in surf.Components)
                {
                    foreach (var stud in comp.GetStudReferences())
                    {
                        if (stud.Connection == null)
                        {
                            if (stud.ConnectorIndex != -1)
                            {
                                if (stud.ConnectorIndex < Connections.Count &&
                                    Connections[stud.ConnectorIndex].ConnectorType == ConnectorType.Custom2DField)
                                {
                                    stud.Connection = (PartConnector<Custom2DFieldConnector>)Connections[stud.ConnectorIndex];
                                }
                                else
                                {
                                    string refID = Utilities.StringUtils.GenerateUUID($"{PartID}_{stud.ConnectorIndex}", 8);
                                    stud.Connection = Connections.OfType<PartConnector<Custom2DFieldConnector>>()
                                        .FirstOrDefault(x => x.RefID == refID);
                                }
                            }
                            else if (!string.IsNullOrEmpty(stud.RefID))
                            {
                                stud.Connection = Connections.OfType<PartConnector<Custom2DFieldConnector>>()
                                    .FirstOrDefault(x => x.RefID == stud.RefID);
                            }
                        }

                        if (stud.Connection != null)
                        {
                            stud.ConnectionID = stud.Connection.RefID;
                            stud.ConnectorIndex = Connections.IndexOf(stud.Connection);
                        }
                    }
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
                        case LDD.Meshes.MeshCullingType.MainModel:
                            component.Comments = "Main surface";
                            break;
                        case LDD.Meshes.MeshCullingType.FemaleStud:
                            component.Comments = "Female studs";
                            break;
                        default:
                            component.Comments = component.ComponentType.ToString();
                            break;
                    }

                    if (component.ComponentType == LDD.Meshes.MeshCullingType.MainModel)
                        component.Comments += " geometry";

                    if (surface.Components.Count(x => x.ComponentType == component.ComponentType) > 1)
                    {
                        int compIndex = surface.Components.Where(x => x.ComponentType == component.ComponentType).IndexOf(component);
                        component.Comments += $" {compIndex + 1}";
                    }

                    if (component.ComponentType != LDD.Meshes.MeshCullingType.MainModel)
                        component.Comments += " geometry";
                }
            }
        }

        public MeshGeometry LoadMesh(PartMesh mesh)
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
