using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.ProjectHandling.ViewInterfaces
{
    public interface IMainWindow
    {
        DockPanel GetDockPanelControl();
    }
}
