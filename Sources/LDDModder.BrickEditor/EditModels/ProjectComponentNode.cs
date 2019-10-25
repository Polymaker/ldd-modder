using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public abstract class ProjectComponentNode<T> : ProjectItemNode where T : PartComponent
    {
        public T Component { get; }

        public PartProject Project => Component.Project;

        protected ProjectComponentNode(T component)
        {
            Component = component;
        }
    }
}
