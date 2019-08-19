using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
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
        public Mesh MainModel { get; set; }
        public List<Mesh> DecorationMeshes { get; } = new List<Mesh>();
        public Primitive PartInfo { get; set; }

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
                PartInfo = Primitive.FromXmlFile(primitiveFile)
            };

            foreach (string meshFile in Directory.GetFiles(meshesPath, $"{partID}.g*"))
            {
                var mesh = Mesh.Read(meshFile);
                if (meshFile.ToLower().EndsWith("g"))
                    meshInfo.MainModel = mesh;
                else
                    meshInfo.DecorationMeshes.Add(mesh);
            }

            return meshInfo;
        }
    }
}
