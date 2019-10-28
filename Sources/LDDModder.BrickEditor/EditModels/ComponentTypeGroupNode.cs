using LDDModder.BrickEditor.Resources;
using LDDModder.LDD.Meshes;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class ComponentTypeGroupNode : ProjectItemNode
    {
        public PartSurface Surface { get; }

        public ModelComponentType ComponentType { get; }

        public IEnumerable<SurfaceComponent> Components => Surface.Components.Where(x => x.ComponentType == ComponentType);

        public ComponentTypeGroupNode(PartSurface surface, ModelComponentType componentType)
        {
            Surface = surface;
            ComponentType = componentType;

            switch (componentType)
            {
                case ModelComponentType.Part:
                    Name = ModelLocalizations.Label_Models;
                    break;
                case ModelComponentType.MaleStud:
                    Name = ModelLocalizations.Label_MaleStuds;
                    break;
                case ModelComponentType.FemaleStud:
                    Name = ModelLocalizations.Label_FemaleStuds;
                    break;
                case ModelComponentType.BrickTube:
                    Name = ModelLocalizations.Label_BrickTubes;
                    break;
            }
        }

        public override void RebuildChildrens()
        {
            //base.RebuildChildrens();
            //if (ComponentType == MeshCullingType.MainModel)
            //{
            //    foreach (var mesh in Components.SelectMany(x => x.Geometries))
            //        Childrens.Add(new PartGeometryNode(mesh));
            //}


            foreach (var comp in Components)
                Childrens.Add(new SurfaceComponentNode(comp));
        }
    }
}
