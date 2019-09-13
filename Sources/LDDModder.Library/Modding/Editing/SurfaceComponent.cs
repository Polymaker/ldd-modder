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
        public List<MeshGeometry> Geometries { get; set; }

        public SurfaceComponent()
        {
            Geometries = new List<MeshGeometry>();
        }

        public static SurfaceComponent FromLDD(MeshFile mesh, MeshCulling culling)
        {
            var geom = mesh.GetCullingGeometry(culling, false);

            switch (culling.Type)
            {
                case MeshCullingType.MainModel:
                    {
                        var model = new SurfaceModel();
                        model.Geometries.Add(geom);
                        return model;
                    }

                case MeshCullingType.Stud:
                    {
                        var item = new SurfaceStud();
                        item.Geometries.Add(geom);

                        if (culling.Studs.Count == 1)
                        {
                            item.Stud = new StudReference(culling.Studs[0]);
                        }
                        else
                            Debug.WriteLine("Stud culling does not reference a stud!");

                        return item;
                    }

                case MeshCullingType.FemaleStud:
                    {
                        var item = new SurfaceFemaleStud();
                        item.Geometries.Add(geom);
                        item.ReplacementGeometry = culling.ReplacementMesh;

                        foreach (var studInfo in culling.Studs)
                            item.Studs.Add(new StudReference(studInfo));

                        return item;
                    }

                case MeshCullingType.Tube:
                    {
                        var item = new SurfaceTube();
                        item.Geometries.Add(geom);

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

                        return item;
                    }
            }

            return null;
        }

        public abstract IEnumerable<StudReference> GetStudReferences();

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase("Component");
            elem.Add(new XAttribute("Type", ComponentType.ToString()));

            foreach (var stud in GetStudReferences())
            {
                elem.Add(stud.SerializeToXml());
            }
            return elem;
        }
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
            return new StudReference[] { Stud };
        }
    }

    public class SurfaceFemaleStud : SurfaceComponent
    {
        public override MeshCullingType ComponentType => MeshCullingType.FemaleStud;

        public MeshGeometry ReplacementGeometry { get; set; }

        public List<StudReference> Studs { get; set; }

        public SurfaceFemaleStud()
        {
            Studs = new List<StudReference>();
        }

        public override IEnumerable<StudReference> GetStudReferences()
        {
            return Studs;
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
    }
}
