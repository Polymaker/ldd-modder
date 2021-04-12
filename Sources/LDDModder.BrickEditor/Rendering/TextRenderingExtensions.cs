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
        //public static void AddText(this QFontDrawing renderer, string text, QFont font, Color color, PointF position, QFontAlignment alignment = QFontAlignment.Left)
        //{
        //    AddText(renderer, text, font, color, new Vector2(position.X, position.Y), alignment);
        //}

        //public static void AddText(this QFontDrawing renderer, string text, QFont font, Color color, Vector2 position, QFontAlignment alignment = QFontAlignment.Left)
        //{
        //    var dp = new QFontDrawingPrimitive(font, new QFontRenderOptions() { Colour = color, LockToPixel = true });
        //    dp.Print(text, new Vector3(position), alignment);
        //    renderer.DrawingPrimitives.Add(dp);
        //}

        public static void AddText(this QFontDrawing renderer, string text, QFont font, Color color, Vector4 bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near)
        {

            AddText(renderer, text, font, color, new RectangleF(bounds.X, bounds.Y, bounds.Z, bounds.W), vAlign, hAlign);
        }

        public static void AddText(this QFontDrawing renderer, string text, QFont font, Color color, RectangleF bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near)
        {
            var dp = new QFontDrawingPrimitive(font, new QFontRenderOptions() { Colour = color, LockToPixel = true });
            AddText(dp, text, bounds, vAlign, hAlign);
        }

        public static void AddText(this QFontDrawingPrimitive dp, string text, Vector2 position,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near,
            float zDepth = 0f)
        {
            AddText(dp, text, new RectangleF(position.X, position.Y, 0, 0), vAlign, hAlign, zDepth);
        }

        public static void AddText(this QFontDrawingPrimitive dp, string text, RectangleF bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near,
            float zDepth = 0f)
        {
            bool noBounds = bounds.Size.IsEmpty;

            var textSize = dp.Font.Measure(text, 
                noBounds ? new SizeF(9999, 9999) : bounds.Size, 
                QFontAlignment.Left);

            textSize.Height += 2;

            if (bounds.Size.IsEmpty)
                bounds = new RectangleF(bounds.X, bounds.Y, textSize.Width, textSize.Height);

            var textPos = Vector2.Zero;

            switch (hAlign)
            {
                case StringAlignment.Near:
                    textPos.X = bounds.X;
                    break;
                case StringAlignment.Far:
                    if (noBounds)
                        textPos.X = bounds.Left - textSize.Width;
                    else
                        textPos.X = bounds.Right - textSize.Width;
                    break;
                case StringAlignment.Center:
                    if (noBounds)
                        textPos.X = bounds.Left - (textSize.Width / 2f);
                    else
                        textPos.X = bounds.Left + ((bounds.Width - textSize.Width) / 2f);
                    break;
            }
            switch (vAlign)
            {
                case StringAlignment.Near:
                    textPos.Y = bounds.Y;
                    break;
                case StringAlignment.Far:
                    if (noBounds)
                        textPos.Y = bounds.Y + textSize.Height;
                    else
                        textPos.Y = bounds.Y - bounds.Height + textSize.Height;
                    break;
                case StringAlignment.Center:
                    if (noBounds)
                        textPos.Y = bounds.Y + (textSize.Height / 2f);
                    else
                        textPos.Y = bounds.Y - ((bounds.Height - textSize.Height) / 2f);
                    break;
            }
            dp.Print(text, new Vector3(textPos.X, textPos.Y, zDepth), bounds.Size, QFontAlignment.Left);
            
        }

        
    }
}
