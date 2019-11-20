using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.UI
{
    public struct SpriteBounds
    {
        public Vector2 Position;

        public Vector2 Size;

        public float X
        {
            get => Position.X;
            set => Position.X = value;
        }

        public float Y
        {
            get => Position.Y;
            set => Position.Y = value;
        }

        public float Width
        {
            get => Size.X;
            set => Size.X = value;
        }

        public float Height
        {
            get => Size.Y;
            set => Size.Y = value;
        }

        public SpriteBounds(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public SpriteBounds(float x, float y, float width, float height) : this()
        {
            Position = new Vector2(x, y);
            Size = new Vector2(width, height);
        }
    }
}
