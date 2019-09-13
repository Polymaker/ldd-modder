using LDDModder.LDD.Data;
using LDDModder.LDD.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public class PartProject
    {
        #region Part Info

        public int PartID { get; set; }

        public string PartDescription { get; set; }

        public string Comments { get; set; }

        public List<int> Aliases { get; set; }

        public Platform Platform { get; set; }

        public MainGroup MainGroup { get; set; }

        public PhysicsAttributes PhysicsAttributes { get; set; }
        public BoundingBox Bounding { get; set; }
        public BoundingBox GeometryBounding { get; set; }
        public Transform DefaultOrientation { get; set; }
        public Camera DefaultCamera { get; set; }

        public VersionInfo PrimitiveFileVersion { get; set; }
        public int PartVersion { get; set; }

        public bool Decorated { get; set; }

        public bool Flexible { get; set; }

        #endregion

        public List<PartSurface> Surfaces { get; set; }

        public List<PartConnector> Connectors { get; set; }

        public List<PartCollision> Collisions { get; set; }

        public PartProject()
        {
            Surfaces = new List<PartSurface>();
            Connectors = new List<PartConnector>();
            Collisions = new List<PartCollision>();
        }

        public void ValidatePart()
        {
            //TODO

            if (Decorated && !Surfaces.Any(x=>x.SurfaceID > 0))
            {

            }
        }

        #region Creation

        public static PartProject CreateFromLddPart(LDD.LDDEnvironment environment, int partID)
        {
            var lddPart = LDD.Parts.PartWrapper.LoadPart(environment, partID);

            var project = new PartProject();
            project.SetBaseInfo(lddPart);

            foreach (var collision in lddPart.Primitive.Collisions)
                project.Collisions.Add(PartCollision.FromLDD(collision));

            foreach (var lddConn in lddPart.Primitive.Connectors)
            {
                var partConn = PartConnector.FromLDD(lddConn);
                if (partConn is StudConnection stud)
                {
                    int connIdx = lddPart.Primitive.Connectors.IndexOf(lddConn);
                    stud.RefID = Utilities.StringUtils.GenerateUUID($"{partID}_{connIdx}", 8);
                }
                project.Connectors.Add(partConn);
            }

            foreach (var meshSurf in lddPart.Surfaces)
            {
                var partSurf = new PartSurface()
                {
                    SurfaceID = meshSurf.SurfaceID,
                    SubMaterialIndex = lddPart.Primitive.GetSurfaceMaterialIndex(meshSurf.SurfaceID)
                };
                project.Surfaces.Add(partSurf);

                foreach (var culling in meshSurf.Mesh.Cullings)
                    partSurf.Components.Add(SurfaceComponent.FromLDD(meshSurf.Mesh, culling));
            }

            project.LinkStudReferences();

            return project;
        }

        #endregion

        #region Saving

        public XDocument GenerateProjectXml()
        {
            var doc = new XDocument(new XElement("LDDPART"));
            var partElem = doc.Root.AddElement("PartInfo");
            partElem.Add(new XElement("PartID", new XAttribute("Value", PartID)));
            partElem.Add(new XElement("Description", new XAttribute("Value", PartDescription)));
            if (!string.IsNullOrEmpty(Comments))
                partElem.Add(new XElement("Comments", Comments));

            var surfacesElem = doc.Root.AddElement("Surfaces");

            foreach (var surf in Surfaces)
            {
                var surfElem = surf.SerializeToXml();
                surfacesElem.Add(surfElem);

                foreach (var comp in surf.Components)
                {
                    var compElem = comp.SerializeToXml();
                    surfElem.Add(compElem);
                }
            }

            var collisionsElem = doc.Root.AddElement("Collisions");
            foreach (var col in Collisions)
                collisionsElem.Add(col.SerializeToXml());

            var connectionsElem = doc.Root.AddElement("Connections");
            foreach (var conn in Connectors)
                connectionsElem.Add(conn.SerializeToXml());

            return doc;
        }

        public void Save(string path)
        {
            var projectXml = GenerateProjectXml();
            projectXml.Save(path);
        }

        #endregion

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
        }

        private void LinkStudReferences()
        {
            foreach (var surf in Surfaces)
            {
                foreach (var comp in surf.Components)
                {
                    foreach (var stud in comp.GetStudReferences())
                    {
                        if (stud.ConnectorIndex != -1)
                        {
                            if (stud.ConnectorIndex < Connectors.Count && Connectors[stud.ConnectorIndex] is StudConnection studConnection)
                            {
                                stud.Connection = studConnection;
                                stud.RefID = studConnection.RefID;
                            }
                            else
                            {
                                string refID = Utilities.StringUtils.GenerateUUID($"{PartID}_{stud.ConnectorIndex}", 8);
                                stud.Connection = Connectors.OfType<StudConnection>().FirstOrDefault(x => x.RefID == refID);
                                stud.RefID = stud.Connection?.RefID;
                            }
                        }
                        else if (!string.IsNullOrEmpty(stud.RefID))
                        {
                            stud.Connection = Connectors.OfType<StudConnection>().FirstOrDefault(x => x.RefID == stud.RefID);
                            stud.ConnectorIndex = Connectors.IndexOf(stud.Connection);
                        }
                    }
                }
            }
        }
    }
}
