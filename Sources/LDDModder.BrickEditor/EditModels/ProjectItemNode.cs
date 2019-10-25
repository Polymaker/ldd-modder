using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class ProjectItemNode
    {
        public ProjectItemNode Parent { get; set; }

        public string Name { get; set; }

        public bool Visible { get; set; }

        public bool IsVisible => (Parent?.IsVisible ?? true) && Visible;

        public bool HasInitializedChildrens { get; set; }

        public List<ProjectItemNode> Childrens { get; set; } = new List<ProjectItemNode>();

        public bool HasChildrens()
        {
            if (!HasInitializedChildrens)
            {
                RebuildChildrens();
                HasInitializedChildrens = true;
            }
            return Childrens.Any();
        }

        public virtual void RebuildChildrens()
        {

        }
    }
}
