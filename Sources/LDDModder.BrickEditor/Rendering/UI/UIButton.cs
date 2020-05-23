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
    public class UIButton : UIElement
    {
        public Texture2D Texture { get; set; }

        public SpriteBounds NormalSprite { get; set; }

        public SpriteBounds OverSprite { get; set; }

        public SpriteBounds SelectedSprite { get; set; }

        //public bool Selectable { get; set; }

        public bool Selected { get; set; }

        public event EventHandler Clicked;

        public override void OnPaint()
        {
            if (Selected && SelectedSprite.Width > 0)
                UIRenderHelper.DrawSprite(Texture, Bounds, SelectedSprite);
            else if (IsMouseOver && OverSprite.Width > 0)
                UIRenderHelper.DrawSprite(Texture, Bounds, OverSprite);
            else if (NormalSprite.Width > 0)
                UIRenderHelper.DrawSprite(Texture, Bounds, NormalSprite);

            base.OnPaint();
        }


        protected override void OnClick(Vector2 position, MouseButton button)
        {
            base.OnClick(position, button);
            if (button == MouseButton.Left)
                Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
