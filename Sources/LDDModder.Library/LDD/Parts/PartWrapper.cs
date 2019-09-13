using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
using LDDModder.Simple3D;

namespace LDDModder.LDD.Parts
{
    public class PartWrapper
    {
        public int PartID { get; set; }
        public Primitives.Primitive Primitive { get; set; }
        public List<PartSurfaceMesh> Surfaces { get; set; }

        public PartSurfaceMesh MainSurface => Surfaces.FirstOrDefault(x => x.SurfaceID == 0);

        public IEnumerable<PartSurfaceMesh> DecorationSurfaces => Surfaces.Where(x => x.SurfaceID > 0);

        public Files.MeshFile MainMesh => MainSurface?.Mesh;

        public IEnumerable<Files.MeshFile> DecorationMeshes => DecorationSurfaces.Select(x => x.Mesh);

        public IEnumerable<Files.MeshFile> AllMeshes => Surfaces.Select(x => x.Mesh);

        public bool IsDecorated => DecorationSurfaces.Any();

        public bool IsFlexible => AllMeshes.Any(x=>x.IsFlexible);

        public PartWrapper()
        {
            Surfaces = new List<PartSurfaceMesh>();
        }

        public PartWrapper(Primitive primitive, IEnumerable<PartSurfaceMesh> surfaces)
        {
            PartID = primitive.ID;
            Primitive = primitive;
            Surfaces = new List<PartSurfaceMesh>(surfaces);
        }

        #region Shader Data Generation

        public void ComputeAverageNormals()
        {
            ShaderDataGenerator.ComputeAverageNormals(AllMeshes.SelectMany(x => x.Triangles));
        }

        public void ComputeEdgeOutlines()
        {
            ShaderDataGenerator.ComputeEdgeOutlines(AllMeshes.SelectMany(x => x.Triangles));
        }

        #endregion

        public BoundingBox CalculateBoundingBox()
        {
            return BoundingBox.FromVertices(AllMeshes.SelectMany(x => x.Vertices));
        }

        #region Loading

        public static PartWrapper LoadPart(LDDEnvironment environment, int partID)
        {
            if (environment.DatabaseExtracted)
            {
                var primitivesDir = Path.Combine(environment.ApplicationDataPath, "db\\Primitives");
                var meshesDir = Path.Combine(environment.ApplicationDataPath, "db\\Primitives\\LOD0");

                var primitiveFile = Path.Combine(primitivesDir, $"{partID}.xml");
                if (!File.Exists(primitiveFile))
                    throw new FileNotFoundException($"Primitive file not found. ({partID}.xml)");

                var surfaces = new List<PartSurfaceMesh>();

                foreach (string meshPath in Directory.GetFiles(meshesDir, $"{partID}.g*"))
                {
                    if (!PartSurfaceMesh.ParseSurfaceID(meshPath, out int surfID))
                        surfID = surfaces.Count;

                    surfaces.Add(new PartSurfaceMesh(partID, surfID, MeshFile.Read(meshPath)));
                }

                if (!surfaces.Any())
                    throw new FileNotFoundException($"Mesh file not found. ({partID}.g)");

                return new PartWrapper(Primitive.Load(primitiveFile), surfaces) { PartID = partID };
            }
            else
            {
                using (var lif = LifFile.Open(Path.Combine(environment.ApplicationDataPath, "db.lif")))
                {
                    var primitiveFolder = lif.GetFolder("Primitives");
                    var meshesFolder = primitiveFolder.GetFolder("LOD0");

                    var primitiveEntry = primitiveFolder.GetFile($"{partID}.xml");
                    if (primitiveEntry == null)
                        throw new FileNotFoundException($"Primitive file not found. ({partID}.xml)");

                    var surfaces = new List<PartSurfaceMesh>();

                    foreach (var meshEntry in meshesFolder.GetFiles($"{partID}.g*"))
                    {
                        if (!PartSurfaceMesh.ParseSurfaceID(meshEntry.Name, out int surfID))
                            surfID = surfaces.Count;

                        surfaces.Add(new PartSurfaceMesh(partID, surfID, MeshFile.Read(meshEntry.GetStream())));
                    }

                    if (!surfaces.Any())
                        throw new FileNotFoundException($"Mesh file not found. ({partID}.g)");

                    var primitive = Primitive.Load(primitiveEntry.GetStream());
                    primitive.ID = partID;
                    return new PartWrapper(primitive, surfaces) { PartID = partID };
                }
            }
        }

        #endregion
    }
}
