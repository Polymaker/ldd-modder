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

        public void Inverse()
        {
            float num = Determinant;
            if (num == 0f)
            {
                A1 = float.NaN;
                A2 = float.NaN;
                A3 = float.NaN;
                A4 = float.NaN;
                B1 = float.NaN;
                B2 = float.NaN;
                B3 = float.NaN;
                B4 = float.NaN;
                C1 = float.NaN;
                C2 = float.NaN;
                C3 = float.NaN;
                C4 = float.NaN;
                D1 = float.NaN;
                D2 = float.NaN;
                D3 = float.NaN;
                D4 = float.NaN;
            }
            float num2 = 1f / num;
            float a = num2 * (B2 * (C3 * D4 - C4 * D3) + B3 * (C4 * D2 - C2 * D4) + B4 * (C2 * D3 - C3 * D2));
            float a2 = (0f - num2) * (A2 * (C3 * D4 - C4 * D3) + A3 * (C4 * D2 - C2 * D4) + A4 * (C2 * D3 - C3 * D2));
            float a3 = num2 * (A2 * (B3 * D4 - B4 * D3) + A3 * (B4 * D2 - B2 * D4) + A4 * (B2 * D3 - B3 * D2));
            float a4 = (0f - num2) * (A2 * (B3 * C4 - B4 * C3) + A3 * (B4 * C2 - B2 * C4) + A4 * (B2 * C3 - B3 * C2));
            float b = (0f - num2) * (B1 * (C3 * D4 - C4 * D3) + B3 * (C4 * D1 - C1 * D4) + B4 * (C1 * D3 - C3 * D1));
            float b2 = num2 * (A1 * (C3 * D4 - C4 * D3) + A3 * (C4 * D1 - C1 * D4) + A4 * (C1 * D3 - C3 * D1));
            float b3 = (0f - num2) * (A1 * (B3 * D4 - B4 * D3) + A3 * (B4 * D1 - B1 * D4) + A4 * (B1 * D3 - B3 * D1));
            float b4 = num2 * (A1 * (B3 * C4 - B4 * C3) + A3 * (B4 * C1 - B1 * C4) + A4 * (B1 * C3 - B3 * C1));
            float c = num2 * (B1 * (C2 * D4 - C4 * D2) + B2 * (C4 * D1 - C1 * D4) + B4 * (C1 * D2 - C2 * D1));
            float c2 = (0f - num2) * (A1 * (C2 * D4 - C4 * D2) + A2 * (C4 * D1 - C1 * D4) + A4 * (C1 * D2 - C2 * D1));
            float c3 = num2 * (A1 * (B2 * D4 - B4 * D2) + A2 * (B4 * D1 - B1 * D4) + A4 * (B1 * D2 - B2 * D1));
            float c4 = (0f - num2) * (A1 * (B2 * C4 - B4 * C2) + A2 * (B4 * C1 - B1 * C4) + A4 * (B1 * C2 - B2 * C1));
            float d = (0f - num2) * (B1 * (C2 * D3 - C3 * D2) + B2 * (C3 * D1 - C1 * D3) + B3 * (C1 * D2 - C2 * D1));
            float d2 = num2 * (A1 * (C2 * D3 - C3 * D2) + A2 * (C3 * D1 - C1 * D3) + A3 * (C1 * D2 - C2 * D1));
            float d3 = (0f - num2) * (A1 * (B2 * D3 - B3 * D2) + A2 * (B3 * D1 - B1 * D3) + A3 * (B1 * D2 - B2 * D1));
            float d4 = num2 * (A1 * (B2 * C3 - B3 * C2) + A2 * (B3 * C1 - B1 * C3) + A3 * (B1 * C2 - B2 * C1));
            A1 = a;
            A2 = a2;
            A3 = a3;
            A4 = a4;
            B1 = b;
            B2 = b2;
            B3 = b3;
            B4 = b4;
            C1 = c;
            C2 = c2;
            C3 = c3;
            C4 = c4;
            D1 = d;
            D2 = d2;
            D3 = d3;
            D4 = d4;
        }

        public static Matrix4 operator *(Matrix4 a, Matrix4 b)
        {
            return new Matrix4(
                a.A1 * b.A1 + a.B1 * b.A2 + a.C1 * b.A3 + a.D1 * b.A4, 
                a.A2 * b.A1 + a.B2 * b.A2 + a.C2 * b.A3 + a.D2 * b.A4, 
                a.A3 * b.A1 + a.B3 * b.A2 + a.C3 * b.A3 + a.D3 * b.A4, 
                a.A4 * b.A1 + a.B4 * b.A2 + a.C4 * b.A3 + a.D4 * b.A4, 
                a.A1 * b.B1 + a.B1 * b.B2 + a.C1 * b.B3 + a.D1 * b.B4, 
                a.A2 * b.B1 + a.B2 * b.B2 + a.C2 * b.B3 + a.D2 * b.B4, 
                a.A3 * b.B1 + a.B3 * b.B2 + a.C3 * b.B3 + a.D3 * b.B4, 
                a.A4 * b.B1 + a.B4 * b.B2 + a.C4 * b.B3 + a.D4 * b.B4, 
                a.A1 * b.C1 + a.B1 * b.C2 + a.C1 * b.C3 + a.D1 * b.C4, 
                a.A2 * b.C1 + a.B2 * b.C2 + a.C2 * b.C3 + a.D2 * b.C4, 
                a.A3 * b.C1 + a.B3 * b.C2 + a.C3 * b.C3 + a.D3 * b.C4, 
                a.A4 * b.C1 + a.B4 * b.C2 + a.C4 * b.C3 + a.D4 * b.C4, 
                a.A1 * b.D1 + a.B1 * b.D2 + a.C1 * b.D3 + a.D1 * b.D4, 
                a.A2 * b.D1 + a.B2 * b.D2 + a.C2 * b.D3 + a.D2 * b.D4, 
                a.A3 * b.D1 + a.B3 * b.D2 + a.C3 * b.D3 + a.D3 * b.D4, 
                a.A4 * b.D1 + a.B4 * b.D2 + a.C4 * b.D3 + a.D4 * b.D4);
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

        public static Matrix4 FromAngleAxis(float radians, Vector3 axis)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float num = (float)Math.Sin((double)radians);
            float num2 = (float)Math.Cos((double)radians);
            float num3 = x * x;
            float num4 = y * y;
            float num5 = z * z;
            float num6 = x * y;
            float num7 = x * z;
            float num8 = y * z;
            Matrix4 result = default(Matrix4);
            result.A1 = num3 + num2 * (1f - num3);
            result.B1 = num6 - num2 * num6 + num * z;
            result.C1 = num7 - num2 * num7 - num * y;
            result.D1 = 0f;
            result.A2 = num6 - num2 * num6 - num * z;
            result.B2 = num4 + num2 * (1f - num4);
            result.C2 = num8 - num2 * num8 + num * x;
            result.D2 = 0f;
            result.A3 = num7 - num2 * num7 + num * y;
            result.B3 = num8 - num2 * num8 - num * x;
            result.C3 = num5 + num2 * (1f - num5);
            result.D3 = 0f;
            result.A4 = 0f;
            result.B4 = 0f;
            result.C4 = 0f;
            result.D4 = 1f;
            return result;
        }

        public static Matrix4 FromTranslation(Vector3 translation)
        {
            Matrix4 identity = Identity;
            identity.A4 = translation.X;
            identity.B4 = translation.Y;
            identity.C4 = translation.Z;
            return identity;
        }
    }
}
