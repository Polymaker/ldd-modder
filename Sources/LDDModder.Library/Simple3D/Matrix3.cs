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

        public Matrix3(float a1, float a2, float a3, float b1, float b2, float b3, float c1, float c2, float c3) : this()
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

        public void Inverse()
        {
            float num = Determinant;
            if (num == 0f)
            {
                A1 = float.NaN;
                A2 = float.NaN;
                A3 = float.NaN;
                B1 = float.NaN;
                B2 = float.NaN;
                B3 = float.NaN;
                C1 = float.NaN;
                C2 = float.NaN;
                C3 = float.NaN;
            }
            float num2 = 1f / num;
            float a = num2 * (B2 * C3 - B3 * C2);
            float a2 = (0f - num2) * (A2 * C3 - A3 * C2);
            float a3 = num2 * (A2 * B3 - A3 * B2);
            float b = (0f - num2) * (B1 * C3 - B3 * C1);
            float b2 = num2 * (A1 * C3 - A3 * C1);
            float b3 = (0f - num2) * (A1 * B3 - A3 * B1);
            float c = num2 * (B1 * C2 - B2 * C1);
            float c2 = (0f - num2) * (A1 * C2 - A2 * C1);
            float c3 = num2 * (A1 * B2 - A2 * B1);
            A1 = a;
            A2 = a2;
            A3 = a3;
            B1 = b;
            B2 = b2;
            B3 = b3;
            C1 = c;
            C2 = c2;
            C3 = c3;
        }

        // TODO: Review to ensure row-major or column-major order consistency.
        //       (OpenTK and Assimp do not use the same order)
        public static Matrix3 operator *(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(a.A1 * b.A1 + a.B1 * b.A2 + a.C1 * b.A3, a.A2 * b.A1 + a.B2 * b.A2 + a.C2 * b.A3, a.A3 * b.A1 + a.B3 * b.A2 + a.C3 * b.A3, a.A1 * b.B1 + a.B1 * b.B2 + a.C1 * b.B3, a.A2 * b.B1 + a.B2 * b.B2 + a.C2 * b.B3, a.A3 * b.B1 + a.B3 * b.B2 + a.C3 * b.B3, a.A1 * b.C1 + a.B1 * b.C2 + a.C1 * b.C3, a.A2 * b.C1 + a.B2 * b.C2 + a.C2 * b.C3, a.A3 * b.C1 + a.B3 * b.C2 + a.C3 * b.C3);
        }

        public float[] ToArray()
        {
            return new float[] { A1, A2, A3, B1, B2, B3, C1, C2, C3 };
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

        // TODO: Review to ensure row-major or column-major order consistency.
        //       (OpenTK and Assimp do not use the same order)
        public static Matrix3 FromAngleAxis(float radians, Vector3 axis)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float num = (float)Math.Sin(radians);
            float num2 = (float)Math.Cos(radians);
            float num3 = x * x;
            float num4 = y * y;
            float num5 = z * z;
            float num6 = x * y;
            float num7 = x * z;
            float num8 = y * z;
            Matrix3 result = default(Matrix3);
            result.A1 = num3 + num2 * (1f - num3);
            result.B1 = num6 - num2 * num6 + num * z;
            result.C1 = num7 - num2 * num7 - num * y;
            result.A2 = num6 - num2 * num6 - num * z;
            result.B2 = num4 + num2 * (1f - num4);
            result.C2 = num8 - num2 * num8 + num * x;
            result.A3 = num7 - num2 * num7 + num * y;
            result.B3 = num8 - num2 * num8 - num * x;
            result.C3 = num5 + num2 * (1f - num5);
            return result;
        }
    }
}
