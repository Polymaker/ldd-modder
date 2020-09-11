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
        public Primitive Primitive { get; set; }

        public List<PartSurfaceMesh> Surfaces { get; set; }

        public PartSurfaceMesh MainSurface => Surfaces.FirstOrDefault(x => x.SurfaceID == 0);

        public IEnumerable<PartSurfaceMesh> DecorationSurfaces => Surfaces.Where(x => x.SurfaceID > 0);

        public MeshFile MainMesh => MainSurface?.Mesh;

        public IEnumerable<Files.MeshFile> DecorationMeshes => DecorationSurfaces.Select(x => x.Mesh);

        public IEnumerable<Files.MeshFile> AllMeshes => Surfaces.Where(x => x.Mesh != null).Select(x => x.Mesh);

        public bool IsDecorated => DecorationSurfaces.Any();

        public bool IsFlexible => AllMeshes.Any(x => x.IsFlexible) || Primitive.FlexBones.Any();

        public string Filepath { get; set; }

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

        public void AddSurfaceMesh(int surfaceID, MeshFile meshFile)
        {
            Surfaces.Add(new PartSurfaceMesh(PartID, surfaceID, meshFile));
        }

        #region Shader Data Generation

        public void ComputeAverageNormals()
        {
            ShaderDataGenerator.ComputeAverageNormals(AllMeshes.SelectMany(x => x.Triangles));
        }

        public void ComputeEdgeOutlines(float breakAngle = 35f)
        {
            OutlinesGenerator.GenerateOutlines(AllMeshes.SelectMany(x => x.Triangles), breakAngle);
            //ShaderDataGenerator.ComputeEdgeOutlines(AllMeshes.SelectMany(x => x.Triangles), breakAngle);
        }

        public void ClearEdgeOutlines()
        {
            foreach (var tri in AllMeshes.SelectMany(x => x.Triangles))
            {
                if (tri.Indices[0].RoundEdgeData != null)
                    tri.Indices[0].RoundEdgeData.Reset();
            }
        }

        #endregion

        public BoundingBox CalculateBoundingBox()
        {
            return BoundingBox.FromVertices(AllMeshes.SelectMany(x => x.Vertices));
        }

        #region Loading

        public static PartWrapper LoadPart(LDDEnvironment environment, int partID, bool loadMeshes)
        {
            if (environment.DatabaseExtracted)
            {
                var primitivesDir = environment.GetAppDataSubDir("db\\Primitives");
                return GetPartFromDirectory(primitivesDir, partID, loadMeshes);
            }
            else
            {
                using (var lif = LifFile.Open(environment.GetLifFilePath(LddLif.DB)))
                    return GetPartFromLif(lif, partID, loadMeshes);
            }
        }

        public static PartWrapper GetPartFromDirectory(string path, int partID, bool loadMeshes)
        {
            //var primitivesDir = Path.Combine(path, "db\\Primitives");
            //var meshesDir = Path.Combine(path, "db\\Primitives\\LOD0");

            var primitivesDir = path;
            var meshesDir = Path.Combine(path, "LOD0");

            var primitiveFile = Path.Combine(primitivesDir, $"{partID}.xml");
            if (!File.Exists(primitiveFile))
                throw new FileNotFoundException($"Primitive file not found. ({partID}.xml)");

            var surfaces = new List<PartSurfaceMesh>();

            if (Directory.Exists(meshesDir))
            {
                foreach (string meshPath in Directory.GetFiles(meshesDir, $"{partID}.g*"))
                {
                    if (!PartSurfaceMesh.ParseSurfaceID(meshPath, out int surfID))
                        surfID = surfaces.Count;

                    var surfaceInfo = new PartSurfaceMesh(partID, surfID, loadMeshes ? MeshFile.Read(meshPath) : null)
                    {
                        Filepath = meshPath
                    };
                    surfaces.Add(surfaceInfo);
                }
            }

            if (!surfaces.Any())
                throw new FileNotFoundException($"Mesh file not found. ({partID}.g)");

            return new PartWrapper(Primitive.Load(primitiveFile), surfaces) 
            { 
                PartID = partID, 
                Filepath = primitiveFile 
            };
        }

        public static PartWrapper GetPartFromLif(LifFile lif, int partID, bool loadMeshes)
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

                var surfaceInfo = new PartSurfaceMesh(partID, surfID,
                    loadMeshes ? MeshFile.Read(meshEntry.GetStream()) : null);
                surfaces.Add(surfaceInfo);
            }

            if (!surfaces.Any())
                throw new FileNotFoundException($"Mesh file not found. ({partID}.g)");

            var primitive = Primitive.Load(primitiveEntry.GetStream());
            primitive.ID = partID;
            return new PartWrapper(primitive, surfaces) { PartID = partID };
        }

        public static Primitive GetPrimitiveInfo(LDDEnvironment environment, int partID)
        {
            if (environment.DatabaseExtracted)
            {
                var primitivesDir = Path.Combine(environment.ApplicationDataPath, "db\\Primitives");

                var primitiveFile = Path.Combine(primitivesDir, $"{partID}.xml");
                if (!File.Exists(primitiveFile))
                    throw new FileNotFoundException($"Primitive file not found. ({partID}.xml)");

                return Primitive.Load(primitiveFile);
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

                    return Primitive.Load(primitiveEntry.GetStream());
                }
            }
        }

        #endregion

        #region Saving

        public void SaveToLdd(LDDEnvironment environment)
        {
            var primitivesDir = environment.GetAppDataSubDir("db\\Primitives\\");
            var meshesDir = environment.GetAppDataSubDir("db\\Primitives\\LOD0\\");

            SaveToDirectory(primitivesDir, meshesDir);
        }

        public void SaveToDirectory(string primitiveDirectory, string meshesDirectory)
        {
            SavePrimitive(primitiveDirectory);
            SaveMeshes(meshesDirectory);
        }

        public void SavePrimitive(string targetDirectory)
        {
            Directory.CreateDirectory(targetDirectory);
            Filepath = Path.Combine(targetDirectory, $"{PartID}.xml");
            Primitive.Save(Filepath);
        }

        public void SaveMeshes(string targetDirectory)
        {
            Directory.CreateDirectory(targetDirectory);

            foreach (var surface in Surfaces)
            {
                string meshPath = Path.Combine(targetDirectory, surface.GetFileName());
                surface.Mesh.Save(meshPath);
                surface.Filepath = meshPath;
            }
        }

        #endregion

        public bool CheckFilesExists(string primitiveDir, string meshesDir)
        {
            if (Directory.Exists(primitiveDir) &&
                File.Exists(Path.Combine(primitiveDir, $"{PartID}.xml")))
                return true;

            if (!Directory.Exists(meshesDir))
                return false;

            foreach (var surface in Surfaces)
            {
                if (File.Exists(Path.Combine(meshesDir, surface.GetFileName())))
                    return true;
            }

            return false;
        }
    
        public static List<PartSurfaceMesh> FindSurfaceMeshes(string directory, int partID)
        {
            var surfaces = new List<PartSurfaceMesh>();

            if (Directory.Exists(directory))
            {
                foreach (string meshPath in Directory.GetFiles(directory, $"{partID}.g*"))
                {
                    if (!PartSurfaceMesh.ParseSurfaceID(meshPath, out int surfID))
                        surfID = surfaces.Count;

                    surfaces.Add(new PartSurfaceMesh(partID, surfID,  null));
                }
            }

            return surfaces;
        }
    }
}
