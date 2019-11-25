using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.UI
{
    public struct BoxSpriteBounds
    {
        public Vector2 Position;

        public Vector2 Size;

        public Vector4 Edges;

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

        public float Left
        {
            get => Edges.X;
            set => Edges.X = value;
        }

        public float Top
        {
            get => Edges.Y;
            set => Edges.Y = value;
        }

        public float Right
        {
            get => Edges.Z;
            set => Edges.Z = value;
        }

        public float Bottom
        {
            get => Edges.W;
            set => Edges.W = value;
        }

        public BoxSpriteBounds(Vector2 position, Vector2 size, Vector4 edges)
        {
            Position = position;
            Size = size;
            Edges = edges;
        }

        public BoxSpriteBounds(float x, float y, float width, float height, float left, float top, float right, float bottom) : this()
        {
            Position = new Vector2(x, y);
            Size = new Vector2(width, height);
            Edges = new Vector4(left, top, right, bottom);
        }

        public float GetColumnPos(int c)
        {
            if (c == 0)
                return X;
            else if (c == 1)
                return X + Left;
            return X + Width - Right;
        }

        public float GetColumnSize(int c)
        {
            if (c == 0)
                return Left;
            else if (c == 1)
                return Width - Left - Right;
            return Right;
        }

        public float GetRowPos(int r)
        {
            if (r == 0)
                return Y;
            else if (r == 1)
                return Y + Top;
            return Y + Height - Bottom;
        }

        public float GetRowSize(int r)
        {
            if (r == 0)
                return Top;
            else if (r == 1)
                return Height - Top - Bottom;
            return Bottom;
        }

        public SpriteBounds GetRegionBounds(int x, int y)
        {
            return new SpriteBounds(GetColumnPos(x), GetRowPos(y), GetColumnSize(x), GetRowSize(y));
            //float posX = 0f;
            //float posY = 0f;
            //float width = 0f;
            //float height = 0f;

            //if (x == 0)
            //{
            //    width = Left;
            //    posX = X;
            //}
            //else if (x == 1)
            //{
            //    width = Width - Left - Right;
            //    posX = X + Left;
            //}
            //else
            //{
            //    width = Right;
            //    posX = X + Width - Right;
            //}

            //if (y == 0)
            //{
            //    height = Top;
            //    posY = Y;
            //}
            //else if (y == 1)
            //{
            //    height = Height - Top - Bottom;
            //    posY = Y + Top;
            //}
            //else
            //{
            //    height = Bottom;
            //    posY = Y + Height - Bottom;
            //}

            //return new SpriteBounds(posX, posY, width, height);
        }
    }
}
