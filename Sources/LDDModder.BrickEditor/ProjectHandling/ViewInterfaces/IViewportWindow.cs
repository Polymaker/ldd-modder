﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.ProjectHandling.ViewInterfaces
{
    public interface IViewportWindow
    {
        void RebuildModels();
        void ForceRender();
    }
}
