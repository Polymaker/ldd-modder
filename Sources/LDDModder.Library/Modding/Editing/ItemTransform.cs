using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public struct ItemTransform
    {
        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }

        //public ItemTransform()
        //{
        //    Position = Vector3.Empty;
        //    Rotation = Vector3.Empty;
        //}

        public ItemTransform(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public static ItemTransform FromMatrix(Matrix4 matrix)
        {
            var rot = matrix.ExtractRotation();

            return new ItemTransform(matrix.ExtractTranslation(), Quaternion.ToEuler(rot));
        }

        public static ItemTransform FromLDD(LDD.Primitives.Transform transform)
        {
            return FromMatrix(transform.ToMatrix4());
        }

        public Matrix4 ToMatrix()
        {
            var quat = Quaternion.FromEuler(Rotation);
            quat.ToAxisAngle(out Vector3 axis, out float angle);
            var rot = Matrix4.FromAngleAxis(angle, axis);
            var trans = Matrix4.FromTranslation(Position);
            return rot * trans;
        }

        public LDD.Primitives.Transform ToLDD()
        {
            return LDD.Primitives.Transform.FromMatrix(ToMatrix());
        }
    }
}
