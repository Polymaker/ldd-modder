﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.ProjectHandling.ViewInterfaces
{
    public interface IMainWindow : IWin32Window
    {
        DockPanel GetDockPanelControl();
    }
}
