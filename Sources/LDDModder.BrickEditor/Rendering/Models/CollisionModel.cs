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
    public class CollisionModel : PartElementModel
    {
        public GLModel BaseModel { get; private set; }

        public Vector3 Scale { get; set; }
        
        public Matrix4 MeshTransform { get; set; }

        public PartCollision PartCollision { get; set; }

        public CollisionType CollisionType => PartCollision.CollisionType;

        public CollisionModel(PartCollision collision, GLModel baseModel) : base(collision)
        {
            PartCollision = collision;
            BaseModel = baseModel;
            //Transform = collision.Transform.ToMatrix().ToGL();
            //SetTransform()
            Vector3 scale = Vector3.One;
            if (collision is PartBoxCollision boxCollision)
                scale = boxCollision.Size.ToGL() * 2f;
            else if (collision is PartSphereCollision sphereCollision)
                scale = new Vector3(sphereCollision.Radius * 2f);

            var baseTransform = collision.Transform.ToMatrix().ToGL();
            //var finalTransform = Matrix4.CreateScale(scale) * baseTransform;
            SetTransform(baseTransform);

            BoundingBox = BBox.FromCenterSize(Vector3.Zero, scale);
            MeshTransform = Matrix4.CreateScale(scale) /** Transform*/; 
            Scale = scale;
        }

        
        protected override void OnTransformChanged()
        {
            base.OnTransformChanged();
            Matrix4 transCopy = Transform;
            transCopy.ClearScale();
            PartCollision.Transform.SetFromMatrix(transCopy.ToLDD());
            PartCollision.SetSize(Transform.ExtractScale().ToLDD());
        }

        public void Draw()
        {
            BaseModel.Draw(CollisionType == CollisionType.Box ? 
                OpenTK.Graphics.OpenGL.PrimitiveType.Quads : 
                OpenTK.Graphics.OpenGL.PrimitiveType.Triangles);

        }

        public override void RenderModel(Camera camera)
        {
            base.RenderModel(camera);
            //MeshTransform
            RenderHelper.BeginDrawModel(BaseModel.VertexBuffer, MeshTransform * Transform, BaseModel.Material);
            RenderHelper.ModelShader.IsSelected.Set(IsSelected);

            BaseModel.Draw(CollisionType == CollisionType.Box ?
                OpenTK.Graphics.OpenGL.PrimitiveType.Quads :
                OpenTK.Graphics.OpenGL.PrimitiveType.Triangles);

            RenderHelper.EndDrawModel(BaseModel.VertexBuffer);
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            if (CollisionType == CollisionType.Box)
                return RayIntersectsBoundingBox(ray, out distance);

            var pos = Vector3.TransformPosition(Vector3.Zero, Transform);
            var sphere = new BSphere(pos, Scale.X / 2f);

            return Ray.IntersectsSphere(ray, sphere, out distance);
        }
    }
}
