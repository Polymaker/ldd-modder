using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;

namespace LDDModder.BrickEditor.Models.Project
{
    public class RenderableElement : ElementExtention
    {
        //private bool _Visible;

        //public event EventHandler VisibleChanged;

        public bool IsSelected => Manager.IsContainedInSelection(Element);

        public RenderableElement(ProjectManager manager, PartElement element) : base(manager, element)
        {

        }
    }
}
