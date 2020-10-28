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
        protected override void DrawLines(Graphics g, Rectangle r, Pen p, TreeListView.Branch br, int glyphMidVertical)
        {
            if (br.Level <= 1)
                return;

            Rectangle rectangle = r;
            rectangle.Width = PIXELS_PER_LEVEL;
            int top = rectangle.Top;
            IList<BrightIdeasSoftware.TreeListView.Branch> ancestors = br.Ancestors;
            int num;
            foreach (BrightIdeasSoftware.TreeListView.Branch item in ancestors)
            {
                if (!item.IsLastChild && !item.IsOnlyBranch)
                {
                    num = rectangle.Left + rectangle.Width / 2;
                    if (item.Level > 1)
                        g.DrawLine(p, num, top, num, rectangle.Bottom);
                }
                rectangle.Offset(PIXELS_PER_LEVEL, 0);
            }
            num = rectangle.Left + rectangle.Width / 2;
            g.DrawLine(p, num, glyphMidVertical, rectangle.Right, glyphMidVertical);
            if (br.IsFirstBranch)
            {
                if (!br.IsLastChild && !br.IsOnlyBranch && br.Level > 1)
                {
                    g.DrawLine(p, num, glyphMidVertical, num, rectangle.Bottom);
                }
            }
            else if (br.IsLastChild)
            {
                g.DrawLine(p, num, top, num, glyphMidVertical);
            }
            else
            {
                g.DrawLine(p, num, top, num, rectangle.Bottom);
            }
        }
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
