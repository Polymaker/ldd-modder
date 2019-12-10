using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightIdeasSoftware;

namespace LDDModder.BrickEditor.UI.Controls
{
    public class TreeRendererEx : BrightIdeasSoftware.TreeListView.TreeRenderer
    {
        protected override Rectangle StandardGetEditRectangle(Graphics g, Rectangle cellBounds, Size preferredSize)
        {
            var baseRect = base.StandardGetEditRectangle(g, cellBounds, preferredSize);

            if (ListItem.IndentCount > 0)
            {
                int wrongIndent = ListView.SmallImageSize.Width * ListItem.IndentCount;
                baseRect.Width += wrongIndent;
                baseRect.X -= wrongIndent;

                int goodIndent = PIXELS_PER_LEVEL * ListItem.IndentCount;
                baseRect.Width -= goodIndent;
                baseRect.X += goodIndent;
            }

            return baseRect;
        }
    }
}
