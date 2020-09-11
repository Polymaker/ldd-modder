using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Matrix4d
    {
        public static readonly Matrix4d Identity = new Matrix4d(1d, 0d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 0d, 1d);

        #region Fields

        public double A1;
        public double A2;
        public double A3;
        public double A4;

        public double B1;
        public double B2;
        public double B3;
        public double B4;

        public double C1;
        public double C2;
        public double C3;
        public double C4;

        public double D1;
        public double D2;
        public double D3;
        public double D4;

        #endregion

        #region Row Properties

        public Vector4d RowA
        {
            get => new Vector4d(A1, A2, A3, A4);
            set
            {
                A1 = value.X;
                A2 = value.Y;
                A3 = value.Z;
                A4 = value.W;
            }
        }

        public Vector4d RowB
        {
            get => new Vector4d(B1, B2, B3, B4);
            set
            {
                B1 = value.X;
                B2 = value.Y;
                B3 = value.Z;
                B4 = value.W;
            }
        }

        public Vector4d RowC
        {
            get => new Vector4d(C1, C2, C3, C4);
            set
            {
                C1 = value.X;
                C2 = value.Y;
                C3 = value.Z;
                C4 = value.W;
            }
        }

        public Vector4d RowD
        {
            get => new Vector4d(D1, D2, D3, D4);
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

        public Vector4d Col1
        {
            get => new Vector4d(A1, B1, C1, D1);
            set
            {
                A1 = value.X;
                B1 = value.Y;
                C1 = value.Z;
                D1 = value.W;
            }
        }

        public Vector4d Col2
        {
            get => new Vector4d(A2, B2, C2, D2);
            set
            {
                A2 = value.X;
                B2 = value.Y;
                C2 = value.Z;
                D2 = value.W;
            }
        }

        public Vector4d Col3
        {
            get => new Vector4d(A3, B3, C3, D3);
            set
            {
                A3 = value.X;
                B3 = value.Y;
                C3 = value.Z;
                D3 = value.W;
            }
        }

        public Vector4d Col4
        {
            get => new Vector4d(A4, B4, C4, D4);
            set
            {
                A4 = value.X;
                B4 = value.Y;
                C4 = value.Z;
                D4 = value.W;
            }
        }

        #endregion

        public double Determinant => A1 * B2 * C3 * D4 - A1 * B2 * C4 * D3 + A1 * B3 * C4 * D2 - A1 * B3 * C2 * D4 + A1 * B4 * C2 * D3 - A1 * B4 * C3 * D2 - A2 * B3 * C4 * D1 + A2 * B3 * C1 * D4 - A2 * B4 * C1 * D3 + A2 * B4 * C3 * D1 - A2 * B1 * C3 * D4 + A2 * B1 * C4 * D3 + A3 * B4 * C1 * D2 - A3 * B4 * C2 * D1 + A3 * B1 * C2 * D4 - A3 * B1 * C4 * D2 + A3 * B2 * C4 * D1 - A3 * B2 * C1 * D4 - A4 * B1 * C2 * D3 + A4 * B1 * C3 * D2 - A4 * B2 * C3 * D1 + A4 * B2 * C1 * D3 - A4 * B3 * C1 * D2 + A4 * B3 * C2 * D1;

        public double this[int x, int y]
        {
            get
            {
                switch (x)
                {
                    case 0:
                        switch (y)
                        {
                            case 0:
                                return A1;
                            case 1:
                                return A2;
                            case 2:
                                return A3;
                            case 3:
                                return A4;
                            default:
                                return 0d;
                        }
                    case 1:
                        switch (y)
                        {
                            case 0:
                                return B1;
                            case 1:
                                return B2;
                            case 2:
                                return B3;
                            case 3:
                                return B4;
                            default:
                                return 0d;
                        }
                    case 2:
                        switch (y)
                        {
                            case 0:
                                return C1;
                            case 1:
                                return C2;
                            case 2:
                                return C3;
                            case 3:
                                return C4;
                            default:
                                return 0d;
                        }
                    case 3:
                        switch (y)
                        {
                            case 0:
                                return D1;
                            case 1:
                                return D2;
                            case 2:
                                return D3;
                            case 3:
                                return D4;
                            default:
                                return 0d;
                        }
                    default:
                        return 0d;
                }
            }
            set
            {
                switch (x)
                {
                    case 0:
                        switch (y)
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
                            case 3:
                                A4 = value;
                                break;
                        }
                        break;
                    case 1:
                        switch (y)
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
                            case 3:
                                B4 = value;
                                break;
                        }
                        break;
                    case 2:
                        switch (y)
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
                            case 3:
                                C4 = value;
                                break;
                        }
                        break;
                    case 3:
                        switch (y)
                        {
                            case 0:
                                D1 = value;
                                break;
                            case 1:
                                D2 = value;
                                break;
                            case 2:
                                D3 = value;
                                break;
                            case 3:
                                D4 = value;
                                break;
                        }
                        break;
                }
            }
        }

        public Matrix4d(double a1, double a2, double a3, double a4, double b1, double b2, double b3, double b4, double c1, double c2, double c3, double c4, double d1, double d2, double d3, double d4)
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

        public Matrix4d(Vector4d a, Vector4d b, Vector4d c, Vector4d d)
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

        public Matrix4d(Matrix3d matrix)
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

        public static Matrix4d operator *(Matrix4d left, Matrix4d right)
        {
            Mult(ref left, ref right, out Matrix4d m);
            return m;
        }

        public static explicit operator Matrix4d(Matrix4 matrix)
        {
            return new Matrix4d(
                (Vector4d)matrix.RowA, 
                (Vector4d)matrix.RowB, 
                (Vector4d)matrix.RowC,
                (Vector4d)matrix.RowD);
        }

        #region Equality

        public static bool operator ==(Matrix4d left, Matrix4d right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Matrix4d left, Matrix4d right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix4d matrix &&
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
            double determinant = Determinant;
            RowA /= determinant;
            RowB /= determinant;
            RowC /= determinant;
            RowD /= determinant;
        }

        public Matrix4d Normalized()
        {
            Matrix4d m = this;
            m.Normalize();
            return m;
        }

        public Matrix4d Inverted()
        {
            Matrix4d m = this;
            if (m.Determinant != 0d)
                m.Invert();
            return m;
        }

        #region Operations

        public static void Mult(ref Matrix4d left, ref Matrix4d right, out Matrix4d result)
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
            this = Invert(this);
        }

        public static Matrix4d Invert(Matrix4d mat)
        {
            int[] colIdx = new int[4];
            int[] rowIdx = new int[4];
            int[] pivotIdx = new int[4]
            {
        -1,
        -1,
        -1,
        -1
            };
            double[,] inverse = new double[4, 4]
            {
        {
            mat.RowA.X,
            mat.RowA.Y,
            mat.RowA.Z,
            mat.RowA.W
        },
        {
            mat.RowB.X,
            mat.RowB.Y,
            mat.RowB.Z,
            mat.RowB.W
        },
        {
            mat.RowC.X,
            mat.RowC.Y,
            mat.RowC.Z,
            mat.RowC.W
        },
        {
            mat.RowD.X,
            mat.RowD.Y,
            mat.RowD.Z,
            mat.RowD.W
        }
            };
            int icol = 0;
            int irow = 0;
            for (int i2 = 0; i2 < 4; i2++)
            {
                double maxPivot = 0.0;
                for (int n = 0; n < 4; n++)
                {
                    if (pivotIdx[n] == 0)
                    {
                        continue;
                    }
                    for (int i = 0; i < 4; i++)
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
                            return mat;
                        }
                    }
                }
                pivotIdx[icol]++;
                if (irow != icol)
                {
                    for (int m = 0; m < 4; m++)
                    {
                        double f2 = inverse[irow, m];
                        inverse[irow, m] = inverse[icol, m];
                        inverse[icol, m] = f2;
                    }
                }
                rowIdx[i2] = irow;
                colIdx[i2] = icol;
                double pivot = inverse[icol, icol];
                if (pivot == 0.0)
                {
                    throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
                }
                double oneOverPivot = 1.0 / pivot;
                inverse[icol, icol] = 1.0;
                for (int l = 0; l < 4; l++)
                {
                    inverse[icol, l] *= oneOverPivot;
                }
                for (int k = 0; k < 4; k++)
                {
                    if (icol != k)
                    {
                        double f = inverse[k, icol];
                        inverse[k, icol] = 0.0;
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
                    double f3 = inverse[k2, ir];
                    inverse[k2, ir] = inverse[k2, ic];
                    inverse[k2, ic] = f3;
                }
            }
            mat.RowA = new Vector4d(inverse[0, 0], inverse[0, 1], inverse[0, 2], inverse[0, 3]);
            mat.RowB = new Vector4d(inverse[1, 0], inverse[1, 1], inverse[1, 2], inverse[1, 3]);
            mat.RowC = new Vector4d(inverse[2, 0], inverse[2, 1], inverse[2, 2], inverse[2, 3]);
            mat.RowD = new Vector4d(inverse[3, 0], inverse[3, 1], inverse[3, 2], inverse[3, 3]);
            return mat;
        }

        #endregion

        #region Functions

        public static Matrix4d FromQuaternion(Quaterniond q)
        {
            q.ToAxisAngle(out Vector3d axis, out double angle);
            return FromAngleAxis(angle, axis);
        }

        public static Matrix4d FromAngleAxis(double radians, Vector3d axis)
        {
            return new Matrix4d(Matrix3d.FromAngleAxis(radians, axis));
        }

        public static Matrix4d FromTranslation(Vector3d translation)
        {
            Matrix4d identity = Identity;
            identity.D1 = translation.X;
            identity.D2 = translation.Y;
            identity.D3 = translation.Z;

            return identity;
        }

        public static Matrix4d FromScale(Vector3d scale)
        {
            var result = Identity;
            result.A1 = scale.X;
            result.B2 = scale.Y;
            result.C3 = scale.Z;
            return result;
        }

        public static Matrix4d LookAt(Vector3d eye, Vector3d target, Vector3d up)
        {
            Vector3d z = (eye - target).Normalized();
            Vector3d x = (Vector3d.Cross(up, z)).Normalized();
            Vector3d y = (Vector3d.Cross(z, x)).Normalized();
            Matrix4d result = default(Matrix4d);
            result.A1 = x.X;
            result.A2 = y.X;
            result.A3 = z.X;
            result.A4 = 0d;
            result.B1 = x.Y;
            result.B2 = y.Y;
            result.B3 = z.Y;
            result.B4 = 0d;
            result.C1 = x.Z;
            result.C2 = y.Z;
            result.C3 = z.Z;
            result.C4 = 0d;
            result.D1 = 0d - (x.X * eye.X + x.Y * eye.Y + x.Z * eye.Z);
            result.D2 = 0d - (y.X * eye.X + y.Y * eye.Y + y.Z * eye.Z);
            result.D3 = 0d - (z.X * eye.X + z.Y * eye.Y + z.Z * eye.Z);
            result.D4 = 1d;
            return result;
        }

        public Quaterniond ExtractRotation(bool row_normalise = true)
        {
            Vector3d row0 = RowA.Xyz;
            Vector3d row = RowB.Xyz;
            Vector3d row2 = RowC.Xyz;
            if (row_normalise)
            {
                row0 = row0.Normalized();
                row = row.Normalized();
                row2 = row2.Normalized();
            }
            Quaterniond q = default(Quaterniond);
            double trace = 0.25 * (((double)row0[0] + (double)row[1] + (double)row2[2]) + 1.0);
            if (trace > 0.0000001)
            {
                double sq8 = Math.Sqrt(trace);
                q.W = (double)sq8;
                sq8 = 1.0 / (4.0 * sq8);
                q.X = (double)((double)(row[2] - row2[1]) * sq8);
                q.Y = (double)((double)(row2[0] - row0[2]) * sq8);
                q.Z = (double)((double)(row0[1] - row[0]) * sq8);
            }
            else if (row0[0] > row[1] && row0[0] > row2[2])
            {
                double sq6 = 2.0 * Math.Sqrt(1.0 + (double)row0[0] - (double)row[1] - (double)row2[2]);
                q.X = (double)(0.25 * sq6);
                sq6 = 1.0 / sq6;
                q.W = (double)((double)(row2[1] - row[2]) * sq6);
                q.Y = (double)((double)(row[0] + row0[1]) * sq6);
                q.Z = (double)((double)(row2[0] + row0[2]) * sq6);
            }
            else if (row[1] > row2[2])
            {
                double sq4 = 2.0 * Math.Sqrt(1.0 + (double)row[1] - (double)row0[0] - (double)row2[2]);
                q.Y = (double)(0.25 * sq4);
                sq4 = 1.0 / sq4;
                q.W = (double)((double)(row2[0] - row0[2]) * sq4);
                q.X = (double)((double)(row[0] + row0[1]) * sq4);
                q.Z = (double)((double)(row2[1] + row[2]) * sq4);
            }
            else
            {
                double sq2 = 2.0 * Math.Sqrt(1.0 + (double)row2[2] - (double)row0[0] - (double)row[1]);
                q.Z = (double)(0.25 * sq2);
                sq2 = 1.0 / sq2;
                q.W = (double)((double)(row[0] - row0[1]) * sq2);
                q.X = (double)((double)(row2[0] + row0[2]) * sq2);
                q.Y = (double)((double)(row2[1] + row[2]) * sq2);
            }
            q.Normalize();
            return q;
        }

        public Vector3d ExtractTranslation()
        {
            return RowD.Xyz;
        }

        public static Vector3d TransformPosition(Matrix4d mat, Vector3d pos)
        {
            Vector3d result = default;
            result.X = pos.X * mat.RowA.X + pos.Y * mat.RowB.X + pos.Z * mat.RowC.X + mat.RowD.X;
            result.Y = pos.X * mat.RowA.Y + pos.Y * mat.RowB.Y + pos.Z * mat.RowC.Y + mat.RowD.Y;
            result.Z = pos.X * mat.RowA.Z + pos.Y * mat.RowB.Z + pos.Z * mat.RowC.Z + mat.RowD.Z;
            return result;
        }

        public Vector3d TransformPosition(Vector3d pos)
        {
            return TransformPosition(this, pos);
        }

        public static Vector3d TransformNormalInverse(Matrix4d invMat, Vector3d vec)
        {
            Vector3d result = default;
            result.X = vec.X * invMat.RowA.X + vec.Y * invMat.RowA.Y + vec.Z * invMat.RowA.Z;
            result.Y = vec.X * invMat.RowB.X + vec.Y * invMat.RowB.Y + vec.Z * invMat.RowB.Z;
            result.Z = vec.X * invMat.RowC.X + vec.Y * invMat.RowC.Y + vec.Z * invMat.RowC.Z;
            return result;
        }

        public Vector3d TransformNormalInverse(Vector3d pos)
        {
            return TransformNormalInverse(this, pos);
        }

        public static Vector3d TransformNormal(Matrix4d mat, Vector3d vec)
        {
            return TransformNormalInverse(mat.Inverted(), vec);
        }

        public Vector3d TransformNormal(Vector3d pos)
        {
            return TransformNormal(this, pos);
        }

        public static Vector3d TransformVector(Matrix4d mat, Vector3d vec)
        {
            Vector3d result = default;
            result.X = vec.X * mat.RowA.X + vec.Y * mat.RowB.X + vec.Z * mat.RowC.X;
            result.Y = vec.X * mat.RowA.Y + vec.Y * mat.RowB.Y + vec.Z * mat.RowC.Y;
            result.Z = vec.X * mat.RowA.Z + vec.Y * mat.RowB.Z + vec.Z * mat.RowC.Z;
            return result;
        }

        public Vector3d TransformVector(Vector3d pos)
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
