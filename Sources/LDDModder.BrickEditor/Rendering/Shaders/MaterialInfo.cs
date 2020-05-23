using System;
using OpenTK;

namespace LDDModder.BrickEditor.Rendering
{
    public struct MaterialInfo
    {
        public Vector4 Diffuse;
        public Vector3 Specular;
        public float Shininess;

        public MaterialInfo(Vector4 diffuse, Vector3 specular, float shininess)
        {
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }
    }
}
