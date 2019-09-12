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
    [Obsolete("User PartWrapper instead.")]
    public class LDDPartFiles
    {
        public int PartID { get; set; }

        public MeshFile MainModel { get; set; }
        public List<MeshFile> DecorationMeshes { get; } = new List<MeshFile>();
        
        public Primitive Info { get; set; }

        public IEnumerable<MeshFile> AllMeshes => new MeshFile[] { MainModel }.Concat(DecorationMeshes);

        public LDDPartFiles()
        {
            Info = new Primitive();
        }

        public static LDDPartFiles Read(string lddDbPath, int partID)
        {
            var primitivesPath = Path.Combine(lddDbPath, "Primitives");
            var meshesPath = Path.Combine(lddDbPath, "Primitives", "LOD0");

            var primitiveFile = Path.Combine(primitivesPath, $"{partID}.xml");
            if (!File.Exists(primitiveFile))
                throw new FileNotFoundException($"Part Info not found. ({partID}.xml)");

            if (!File.Exists(Path.Combine(meshesPath, $"{partID}.g")))
                throw new FileNotFoundException($"Part Mesh not found. ({partID}.g)");

            var meshInfo = new LDDPartFiles()
            {
                PartID = partID,
                Info = Primitive.Load(primitiveFile)
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

        public void SaveAll(string directory, string name)
        {
            SaveMeshes(directory, name);
            SavePrimitive(directory, name);
        }

        public void SaveMeshes(string directory, string name)
        {
            using (var fs = File.Open(Path.Combine(directory, name + ".g"), FileMode.Create))
                GFileWriter.WriteMesh(fs, MainModel);

            for (int i = 0; i < DecorationMeshes.Count; i++)
            {
                using (var fs = File.Open(Path.Combine(directory, name + $".g{i + 1}"), FileMode.Create))
                    GFileWriter.WriteMesh(fs, DecorationMeshes[i]);
            }
        }

        public void SavePrimitive(string directory, string name)
        {
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
            ShaderDataGenerator.ComputeAverageNormals(AllMeshes.SelectMany(x => x.Triangles));
        }

        public void ComputeRoundEdgeShader()
        {
            ShaderDataGenerator.ComputeEdgeOutlines(AllMeshes.SelectMany(x => x.Triangles));
        }
    }
}
