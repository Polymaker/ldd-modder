using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Matrix3d
    {
        public static readonly Matrix3d Identity = new Matrix3d(1d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 1d);

        public double A1;
        public double A2;
        public double A3;

        public double B1;
        public double B2;
        public double B3;

        public double C1;
        public double C2;
        public double C3;

        public Vector3d RowA
        {
            get => new Vector3d(A1, A2, A3);
            set
            {
                A1 = value.X;
                A2 = value.Y;
                A3 = value.Z;
            }
        }

        public Vector3d RowB
        {
            get => new Vector3d(B1, B2, B3);
            set
            {
                B1 = value.X;
                B2 = value.Y;
                B3 = value.Z;
            }
        }

        public Vector3d RowC
        {
            get => new Vector3d(C1, C2, C3);
            set
            {
                C1 = value.X;
                C2 = value.Y;
                C3 = value.Z;
            }
        }

        public Vector3d Col1
        {
            get => new Vector3d(A1, B1, C1);
            set
            {
                A1 = value.X;
                B1 = value.Y;
                C1 = value.Z;
            }
        }

        public Vector3d Col2
        {
            get => new Vector3d(A2, B2, C2);
            set
            {
                A2 = value.X;
                B2 = value.Y;
                C2 = value.Z;
            }
        }

        public Vector3d Col3
        {
            get => new Vector3d(A3, B3, C3);
            set
            {
                A3 = value.X;
                B3 = value.Y;
                C3 = value.Z;
            }
        }

        public double Determinant => A1 * B2 * C3 - A1 * B3 * C2 + A2 * B3 * C1 - A2 * B1 * C3 + A3 * B1 * C2 - A3 * B2 * C1;

        public double this[int col, int row]
        {
            get
            {
                switch (row)
                {
                    case 1:
                        switch (col)
                        {
                            case 1:
                                return A1;
                            case 2:
                                return A2;
                            case 3:
                                return A3;
                            default:
                                return 0d;
                        }
                    case 2:
                        switch (col)
                        {
                            case 1:
                                return B1;
                            case 2:
                                return B2;
                            case 3:
                                return B3;
                            default:
                                return 0d;
                        }
                    case 3:
                        switch (col)
                        {
                            case 1:
                                return C1;
                            case 2:
                                return C2;
                            case 3:
                                return C3;
                            default:
                                return 0d;
                        }
                    default:
                        return 0d;
                }
            }
            set
            {
                switch (row)
                {
                    case 1:
                        switch (col)
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
                        }
                        break;
                    case 2:
                        switch (col)
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
                        }
                        break;
                    case 3:
                        switch (col)
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
                        }
                        break;
                }
            }
        }

        public double this[int index]
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

        public Matrix3d(double a1, double a2, double a3, double b1, double b2, double b3, double c1, double c2, double c3)
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

        public Matrix3d(Vector3d a, Vector3d b, Vector3d c)
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

        public static Matrix3d operator *(Matrix3d left, Matrix3d right)
        {
            Mult(ref left, ref right, out Matrix3d m);
            return m;
        }

        public static bool operator ==(Matrix3d left, Matrix3d right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Matrix3d left, Matrix3d right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix3d matrix &&
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
            double determinant = Determinant;
            RowA /= determinant;
            RowB /= determinant;
            RowC /= determinant;
        }

        public Matrix3d Normalized()
        {
            Matrix3d m = this;
            m.Normalize();
            return m;
        }

        public Matrix3d Inverted()
        {
            Matrix3d m = this;
            if (m.Determinant != 0f)
                m.Invert();
            return m;
        }

        public void Invert()
        {
            int[] colIdx = new int[3];
            int[] rowIdx = new int[3];
            int[] pivotIdx = new int[3] { -1, -1, -1 };
            double[,] obj = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    obj[i, j] = this[i, j];
            }
            double[,] inverse = obj;
            int icol = 0;
            int irow = 0;
            for (int i2 = 0; i2 < 3; i2++)
            {
                double maxPivot = 0f;
                for (int n = 0; n < 3; n++)
                {
                    if (pivotIdx[n] != 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (pivotIdx[i] == -1)
                            {
                                double absVal = Math.Abs(inverse[n, i]);
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
                        double f2 = inverse[irow, m];
                        double[,] array = inverse;
                        int num = irow;
                        int num2 = m;
                        double num3 = inverse[icol, m];
                        array[num, num2] = num3;
                        inverse[icol, m] = f2;
                    }
                }
                rowIdx[i2] = irow;
                colIdx[i2] = icol;
                double pivot = inverse[icol, icol];
                if (pivot == 0f)
                {
                    throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
                }
                double oneOverPivot = 1f / pivot;
                inverse[icol, icol] = 1f;
                for (int l = 0; l < 3; l++)
                {
                    inverse[icol, l] *= oneOverPivot;
                }
                for (int k = 0; k < 3; k++)
                {
                    if (icol != k)
                    {
                        double f = inverse[k, icol];
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
                    double f3 = inverse[k2, ir];
                    double[,] array2 = inverse;
                    int num4 = k2;
                    int num5 = ir;
                    double num6 = inverse[k2, ic];
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

        public static void Mult(ref Matrix3d left, ref Matrix3d right, out Matrix3d result)
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

        public static Matrix3d FromAngleAxis(double radians, Vector3d axis)
        {
            axis.Normalize();
            double cos = Math.Cos((0d - radians));
            double sin = Math.Sin((0d - radians));
            double t = 1f - cos;
            double tXX = t * axis.X * axis.X;
            double tXY = t * axis.X * axis.Y;
            double tXZ = t * axis.X * axis.Z;
            double tYY = t * axis.Y * axis.Y;
            double tYZ = t * axis.Y * axis.Z;
            double tZZ = t * axis.Z * axis.Z;
            double sinX = sin * axis.X;
            double sinY = sin * axis.Y;
            double sinZ = sin * axis.Z;
            Matrix3d result = default(Matrix3d);
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

        public Quaterniond ExtractRotation(bool row_normalise = true)
        {
            Vector3d vector3d = RowA;
            Vector3d vector3d2 = RowB;
            Vector3d vector3d3 = RowC;
            if (row_normalise)
            {
                vector3d = vector3d.Normalized();
                vector3d2 = vector3d2.Normalized();
                vector3d3 = vector3d3.Normalized();
            }
            Quaterniond result = default(Quaterniond);
            double num = 0.25 * (vector3d[0] + vector3d2[1] + vector3d3[2] + 1.0);
            if (num > 0.0)
            {
                double num2 = 1.0 / (4.0 * (result.W = Math.Sqrt(num)));
                result.X = (vector3d2[2] - vector3d3[1]) * num2;
                result.Y = (vector3d3[0] - vector3d[2]) * num2;
                result.Z = (vector3d[1] - vector3d2[0]) * num2;
            }
            else if (vector3d[0] > vector3d2[1] && vector3d[0] > vector3d3[2])
            {
                double num3 = 2.0 * Math.Sqrt(1.0 + vector3d[0] - vector3d2[1] - vector3d3[2]);
                result.X = 0.25 * num3;
                num3 = 1.0 / num3;
                result.W = (vector3d3[1] - vector3d2[2]) * num3;
                result.Y = (vector3d2[0] + vector3d[1]) * num3;
                result.Z = (vector3d3[0] + vector3d[2]) * num3;
            }
            else if (vector3d2[1] > vector3d3[2])
            {
                double num4 = 2.0 * Math.Sqrt(1.0 + vector3d2[1] - vector3d[0] - vector3d3[2]);
                result.Y = 0.25 * num4;
                num4 = 1.0 / num4;
                result.W = (vector3d3[0] - vector3d[2]) * num4;
                result.X = (vector3d2[0] + vector3d[1]) * num4;
                result.Z = (vector3d3[1] + vector3d2[2]) * num4;
            }
            else
            {
                double num5 = 2.0 * Math.Sqrt(1.0 + vector3d3[2] - vector3d[0] - vector3d2[1]);
                result.Z = 0.25 * num5;
                num5 = 1.0 / num5;
                result.W = (vector3d2[0] - vector3d[1]) * num5;
                result.X = (vector3d3[0] + vector3d[2]) * num5;
                result.Y = (vector3d3[1] + vector3d2[2]) * num5;
            }
            result.Normalize();
            return result;
        }

        //public static Matrix3d FromQuaternion(Quaternion q)
        //{
        //    q.ToAxisAngle(out Vector3d axis, out double angle);
        //    return FromAngleAxis(angle, axis);
        //}

        #endregion
    }
}
