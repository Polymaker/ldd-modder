using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public interface IVertexData
    {
        
    }

    public struct VertVN : IVertexData
    {
        public Vector3 Position;
        public Vector3 Normal;

        public VertVN(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }

        public static explicit operator VertVN(VertVNT vert)
        {
            return new VertVN(vert.Position, vert.Normal);
        }
    }

    public struct VertVNT : IVertexData
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoord;

        public T Cast<T>() where T : struct
        {
            if (typeof(T) == typeof(VertVNT))
                return (T)(object)this;
            else if (typeof(T) == typeof(VertVT))
                return (T)(object)new VertVT(Position, TexCoord);
            else if (typeof(T) == typeof(VertVN))
                return (T)(object)new VertVN(Position, Normal);
            else if (typeof(T) == typeof(Vector3))
                return (T)(object)Position;

            return default(T);
        }
    }

    public struct VertVBW : IVertexData
    {
        public Vector3 Position;
        public float BoneWeight;
    }

    public struct VertVT : IVertexData
    {
        public Vector3 Position;
        public Vector2 TexCoord;

        public VertVT(Vector3 position, Vector2 texCoord)
        {
            Position = position;
            TexCoord = texCoord;
        }

        public static explicit operator VertVT(VertVNT vert)
        {
            return new VertVT(vert.Position, vert.TexCoord);
        }
    }
}
