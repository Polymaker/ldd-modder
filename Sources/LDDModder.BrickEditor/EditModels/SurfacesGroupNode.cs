using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class SurfacesGroupNode : ComponentGroupNode
    {
        public ElementCollection<PartSurface> Surfaces { get; }

        public SurfacesGroupNode(PartProject project) : base (project.Surfaces)
        {
            Surfaces = project.Surfaces;
            Name = ModelLocalizations.Label_Surfaces;
        }

        public override void RebuildChildrens()
        {
            base.RebuildChildrens();

            foreach(var surface in Surfaces)
                Childrens.Add(new PartSurfaceNode(surface));
        }
    }
}
