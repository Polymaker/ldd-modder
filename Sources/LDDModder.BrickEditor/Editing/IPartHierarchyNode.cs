using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Editing
{
    public interface IPartHierarchyNode
    {
        string Name { get; }
        IPartHierarchyNode Parent { get; }
    }
}
