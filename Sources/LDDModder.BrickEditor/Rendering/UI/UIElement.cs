using ObjectTK.Textures;
using OpenTK;
using OpenTK.Input;
using QuickFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.Rendering.UI
{
    public class UIElement
    {
        private Vector4 _Bounds;

        public Vector4 Bounds
        {
            get => _Bounds;
            set => SetBounds(value);
        }

        public float X
        {
            get => Bounds.X;
            set => SetBoundsCore(value, 0, 0, 0, BoundsSpecified.X);
        }

        public float Y
        {
            get => Bounds.Y;
            set => SetBoundsCore(0, value, 0, 0, BoundsSpecified.Y);
        }

        public float Width
        {
            get => Bounds.Z;
            set => SetBoundsCore(0, 0, value, 0, BoundsSpecified.Width);
        }

        public float Height
        {
            get => Bounds.W;
            set => SetBoundsCore(0, 0, 0, value, BoundsSpecified.Height);
        }

        public float ZOrder { get; set; }

        public bool IsMouseOver { get; private set; }

        public string Text { get; set; }

        public QFont Font { get; set; }

        public Vector4 TextColor { get; set; }

        public bool Visible { get; set; } = true;

        public UIAnchor Anchor { get; set; }

        public event EventHandler BoundsChanged;

        public UIElement()
        {
            Font = UIRenderHelper.NormalFont;
            Text = string.Empty;
            TextColor = new Vector4(0, 0, 0, 1);
            _Bounds = new Vector4(0, 0, 1, 1);
        }

        #region Bounds Handling

        protected virtual void SetBounds(Vector4 bounds)
        {
            SetBoundsCore(bounds.X, bounds.Y, bounds.Z, bounds.W, BoundsSpecified.All);
        }

        protected virtual void SetBoundsCore(float x, float y, float width, float height, BoundsSpecified specified)
        {
            var oldBounds = _Bounds;

            if (!specified.HasFlag(BoundsSpecified.X))
                x = _Bounds.X;
            if (!specified.HasFlag(BoundsSpecified.Y))
                y = _Bounds.Y;
            if (!specified.HasFlag(BoundsSpecified.Width))
                width = _Bounds.Z;
            if (!specified.HasFlag(BoundsSpecified.Height))
                height = _Bounds.W;
            
            _Bounds = new Vector4(x, y, width, height);

            if (oldBounds != Bounds)
                BoundsChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public virtual void OnPaint()
        {
            if (!string.IsNullOrEmpty(Text) && Font != null)
            {
                UIRenderHelper.DrawText(Text, Font, TextColor, Bounds,
                    System.Drawing.StringAlignment.Center,
                    System.Drawing.StringAlignment.Center);
            }
        }

        public void Draw()
        {
            if (Visible)
                OnPaint();
        }

        public virtual bool Contains(Vector2 position)
        {
            return position.X >= Bounds.X && position.X <= Bounds.X + Bounds.Z &&
                position.Y >= Bounds.Y && position.Y <= Bounds.Y + Bounds.W;
        }

        public virtual void OnMouseEnter()
        {

        }

        public virtual void OnMouseLeave()
        {

        }

        public void PerformClick(Vector2 position, MouseButton button)
        {
            OnClick(position, button);
        }

        protected virtual void OnClick(Vector2 position, MouseButton button)
        {

        }

        public void SetIsOver(bool isOver)
        {
            bool wasOver = IsMouseOver;
            IsMouseOver = isOver;
            if (wasOver != IsMouseOver)
            {
                if (IsMouseOver)
                    OnMouseEnter();
                else
                    OnMouseLeave();
            }
        }
    }
}
