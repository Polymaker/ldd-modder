using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives.Connectors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public abstract class SurfaceComponent : PartComponent
    {
        public const string NODE_NAME = "Component";

        public abstract MeshCullingType ComponentType { get; }

        public ComponentCollection<PartMesh> Geometries { get; }

        public SurfaceComponent()
        {
            Geometries = new ComponentCollection<PartMesh>(this);
        }

        public static SurfaceComponent FromLDD(MeshFile mesh, MeshCulling culling)
        {
            var geom = mesh.GetCullingGeometry(culling, false);
            SurfaceComponent component = null;

            switch (culling.Type)
            {
                case MeshCullingType.MainModel:
                    {
                        component = new SurfaceModel();
                        break;
                    }

                case MeshCullingType.Stud:
                    {
                        var item = new SurfaceStud();

                        if (culling.Studs.Count == 1)
                        {
                            item.Stud = new StudReference(culling.Studs[0]);
                        }
                        else
                            Debug.WriteLine("Stud culling does not reference a stud!");

                        component = item;
                        break;
                    }

                case MeshCullingType.FemaleStud:
                    {
                        var item = new SurfaceFemaleStud();
                        item.ReplacementGeometries.Add(new PartMesh(culling.ReplacementMesh));

                        foreach (var studInfo in culling.Studs)
                            item.Studs.Add(new StudReference(studInfo));

                        component = item;
                        break;
                    }

                case MeshCullingType.Tube:
                    {
                        var item = new SurfaceTube();

                        if (culling.Studs.Count == 1)
                        {
                            item.TubeStud = new StudReference(culling.Studs[0]);
                        }
                        else
                            Debug.WriteLine("Tube culling does not reference a stud!");

                        if (culling.AdjacentStuds.Any())
                        {
                            foreach (var fIdx in culling.AdjacentStuds[0].FieldIndices)
                            {
                                item.AdjacentStuds.Add(new StudReference(culling.AdjacentStuds[0].ConnectorIndex,
                                    fIdx.Index, fIdx.Value2, fIdx.Value4));
                            }
                        }

                        component = item;
                        break;
                    }
            }
            if (component != null)
            {
                component.Geometries.Add(new PartMesh(geom));
            }
            return component;
        }

        public abstract IEnumerable<StudReference> GetStudReferences();

        public virtual IEnumerable<PartMesh> GetAllMeshes()
        {
            return Geometries;
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(new XAttribute("Type", ComponentType.ToString()));

            var geomElem = elem.AddElement("Geometries");

            foreach (var geom in Geometries)
                geomElem.Add(geom.SerializeToXml());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            var geomElem = element.Element("Geometries");

            if (geomElem != null)
            {
                foreach (var elem in geomElem.Elements(PartMesh.NODE_NAME))
                {
                    var mesh = new PartMesh();
                    mesh.LoadFromXml(elem);
                    Geometries.Add(mesh);
                }
            }
        }

        public static SurfaceComponent FromXml(XElement element)
        {
            SurfaceComponent component = null;

            switch (element.Attribute("Type").Value)
            {
                case "MainModel":
                    component = new SurfaceModel();
                    break;
                case "Stud":
                    component = new SurfaceStud();
                    break;
                case "FemaleStud":
                    component = new SurfaceFemaleStud();
                    break;
                case "Tube":
                    component = new SurfaceTube();
                    break;
            }

            if (component != null)
                component.LoadFromXml(element);

            return component;
        }

        #endregion
    }

    public class SurfaceModel : SurfaceComponent
    {
        public override MeshCullingType ComponentType => MeshCullingType.MainModel;

        public override IEnumerable<StudReference> GetStudReferences()
        {
            return Enumerable.Empty<StudReference>();
        }
    }

    public class SurfaceStud : SurfaceComponent
    {
        public override MeshCullingType ComponentType => MeshCullingType.Stud;

        public StudReference Stud { get; set; }

        public PartConnector<Custom2DFieldConnector> LinkedConnection => Stud?.Connection;

        public override IEnumerable<StudReference> GetStudReferences()
        {
            if (Stud != null)
                yield return Stud;
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            if (Stud != null)
                elem.Add(Stud.SerializeToXml());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.Element("Stud") != null)
                Stud = StudReference.FromXml(element.Element("Stud"));
        }
    }

    public class SurfaceFemaleStud : SurfaceComponent
    {
        public override MeshCullingType ComponentType => MeshCullingType.FemaleStud;

        //public MeshGeometry ReplacementGeometry { get; set; }
        public ComponentCollection<PartMesh> ReplacementGeometries { get; set; }

        public List<StudReference> Studs { get; set; }

        public PartConnector<Custom2DFieldConnector> LinkedConnection => Studs.FirstOrDefault()?.Connection;

        public SurfaceFemaleStud()
        {
            Studs = new List<StudReference>();
            ReplacementGeometries = new ComponentCollection<PartMesh>(this);
        }

        public override IEnumerable<StudReference> GetStudReferences()
        {
            return Studs;
        }

        public override IEnumerable<PartMesh> GetAllMeshes()
        {
            return base.GetAllMeshes().Concat(ReplacementGeometries);
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            elem.Add(new XComment("This geometry is used when all the studs (defined bellow) are connected"));
            var geomElem = elem.AddElement("ReplacementGeometries");
            foreach (var geom in ReplacementGeometries)
                geomElem.Add(geom.SerializeToXml());

            var studsElem = elem.AddElement("Studs");
            foreach (var stud in Studs)
                studsElem.Add(stud.SerializeToXml());

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            var geomElem = element.Element("ReplacementGeometries");
            if (geomElem != null)
            {
                foreach (var elem in geomElem.Elements(PartMesh.NODE_NAME))
                {
                    var mesh = new PartMesh();
                    mesh.LoadFromXml(elem);
                    ReplacementGeometries.Add(mesh);
                }
            }

            if (element.Element("Studs") != null)
            {
                foreach (var studElem in element.Element("Studs").Elements("Stud"))
                    Studs.Add(StudReference.FromXml(studElem));
            }
        }
    }

    public class SurfaceTube : SurfaceComponent
    {
        public override MeshCullingType ComponentType => MeshCullingType.Tube;

        public StudReference TubeStud { get; set; }

        public List<StudReference> AdjacentStuds { get; set; }

        public SurfaceTube()
        {
            AdjacentStuds = new List<StudReference>();
        }

        public override IEnumerable<StudReference> GetStudReferences()
        {
            if (TubeStud != null)
                yield return TubeStud;
            foreach (var stud in AdjacentStuds)
                yield return stud;
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            if (TubeStud != null)
                elem.Add(TubeStud.SerializeToXml("TubeStud"));
            elem.Add(new XComment("The following 4 studs are adjacent to the tube"));
            var studsElem = elem.AddElement("AdjacentStuds");
            foreach (var stud in AdjacentStuds)
                studsElem.Add(stud.SerializeToXml());
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            if (element.Element("TubeStud") != null)
                TubeStud = StudReference.FromXml(element.Element("TubeStud"));

            if (element.Element("AdjacentStuds") != null)
            {
                foreach (var studElem in element.Element("AdjacentStuds").Elements("Stud"))
                    AdjacentStuds.Add(StudReference.FromXml(studElem));
            }
        }
    }
}
