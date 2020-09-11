using LDDModder.BrickEditor.Models.Navigation;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.ProjectHandling
{
    public interface IProjectManager
    {
        PartProject CurrentProject { get; }

        ProjectTreeNodeCollection NavigationTreeNodes { get; }

        bool ShowPartModels { get; }

        bool ShowCollisions { get; }

        bool ShowConnections { get; }

        void RebuildNavigationTree();

        void RefreshNavigationNode(ProjectTreeNode node);
    }
}
