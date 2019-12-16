using LDDModder.BrickEditor.Models.Navigation;
using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.ProjectHandling
{
    class ProjectWrapper
    {
        public PartProject Project { get; set; }

        public ProjectManager Manager { get; set; }

        public List<BaseProjectNode> NavigationNodes { get; set; } = new List<BaseProjectNode>();

        public void RebuildNavigationNodes()
        {
            NavigationNodes.Clear();

            NavigationNodes.Add(new ProjectCollectionNode(
                Project.Surfaces, ModelLocalizations.Label_Surfaces));

            if (Project.Properties.Flexible)
            {
                NavigationNodes.Add(new ProjectCollectionNode(Project.Bones, "Bones"));
            }
            else
            {
                NavigationNodes.Add(new ProjectCollectionNode(
                    Project.Collisions, ModelLocalizations.Label_Collisions));

                NavigationNodes.Add(new ProjectCollectionNode(
                    Project.Connections, ModelLocalizations.Label_Connections));
            }
        }
    }
}
