using LDDModder.BrickEditor.EditModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Panels
{
    public abstract class ProjectDocumentPanel : DockContent
    {

        protected virtual void OnProjectLoaded(ProjectDocument document)
        {

        }


    }
}
