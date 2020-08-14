using LDDModder.BrickEditor.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.ProjectHandling.ViewInterfaces
{
    public interface INavigationWindow
    {
        void RefreshNavigationNode(ProjectTreeNode node);
    }
}
