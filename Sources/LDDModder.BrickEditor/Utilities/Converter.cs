using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor
{
    public static class Converter
    {
        #region To OpenTK

        public static OpenTK.Vector2 ToGL(this LDDModder.Simple3D.Vector2 vector)
        {
            return new OpenTK.Vector2(vector.X, vector.Y);
        }

        public static OpenTK.Vector2 ToGL(this Assimp.Vector2D vector)
        {
            return new OpenTK.Vector2(vector.X, vector.Y);
        }

        public static OpenTK.Vector3 ToGL(this LDDModder.Simple3D.Vector3 vector)
        {
            return new OpenTK.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static OpenTK.Vector3 ToGL(this Assimp.Vector3D vector)
        {
            return new OpenTK.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static OpenTK.Vector4 ToGL(this LDDModder.Simple3D.Vector4 vector)
        {
            return new OpenTK.Vector4(vector.X, vector.Y, vector.Z, vector.W);
        }

        public static OpenTK.Matrix3 ToGL(this LDDModder.Simple3D.Matrix3 matrix)
        {
            return new OpenTK.Matrix3(matrix.A1, matrix.A2, matrix.A3, matrix.B1, matrix.B2, matrix.B3, matrix.C1, matrix.C2, matrix.C3);
        }

        public static OpenTK.Matrix3 ToGL(this Assimp.Matrix3x3 matrix)
        {
            var m = matrix;
            m.Transpose();
            return new OpenTK.Matrix3(m.A1, m.A2, m.A3, m.B1, m.B2, m.B3, m.C1, m.C2, m.C3);
        }

        public static OpenTK.Matrix4 ToGL(this LDDModder.Simple3D.Matrix4 matrix)
        {
            return new OpenTK.Matrix4(
                matrix.A1, matrix.A2, matrix.A3, matrix.A4,
                matrix.B1, matrix.B2, matrix.B3, matrix.B4,
                matrix.C1, matrix.C2, matrix.C3, matrix.C4,
                matrix.D1, matrix.D2, matrix.D3, matrix.D4);
        }

        public static OpenTK.Matrix4 ToGL(this Assimp.Matrix4x4 matrix)
        {
            var m = matrix;
            m.Transpose();
            return new OpenTK.Matrix4(
                m.A1, m.A2, m.A3, m.A4,
                m.B1, m.B2, m.B3, m.B4,
                m.C1, m.C2, m.C3, m.C4,
                m.D1, m.D2, m.D3, m.D4);
        }

        #endregion

        #region MyRegion

        #endregion



    }
}
