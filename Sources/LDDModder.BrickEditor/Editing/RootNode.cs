using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Editing
{
    public class RootNode : PartNode
    {
        public PartProject Project { get; protected set; }

        public RootNode(string name, PartProject project)
        {
            ID = name;
            Project = project;
        }
    }
}
