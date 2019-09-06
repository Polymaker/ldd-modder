using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Editing
{
    public class RootNode : PartNode
    {
        private PartProject _Project;

        public override PartProject Project => _Project;

        public RootNode(string name, PartProject project)
        {
            GenerateID();
            Description = name;
            _Project = project;
        }
    }
}
