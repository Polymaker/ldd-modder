using LDDModder.BrickEditor.Resources;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class SurfaceComponentNode : ProjectComponentNode<SurfaceComponent>
    {

        public SurfaceComponentNode(SurfaceComponent component) : base(component)
        {
            switch (component.ComponentType)
            {
                case MeshCullingType.MainModel:
                    {
                        Name = ModelLocalizations.Label_Models;

                    }
                    break;
                case MeshCullingType.Stud:
                    {
                        if (component is SurfaceStud surfaceStud &&
                            surfaceStud.Stud != null &&
                            surfaceStud.Stud.FieldNode != null)
                        {
                            var maleConnectors = Project.Connections.Where(x =>
                                x.ConnectorType == ConnectorType.Custom2DField &&
                                x.Connector.SubType == 23).ToList();

                            int studX = (surfaceStud.Stud.FieldNode.X + 1) / 2;
                            int studY = (surfaceStud.Stud.FieldNode.Y + 1) / 2;

                            if (maleConnectors.Count > 1)
                            {
                                int myIndex = maleConnectors.IndexOf(surfaceStud.LinkedConnection);

                                Name = $"{ModelLocalizations.Label_Studs} {myIndex} [{studX}, {studY}]";
                            }
                            else
                            {
                                Name = $"{ModelLocalizations.Label_Stud} [{studX}, {studY}]";
                            }
                        }
                    }
                    break;
                case MeshCullingType.FemaleStud:
                    {
                        if (component is SurfaceFemaleStud surfaceStud)
                        {
                            var femmaleConnectors = Project.Connections.Where(x =>
                                x.ConnectorType == ConnectorType.Custom2DField &&
                                x.Connector.SubType == 22).ToList();

                            if (femmaleConnectors.Count > 1)
                            {
                                int myIndex = femmaleConnectors.IndexOf(surfaceStud.LinkedConnection);

                                Name = $"{ModelLocalizations.Label_FemaleStud} {myIndex + 1}";
                            }
                            else
                            {
                                Name = $"{ModelLocalizations.Label_FemaleStud}";
                            }
                        }
                    }
                    break;
                case MeshCullingType.Tube:
                    Name = ModelLocalizations.Label_BrickTubes;
                    break;
            }
            //if (component.SurfaceID == 0)
            //    Name = ModelLocalizations.Label_MainSurface;
            //else
            //    Name = ModelLocalizations.Label_DecorationSurface + " " + component.SurfaceID;
        }

        public override void RebuildChildrens()
        {
            //base.RebuildChildrens();
            foreach(var mesh in Component.Geometries)
                Childrens.Add(new PartGeometryNode(mesh));

            if (Component is SurfaceFemaleStud femaleStud && femaleStud.ReplacementGeometries.Any())
            {
                foreach(var mesh in femaleStud.ReplacementGeometries)
                    Childrens.Add(new PartGeometryNode(mesh, true));
            }
        }
    }
}
