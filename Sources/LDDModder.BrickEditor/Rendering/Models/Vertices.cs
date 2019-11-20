﻿using OpenTK;
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
    }

    public struct VertVNT : IVertexData
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoord;
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


    }
}
