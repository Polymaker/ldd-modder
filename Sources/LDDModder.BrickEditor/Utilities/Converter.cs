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

        public static OpenTK.Quaternion ToGL(this Assimp.Quaternion quaternion)
        {
            return OpenTK.Quaternion.FromMatrix(quaternion.GetMatrix().ToGL());
        }

        public static OpenTK.Quaternion ToGL(this Simple3D.Quaternion quaternion)
        {
            var mat = Simple3D.Matrix4.FromQuaternion(quaternion);
            return mat.ToGL().ExtractRotation();
        }

        #endregion

        #region To Assimp

        public static Assimp.Vector2D ToAssimp(this OpenTK.Vector2 vector)
        {
            return new Assimp.Vector2D(vector.X, vector.Y);
        }

        public static Assimp.Vector2D ToAssimp(this Simple3D.Vector2 vector)
        {
            return new Assimp.Vector2D(vector.X, vector.Y);
        }

        public static Assimp.Vector3D ToAssimp(this OpenTK.Vector3 vector)
        {
            return new Assimp.Vector3D(vector.X, vector.Y, vector.Z);
        }

        public static Assimp.Vector3D ToAssimp(this Simple3D.Vector3 vector)
        {
            return new Assimp.Vector3D(vector.X, vector.Y, vector.Z);
        }

        public static Assimp.Matrix3x3 ToAssimp(this OpenTK.Matrix3 matrix)
        {
            var m = matrix;
            m.Transpose();
            return new Assimp.Matrix3x3(m.M11, m.M12, m.M13, m.M21, m.M22, m.M23, m.M31, m.M32, m.M33);
        }

        public static Assimp.Matrix4x4 ToAssimp(this Simple3D.Matrix4 matrix)
        {
            return matrix.ToGL().ToAssimp();
        }

        public static Assimp.Matrix4x4 ToAssimp(this OpenTK.Matrix4 matrix)
        {
            var m = matrix;
            m.Transpose();
            return new Assimp.Matrix4x4(
                m.M11, m.M12, m.M13, m.M14, 
                m.M21, m.M22, m.M23, m.M24, 
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44);
        }

        public static Assimp.Matrix3x3 ToAssimp(this Simple3D.Matrix3 matrix)
        {
            return matrix.ToGL().ToAssimp();
        }

        public static Assimp.Quaternion ToAssimp(this OpenTK.Quaternion quaternion)
        {
            var mat = OpenTK.Matrix3.CreateFromQuaternion(quaternion).ToAssimp();
            return new Assimp.Quaternion(mat);
        }


        public static Assimp.Quaternion ToAssimp(this Simple3D.Quaternion quaternion)
        {
            var mat = Simple3D.Matrix3.FromQuaternion(quaternion).ToAssimp();
            return new Assimp.Quaternion(mat);
        }

        #endregion

        #region To LDD

        public static LDDModder.Simple3D.Vector3 ToLDD(this OpenTK.Vector3 vector)
        {
            return new Simple3D.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static LDDModder.Simple3D.Vector3 ToLDD(this Assimp.Vector3D vector)
        {
            return new Simple3D.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static LDDModder.Simple3D.Vector4 ToLDD(this OpenTK.Vector4 vector)
        {
            return new Simple3D.Vector4(vector.X, vector.Y, vector.Z, vector.W);
        }

        public static LDDModder.Simple3D.Matrix3 ToLDD(this OpenTK.Matrix3 matrix)
        {
            return new Simple3D.Matrix3(matrix.Row0.ToLDD(), matrix.Row1.ToLDD(), matrix.Row2.ToLDD());
        }

        public static LDDModder.Simple3D.Matrix4 ToLDD(this OpenTK.Matrix4 matrix)
        {
            return new Simple3D.Matrix4(matrix.Row0.ToLDD(), matrix.Row1.ToLDD(), matrix.Row2.ToLDD(), matrix.Row3.ToLDD());
        }

        public static LDDModder.Simple3D.Matrix3 ToLDD(this Assimp.Matrix3x3 matrix)
        {
            return ToLDD(matrix.ToGL());
        }

        public static LDDModder.Simple3D.Matrix4 ToLDD(this Assimp.Matrix4x4 matrix)
        {
            return ToLDD(matrix.ToGL());
        }

        #endregion


    }
}
