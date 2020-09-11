using ObjectTK.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.UI
{
    public class UIPanel : UIElement
    {
        public Texture2D Texture { get; set; }

        public SpriteBounds TextureRect { get; set; }

        public override void OnPaint()
        {
            if (Texture != null && TextureRect.Width > 0 && TextureRect.Height > 0)
            {
                UIRenderHelper.DrawSprite(Texture, Bounds, TextureRect);
            }
        }
    }
}
