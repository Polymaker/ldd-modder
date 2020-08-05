using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Matrix3
    {
        public static readonly Matrix3 Identity = new Matrix3(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);

        public static readonly Matrix3 Zero = new Matrix3();

        public float A1;
        public float A2;
        public float A3;

        public float B1;
        public float B2;
        public float B3;

        public float C1;
        public float C2;
        public float C3;

        public Vector3 RowA
        {
            get => new Vector3(A1, A2, A3);
            set
            {
                A1 = value.X;
                A2 = value.Y;
                A3 = value.Z;
            }
        }

        public Vector3 RowB
        {
            get => new Vector3(B1, B2, B3);
            set
            {
                B1 = value.X;
                B2 = value.Y;
                B3 = value.Z;
            }
        }

        public Vector3 RowC
        {
            get => new Vector3(C1, C2, C3);
            set
            {
                C1 = value.X;
                C2 = value.Y;
                C3 = value.Z;
            }
        }

        public Vector3 Col1
        {
            get => new Vector3(A1, B1, C1);
            set
            {
                A1 = value.X;
                B1 = value.Y;
                C1 = value.Z;
            }
        }

        public Vector3 Col2
        {
            get => new Vector3(A2, B2, C2);
            set
            {
                A2 = value.X;
                B2 = value.Y;
                C2 = value.Z;
            }
        }

        public Vector3 Col3
        {
            get => new Vector3(A3, B3, C3);
            set
            {
                A3 = value.X;
                B3 = value.Y;
                C3 = value.Z;
            }
        }

        public float Determinant => A1 * B2 * C3 - A1 * B3 * C2 + A2 * B3 * C1 - A2 * B1 * C3 + A3 * B1 * C2 - A3 * B2 * C1;

        public float this[int col, int row]
        {
            get
            {
                switch (row)
                {
                    case 0:
                        switch (col)
                        {
                            case 0:
                                return A1;
                            case 1:
                                return A2;
                            case 2:
                                return A3;
                            default:
                                return 0f;
                        }
                    case 1:
                        switch (col)
                        {
                            case 0:
                                return B1;
                            case 1:
                                return B2;
                            case 2:
                                return B3;
                            default:
                                return 0f;
                        }
                    case 2:
                        switch (col)
                        {
                            case 0:
                                return C1;
                            case 1:
                                return C2;
                            case 2:
                                return C3;
                            default:
                                return 0f;
                        }
                    default:
                        return 0f;
                }
            }
            set
            {
                switch (row)
                {
                    case 0:
                        switch (col)
                        {
                            case 0:
                                A1 = value;
                                break;
                            case 1:
                                A2 = value;
                                break;
                            case 2:
                                A3 = value;
                                break;
                        }
                        break;
                    case 1:
                        switch (col)
                        {
                            case 0:
                                B1 = value;
                                break;
                            case 1:
                                B2 = value;
                                break;
                            case 2:
                                B3 = value;
                                break;
                        }
                        break;
                    case 2:
                        switch (col)
                        {
                            case 0:
                                C1 = value;
                                break;
                            case 1:
                                C2 = value;
                                break;
                            case 2:
                                C3 = value;
                                break;
                        }
                        break;
                }
            }
        }

        public float this[int index]
        {
            get
            {
                int x = index % 3;
                int y = (int)Math.Floor(index / 3d);
                return this[x, y];
            }
            set
            {
                int x = index % 3;
                int y = (int)Math.Floor(index / 3d);
                this[x, y] = value;
            }
        }

        public Matrix3(float a1, float a2, float a3, float b1, float b2, float b3, float c1, float c2, float c3)
        {
            A1 = a1;
            A2 = a2;
            A3 = a3;
            B1 = b1;
            B2 = b2;
            B3 = b3;
            C1 = c1;
            C2 = c2;
            C3 = c3;
        }

        public Matrix3(Vector3 a, Vector3 b, Vector3 c)
        {
            A1 = a.X;
            A2 = a.Y;
            A3 = a.Z;
            B1 = b.X;
            B2 = b.Y;
            B3 = b.Z;
            C1 = c.X;
            C2 = c.Y;
            C3 = c.Z;
        }

        public Matrix3(Matrix4 matrix)
        {
            A1 = matrix.A1;
            A2 = matrix.A2;
            A3 = matrix.A3;
            B1 = matrix.B1;
            B2 = matrix.B2;
            B3 = matrix.B3;
            C1 = matrix.C1;
            C2 = matrix.C2;
            C3 = matrix.C3;
        }

        public static Matrix3 operator *(Matrix3 left, Matrix3 right)
        {
            Mult(ref left, ref right, out Matrix3 m);
            return m;
        }

        public static bool operator ==(Matrix3 left, Matrix3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Matrix3 left, Matrix3 right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix3 matrix &&
                   A1 == matrix.A1 &&
                   A2 == matrix.A2 &&
                   A3 == matrix.A3 &&
                   B1 == matrix.B1 &&
                   B2 == matrix.B2 &&
                   B3 == matrix.B3 &&
                   C1 == matrix.C1 &&
                   C2 == matrix.C2 &&
                   C3 == matrix.C3;
        }

        public override int GetHashCode()
        {
            var hashCode = -1036704979;
            hashCode = hashCode * -1521134295 + A1.GetHashCode();
            hashCode = hashCode * -1521134295 + A2.GetHashCode();
            hashCode = hashCode * -1521134295 + A3.GetHashCode();
            hashCode = hashCode * -1521134295 + B1.GetHashCode();
            hashCode = hashCode * -1521134295 + B2.GetHashCode();
            hashCode = hashCode * -1521134295 + B3.GetHashCode();
            hashCode = hashCode * -1521134295 + C1.GetHashCode();
            hashCode = hashCode * -1521134295 + C2.GetHashCode();
            hashCode = hashCode * -1521134295 + C3.GetHashCode();
            return hashCode;
        }

        #region Operations

        public void Normalize()
        {
            float determinant = Determinant;
            RowA /= determinant;
            RowB /= determinant;
            RowC /= determinant;
        }

        public Matrix3 Normalized()
        {
            Matrix3 m = this;
            m.Normalize();
            return m;
        }

        public Matrix3 Inverted()
        {
            Matrix3 m = this;
            if (m.Determinant != 0f)
                m.Invert();
            return m;
        }

        public void Invert()
        {
            int[] colIdx = new int[3];
            int[] rowIdx = new int[3];
            int[] pivotIdx = new int[3] { -1, -1, -1 };
            float[,] obj = new float[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    obj[i, j] = this[i, j];
            }
            float[,] inverse = obj;
            int icol = 0;
            int irow = 0;
            for (int i2 = 0; i2 < 3; i2++)
            {
                float maxPivot = 0f;
                for (int n = 0; n < 3; n++)
                {
                    if (pivotIdx[n] != 0)
                    {
                        for (int i = 0; i < 3; i++)
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
                    for (int m = 0; m < 3; m++)
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
                for (int l = 0; l < 3; l++)
                {
                    inverse[icol, l] *= oneOverPivot;
                }
                for (int k = 0; k < 3; k++)
                {
                    if (icol != k)
                    {
                        float f = inverse[k, icol];
                        inverse[k, icol] = 0f;
                        for (int j = 0; j < 3; j++)
                        {
                            inverse[k, j] -= inverse[icol, j] * f;
                        }
                    }
                }
            }
            for (int j2 = 2; j2 >= 0; j2--)
            {
                int ir = rowIdx[j2];
                int ic = colIdx[j2];
                for (int k2 = 0; k2 < 3; k2++)
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

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    this[i, j] = inverse[i, j];
            }
        }

        public static void Mult(ref Matrix3 left, ref Matrix3 right, out Matrix3 result)
        {
            result.A1 = left.A1 * right.A1 + left.A2 * right.B1 + left.A3 * right.C1;
            result.A2 = left.A1 * right.A2 + left.A2 * right.B2 + left.A3 * right.C2;
            result.A3 = left.A1 * right.A3 + left.A2 * right.B3 + left.A3 * right.C3;
            result.B1 = left.B1 * right.A1 + left.B2 * right.B1 + left.B3 * right.C1;
            result.B2 = left.B1 * right.A2 + left.B2 * right.B2 + left.B3 * right.C2;
            result.B3 = left.B1 * right.A3 + left.B2 * right.B3 + left.B3 * right.C3;
            result.C1 = left.C1 * right.A1 + left.C2 * right.B1 + left.C3 * right.C1;
            result.C2 = left.C1 * right.A2 + left.C2 * right.B2 + left.C3 * right.C2;
            result.C3 = left.C1 * right.A3 + left.C2 * right.B3 + left.C3 * right.C3;
        }

        #endregion

        #region Functions

        public static Matrix3 FromAngleAxis(float radians, Vector3 axis)
        {
            axis.Normalize();
            float cos = (float)Math.Cos((0f - radians));
            float sin = (float)Math.Sin((0f - radians));
            float t = 1f - cos;
            float tXX = t * axis.X * axis.X;
            float tXY = t * axis.X * axis.Y;
            float tXZ = t * axis.X * axis.Z;
            float tYY = t * axis.Y * axis.Y;
            float tYZ = t * axis.Y * axis.Z;
            float tZZ = t * axis.Z * axis.Z;
            float sinX = sin * axis.X;
            float sinY = sin * axis.Y;
            float sinZ = sin * axis.Z;
            Matrix3 result = default(Matrix3);
            result.A1 = tXX + cos;
            result.A2 = tXY - sinZ;
            result.A3 = tXZ + sinY;
            result.B1 = tXY + sinZ;
            result.B2 = tYY + cos;
            result.B3 = tYZ - sinX;
            result.C1 = tXZ - sinY;
            result.C2 = tYZ + sinX;
            result.C3 = tZZ + cos;
            return result;
        }

        public static Matrix3 FromQuaternion(Quaternion q)
        {
            q.ToAxisAngle(out Vector3 axis, out float angle);
            return FromAngleAxis(angle, axis);
        }

        #endregion

        public float[] ToArray()
        {
            return new float[] 
            { 
                A1, A2, A3, 
                B1, B2, B3, 
                C1, C2, C3 
            };
        }

        public override string ToString()
        {
            return $"{RowA}\n{RowB}\n{RowC}";
        }
    }
}
