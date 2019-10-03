using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using QuickFont;
using QuickFont.Configuration;

namespace QuickFont
{
    public static class TextRenderingExtensions
    {
        public static void AddText(this QFontDrawing renderer, string text, QFont font, Color color, PointF position, QFontAlignment alignment = QFontAlignment.Left)
        {
            AddText(renderer, text, font, color, new Vector2(position.X, position.Y), alignment);
        }

        public static void AddText(this QFontDrawing renderer, string text, QFont font, Color color, Vector2 position, QFontAlignment alignment = QFontAlignment.Left)
        {
            var dp = new QFontDrawingPrimitive(font, new QFontRenderOptions() { Colour = color, LockToPixel = true });
            dp.Print(text, new Vector3(position), alignment);
            renderer.DrawingPrimitives.Add(dp);
        }

        public static void AddText(this QFontDrawing renderer, string text, QFont font, Color color, RectangleF bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near)
        {
            var textSize = font.Measure(text, bounds.Size, QFontAlignment.Left);
            var textPos = Vector2.Zero;
            switch (hAlign)
            {
                case StringAlignment.Near:
                    textPos.X = bounds.X;
                    break;
                case StringAlignment.Far:
                    textPos.X = bounds.Right - textSize.Width;
                    break;
                case StringAlignment.Center:
                    textPos.X = bounds.Left + ((bounds.Width - textSize.Width) / 2f);
                    break;
            }
            switch (vAlign)
            {
                case StringAlignment.Near:
                    textPos.Y = bounds.Y;
                    break;
                case StringAlignment.Far:
                    textPos.Y = bounds.Y - bounds.Height + textSize.Height;
                    break;
                case StringAlignment.Center:
                    textPos.Y = bounds.Y - ((bounds.Height - textSize.Height) / 2f);
                    break;
            }
            var dp = new QFontDrawingPrimitive(font, new QFontRenderOptions() { Colour = color, LockToPixel = true });
            dp.Print(text, new Vector3(textPos.X, textPos.Y, 0), bounds.Size, QFontAlignment.Left);
            renderer.DrawingPrimitives.Add(dp);
        }

        public static void AddText(this QFontDrawing renderer, string text, QFont font, Color color, Vector4 bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near)
        {
            
            AddText(renderer, text, font, color, new RectangleF(bounds.X, bounds.Y, bounds.Z, bounds.W), vAlign, hAlign);
        }
    }
}
