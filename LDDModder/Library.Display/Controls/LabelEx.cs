using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LDDModder.Display.Controls
{
    public partial class LabelEx : Label
    {
        // Fields...
        private TextImageRelation _TextImageRelation;

        [DefaultValue(TextImageRelation.Overlay)]
        public TextImageRelation TextImageRelation
        {
            get { return _TextImageRelation; }
            set
            {
                if (_TextImageRelation == value)
                    return;
                _TextImageRelation = value;
                AdjustSize();
                Invalidate();
            }
        }

        private bool ImageNotOverlayed
        {
            get
            {
                return TextImageRelation == TextImageRelation.TextBeforeImage || TextImageRelation == TextImageRelation.ImageBeforeText;
            }
        }

        public LabelEx()
        {
            _TextImageRelation = TextImageRelation.Overlay;
            InitializeComponent();
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var resultSize = base.GetPreferredSize(proposedSize);
            if (Image != null)
            {
                var txtImgRel = TextImageRelation;
                if (txtImgRel == TextImageRelation.Overlay)
                {
                    resultSize.Height = Math.Max(resultSize.Height, Image.Height);
                    resultSize.Width = Math.Max(resultSize.Width, Image.Width);
                }
                else if (txtImgRel == TextImageRelation.TextBeforeImage || txtImgRel == TextImageRelation.ImageBeforeText)
                {
                    resultSize.Width += Image.Width;
                    resultSize.Height = Math.Max(resultSize.Height, Image.Height);
                }
                else
                {
                    resultSize.Height += Image.Height;
                    resultSize.Width = Math.Max(resultSize.Width, Image.Width);
                }
            }
            return resultSize;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Animate();
            ImageAnimator.UpdateFrames(Image);

            Rectangle rectangle = DeflateRect(ClientRectangle, Padding);
            var textRectangle = GetTextRect(rectangle);

            if (Image != null)
            {
                DrawImage(e.Graphics, Image, GetImageRect(rectangle), base.RtlTranslateAlignment(this.ImageAlign));
            }
            
            var nearestColor = NativeHelper.GetNearestColor(Enabled ? ForeColor : DisabledColor, e.Graphics);

            if (UseCompatibleTextRendering)
            {
                using (StringFormat stringFormat = CreateStringFormat())
                {
                    if (Enabled)
                    {
                        using (Brush brush = new SolidBrush(nearestColor))
                        {
                            e.Graphics.DrawString(Text, Font, brush, textRectangle, stringFormat);
                            return;
                        }
                    }

                    ControlPaint.DrawStringDisabled(e.Graphics, Text, Font, nearestColor, textRectangle, stringFormat);
                    return;
                }
            }

            var flags = CreateTextFormatFlags();
            if (Enabled)
            {
                TextRenderer.DrawText(e.Graphics, Text, Font, textRectangle, nearestColor, flags);
            }
            else
            {
                Color foreColor = DisabledTextColor(BackColor);
                TextRenderer.DrawText(e.Graphics, Text, Font, textRectangle, foreColor, flags);
            }
        }

        private Rectangle GetTextRect(Rectangle rectangle)
        {
            if (Image == null || TextImageRelation == TextImageRelation.Overlay)
                return rectangle;
            switch (TextImageRelation)
            {
                case TextImageRelation.ImageBeforeText:
                    rectangle.Width -= Image.Width;
                    rectangle.X += Image.Width;
                    break;
                case TextImageRelation.TextBeforeImage:
                    rectangle.Width -= Image.Width;
                    break;
                case TextImageRelation.ImageAboveText:
                    rectangle.Height -= Image.Height;
                    rectangle.Y += Image.Height;
                    break;
                case TextImageRelation.TextAboveImage:
                    rectangle.Height -= Image.Height;
                    break;
            }
            return rectangle;
        }

        private Rectangle GetImageRect(Rectangle rectangle)
        {
            if (Image == null || TextImageRelation == TextImageRelation.Overlay)
                return rectangle;
            switch (TextImageRelation)
            {
                case TextImageRelation.ImageBeforeText:
                    rectangle.Width = Image.Width;
                    break;
                case TextImageRelation.TextBeforeImage:
                    rectangle.X = rectangle.Right - Image.Width;
                    rectangle.Width = Image.Width;
                    break;
                case TextImageRelation.ImageAboveText:
                    rectangle.Height = Image.Height;
                    break;
                case TextImageRelation.TextAboveImage:
                    rectangle.Y = rectangle.Bottom - Image.Height;
                    rectangle.Height = Image.Height;
                    break;
            }
            return rectangle;
        }

        #region System.Windows.Forms.Label internal

        private static MethodInfo AnimateMethod;
        private static MethodInfo CreateStringFormatMethod;
        private static MethodInfo CreateTextFormatFlagsMethod;
        private static MethodInfo DisabledTextColorMethod;
        private static MethodInfo AdjustSizeMethod;
        private static PropertyInfo DisabledColorProperty;

        protected Color DisabledColor
        {
            get
            {
                if (DisabledColorProperty == null)
                    DisabledColorProperty = typeof(Control).GetProperty("DisabledColor", BindingFlags.Instance | BindingFlags.NonPublic);
                if (DisabledColorProperty != null)
                    return (Color)DisabledColorProperty.GetValue(this, null);
                return SystemColors.ControlDark;
            }
        }

        protected void AdjustSize()
        {
            if (AdjustSizeMethod == null)
                AdjustSizeMethod = FindMethod(typeof(Label), "AdjustSize", 0);
            if (AdjustSizeMethod != null)
                AdjustSizeMethod.Invoke(this, null);
        }

        protected void Animate()
        {
            if (AnimateMethod == null)
                AnimateMethod = FindMethod(typeof(Label), "Animate", 0);
            if (AnimateMethod != null)
                AnimateMethod.Invoke(this, null);
        }

        protected StringFormat CreateStringFormat()
        {
            if (CreateStringFormatMethod == null)
                CreateStringFormatMethod = FindMethod(typeof(Label), "CreateStringFormat", 0);
            if (CreateStringFormatMethod != null)
                return (StringFormat)CreateStringFormatMethod.Invoke(this, null);
            return StringFormat.GenericDefault;
        }

        protected TextFormatFlags CreateTextFormatFlags()
        {
            if (CreateTextFormatFlagsMethod == null)
                CreateTextFormatFlagsMethod = FindMethod(typeof(Label), "CreateTextFormatFlags", 0);
            if (CreateTextFormatFlagsMethod != null)
                return (TextFormatFlags)CreateTextFormatFlagsMethod.Invoke(this, null);
            return TextFormatFlags.Default;
        }

        public static Rectangle DeflateRect(Rectangle rect, Padding padding)
        {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Horizontal;
            rect.Height -= padding.Vertical;
            return rect;
        }

        internal static Color DisabledTextColor(Color color)
        {
            if (DisabledTextColorMethod == null)
                DisabledTextColorMethod = typeof(TextRenderer).GetMethod("DisabledTextColor", BindingFlags.NonPublic | BindingFlags.Static);
            if (DisabledTextColorMethod != null)
                return (Color)DisabledTextColorMethod.Invoke(null, new object[] { color });

            return SystemColors.ControlDark;
        }

        private static MethodInfo FindMethod(Type ownerType, string methodName, int paramCount)
        {
            var methods = ownerType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return methods.Where(m=>m.Name == methodName).OrderByDescending(m=> m.GetParameters().Length == paramCount).FirstOrDefault();
        }


        #endregion
    }
}
