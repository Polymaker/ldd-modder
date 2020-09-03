using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Parts
{
    public class PartSurfaceMesh
    {
        public int PartID { get; }
        public int SurfaceID { get; set; }
        public MeshFile Mesh { get; set; }

        public string Filepath { get; set; }

        public string GetFileName()
        {
            return GetFileName(PartID, SurfaceID);
        }

        public static string GetFileName(int partID, int surfaceID)
        {
            if (surfaceID > 0)
                return $"{partID}.g{surfaceID}";
            return $"{partID}.g";
        }

        public PartSurfaceMesh(int partID, int surfaceID)
        {
            PartID = partID;
            SurfaceID = surfaceID;
        }

        public PartSurfaceMesh(int partID, int surfaceID, MeshFile mesh)
        {
            PartID = partID;
            SurfaceID = surfaceID;
            Mesh = mesh;
        }

        public static int ParseSurfaceID(string filename)
        {
            string surfIdStr = System.IO.Path.GetExtension(filename).TrimStart('.').Replace('g', '0');
            if (int.TryParse(surfIdStr, out int surfID))
                return surfID;
            return 0;
        }

        public static bool ParseSurfaceID(string filename, out int surfaceID)
        {
            string surfIdStr = System.IO.Path.GetExtension(filename).TrimStart('.').Replace('g', '0');
            if (int.TryParse(surfIdStr, out surfaceID))
                return true;
            surfaceID = 0;
            return false;
        }
    }
}
