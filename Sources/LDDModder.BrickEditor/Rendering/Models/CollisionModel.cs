using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Modding.Editing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class CollisionModel
    {
        public GLModel BaseModel { get; set; }
        public Vector3 Scale { get; set; }
        public Matrix4 Transform { get; set; }

        public PartCollision PartCollision { get; set; }

        public CollisionType CollisionType => PartCollision.CollisionType;

        public CollisionModel(PartCollision collision, GLModel baseModel, Matrix4 transform, Vector3 scale)
        {
            PartCollision = collision;
            BaseModel = baseModel;
            Transform = Matrix4.CreateScale(scale) * transform;
            Scale = scale;
        }

        public void Draw()
        {
            BaseModel.Draw(CollisionType == CollisionType.Box ? 
                OpenTK.Graphics.OpenGL.PrimitiveType.Quads : 
                OpenTK.Graphics.OpenGL.PrimitiveType.Triangles);
        }
    }
}
