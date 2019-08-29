using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Data
{
    public class PartMesh
    {
        public MeshFile MainModel { get; set; }
        public List<MeshFile> DecorationMeshes { get; } = new List<MeshFile>();
        
        public Primitive Info { get; set; }

        public IEnumerable<MeshFile> AllMeshes => new MeshFile[] { MainModel }.Concat(DecorationMeshes);

        public PartMesh()
        {
            Info = new Primitive();
        }

        public static PartMesh Read(string lddDbPath, int partID)
        {
            var primitivesPath = Path.Combine(lddDbPath, "Primitives");
            var meshesPath = Path.Combine(lddDbPath, "Primitives", "LOD0");

            var primitiveFile = Path.Combine(primitivesPath, $"{partID}.xml");
            if (!File.Exists(primitiveFile))
                throw new FileNotFoundException($"Part Info not found. ({partID}.xml)");

            if (!File.Exists(Path.Combine(meshesPath, $"{partID}.g")))
                throw new FileNotFoundException($"Part Mesh not found. ({partID}.g)");

            var meshInfo = new PartMesh()
            {
                Info = Primitive.FromXmlFile(primitiveFile)
            };

            foreach (string meshFile in Directory.GetFiles(meshesPath, $"{partID}.g*"))
            {
                var mesh = MeshFile.Read(meshFile);
                if (meshFile.ToLower().EndsWith("g"))
                    meshInfo.MainModel = mesh;
                else
                    meshInfo.DecorationMeshes.Add(mesh);
            }

            return meshInfo;
        }

        public void Save(string directory, string name)
        {
            using (var fs = File.Open(Path.Combine(directory, name + ".g"), FileMode.Create))
                GFileWriter.WriteMesh(fs, MainModel);

            for (int i = 0; i < DecorationMeshes.Count; i++)
            {
                using (var fs = File.Open(Path.Combine(directory, name + $".g{i + 1}"), FileMode.Create))
                    GFileWriter.WriteMesh(fs, DecorationMeshes[i]);
            }

            Info.Save(Path.Combine(directory, name + ".xml"));
        }

        public bool CheckFilesExists(string directory, string name)
        {
            if (File.Exists(Path.Combine(directory, name + ".g")))
                return true;

            if (File.Exists(Path.Combine(directory, name + ".xml")))
                return true;

            for (int i = 0; i < DecorationMeshes.Count; i++)
            {
                if (File.Exists(Path.Combine(directory, name + $".g{i + 1}")))
                    return true;
            }

            return false;
        }

        public BoundingBox GetBoundingBox()
        {
            var bounds = new List<BoundingBox>();
            foreach (var m in AllMeshes)
                bounds.Add(BoundingBox.FromVertices(m.Vertices));

            var min = bounds[0].Min;
            var max = bounds[0].Max;

            for (int i = 1; i < bounds.Count; i++)
            {
                min = Simple3D.Vector3.Min(min, bounds[i].Min);
                max = Simple3D.Vector3.Max(max, bounds[i].Max);
            }

            return new BoundingBox() { Min = min, Max = max };
        }

        public void ComputeAverageNormals()
        {
            var vfD = new Dictionary<Vector3, List<Triangle>>();

            foreach (var m in AllMeshes)
            {
                foreach (var tri in m.Triangles)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (!vfD.ContainsKey(tri.Vertices[i].Position))
                            vfD.Add(tri.Vertices[i].Position, new List<Triangle>());
                        //if (!vfD[tri.Vertices[i].Position].Contains(tri))
                        vfD[tri.Vertices[i].Position].Add(tri);
                    }
                }
            }

            var vND = new Dictionary<Vector3, Vector3>();

            foreach (var kv in vfD)
            {
                var faceNormals = kv.Value.Select(x => x.Normal).DistinctValues().ToList();
                Vector3 avgNormal = Vector3.Zero;
                foreach (var norm in faceNormals)
                    avgNormal += norm;

                avgNormal /= faceNormals.Count;
                avgNormal.Normalize();

                vND.Add(kv.Key, avgNormal);
            }

            foreach (var m in AllMeshes)
            {
                foreach(var idx in m.Indices)
                    idx.AverageNormal = vND[idx.Vertex.Position];
            }
        }

        public void ComputeRoundEdgeShader()
        {

        }
    }
}
