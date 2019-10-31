using System;
using OpenTK;
namespace LDDModder.BrickEditor.Rendering
{
    public struct LightInfo
    {
        public Vector3 Position;

        public float Constant;
        public float Linear;
        public float Quadratic;

        public Vector3 Ambient;
        public Vector3 Diffuse;
        public Vector3 Specular;
    }
}
