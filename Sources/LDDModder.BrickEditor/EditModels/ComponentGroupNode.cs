using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.EditModels
{
    public class ComponentGroupNode : ProjectItemNode
    {
        public IElementCollection Collection { get; }

        public ComponentGroupNode(IElementCollection collections)
        {
            Collection = collections;
        }
    }
}
