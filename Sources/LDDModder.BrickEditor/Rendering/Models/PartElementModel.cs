using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public abstract class PartElementModel : ModelBase
    {
        public PartElement Element { get; set; }

        public bool IsSelected { get; set; }

        protected PartElementModel(PartElement element)
        {
            Element = element;
        }
    }
}
