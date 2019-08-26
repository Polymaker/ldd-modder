using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;

namespace Assimp
{
    public static class AssimpConverter
    {
        public static Vector3D Convert(this LDDModder.Simple3D.Vector3 v)
        {
            return new Vector3D(v.X, v.Y, v.Z);
        }

        public static Matrix3x3 Convert(this LDDModder.Simple3D.Matrix3 matrix)
        {
            var mat = new Matrix3x3(
                matrix.A1, matrix.A2, matrix.A3,
                matrix.B1, matrix.B2, matrix.B3,
                matrix.C1, matrix.C2, matrix.C3);
            mat.Transpose();
            return mat;
        }

        public static Matrix4x4 Convert(this LDDModder.Simple3D.Matrix4 matrix)
        {
            var mat = new Matrix4x4(
                matrix.A1, matrix.A2, matrix.A3, matrix.A4,
                matrix.B1, matrix.B2, matrix.B3, matrix.B4,
                matrix.C1, matrix.C2, matrix.C3, matrix.C4,
                matrix.D1, matrix.D2, matrix.D3, matrix.D4);
            mat.Transpose();
            return mat;
        }
    }
}
