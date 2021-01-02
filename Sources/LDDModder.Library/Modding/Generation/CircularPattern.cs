using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding
{
    public class CircularPattern : ClonePattern
    {
        public Vector3 Origin { get; set; }

        public Vector3 Axis { get; set; }

        public float Angle { get; set; }

        public bool EqualSpacing { get; set; }

        public override ItemTransform ApplyTransform(ItemTransform transform, int instance)
        {
            var baseTrans = transform.ToMatrixD();

            var transOrigMat = Matrix4d.FromDirection((Vector3d)Axis, Vector3d.UnitZ);
            transOrigMat *= Matrix4d.FromTranslation((Vector3d)Origin);

            var localMat = baseTrans * transOrigMat.Inverted();

            float angleInc = EqualSpacing ? (Angle / (Repetitions)) : Angle;
            var rotMatrix = Matrix4d.FromAngleAxis(angleInc * instance, Vector3d.UnitZ);

            var finalMat = localMat * transOrigMat * rotMatrix;

            return ItemTransform.FromMatrix(finalMat);
        }
    }
}
