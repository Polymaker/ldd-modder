using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Matrix4
    {
        public static readonly Matrix4 Identity = new Matrix4(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);

        #region Fields

        public float A1;
        public float A2;
        public float A3;
        public float A4;

        public float B1;
        public float B2;
        public float B3;
        public float B4;

        public float C1;
        public float C2;
        public float C3;
        public float C4;

        public float D1;
        public float D2;
        public float D3;
        public float D4;

        #endregion

        #region Row Properties

        public Vector4 RowA
        {
            get => new Vector4(A1, A2, A3, A4);
            set
            {
                A1 = value.X;
                A2 = value.Y;
                A3 = value.Z;
                A4 = value.W;
            }
        }

        public Vector4 RowB
        {
            get => new Vector4(B1, B2, B3, B4);
            set
            {
                B1 = value.X;
                B2 = value.Y;
                B3 = value.Z;
                B4 = value.W;
            }
        }

        public Vector4 RowC
        {
            get => new Vector4(C1, C2, C3, C4);
            set
            {
                C1 = value.X;
                C2 = value.Y;
                C3 = value.Z;
                C4 = value.W;
            }
        }

        public Vector4 RowD
        {
            get => new Vector4(D1, D2, D3, D4);
            set
            {
                D1 = value.X;
                D2 = value.Y;
                D3 = value.Z;
                D4 = value.W;
            }
        }

        #endregion

        #region Column Properties

        public Vector4 Col1
        {
            get => new Vector4(A1, B1, C1, D1);
            set
            {
                A1 = value.X;
                B1 = value.Y;
                C1 = value.Z;
                D1 = value.W;
            }
        }

        public Vector4 Col2
        {
            get => new Vector4(A2, B2, C2, D2);
            set
            {
                A2 = value.X;
                B2 = value.Y;
                C2 = value.Z;
                D2 = value.W;
            }
        }

        public Vector4 Col3
        {
            get => new Vector4(A3, B3, C3, D3);
            set
            {
                A3 = value.X;
                B3 = value.Y;
                C3 = value.Z;
                D3 = value.W;
            }
        }

        public Vector4 Col4
        {
            get => new Vector4(A4, B4, C4, D4);
            set
            {
                A4 = value.X;
                B4 = value.Y;
                C4 = value.Z;
                D4 = value.W;
            }
        }

        #endregion

        public float Determinant => A1 * B2 * C3 * D4 - A1 * B2 * C4 * D3 + A1 * B3 * C4 * D2 - A1 * B3 * C2 * D4 + A1 * B4 * C2 * D3 - A1 * B4 * C3 * D2 - A2 * B3 * C4 * D1 + A2 * B3 * C1 * D4 - A2 * B4 * C1 * D3 + A2 * B4 * C3 * D1 - A2 * B1 * C3 * D4 + A2 * B1 * C4 * D3 + A3 * B4 * C1 * D2 - A3 * B4 * C2 * D1 + A3 * B1 * C2 * D4 - A3 * B1 * C4 * D2 + A3 * B2 * C4 * D1 - A3 * B2 * C1 * D4 - A4 * B1 * C2 * D3 + A4 * B1 * C3 * D2 - A4 * B2 * C3 * D1 + A4 * B2 * C1 * D3 - A4 * B3 * C1 * D2 + A4 * B3 * C2 * D1;

        public float this[int x, int y]
        {
            get
            {
                switch (x)
                {
                    case 1:
                        switch (y)
                        {
                            case 1:
                                return A1;
                            case 2:
                                return A2;
                            case 3:
                                return A3;
                            case 4:
                                return A4;
                            default:
                                return 0f;
                        }
                    case 2:
                        switch (y)
                        {
                            case 1:
                                return B1;
                            case 2:
                                return B2;
                            case 3:
                                return B3;
                            case 4:
                                return B4;
                            default:
                                return 0f;
                        }
                    case 3:
                        switch (y)
                        {
                            case 1:
                                return C1;
                            case 2:
                                return C2;
                            case 3:
                                return C3;
                            case 4:
                                return C4;
                            default:
                                return 0f;
                        }
                    case 4:
                        switch (y)
                        {
                            case 1:
                                return D1;
                            case 2:
                                return D2;
                            case 3:
                                return D3;
                            case 4:
                                return D4;
                            default:
                                return 0f;
                        }
                    default:
                        return 0f;
                }
            }
            set
            {
                switch (x)
                {
                    case 1:
                        switch (y)
                        {
                            case 1:
                                A1 = value;
                                break;
                            case 2:
                                A2 = value;
                                break;
                            case 3:
                                A3 = value;
                                break;
                            case 4:
                                A4 = value;
                                break;
                        }
                        break;
                    case 2:
                        switch (y)
                        {
                            case 1:
                                B1 = value;
                                break;
                            case 2:
                                B2 = value;
                                break;
                            case 3:
                                B3 = value;
                                break;
                            case 4:
                                B4 = value;
                                break;
                        }
                        break;
                    case 3:
                        switch (y)
                        {
                            case 1:
                                C1 = value;
                                break;
                            case 2:
                                C2 = value;
                                break;
                            case 3:
                                C3 = value;
                                break;
                            case 4:
                                C4 = value;
                                break;
                        }
                        break;
                    case 4:
                        switch (y)
                        {
                            case 1:
                                D1 = value;
                                break;
                            case 2:
                                D2 = value;
                                break;
                            case 3:
                                D3 = value;
                                break;
                            case 4:
                                D4 = value;
                                break;
                        }
                        break;
                }
            }
        }

        public Matrix4(float a1, float a2, float a3, float a4, float b1, float b2, float b3, float b4, float c1, float c2, float c3, float c4, float d1, float d2, float d3, float d4)
        {
            A1 = a1;
            A2 = a2;
            A3 = a3;
            A4 = a4;
            B1 = b1;
            B2 = b2;
            B3 = b3;
            B4 = b4;
            C1 = c1;
            C2 = c2;
            C3 = c3;
            C4 = c4;
            D1 = d1;
            D2 = d2;
            D3 = d3;
            D4 = d4;
        }

        public Matrix4(Vector4 a, Vector4 b, Vector4 c, Vector4 d)
        {
            A1 = a.X;
            A2 = a.Y;
            A3 = a.Z;
            A4 = a.W;
            B1 = b.X;
            B2 = b.Y;
            B3 = b.Z;
            B4 = b.W;
            C1 = c.X;
            C2 = c.Y;
            C3 = c.Z;
            C4 = c.W;
            D1 = d.X;
            D2 = d.Y;
            D3 = d.Z;
            D4 = d.W;
        }

        public Matrix4(Matrix3 matrix)
        {
            A1 = matrix.A1;
            A2 = matrix.A2;
            A3 = matrix.A3;
            A4 = 0;
            B1 = matrix.B1;
            B2 = matrix.B2;
            B3 = matrix.B3;
            B4 = 0;
            C1 = matrix.C1;
            C2 = matrix.C2;
            C3 = matrix.C3;
            C4 = 0;
            D1 = 0;
            D2 = 0;
            D3 = 0;
            D4 = 1;
        }

        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            Mult(ref left, ref right, out Matrix4 m);
            return m;
        }

        #region Equality

        public static bool operator ==(Matrix4 left, Matrix4 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Matrix4 left, Matrix4 right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix4 matrix &&
                   A1 == matrix.A1 &&
                   A2 == matrix.A2 &&
                   A3 == matrix.A3 &&
                   A4 == matrix.A4 &&
                   B1 == matrix.B1 &&
                   B2 == matrix.B2 &&
                   B3 == matrix.B3 &&
                   B4 == matrix.B4 &&
                   C1 == matrix.C1 &&
                   C2 == matrix.C2 &&
                   C3 == matrix.C3 &&
                   C4 == matrix.C4 &&
                   D1 == matrix.D1 &&
                   D2 == matrix.D2 &&
                   D3 == matrix.D3 &&
                   D4 == matrix.D4;
        }

        public override int GetHashCode()
        {
            var hashCode = -58098712;
            hashCode = hashCode * -1521134295 + A1.GetHashCode();
            hashCode = hashCode * -1521134295 + A2.GetHashCode();
            hashCode = hashCode * -1521134295 + A3.GetHashCode();
            hashCode = hashCode * -1521134295 + A4.GetHashCode();
            hashCode = hashCode * -1521134295 + B1.GetHashCode();
            hashCode = hashCode * -1521134295 + B2.GetHashCode();
            hashCode = hashCode * -1521134295 + B3.GetHashCode();
            hashCode = hashCode * -1521134295 + B4.GetHashCode();
            hashCode = hashCode * -1521134295 + C1.GetHashCode();
            hashCode = hashCode * -1521134295 + C2.GetHashCode();
            hashCode = hashCode * -1521134295 + C3.GetHashCode();
            hashCode = hashCode * -1521134295 + C4.GetHashCode();
            hashCode = hashCode * -1521134295 + D1.GetHashCode();
            hashCode = hashCode * -1521134295 + D2.GetHashCode();
            hashCode = hashCode * -1521134295 + D3.GetHashCode();
            hashCode = hashCode * -1521134295 + D4.GetHashCode();
            return hashCode;
        }

        #endregion

        public void Normalize()
        {
            float determinant = Determinant;
            RowA /= determinant;
            RowB /= determinant;
            RowC /= determinant;
            RowD /= determinant;
        }

        public Matrix4 Normalized()
        {
            Matrix4 m = this;
            m.Normalize();
            return m;
        }

        public Matrix4 Inverted()
        {
            Matrix4 m = this;
            if (m.Determinant != 0f)
                m.Invert();
            return m;
        }

        #region Operations

        public static void Mult(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.A1 = left.A1 * right.A1 + left.A2 * right.B1 + left.A3 * right.C1 + left.A4 * right.D1;
            result.A2 = left.A1 * right.A2 + left.A2 * right.B2 + left.A3 * right.C2 + left.A4 * right.D2;
            result.A3 = left.A1 * right.A3 + left.A2 * right.B3 + left.A3 * right.C3 + left.A4 * right.D3;
            result.A4 = left.A1 * right.A4 + left.A2 * right.B4 + left.A3 * right.C4 + left.A4 * right.D4;
            result.B1 = left.B1 * right.A1 + left.B2 * right.B1 + left.B3 * right.C1 + left.B4 * right.D1;
            result.B2 = left.B1 * right.A2 + left.B2 * right.B2 + left.B3 * right.C2 + left.B4 * right.D2;
            result.B3 = left.B1 * right.A3 + left.B2 * right.B3 + left.B3 * right.C3 + left.B4 * right.D3;
            result.B4 = left.B1 * right.A4 + left.B2 * right.B4 + left.B3 * right.C4 + left.B4 * right.D4;
            result.C1 = left.C1 * right.A1 + left.C2 * right.B1 + left.C3 * right.C1 + left.C4 * right.D1;
            result.C2 = left.C1 * right.A2 + left.C2 * right.B2 + left.C3 * right.C2 + left.C4 * right.D2;
            result.C3 = left.C1 * right.A3 + left.C2 * right.B3 + left.C3 * right.C3 + left.C4 * right.D3;
            result.C4 = left.C1 * right.A4 + left.C2 * right.B4 + left.C3 * right.C4 + left.C4 * right.D4;
            result.D1 = left.D1 * right.A1 + left.D2 * right.B1 + left.D3 * right.C1 + left.D4 * right.D1;
            result.D2 = left.D1 * right.A2 + left.D2 * right.B2 + left.D3 * right.C2 + left.D4 * right.D2;
            result.D3 = left.D1 * right.A3 + left.D2 * right.B3 + left.D3 * right.C3 + left.D4 * right.D3;
            result.D4 = left.D1 * right.A4 + left.D2 * right.B4 + left.D3 * right.C4 + left.D4 * right.D4;
        }

        public void Invert()
        {
            int[] colIdx = new int[4];
            int[] rowIdx = new int[4];
            int[] pivotIdx = new int[4] { -1, -1, -1, -1 };
            float[,] obj = new float[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    obj[i, j] = this[i, j];
            }

            float[,] inverse = obj;
            int icol = 0;
            int irow = 0;
            for (int i2 = 0; i2 < 4; i2++)
            {
                float maxPivot = 0f;
                for (int n = 0; n < 4; n++)
                {
                    if (pivotIdx[n] != 0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (pivotIdx[i] == -1)
                            {
                                float absVal = Math.Abs(inverse[n, i]);
                                if (absVal > maxPivot)
                                {
                                    maxPivot = absVal;
                                    irow = n;
                                    icol = i;
                                }
                            }
                            else if (pivotIdx[i] > 0)
                            {
                                return;
                            }
                        }
                    }
                }
                pivotIdx[icol]++;
                if (irow != icol)
                {
                    for (int m = 0; m < 4; m++)
                    {
                        float f2 = inverse[irow, m];
                        float[,] array = inverse;
                        int num = irow;
                        int num2 = m;
                        float num3 = inverse[icol, m];
                        array[num, num2] = num3;
                        inverse[icol, m] = f2;
                    }
                }
                rowIdx[i2] = irow;
                colIdx[i2] = icol;
                float pivot = inverse[icol, icol];
                if (pivot == 0f)
                {
                    throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
                }
                float oneOverPivot = 1f / pivot;
                inverse[icol, icol] = 1f;
                for (int l = 0; l < 4; l++)
                {
                    inverse[icol, l] *= oneOverPivot;
                }
                for (int k = 0; k < 4; k++)
                {
                    if (icol != k)
                    {
                        float f = inverse[k, icol];
                        inverse[k, icol] = 0f;
                        for (int j = 0; j < 4; j++)
                        {
                            inverse[k, j] -= inverse[icol, j] * f;
                        }
                    }
                }
            }
            for (int j2 = 3; j2 >= 0; j2--)
            {
                int ir = rowIdx[j2];
                int ic = colIdx[j2];
                for (int k2 = 0; k2 < 4; k2++)
                {
                    float f3 = inverse[k2, ir];
                    float[,] array2 = inverse;
                    int num4 = k2;
                    int num5 = ir;
                    float num6 = inverse[k2, ic];
                    array2[num4, num5] = num6;
                    inverse[k2, ic] = f3;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    this[i, j] = inverse[i, j];
            }
        }

        #endregion

        #region Functions

        public static Matrix4 FromQuaternion(Quaternion q)
        {
            q.ToAxisAngle(out Vector3 axis, out float angle);
            return FromAngleAxis(angle, axis);
        }

        public static Matrix4 FromAngleAxis(float radians, Vector3 axis)
        {
            return new Matrix4(Matrix3.FromAngleAxis(radians, axis));
        }

        public static Matrix4 FromTranslation(Vector3 translation)
        {
            Matrix4 identity = Identity;
            identity.D1 = translation.X;
            identity.D2 = translation.Y;
            identity.D3 = translation.Z;

            return identity;
        }

        public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 z = (eye - target).Normalized();
            Vector3 x = (Vector3.Cross(up, z)).Normalized();
            Vector3 y = (Vector3.Cross(z, x)).Normalized();
            Matrix4 result = default(Matrix4);
            result.A1 = x.X;
            result.A2 = y.X;
            result.A3 = z.X;
            result.A4 = 0f;
            result.B1 = x.Y;
            result.B2 = y.Y;
            result.B3 = z.Y;
            result.B4 = 0f;
            result.C1 = x.Z;
            result.C2 = y.Z;
            result.C3 = z.Z;
            result.C4 = 0f;
            result.D1 = 0f - (x.X * eye.X + x.Y * eye.Y + x.Z * eye.Z);
            result.D2 = 0f - (y.X * eye.X + y.Y * eye.Y + y.Z * eye.Z);
            result.D3 = 0f - (z.X * eye.X + z.Y * eye.Y + z.Z * eye.Z);
            result.D4 = 1f;
            return result;
        }

        public Quaternion ExtractRotation(bool row_normalise = true)
        {
            Vector3 row0 = RowA.Xyz;
            Vector3 row = RowB.Xyz;
            Vector3 row2 = RowC.Xyz;
            if (row_normalise)
            {
                row0 = row0.Normalized();
                row = row.Normalized();
                row2 = row2.Normalized();
            }
            Quaternion q = default(Quaternion);
            double trace = 0.25 * ((double)(row0[0] + row[1] + row2[2]) + 1.0);
            if (trace > 0.0)
            {
                double sq8 = Math.Sqrt(trace);
                q.W = (float)sq8;
                sq8 = 1.0 / (4.0 * sq8);
                q.X = (float)((double)(row[2] - row2[1]) * sq8);
                q.Y = (float)((double)(row2[0] - row0[2]) * sq8);
                q.Z = (float)((double)(row0[1] - row[0]) * sq8);
            }
            else if (row0[0] > row[1] && row0[0] > row2[2])
            {
                double sq6 = 2.0 * Math.Sqrt(1.0 + (double)row0[0] - (double)row[1] - (double)row2[2]);
                q.X = (float)(0.25 * sq6);
                sq6 = 1.0 / sq6;
                q.W = (float)((double)(row2[1] - row[2]) * sq6);
                q.Y = (float)((double)(row[0] + row0[1]) * sq6);
                q.Z = (float)((double)(row2[0] + row0[2]) * sq6);
            }
            else if (row[1] > row2[2])
            {
                double sq4 = 2.0 * Math.Sqrt(1.0 + (double)row[1] - (double)row0[0] - (double)row2[2]);
                q.Y = (float)(0.25 * sq4);
                sq4 = 1.0 / sq4;
                q.W = (float)((double)(row2[0] - row0[2]) * sq4);
                q.X = (float)((double)(row[0] + row0[1]) * sq4);
                q.Z = (float)((double)(row2[1] + row[2]) * sq4);
            }
            else
            {
                double sq2 = 2.0 * Math.Sqrt(1.0 + (double)row2[2] - (double)row0[0] - (double)row[1]);
                q.Z = (float)(0.25 * sq2);
                sq2 = 1.0 / sq2;
                q.W = (float)((double)(row[0] - row0[1]) * sq2);
                q.X = (float)((double)(row2[0] + row0[2]) * sq2);
                q.Y = (float)((double)(row2[1] + row[2]) * sq2);
            }
            q.Normalize();
            return q;
        }

        public Vector3 ExtractTranslation()
        {
            return RowD.Xyz;
        }

        public static Vector3 TransformPosition(Matrix4 mat, Vector3 pos)
        {
            Vector3 result = default;
            result.X = pos.X * mat.RowA.X + pos.Y * mat.RowB.X + pos.Z * mat.RowC.X + mat.RowD.X;
            result.Y = pos.X * mat.RowA.Y + pos.Y * mat.RowB.Y + pos.Z * mat.RowC.Y + mat.RowD.Y;
            result.Z = pos.X * mat.RowA.Z + pos.Y * mat.RowB.Z + pos.Z * mat.RowC.Z + mat.RowD.Z;
            return result;
        }

        public Vector3 TransformPosition(Vector3 pos)
        {
            return TransformPosition(this, pos);
        }

        public static Vector3 TransformVector(Matrix4 mat, Vector3 vec)
        {
            Vector3 result = default;
            result.X = vec.X * mat.RowA.X + vec.Y * mat.RowB.X + vec.Z * mat.RowC.X;
            result.Y = vec.X * mat.RowA.Y + vec.Y * mat.RowB.Y + vec.Z * mat.RowC.Y;
            result.Z = vec.X * mat.RowA.Z + vec.Y * mat.RowB.Z + vec.Z * mat.RowC.Z;
            return result;
        }

        public Vector3 TransformVector(Vector3 pos)
        {
            return TransformVector(this, pos);
        }


        #endregion

        public override string ToString()
        {
            return $"{RowA}\n{RowB}\n{RowC}\n{RowD}";
        }
    }
}
