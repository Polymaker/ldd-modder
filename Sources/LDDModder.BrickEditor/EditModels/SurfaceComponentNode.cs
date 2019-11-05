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
                case ModelComponentType.Part:
                    {
                        Name = ModelLocalizations.Label_Models;

                    }
                    break;
                case ModelComponentType.MaleStud:
                    {
                        if (component is MaleStudModel surfaceStud &&
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
                                int myIndex = maleConnectors.IndexOf(surfaceStud.GetLinkedConnection());

                                Name = $"{ModelLocalizations.Label_Studs} {myIndex} [{studX}, {studY}]";
                            }
                            else
                            {
                                Name = $"{ModelLocalizations.Label_Stud} [{studX}, {studY}]";
                            }
                        }
                    }
                    break;
                case ModelComponentType.FemaleStud:
                    {
                        if (component is FemaleStudModel surfaceStud)
                        {
                            var femmaleConnectors = Project.Connections.Where(x =>
                                x.ConnectorType == ConnectorType.Custom2DField &&
                                x.Connector.SubType == 22).ToList();

                            if (femmaleConnectors.Count > 1)
                            {
                                int myIndex = femmaleConnectors.IndexOf(surfaceStud.GetLinkedConnection());

                                Name = $"{ModelLocalizations.Label_FemaleStud} {myIndex + 1}";
                            }
                            else
                            {
                                Name = $"{ModelLocalizations.Label_FemaleStud}";
                            }
                        }
                    }
                    break;
                case ModelComponentType.BrickTube:
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
            //foreach(var mesh in Component.Meshes)
            //    Childrens.Add(new PartGeometryNode(mesh));

            //if (Component is FemaleStudModel femaleStud && femaleStud.ReplacementMeshes.Any())
            //{
            //    foreach(var mesh in femaleStud.ReplacementGeometries)
            //        Childrens.Add(new PartGeometryNode(mesh, true));
            //}
        }
    }
}
