using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.Utilities
{
    public class ClipboardDataObject : DataObject
    {
        public IList Elements { get; private set; }

        public ClipboardDataObject(IList elements)
        {
            Elements = elements;
        }
    }
}
