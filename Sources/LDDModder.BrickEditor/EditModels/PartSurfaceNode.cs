using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class PartSurfaceNode : ProjectComponentNode<PartSurface>
    {
        public PartSurface Surface => Component;

        public PartSurfaceNode(PartSurface surface) : base(surface)
        {
            if (surface.SurfaceID == 0)
                Name = ModelLocalizations.Label_MainSurface;
            else
                Name = ModelLocalizations.Label_DecorationSurface + " " + surface.SurfaceID;
        }

        public override void RebuildChildrens()
        {
            base.RebuildChildrens();

            var distinctTypes = Surface.Components.Select(x => x.ComponentType).Distinct().ToList();


            foreach (var compType in distinctTypes)
                Childrens.Add(new ComponentTypeGroupNode(Surface, compType));

        }
    }
}
