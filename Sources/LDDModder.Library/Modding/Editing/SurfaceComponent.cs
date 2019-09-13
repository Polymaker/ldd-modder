using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
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
        public abstract MeshCullingType ComponentType { get; }
        public List<PartMesh> Geometries { get; set; }



        public SurfaceComponent()
        {
            Geometries = new List<PartMesh>();
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

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase("Component");
            elem.Add(new XAttribute("Type", ComponentType.ToString()));

            //foreach (var stud in GetStudReferences())
            //{
            //    elem.Add(stud.SerializeToXml());
            //}
            var geomElem = elem.AddElement("Geometries");
            foreach (var geom in Geometries)
                geomElem.Add(geom.SerializeToXml());
            return elem;
        }

        //protected object[] SerializeStud(StudReference stud, string elementName = "Stud")
        //{
        //    if (stud.FieldNode != null)
        //    {
        //        return new object[]
        //        {
        //            new XComment($"Stud position X: {stud.FieldNode.X} Y: {stud.FieldNode.Y}"),
        //            stud.SerializeToXml(elementName)
        //        };
        //    }

        //    return new object[]
        //    {
        //        stud.SerializeToXml(elementName)
        //    };
        //}
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
    }

    public class SurfaceFemaleStud : SurfaceComponent
    {
        public override MeshCullingType ComponentType => MeshCullingType.FemaleStud;

        //public MeshGeometry ReplacementGeometry { get; set; }
        public List<PartMesh> ReplacementGeometries { get; set; }

        public List<StudReference> Studs { get; set; }

        public SurfaceFemaleStud()
        {
            Studs = new List<StudReference>();
            ReplacementGeometries = new List<PartMesh>();
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
    }
}
