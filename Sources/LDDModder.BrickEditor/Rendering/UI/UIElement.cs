using ObjectTK.Textures;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.UI
{
    public class UIElement
    {
        public Vector4 Bounds { get; set; }

        public float ZOrder { get; set; }

        public bool IsMouseOver { get; private set; }

        public string Text { get; set; }

        public Vector4 TextColor { get; set; }

        public virtual void OnPaint()
        {
            if (!string.IsNullOrEmpty(Text))
                UIRenderHelper.DrawText(Text, UIRenderHelper.NormalFont, TextColor, Bounds, 
                    System.Drawing.StringAlignment.Center, 
                    System.Drawing.StringAlignment.Center);
        }

        public void Draw()
        {
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
