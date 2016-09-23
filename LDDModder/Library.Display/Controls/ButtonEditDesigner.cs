using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.Design;

namespace LDDModder.Display.Controls
{
    internal class ButtonEditDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                return base.SelectionRules ^ SelectionRules.TopSizeable ^ SelectionRules.BottomSizeable;
            }
        }
    }
}
