using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Data
{
    public class PartSurfaceMesh
    {
        public int PartID { get; }
        public int SurfaceID { get; set; }
        public MeshFile Mesh { get; set; }

        public string GetFileName()
        {
            if (SurfaceID > 0)
                return $"{PartID}.g{SurfaceID}";
            return $"{PartID}.g";
        }

        public PartSurfaceMesh(int partID, int surfaceID, MeshFile mesh)
        {
            PartID = partID;
            SurfaceID = surfaceID;
            Mesh = mesh;
        }


    }
}
