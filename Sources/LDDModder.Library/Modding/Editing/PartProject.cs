using LDDModder.LDD.Data;
using LDDModder.LDD.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class PartProject
    {
        #region Part Info

        public int PartID { get; set; }

        public string PartDescription { get; set; }

        public List<int> Aliases { get; set; }

        public Platform Platform { get; set; }

        public MainGroup MainGroup { get; set; }

        public PhysicsAttributes PhysicsAttributes { get; set; }
        public BoundingBox Bounding { get; set; }
        public BoundingBox GeometryBounding { get; set; }
        public Transform DefaultOrientation { get; set; }
        public Camera DefaultCamera { get; set; }

        public VersionInfo PrimitiveFileVersion { get; set; }
        public int PartVersion { get; set; }

        public bool Decorated { get; set; }

        public bool Flexible { get; set; }

        #endregion

        public List<PartSurface> Surfaces { get; set; }

        public List<PartConnector> Connectors { get; set; }

        public List<PartCollision> Collisions { get; set; }

        public PartProject()
        {
            Surfaces = new List<PartSurface>();
            Connectors = new List<PartConnector>();
            Collisions = new List<PartCollision>();
        }

        public void ValidatePart()
        {
            //TODO

            if (Decorated && !Surfaces.Any(x=>x.SurfaceID > 0))
            {

            }
        }

        #region MyRegion

        //public static PartProject CreateFromLddPart(LDD.Parts.PartWrapper lddPart)
        //{
        //    var project = new PartProject()
        //    {
        //        PartID = lddPart.PartID,
        //        PartDescription = lddPart.Primitive.Name,
        //        PartVersion = lddPart.Primitive.PartVersion,
        //    };

        //    foreach (var meshSurf in lddPart.Surfaces)
        //    {
        //        var partSurf = new PartSurface()
        //        {
        //            SurfaceID = meshSurf.SurfaceID,
        //            SubMaterialIndex = lddPart.Primitive.GetSurfaceMaterialIndex(meshSurf.SurfaceID)
        //        };
        //        project.Surfaces.Add(partSurf);
        //    }

        //    foreach (var collision in lddPart.Primitive.Collisions)
        //        project.Collisions.Add(PartCollision.FromLDD(collision));

        //    return project;
        //}

        public static PartProject CreateFromLddPart(LDD.LDDEnvironment environment, int partID)
        {
            var lddPart = LDD.Parts.PartWrapper.LoadPart(environment, partID);

            var project = new PartProject()
            {
                PartID = lddPart.PartID,
                PartDescription = lddPart.Primitive.Name,
                PartVersion = lddPart.Primitive.PartVersion,
            };

            foreach (var meshSurf in lddPart.Surfaces)
            {
                var partSurf = new PartSurface()
                {
                    SurfaceID = meshSurf.SurfaceID,
                    SubMaterialIndex = lddPart.Primitive.GetSurfaceMaterialIndex(meshSurf.SurfaceID)
                };
                project.Surfaces.Add(partSurf);

                foreach (var cull in meshSurf.Mesh.Cullings)
                    partSurf.Components.Add(SurfaceComponent.FromLDD(meshSurf.Mesh, cull));
            }

            foreach (var collision in lddPart.Primitive.Collisions)
                project.Collisions.Add(PartCollision.FromLDD(collision));

            foreach (var lddConn in lddPart.Primitive.Connectors)
            {
                var partConn = PartConnector.FromLDD(lddConn);
                if (partConn is StudConnection stud)
                {
                    int connIdx = lddPart.Primitive.Connectors.IndexOf(lddConn);
                    stud.RefID = Utilities.StringUtils.GenerateUUID($"{partID}_{connIdx}", 8);
                }
                project.Connectors.Add(partConn);
            }

            return project;
        }

        #endregion
    }
}
